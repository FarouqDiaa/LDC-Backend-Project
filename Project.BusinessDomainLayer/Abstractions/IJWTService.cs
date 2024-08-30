
namespace Project.BusinessDomainLayer.Abstractions
{
    public interface IJWTService
    {
        public string GenerateToken(Guid userId, string email, bool isAdmin);
    }
}
