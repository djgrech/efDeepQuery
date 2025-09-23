using AutoMapper;
using DataDomain.Interfaces;
using DataDomain.Interfaces.Domain;
using DataService.DTOs;

namespace DataService.Services;

public interface ICustomerService : IBaseService<CustomerDTO, CustomerEntity>;

public class CustomerService(ICustomerRepository repository, IMapper mapper)
    : BaseService<CustomerDTO, CustomerEntity>(repository, mapper), ICustomerService;
