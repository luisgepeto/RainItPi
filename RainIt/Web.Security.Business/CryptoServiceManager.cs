
using System;
using System.Security.Cryptography;
using Web.Security.Interfaces;

namespace Web.Security.Business
{
    public class CryptoServiceManager : ICryptoServiceManager
    {
        public string CreateRandomSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[16];
            rng.GetBytes(buff);
            var currentSalt = Convert.ToBase64String(buff);
            return currentSalt;
        }

        public string GetHashFrom(string concatenatedPass)
        {
            HashAlgorithm hashAlg = new SHA256CryptoServiceProvider(); 
            byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(concatenatedPass);
            byte[] bytHash = hashAlg.ComputeHash(bytValue);
            string base64 = Convert.ToBase64String(bytHash);
            return base64;
        }
    }
}
