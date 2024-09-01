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
            CreateMap<OrderItem, NewOrderItemDTO>().ReverseMap();
            CreateMap<OrderVM, Order>().ReverseMap();
            CreateMap<OrderVM, NewOrderDTO>();
            CreateMap<OrderItemVM, OrderItem>().ReverseMap();
            CreateMap<OrderItemVM,  OrderItemDTO>().ReverseMap(); 
            CreateMap<Order, OrderDTO>()
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems)).ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OrderitemId));
            CreateMap<OrderItemDTO, OrderItemResVM>();
            CreateMap<OrderDTO, OrderResVM>();
            CreateMap<OrderItemVM, NewOrderItemDTO>();
            CreateMap<NewOrderDTO, Order>()
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId));
            CreateMap<NewOrderItemDTO, OrderItem>()
            .ForMember(dest => dest.OrderitemId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
