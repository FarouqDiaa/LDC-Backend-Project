using Project.BusinessDomainLayer.DTOs;
using Project.InfrastructureLayer.Entities;
using AutoMapper;

namespace Project.RuntimeLayer.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<NewProductDTO, Product>();
        }
    }
}
