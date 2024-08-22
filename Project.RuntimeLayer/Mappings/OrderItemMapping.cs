using Project.BusinessDomainLayer.DTOs;
using Project.InfrastructureLayer.Entities;
using AutoMapper;

namespace Project.RuntimeLayer.Mappings
{
    public class OrderItemMappingProfile : Profile
    {
        public OrderItemMappingProfile()
        {
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<NewOrderItemDTO, OrderItem>();
        }
    }
}
