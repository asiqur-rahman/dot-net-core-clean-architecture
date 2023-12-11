using Microsoft.Extensions.DependencyInjection.Extensions;
using Project.App.Hubs;

namespace Project.App.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection RegisterService(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #region Program
            services.AddSingleton<List<StreamingUser>>();
            services.AddSingleton<List<UserCall>>();
            services.AddSingleton<List<CallOffer>>();
            #endregion

            //#region Repositories
            //services.AddTransient<IProductRepository, ProductRepository>();
            //services.AddTransient<ICustomerRepository, CustomerRepository>();
            //services.AddTransient<ICustomerFingerRepository, CustomerFingerRepository>();

            //#endregion

            //#region Mapper
            //var configuration = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<Product, ProductViewModel>();
            //    cfg.CreateMap<ProductViewModel, Product>();

            //    cfg.CreateMap<Customer, CustomerViewModel>();
            //    cfg.CreateMap<CustomerViewModel, Customer>();
            //});

            //IMapper mapper = configuration.CreateMapper();

            //services.AddSingleton<IBaseMapper<Product, ProductViewModel>>(new BaseMapper<Product, ProductViewModel>(mapper));
            //services.AddSingleton<IBaseMapper<ProductViewModel, Product>>(new BaseMapper<ProductViewModel, Product>(mapper));

            //services.AddSingleton<IBaseMapper<Customer, CustomerViewModel>>(new BaseMapper<Customer, CustomerViewModel>(mapper));
            //services.AddSingleton<IBaseMapper<CustomerViewModel, Customer>>(new BaseMapper<CustomerViewModel, Customer>(mapper));

            //#endregion

            return services;
        }
    }
}
