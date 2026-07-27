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
            // Image maps must be declared before the product maps that use them.
            CreateMap<ProductImage, ProductImageDTO>()
                .ForMember(dest => dest.ProductImageId, opt => opt.MapFrom(src => src.ProductimageId));
            CreateMap<ProductImage, ProductImageVM>()
                .ForMember(dest => dest.ProductImageId, opt => opt.MapFrom(src => src.ProductimageId));
            CreateMap<ProductImageDTO, ProductImageVM>();
            CreateMap<ProductImageVM, ProductImageDTO>();
            CreateMap<ProductImageVM, ProductImage>()
                .ForMember(dest => dest.ProductimageId, opt => opt.MapFrom(src => src.ProductImageId));
            CreateMap<ProductImageDTO, ProductImage>()
                // Incoming payloads only carry a Url, so mint ids server-side.
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.ProductimageId, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.ProductId, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages))
                // Surface the cover url on its own so listings don't have to dig
                // through the gallery; fall back to the first image for products
                // created before covers existed.
                .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom(src =>
                    src.ProductImages.Where(i => i.IsCover).Select(i => i.Url).FirstOrDefault()
                    ?? src.ProductImages.Select(i => i.Url).FirstOrDefault()))
                .ReverseMap()
                .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages));

            CreateMap<Product, ProductVM>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Cost))
                .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom(src =>
                    src.ProductImages.Where(i => i.IsCover).Select(i => i.Url).FirstOrDefault()))
                .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages));

            CreateMap<NewProductDTO, Product>()
                .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages));
            // Images are left alone on update: overwriting the tracked collection
            // orphans the existing rows. Replacing them needs explicit handling
            // in ProductService.
            CreateMap<UpdateProductDTO, Product>()
                .ForMember(dest => dest.ProductImages, opt => opt.Ignore());

            CreateMap<ProductVM, Product>()
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Amount));
            CreateMap<ProductVM, UpdateProductDTO>()
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Amount));
            CreateMap<ProductVM, NewProductDTO>()
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Amount));

            CreateMap<ProductDTO, ProductResVM>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Cost))
                .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages));

            CreateMap<CategoryDTO, CategoryResVM>();
        }
    }
}
