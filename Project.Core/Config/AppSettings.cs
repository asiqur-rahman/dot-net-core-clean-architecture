using AutoMapper;
using Project.Core.Entities.Common.User;
using Project.Core.Entities.Common.User.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Config
{
    public class AppSettings
    {
        public static AppSettings Current;

        public AppSettings()
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                FilePath = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Uploads";
            }

            Current = this;
        }

        public bool TestingMode { get; set; }
        public string FilePath { get; set; }
        public string LogPath { get; set; }
        public string EzyBankBaseUrl { get; set; }
        public string WebApiBaseUrl { get; set; }
        public bool EcFingerVerify { get; set; }
        public string ReportingBaseUrl { get; set; }
        public string EcBaseUrl { get; set; }
        public string EcLoginUser { get; set; }
        public string EcLoginPass { get; set; }
        public string PadmaBaseUrl { get; set; }
        public bool PadmaVpn { get; set; }
        public bool MacBindings { get; set; }
        public bool CrawlingEnable { get; set; }
        public MgblSettings MgblSettings { get; set; }
        public MMBLSettings MMBLSettings { get; set; }
        public string DatabaseType { get; set; }
        public bool RemittanceVerifyThroughCrawling { get; set; }
        public bool AuditLogEnabled { get; set; }
    }
    public class CustomerVerifyApi
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string ApiBaseUrl { get; set; }
        public bool Connectivity { get; set; }
        public string Percentage { get; set; }
    }

    public class MgblSettings
    {
        public string BaseUrl { get; set; }
        public string SoupUrl { get; set; }
        public bool Vpn { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string LocalApiUrl { get; set; }
    }

    public class MMBLSettings
    {
        public string BHATA_Url { get; set; }
        public string BHATA_API_Key { get; set; }
        public bool BHATA_Cust_Finger { get; set; }
        public string RTGS_Url { get; set; }
        public string EFT_Url { get; set; }
        public string BaseUrl { get; set; }
        public string CbsBaseAddress { get; set; }
        public string SoupUrl { get; set; }
        public bool Vpn { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string LocalApiUrl { get; set; }
        public bool UNDP_DirectAccOpenWithCBS { get; set; }

        public bool CbsConnectivity { get; set; }
        public string AmlBaseAddress { get; set; }
        public bool AmlConnectivity { get; set; }
        public bool DeDupCheck { get; set; }

    }

    public class PayWellSettings
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string TokenCreationUsername { get; set; }
        public string TokenCreationPassword { get; set; }
        public string ApiKey { get; set; }
        public string EncryptionKey { get; set; }
        public string BaseUrl { get; set; }
        public string PublicKey { get; set; }
        public bool Connection { get; set; }

    }

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //AllowNullDestinationValues = true;
            // Add as many of these lines as you need to map your objects
            CreateMap<User, UserDto>();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap(typeof(Source<>), typeof(Destination<>));
            CreateMap(typeof(Source<>), typeof(Destination<>)).ReverseMap();
        }
    }
    public class Source<T>
    {
        public T Value { get; set; }
    }

    public class Destination<T>
    {
        public T Value { get; set; }
    }

    public static class Extensions
    {
        public static void IgnoreSourceWhenDefault<TSource, TDestination>(this IMemberConfigurationExpression<TSource, TDestination, object> opt)
        {
            var destinationType = opt.DestinationMember.GetMemberType();
            object defaultValue = destinationType.GetTypeInfo().IsValueType
                ? Activator.CreateInstance(destinationType)
                : null;
            opt.Condition((src, dest, srcValue) => !Equals(srcValue, defaultValue));
        }

        public static Type GetMemberType(this MemberInfo memberInfo)
        {
            if (memberInfo is MethodInfo)
                return ((MethodInfo)memberInfo).ReturnType;
            if (memberInfo is PropertyInfo)
                return ((PropertyInfo)memberInfo).PropertyType;
            if (memberInfo is FieldInfo)
                return ((FieldInfo)memberInfo).FieldType;
            return null;
        }

        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>
            (this IMappingExpression<TSource, TDestination> expression)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            var sourceType = typeof(TSource);
            var destinationProperties = typeof(TDestination).GetProperties(flags);

            foreach (var property in destinationProperties)
            {
                if (sourceType.GetProperty(property.Name, flags) == null)
                {
                    expression.ForMember(property.Name, opt => opt.Ignore());
                }
            }
            return expression;
        }
    }


    //public abstract class BaseEntityMapperViewModel<TViewModel, TEntity>
    //{
    //    /// <summary>
    //    /// Initializes the <see cref="BaseEntityMapperViewModel{TViewModel,TEntity}"/> class.
    //    /// </summary>
    //    static BaseEntityMapperViewModel()
    //    {
    //        Mapper.Initialize(cfg =>
    //        {
    //            cfg.ValidateInlineMaps = false;
    //            cfg.CreateMap(typeof(Source<TViewModel>), typeof(Destination<TEntity>)).ReverseMap();
    //        });
    //        // Define the default mapping, 
    //        // custom configuration can be also defined and will be merged with this one
    //    }

    //    /// <summary>
    //    /// Maps the specified view model to a entity object.
    //    /// </summary>
    //    public TEntity MapToEntity()
    //    {
    //        // Map the derived class to the represented view model

    //        return Mapper.Map<TEntity>(CastToDerivedClass(this));
    //    }

    //    /// <summary>
    //    /// Maps the specified view model to list entity object.
    //    /// </summary>
    //    /// <param name="viewModels"></param>
    //    /// <returns></returns>
    //    public static List<TEntity> MapToListEntity(List<TViewModel> viewModels)
    //    {
    //        return Mapper.Map<List<TEntity>>(viewModels);
    //    }

    //    /// <summary>
    //    /// Maps a entity to a view model instance.
    //    /// </summary>
    //    public static TViewModel MapFromEntity(TEntity model)
    //    {
    //        return Mapper.Map<TViewModel>(model);
    //    }

    //    /// <summary>
    //    /// Maps list entity to list view model instance
    //    /// </summary>
    //    /// <param name="model"></param>
    //    /// <returns></returns>
    //    public static List<TViewModel> MapFromListEntity(List<TEntity> model)
    //    {
    //        return Mapper.Map<List<TViewModel>>(model);
    //    }

    //    /// <summary>
    //    /// Gets the derived class.
    //    /// </summary>
    //    private static TViewModel CastToDerivedClass(BaseEntityMapperViewModel<TViewModel, TEntity> baseInstance)
    //    {
    //        return Mapper.Map<TViewModel>(baseInstance);
    //    }
    //}
}
