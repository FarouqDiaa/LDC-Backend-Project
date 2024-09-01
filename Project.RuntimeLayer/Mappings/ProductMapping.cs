using Project.BusinessDomainLayer.DTOs;
using Project.InfrastructureLayer.Entities;
using AutoMapper;
using Project.BusinessDomainLayer.VMs;

namespace Project.RuntimeLayer.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<NewProductDTO, Product>();
            CreateMap<ProductVM, Product>();
            CreateMap<IEnumerable<ProductDTO>, IEnumerable<ProductResVM>>();
            CreateMap<IEnumerable<Product>, IEnumerable<ProductDTO>>();
            CreateMap<ProductVM, UpdateProductDTO>();
            CreateMap<ProductVM, NewProductDTO>()
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Amount));
            CreateMap<ProductDTO, ProductResVM>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Cost));
        }
    }
}
