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
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<NewOrderVM, Order>().ReverseMap();
            CreateMap<NewOrderVM, NewOrderDTO>();
            CreateMap<NewOrderItemVM, OrderItem>().ReverseMap();
            CreateMap<NewOrderItemVM,  OrderItemRequestDTO>().ReverseMap(); 
        }
    }
}
