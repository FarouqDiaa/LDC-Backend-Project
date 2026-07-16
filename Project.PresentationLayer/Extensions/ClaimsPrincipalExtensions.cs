using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Project.PresentationLayer.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid? GetCustomerId(this ClaimsPrincipal user)
        {
            var value = user.FindFirstValue(JwtRegisteredClaimNames.Sub);
            return Guid.TryParse(value, out var customerId) ? customerId : null;
        }
    }
}
