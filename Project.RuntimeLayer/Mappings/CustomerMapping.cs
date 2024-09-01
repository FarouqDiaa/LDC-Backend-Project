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
            CreateMap<CustomerVM, Customer>();
            CreateMap<CustomerVM, NewCustomerDTO>();
            CreateMap<CustomerVM, CustomerDTO>();
            CreateMap<Customer, CustomerResVM>();
            CreateMap<NewCustomerDTO, Customer>();
            CreateMap<CustomerDTO, CustomerResVM>();
            CreateMap<LoginVM, LoginDTO>();
        }
    }
}
