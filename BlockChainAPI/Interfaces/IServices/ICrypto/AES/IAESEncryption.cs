using BlockChain_DB;

namespace BlockChainAPI.Interfaces.IServices.ICrypto.AES
{
    public interface IAESEncryption
    {
        public void GetKeyAndIv(User user);
        public Task<string> EncryptDocument(string data);
        public Task<string> DecryptDocument(byte[] data);
    }
}
