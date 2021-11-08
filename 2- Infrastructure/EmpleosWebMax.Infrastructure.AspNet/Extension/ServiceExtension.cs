using EmpleosWebMax.Domain.Core.Interfaces;
using EmpleosWebMax.Infrastructure.Core;
using EmpleosWebMax.Infrastructure.Data.Data;
using EmpleosWebMax.Infrastructure.Interface.InterfaceService;
using EmpleosWebMax.Infrastructure.Interface.Module;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EmpleosWebMax.Infrastructure.AspNet.Extension
{
   public static class ServiceExtension
    {

        public static IServiceCollection AddService(this IServiceCollection services)
        {
            //services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(config.GetConnectionString("ERPConnection")), ServiceLifetime.Scoped);
            services.AddScoped<IEmpleoMaxUnitOfWork, UnitofWork>();
            // services.AddScoped(typeof(IBaseService), typeof(BaseService));
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IModuleSequenceService, ModuleSequenceService>();
            services.AddScoped<IPlanServiceService, PlanServiceService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<ITaxReceiptService, TaxReceiptService>();
            services.AddScoped<IInvoiceServices, InvoiceService>();
            //  Assembly.GetEntryAssembly().GetTypesAssignableFrom<IBaseService>().ForEach((t) => { services.AddScoped(typeof(IBaseService), t); });

            return services;
        }

        //public static List<Type> GetTypesAssignableFrom<T>(this Assembly assembly)
        //{
        //    return assembly.GetTypesAssignableFrom(typeof(T));
        //}
        //public static List<Type> GetTypesAssignableFrom(this Assembly assembly, Type compareType)
        //{
        //    List<Type> ret = new List<Type>();
        //    foreach (var type in assembly.DefinedTypes)
        //    {
        //        if (compareType.IsAssignableFrom(type) && compareType != type)
        //        {
        //            ret.Add(type);
        //        }
        //    }
        //    return ret;
        //}
    }
}
