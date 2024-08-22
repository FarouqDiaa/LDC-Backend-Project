using Project.BusinessDomainLayer.DTOs;
using Project.InfrastructureLayer.Entities;
using AutoMapper;

namespace Project.RuntimeLayer.Mappings
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<NewOrderDTO, Order>();
        }
    }
}
