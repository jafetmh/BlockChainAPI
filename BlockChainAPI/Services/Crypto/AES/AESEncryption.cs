using BlockChain_DB;
using BlockChainAPI.Interfaces.IServices.ICrypto.AES;

namespace BlockChainAPI.Services.Crypto.AES
{
    public class AESEncryption : IAESEncryption
    {
        private byte[] key;
        private byte[] iv;
        private readonly ICryptography _cyptography;

        public AESEncryption(ICryptography cyptography)
        {
            _cyptography = cyptography;
        }
        public async Task<string> EncryptDocument(string data)
        {
            byte[] encryptedData = await _cyptography.Encrypt(data, key, iv);
            return Convert.ToBase64String(encryptedData);
        }

        public async Task<string> DecryptDocument(byte[] data)
        {
            return await _cyptography.Decrypt(data, key, iv);
        }

        public void GetKeyAndIv(User user)
        {
            KeyGenerator.DeriveKeyIv(user.Password, user.Salt, out key, out iv);
        }
    }
}
