using BlockChain_DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using BlockChainAPI.Interfaces.ICrypto;
using BlockChainAPI.Interfaces.IMining;

namespace BlockChainAPI.Services.Mining
{
    public class MiningService : IMiningService
    {
        private readonly BlockChainContext _context;
        private readonly ICryptography _cryptography;

        public MiningService(BlockChainContext context, ICryptography cryptography)
        {
            _context = context;
            _cryptography = cryptography;
        }

        public async Task MineBlockAsync(int blockId, int requiredZeros)
        {
            var block = await _context.Blocks
                                       .Include(b => b.Documents)
                                       .FirstOrDefaultAsync(b => b.Id == blockId);

            if (block == null)
                throw new Exception("Block not found");

            string validation = new string('0', requiredZeros);  // Create  "0000"
            bool isMined = false;
            int attempts = 0;
            DateTime miningStartDate = DateTime.Now;
            long milliseconds = 0;

            block.MiningDate = miningStartDate;
            block.Attempts = attempts;

            // Keys and IV for AES encryption (ensure these are securely managed)
            byte[] key = Encoding.UTF8.GetBytes("YourSecretKey12345");  // AES key 
            byte[] iv = Encoding.UTF8.GetBytes("YourIVInitializationVector");  // AES IV 

            // Start mining loop
            while (!isMined)
            {
                // Create hash string for mining (not encrypted yet)
                var blockData = $"{block.Previous_Hash}{block.MiningDate}{block.Attempts}{block.ChainID}{string.Join("", block.Documents.Select(d => d.BlockID))}";//hashing the block data
                string newHash = await GenerateHash(blockData); // Generate hash 

                // Check if the hash starts with the required number of zeros
                if (newHash.StartsWith(validation))
                {
                    isMined = true;
                    block.Hash = newHash;

                    // Encrypt the hash before storing 
                    byte[] encryptedHash = await _cryptography.Encrypt(newHash, key, iv);

                   // block.Doc_encode

                    block.Milliseconds = (int)milliseconds;
                }
                else
                {
                    // Increment the attempts
                    block.Attempts++;
                    block.MiningDate = DateTime.Now;

                    // Update milliseconds
                    milliseconds = (long)(DateTime.Now - miningStartDate).TotalMilliseconds;
                }
            }

        
            await _context.SaveChangesAsync();
        }

        private async Task<string> GenerateHash(string data)
        {
            // Use the cryptography service 
            var key = Encoding.UTF8.GetBytes("YourSecretKey12345");
            var iv = Encoding.UTF8.GetBytes("YourInitializationVector12345");

            byte[] encryptedData = await _cryptography.Encrypt(data, key, iv);
            
        
            return BitConverter.ToString(encryptedData).Replace("-", "").ToLower();
        }
    }
}
