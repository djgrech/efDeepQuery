using AutoMapper;
using DataDomain.Interfaces;
using DataDomain.Interfaces.Domain;
using DataService.DTOs;
using EFDeepQueryDynamicLinq;

namespace DataService.Services;
public interface IProductService : IBaseService<ProductDTO, ProductEntity>;

public class ProductService(IProductRepository repository, IMapper mapper) 
    : BaseService<ProductDTO, ProductEntity>(repository, mapper), IProductService;
