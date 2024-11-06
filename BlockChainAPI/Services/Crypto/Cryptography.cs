﻿using BlockChain_DB;
using BlockChainAPI.Interfaces.ICrypto;
using System.Security.Cryptography;
using System.Text;

namespace BlockChainAPI.Services.Crypto
{
    public class Cryptography : ICryptography
    {

        //Encryptor
        public async Task<byte[]> Encrypt(string data, byte[] key, byte[] iv)
        {
            try
            {
                using Aes aes = Aes.Create();
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor();
                using MemoryStream memoryStream = new MemoryStream();
                using CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                using StreamWriter streamWriter = new StreamWriter(cryptoStream, Encoding.UTF8);

                await streamWriter.WriteAsync(data);
                await streamWriter.FlushAsync();
                cryptoStream.FlushFinalBlock();

                return memoryStream.ToArray();

            }
            catch(Exception exc) { 
                throw new CryptographicException("Error al cifrar: ", exc.Message);
            }
        }

        //Decryptor
        public async Task<string> Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            try
            {
                using Aes aes = Aes.Create();
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor();
                await using MemoryStream memoryStream = new(data);
                await using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
                using StreamReader reader = new(cryptoStream);

                return await reader.ReadToEndAsync();

            }
            catch (Exception exc) {
                throw new CryptographicException("Error al descrifrar: ", exc.Message);
            }
        }
    }
}