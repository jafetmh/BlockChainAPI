using BlockChain_DB;
using BlockChain_DB.General;
using BlockChain_DB.General.Message;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Interfaces.IRepository;
using BlockChainAPI.Interfaces.IServices.IAppServices;
using BlockChainAPI.Interfaces.IServices.ICrypto.AES;
using BlockChainAPI.Interfaces.IServices.ICrypto.SHA256;
using BlockChainAPI.Interfaces.IServices.Utilities;
using BlockChainAPI.Utilities;
using BlockChainAPI.Utilities.ResponseMessage;
using System.Diagnostics;

namespace BlockChainAPI.Services.AppServices
{
    public class BlockService : IBlockService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBlockRepository _blockRepository;
        private readonly ISHA256Hash _sha256Hash;
        private readonly IChainRepository _chainRepository;
        private readonly IDocumentService _documentService;
        private readonly IMemPoolDocumentService _memPoolDocumentService;
        private readonly IAESEncryption _encryption;
        private readonly IConfigurationRepository _configurationRepository;
        private readonly ILogService _logService;   
        private readonly Message _message;

        public BlockService(IUserRepository userRepository,
                            IBlockRepository blockRepository,
                            ISHA256Hash sha256Hash, 
                            IChainRepository chainRepository,
                            IDocumentService documentService,
                            IMemPoolDocumentService memPoolDocumentService,
                            IAESEncryption encryption,
                            IConfigurationRepository configurationRepository,
                            ILogService logService,
                            IMessageService messages) {
            _userRepository = userRepository;
            _blockRepository = blockRepository;
            _sha256Hash = sha256Hash;
            _chainRepository = chainRepository;
            _documentService = documentService;
            _memPoolDocumentService = memPoolDocumentService;
            _encryption = encryption;
            _logService = logService;
            _configurationRepository = configurationRepository;
            
            _message = messages.Get_Message();
        }

        public async Task<Response<Block>> StartMiningTask(int userId, List<MemPoolDocument> documents)
        {
            Response<SystemConfig> sysconfig = await _configurationRepository.GetMaxBlockDocuments();
            if (documents.Count > int.Parse(sysconfig.Data.Value)) return ResponseResult.CreateResponse<Block>(false, _message.InvalidMaxDocuments);
            return await Task.Run(() => BuildBlock(userId, documents));
        }

        public async Task<Response<Block>> BuildBlock(int userId, List<MemPoolDocument> documents)
        {
            try
            {
                List<MemPoolDocument> documentsToMine = await _memPoolDocumentService.FilterMemPoolDocument(userId ,documents);
                Block block = new Block();
                Response<User> responseResult = await _userRepository.GetUser(userId);
                User user = responseResult.Data;
                if (user == null) { return ResponseResult.CreateResponse<Block>(false, _message.Failure.Set); }
                Chain chain = await _chainRepository.CreateChain(user.Id);
                block.Id = chain.Blocks.Count == 0? 1: (chain.Blocks.Last().Id + 1);
                block.ChainID = chain.Id;
                block.Previous_Hash = chain.Blocks.Count == 0 ? new string('0', 64) : chain.Blocks.Last().Hash;
                string docs_base64String = string.Empty;
                foreach (MemPoolDocument document in documentsToMine)
                {
                    docs_base64String += document.Doc_encode;
                }
                MiningBlock(block, user, docs_base64String);
                int entriesWriten = await _blockRepository.CreateBlock(block);
                if (entriesWriten <= 0) { return ResponseResult.CreateResponse<Block>(false, _message.Failure.Set); }
                List<Document> documentsTocreate = documentsToMine.Select(Document.FromMempoolDocument).ToList();
                await _documentService.BulkCreateDocuments(user, documentsTocreate, block);
                await _logService.Log(_message.LogMessages.CreateBlock, user.Name, new { data = block});
                return ResponseResult.CreateResponse(true, _message.Success.Set, block);
            }
            catch(Exception ex)
            {
                return ResponseResult.CreateResponse<Block>(false, _message.Failure.Set);
            }

        }

        public void MiningBlock(Block block, User user, string docsBase64)
        {
            string validation = "0000";
            bool isMined = false;
            DateTime miningStartDate = DateTime.Now;
            block.MiningDate = miningStartDate;
            block.Attempts = 0;
            Stopwatch stopwatch = Stopwatch.StartNew();

            while (!isMined)
            {
                string blockData = $"{block.Previous_Hash}{block.MiningDate}{block.Attempts}{docsBase64}";
                string hash = _sha256Hash.GenerateHash(blockData);
                if (hash.StartsWith(validation))
                {
                    isMined = true;
                    block.Hash = hash;
                    block.Milliseconds = (int)(DateTime.Now - miningStartDate).TotalMilliseconds;
                }
                block.Attempts++;
                DateTime currentTime = DateTime.Now;
                if ((currentTime - miningStartDate).TotalSeconds >= 1)
                {
                    miningStartDate = currentTime;
                    block.MiningDate = currentTime;
                    block.Attempts = 0;
                }
            }
            stopwatch.Stop();
            Console.WriteLine($"Tiempo de minado: {stopwatch.Elapsed}");
        }


        public async Task<Response<BlockResponse>> GetBlocks(int userId)
        {
            try
            {
                Response<List<Block>> responseResult = await _blockRepository.GetBlocks(userId);
                var blocks = responseResult.Data?? new List<Block>();

                if (!responseResult.Success || !blocks.Any())
                {
                    return ResponseResult.CreateResponse<BlockResponse>(false, _message.NotFound);
                }
                Response<User> user = await _userRepository.GetUser(userId);
                _encryption.GetKeyAndIv(user.Data);
                List<Block> incosistentBlocks = VerifyBlockHashesConsistency(responseResult.Data);
                if (incosistentBlocks.Count > 0) await _logService.Log(_message.LogMessages.ChainValidation, user.Data.Name, new { data = incosistentBlocks });
                List<Block> alteredBlocks = await VerifyBlockIntegrity(responseResult.Data);
                if (incosistentBlocks.Count > 0) await _logService.Log(_message.LogMessages.AlteredBlocks, user.Data.Name, new { data = alteredBlocks });

                BlockResponse response = new ()
                {
                    Blocks = responseResult.Data,
                    InconsistentBlocks = incosistentBlocks.Count != 0? incosistentBlocks: null,
                    AlteredBlocks = alteredBlocks.Count != 0? alteredBlocks: null

                };
                return ResponseResult.CreateResponse(true, _message.Success.Get, response);
            }
            catch (Exception ex) { 
                return ResponseResult.CreateResponse<BlockResponse>(false, ex.Message); 
            }

        }

        public List<Block> VerifyBlockHashesConsistency(List<Block> blocks)
        {
            List<Block> blockInconsistency = new();
            for(int i = 1; i < blocks.Count; i++)
            {
                if (blocks[i].Previous_Hash != blocks[i-1].Hash)
                {
                    blockInconsistency.Add(blocks[i]);
                }
            }
            return blockInconsistency;

        }

        public async Task<List<Block>> VerifyBlockIntegrity(List<Block> blocks)
        {
            List<Block> changedBlocks = new();
            foreach (Block block in blocks) {

                List<Document> documents = block.Documents.ToList();
                await DecryptDocuments(documents);
                string docsBase64 = string.Empty;
                foreach (Document document in documents)
                {
                    docsBase64 += document.Doc_encode;
                }
                string blockData = $"{block.Previous_Hash}{block.MiningDate}{block.Attempts}{docsBase64}";
                string blockHash = _sha256Hash.GenerateHash(blockData);
                if (block.Hash != blockHash) { 
                    changedBlocks.Add(block);
                }
            }
            return changedBlocks;

        }

        public async Task DecryptDocuments(List<Document> documents)
        {
            foreach (Document document in documents)
            {
                byte[] cipherDoc = Convert.FromBase64String(document.Doc_encode);
                document.Doc_encode = await _encryption.DecryptDocument(cipherDoc);
            }
        }

    }
}
