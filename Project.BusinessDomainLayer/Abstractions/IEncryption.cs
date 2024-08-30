namespace Project.BusinessDomainLayer.Abstractions
{
    public interface IEncryption
    {
        public string GenerateSaltedPassword();
        public string GenerateEncryptedPassword(string saltedPassword, string password);
        public bool ValidateEncryptedData(string valueToValidate, string hashValue, string salt);
    }
}
