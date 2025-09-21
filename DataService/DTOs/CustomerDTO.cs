using AutoMapper;
using DataDomain.Interfaces.Domain;

namespace DataService.DTOs;

public class CustomerDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}


public class CustomerToCustomerDTOMappingProfile : Profile
{
    public CustomerToCustomerDTOMappingProfile()
    {
        CreateMap<CustomerEntity, CustomerDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));
    }
}


