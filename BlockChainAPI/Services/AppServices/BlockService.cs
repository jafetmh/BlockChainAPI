using BlockChain_DB;
using BlockChain_DB.General.Message;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Interfaces.IRepository;
using BlockChainAPI.Interfaces.IServices.IAppServices;
using BlockChainAPI.Interfaces.IServices.ICrypto;
using BlockChainAPI.Services.Crypto;
using BlockChainAPI.Utilities;
using BlockChainAPI.Utilities.ResponseMessage;

namespace BlockChainAPI.Services.AppServices
{
    public class BlockService : IBlockService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBlockRepository _blockRepository;
        private readonly ICryptography _cryptography;
        private readonly IChainRepository _chainRepository;
        private readonly IDocumentService _documentService;
        private readonly Message _message;

        public BlockService(IUserRepository userRepository,
                            IBlockRepository blockRepository, 
                            ICryptography cryptography, 
                            IChainRepository chainRepository,
                            IDocumentService documentService,
                            MessageService messages) {
            _userRepository = userRepository;
            _blockRepository = blockRepository;
            _cryptography = cryptography;
            _chainRepository = chainRepository;
            _documentService = documentService;
            _message = messages.Get_Message();
        }

        public async Task<Response<Block>> BuildBlock(int userId, List<Document> documents)
        {

            try
            {
                Block block = new Block();
                Response<User> responseResult = await _userRepository.GetUser(userId);
                User user = responseResult.Data;
                if (user == null) { return ResponseResult.CreateResponse<Block>(false, _message.Failure.Set); }
                Chain chain = await _chainRepository.GetUserChain(user.Id);
                if (chain.Blocks.Count == 0)
                {
                    block.Previous_Hash = new string('0', 64);
                }
                else
                {
                    block.Previous_Hash = chain.Blocks.Last().Hash;
                }
                block.ChainID = chain.Id;
                string docsBase64string = GetDocsBase64tring(documents);
                await MineBlock(block, user, docsBase64string);
                int entriesWriten = await _blockRepository.CreateBlock(block);

                if (entriesWriten <= 0) { return ResponseResult.CreateResponse<Block>(false, _message.Failure.Set); }
                Response<Document> result = await _documentService.BulkCreateDocuments(user.Id, documents, block);
                return ResponseResult.CreateResponse(true, _message.Success.Set, block);
            }
            catch(Exception ex)
            {
                return ResponseResult.CreateResponse<Block>(false, _message.Failure.Set);
            }

        }

        public async Task MineBlock(Block block, User user, string docsBase64)
        {
            int attempts = 0;
            string validation = "0000";
            bool isMined = false;
            DateTime miningStartDate = DateTime.Now;

            while (!isMined)
            {
                string blockData = $"{block.Previous_Hash}{block.MiningDate}{block.Attempts}{docsBase64}";
                string hash = await GenerateHash(blockData, user);
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
            block.MiningDate = miningStartDate;
            block.Attempts = attempts;
        }

        public async Task<string> GenerateHash(string data, User user)
        {
            KeyGenerator.DeriveKeyIv(user.Password, user.Salt, out byte[] key, out byte[] iv);
            byte[] encryptedData = await _cryptography.Encrypt(data, key, iv);
            return BitConverter.ToString(encryptedData).Replace("-", "").ToLower();
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
    }
}
