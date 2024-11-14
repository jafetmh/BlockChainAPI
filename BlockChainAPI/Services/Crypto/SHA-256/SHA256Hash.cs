
using BlockChainAPI.Interfaces.IServices.ICrypto.SHA256;
using System.Security.Cryptography;
using System.Text;

namespace BlockChainAPI.Services.Crypto.SHA_256
{
    public class SHA256Hash : ISHA256Hash
    {
        public string GenerateHash(string data)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] bytes = sha256.ComputeHash(dataBytes);
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
