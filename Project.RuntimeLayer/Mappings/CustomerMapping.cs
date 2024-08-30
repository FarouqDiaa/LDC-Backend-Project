using AutoMapper;
using Project.BusinessDomainLayer.DTOs;
using Project.BusinessDomainLayer.VMs;
using Project.InfrastructureLayer.Entities;

namespace Project.RuntimeLayer.Mappings
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<NewCustomerVM, Customer>();
            CreateMap<NewCustomerVM, NewCustomerDTO>();
            CreateMap<NewCustomerVM, CustomerDTO>();
        }
    }
}
