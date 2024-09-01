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
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<OrderItem, NewOrderItemDTO>().ReverseMap();
            CreateMap<OrderVM, Order>().ReverseMap();
            CreateMap<OrderVM, NewOrderDTO>();
            CreateMap<OrderItemVM, OrderItem>().ReverseMap();
            CreateMap<OrderItemVM,  OrderItemDTO>().ReverseMap(); 
        }
    }
}
