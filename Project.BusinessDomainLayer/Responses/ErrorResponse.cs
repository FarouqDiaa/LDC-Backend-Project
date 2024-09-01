
namespace Project.BusinessDomainLayer.Responses
{
    public class ErrorResponse : BaseResponse
    {
        public Dictionary<string, string[]>? Errors { get; set; } = [];
    }
}
