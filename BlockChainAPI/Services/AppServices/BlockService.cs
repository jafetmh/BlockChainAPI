using BlockChain_DB;
using BlockChain_DB.General.Message;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Interfaces.IRepository;
using BlockChainAPI.Interfaces.IServices.IAppServices;
using BlockChainAPI.Interfaces.IServices.ICrypto.AES;
using BlockChainAPI.Interfaces.IServices.ICrypto.SHA256;
using BlockChainAPI.Utilities;
using BlockChainAPI.Utilities.ResponseMessage;
using System.Diagnostics;

namespace BlockChainAPI.Services.AppServices
{
    public class BlockService : IBlockService
    {
        private readonly BlockChainContext _blockChainContext;
        private readonly IUserRepository _userRepository;
        private readonly IBlockRepository _blockRepository;
        private readonly ISHA256Hash _sha256Hash;
        private readonly IChainRepository _chainRepository;
        private readonly IDocumentService _documentService;
        private readonly IMemPoolDocumentService _memPoolDocumentService;
        private readonly IAESEncryption _encryption;
        private readonly IConfigurationRepository _configurationRepository;
        private readonly Message _message;

        public BlockService(BlockChainContext blockChainContext,
                            IUserRepository userRepository,
                            IBlockRepository blockRepository,
                            ISHA256Hash sha256Hash, 
                            IChainRepository chainRepository,
                            IDocumentService documentService,
                            IMemPoolDocumentService memPoolDocumentService,
                            IAESEncryption encryption,
                            IConfigurationRepository configurationRepository,
                            MessageService messages) {
            _blockChainContext = blockChainContext;
            _userRepository = userRepository;
            _blockRepository = blockRepository;
            _sha256Hash = sha256Hash;
            _chainRepository = chainRepository;
            _documentService = documentService;
            _memPoolDocumentService = memPoolDocumentService;
            _encryption = encryption;
            _configurationRepository = configurationRepository;
            _message = messages.Get_Message();
        }

        public async Task<Response<Block>> StartMiningTask(int userId, List<Document> documents)
        {
            Response<SystemConfig> sysconfig = await _configurationRepository.GetMaxBlockDocuments();
            if (documents.Count > int.Parse(sysconfig.Data.Value)) return ResponseResult.CreateResponse<Block>(false, _message.InvalidMaxDocuments);
            return await Task.Run(() => BuildBlock(userId, documents));
        }

        public async Task<Response<Block>> BuildBlock(int userId, List<Document> documents)
        {
            var transaction = await _blockChainContext.Database.BeginTransactionAsync();
            try
            {
                Block block = new Block();
                Response<User> responseResult = await _userRepository.GetUser(userId);
                User user = responseResult.Data;
                if (user == null) { return ResponseResult.CreateResponse<Block>(false, _message.Failure.Set); }
                Chain chain = await _chainRepository.CreateChain(user.Id);
                block.Id = chain.Blocks.Count == 0? 1: (chain.Blocks.Last().Id + 1);
                block.ChainID = chain.Id;
                block.Previous_Hash = chain.Blocks.Count == 0 ? new string('0', 64) : chain.Blocks.Last().Hash;
                string docsBase64string = GetDocsBase64tring(documents);
                MiningBlock(block, user, docsBase64string);
                int entriesWriten = await _blockRepository.CreateBlock(block);
                if (entriesWriten <= 0) { return ResponseResult.CreateResponse<Block>(false, _message.Failure.Set); }
                List<MemPoolDocument> memPoolDocuments = documents.Select(MemPoolDocument.FromDocument).ToList();
                await _memPoolDocumentService.BulkDeleteMemPoolDocuments(memPoolDocuments);
                await _documentService.BulkCreateDocuments(user, documents, block);
                await transaction.CommitAsync();
                return ResponseResult.CreateResponse(true, _message.Success.Set, block);
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
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
                    break;
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

        //concat base64 of documnts
        public string GetDocsBase64tring(List<Document> documents)
        {
            string docs_base64String = string.Empty;
            foreach (Document document in documents)
            {
                docs_base64String += document.Doc_encode;
            }
            return docs_base64String;
        }

        public async Task<Response<List<Block>>> GetBlocks(int userId)
        {
            try
            {
                Response<List<Block>> blocks = await _blockRepository.GetBlocks(userId);
                if (!blocks.Success || blocks.Data.Count == 0) return blocks;
                Response<User> user = await _userRepository.GetUser(userId);
                _encryption.GetKeyAndIv(user.Data);
                foreach (Block block in blocks.Data)
                {
                    foreach (Document document in block.Documents)
                    {
                        byte[] cipherDoc = Convert.FromBase64String(document.Doc_encode);
                        document.Doc_encode = await _encryption.DecryptDocument(cipherDoc);
                    }
                }
                return blocks;
            }
            catch (Exception ex) { return ResponseResult.CreateResponse<List<Block>>(false, ex.Message); }

        }
    }
}
