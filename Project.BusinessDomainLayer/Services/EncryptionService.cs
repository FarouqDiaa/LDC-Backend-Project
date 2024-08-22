﻿using Project.BusinessDomainLayer.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Project.BusinessDomainLayer.Services
{
    public class EncryptionService : IEncryption
    {
        public async Task<string> GenerateSaltedPassword()
        {
            var crypto = new RNGCryptoServiceProvider();
            byte[] salt = new byte[32];
            crypto.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }

        public async Task<string> GenerateEncryptedPassword(string saltedPassword, string password)
        {
            byte[] newPassword = Encoding.UTF8.GetBytes(saltedPassword + password);
            SHA256Managed hashstr = new SHA256Managed();
            byte[] hash = hashstr.ComputeHash(newPassword);
            return Convert.ToBase64String(hash);
        }

        public async Task<bool> ValidateEncryptedData(string valueToValidate, string hashValue, string salt)
        {
            byte[] saltedValue = Encoding.UTF8.GetBytes(salt + valueToValidate);
            SHA256Managed hashstr = new SHA256Managed();
            byte[] hash = hashstr.ComputeHash(saltedValue);
            string enteredValueToValidate = Convert.ToBase64String(hash);
            return hashValue.Equals(enteredValueToValidate);
        }
    }
}
