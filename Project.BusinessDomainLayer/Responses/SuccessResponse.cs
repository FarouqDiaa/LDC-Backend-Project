
namespace Project.BusinessDomainLayer.Responses
{
    public class SuccessResponse<T> : BaseResponse where T : class
    {
        public T? Data { get; set; }
    }
}
