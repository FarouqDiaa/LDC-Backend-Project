namespace Project.BusinessDomainLayer.DTOs
{
    public class CategoryDTO
    {
        public string Type { get; set; } = string.Empty;
        public int Count { get; set; }
        public string? SampleImageUrl { get; set; }
    }
}
