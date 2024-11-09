using BlockChain_DB;
using BlockChain_DB.General.Message;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Interfaces.IServices.ICrypto;
using BlockChainAPI.Services.Crypto;
using BlockChainAPI.Utilities;
using BlockChainAPI.Utilities.ResponseMessage;
using Microsoft.EntityFrameworkCore;

namespace BlockChainAPI.Repository
{
    public class BlockService : IBlockService
    {
        private readonly BlockChainContext _context;
        private readonly ICryptography _cryptography;
        private readonly Message message;

        public BlockService(BlockChainContext context, ICryptography cryptography, MessageService messages)
        {
            _context = context;
            _cryptography = cryptography;
            message = messages.Get_Message();
        }

        public async Task<Response<Block>> BuildBlock(User user, List<Document> documents)
        {

            Block block = new Block();
            Chain chain = await GetUserChain(user.Id);
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
            _context.Blocks.Add(block);
            int entries = await _context.SaveChangesAsync();
            if (entries > 0) { return ResponseResult.CreateResponse(true, message.Success.Set, block); }
            return ResponseResult.CreateResponse<Block>(false, message.Failure.Set);
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

        //concat base64 of documnets
        public string GetDocsBase64tring(List<Document> documents)
        {
            string docs_base64String = string.Empty;
            foreach (Document document in documents)
            {
                docs_base64String += document.Doc_encode;
            }
            return docs_base64String;
        }

        //Get or create a user chain
        public async Task<Chain> GetUserChain(int userId)
        {
            try
            {
                Chain chain = await _context.Chains
                    .Include(ch => ch.Blocks)
                    .FirstOrDefaultAsync(ch => ch.UserID == userId);

                if (chain == null)
                {
                    chain = new Chain { UserID = userId };
                    _context.Chains.Add(chain);
                    await _context.SaveChangesAsync();
                }
                return chain;
            }
            catch { throw; }
        }
    }
}
