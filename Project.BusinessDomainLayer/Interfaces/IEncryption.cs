namespace Project.BusinessDomainLayer.Interfaces
{
    public interface IEncryption
    {
        public Task<string> GenerateSaltedPassword();
        public Task<string> GenerateEncryptedPassword(string saltedPassword, string password);
        public Task<bool> ValidateEncryptedData(string valueToValidate, string hashValue, string salt);
    }
}
