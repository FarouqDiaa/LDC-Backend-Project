using Project.BusinessDomainLayer.DTOs;
using Project.InfrastructureLayer.Entities;
using AutoMapper;
using Project.BusinessDomainLayer.VMs;

namespace Project.RuntimeLayer.Mappings
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<OrderVM, Order>().ReverseMap();
            CreateMap<OrderVM, NewOrderDTO>();
            CreateMap<Order, OrderDTO>()
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems)).ReverseMap();
            CreateMap<OrderDTO, OrderResVM>();
            CreateMap<NewOrderDTO, Order>()
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId));
            CreateMap<OrderItemVM, OrderItem>().ReverseMap();
            CreateMap<OrderItemVM, OrderItemDTO>().ReverseMap();
            CreateMap<NewOrderItemDTO, OrderItem>()
            .ForMember(dest => dest.OrderitemId, opt => opt.MapFrom(src => src.Id));
            CreateMap<OrderItem, NewOrderItemDTO>().ReverseMap();
            CreateMap<OrderItemVM, NewOrderItemDTO>();
            CreateMap<OrderItem, OrderItemDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OrderitemId));
            CreateMap<OrderItemDTO, OrderItemResVM>();
        }
    }
}
