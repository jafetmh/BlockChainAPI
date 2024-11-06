﻿using BlockChain_DB;

namespace BlockChainAPI.Interfaces.ICrypto
{
    public interface ICryptography
    {
        public Task<byte[]> Encrypt(string data, byte[] key, byte[] iv);
        public Task<string> Decrypt(byte[] data, byte[] key, byte[] iv);

    }
}