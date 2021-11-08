using EmpleosWebMax.Domain.Core.Interfaces;
using EmpleosWebMax.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EmpleosWebMax.Domain.Dtos;
using EmpleosWebMax.Common.Enum;
using EmpleosWebMax.Domain.Core.Result;

namespace EmpleosWebMax.Infrastructure.Interface.InterfaceService
{
    public interface IServiceService : IBaseService
    {
        Task<ServiceResultList<ServiceDto>> GetServices();
        Task<ServiceResultList<ServiceDto>> GetServicesByCategory(Category category);
        Task<ServiceResult<ServiceDto>> GetServiceById(Guid id);
        Task<ServiceResult> AddService(ServiceDto  service);
        Task<ServiceResult> UpdateService(ServiceDto service);
        Task<ServiceResult> DeleteService(Guid serviceId);
    }
}
