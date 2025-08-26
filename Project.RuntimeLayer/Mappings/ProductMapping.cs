using Project.BusinessDomainLayer.DTOs;
using Project.InfrastructureLayer.Entities;
using AutoMapper;
using Project.BusinessDomainLayer.VMs;
using Project.BusinessDomainLayer.VMs.ProductVMs;

namespace Project.RuntimeLayer.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<NewProductDTO, Product>();
            CreateMap<ProductVM, Product>();
            CreateMap<ProductVM, UpdateProductDTO>()
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Amount));
            CreateMap<UpdateProductDTO, Product>();
            CreateMap<ProductVM, NewProductDTO>()
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Amount));
            CreateMap<ProductDTO, ProductResVM>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Cost));

            CreateMap<ProductImage, ProductImageDTO>();
            CreateMap<ProductImage, ProductImageVM>();

            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages));

            CreateMap<Product, ProductVM>()
                .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages));
        }
    }
}
