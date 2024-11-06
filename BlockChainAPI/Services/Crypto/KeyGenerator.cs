using System.Security.Cryptography;

namespace BlockChainAPI.Services.Crypto
{
    public class KeyGenerator
    {
        public static void DeriveKeyIv(string password, byte[] salt, out byte[] key, out byte[] iv)
        {
            int iterations = 10000;
            using (Rfc2898DeriveBytes derive = new Rfc2898DeriveBytes(password, salt, iterations)) {
                key = derive.GetBytes(16);
                iv = derive.GetBytes(16);
            }
        }
    }
}
