using AutoMapper;
using DataDomain.Interfaces;
using DataDomain.Interfaces.Domain;
using DataService.DTOs;
using EFDeepQueryDynamicLinq;

namespace DataService.Services;
public interface IProductService : IBaseService<ProductDTO, Product>;

public class ProductService(IProductRepository repository, IMapper mapper) 
    : BaseService<ProductDTO, Product>(repository, mapper), IProductService;
