using AutoMapper;
using Project.BusinessDomainLayer.DTOs;
using Project.InfrastructureLayer.Entities;

namespace Project.RuntimeLayer.Mappings
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<NewCustomerDTO, Customer>();
        }
    }
}
