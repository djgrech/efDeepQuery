using AutoMapper;
using DataDomain.Interfaces.Domain;

namespace DataService.DTOs;

public class OrderDTO
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public DateTime OrderDate { get; set; }
    public int CustomerId { get; set; }
}


public class OrderToOrderDTOMappingProfile : Profile
{
    public OrderToOrderDTOMappingProfile()
    {
        CreateMap<OrderEntity, OrderDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId));
    }
}


