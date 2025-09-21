using AutoMapper;
using DataDomain.Interfaces.Domain;

namespace DataService.DTOs;

public class ProductDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
}


public class ProductToProductDTOMappingProfile : Profile
{
    public ProductToProductDTOMappingProfile()
    {
        CreateMap<ProductEntity, ProductDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
    }
}


