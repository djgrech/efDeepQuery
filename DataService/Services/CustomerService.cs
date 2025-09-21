using AutoMapper;
using DataDomain.Interfaces;
using DataDomain.Interfaces.Domain;
using DataService.DTOs;
using EFDeepQueryDynamicLinq;

namespace DataService.Services;

public interface ICustomerService : IBaseService<CustomerDTO, Customer>;

public class CustomerService(ICustomerRepository repository, IMapper mapper)
    : BaseService<CustomerDTO, Customer>(repository, mapper), ICustomerService;
