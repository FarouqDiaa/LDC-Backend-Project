
namespace Project.BusinessDomainLayer.Responses
{
    public class BaseResponse
    {
        public required int StatusCode { get; set; }
        public required string Message { get; set; }
    }
}
