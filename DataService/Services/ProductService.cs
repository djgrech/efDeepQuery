using AutoMapper;
using DataDomain;
using DataDomain.Interfaces;
using DataService.DTOs;

namespace DataService.Services;
public interface IProductService : IBaseService<ProductDTO, ProductEntity>;

public class ProductService(IProductRepository repository, IMapper mapper) 
    : BaseService<ProductDTO, ProductEntity>(repository, mapper), IProductService;
