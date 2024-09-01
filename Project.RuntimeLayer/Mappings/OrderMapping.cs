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
            CreateMap<OrderItem, OrderItemDTO>();
            CreateMap<OrderDTO, OrderResVM>();
        }
    }
}
