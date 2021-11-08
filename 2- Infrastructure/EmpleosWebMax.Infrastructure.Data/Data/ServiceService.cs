using EmpleosWebMax.Common.Enum;
using EmpleosWebMax.Domain.Core.Result;
using EmpleosWebMax.Domain.Dtos;
using EmpleosWebMax.Domain.Entity;
using EmpleosWebMax.Infrastructure.Core;
using EmpleosWebMax.Infrastructure.Interface.InterfaceService;
using EmpleosWebMax.Infrastructure.Interface.Module;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpleosWebMax.Infrastructure.Data.Data
{
    public class ServiceService : BaseService, IServiceService
    {
        public ServiceService(IEmpleoMaxUnitOfWork empleoMaxUnitOfWork) : base(empleoMaxUnitOfWork)
        {

        }
        public async Task<ServiceResult> AddService(ServiceDto service)
        {
            if (service == null)
                return ServiceResult.ResultError("El parametro esta nulo");

            var addService = new Service
            {
                Name = service.Name,
                Category = service.Category,
                Code = service.Code,
                Description = service.Description,
                Price = service.Price,
                State = Common.Enum.State.Active,
                CreatedAt = service.CreatedAt,
                CreatedBy = service.CreatedBy
            };
            _unitOfWork.ServicesRepository.Add(addService);
            
            var result = await _unitOfWork.SaveChangesAsync();
            
            if (result == 1)
                return ServiceResult.ResultSuccess();

            return ServiceResult.ResultError("No se pudo guardar el registro");

        }

        public async Task<ServiceResult> DeleteService(Guid serviceId)
        {
            if (serviceId == null)
                return null;
            var result = await _unitOfWork.ServicesRepository.FirstOrDefaultAsync(x => x.Id == serviceId);

            if (result == null)
                return ServiceResult.ResultError("No Se encontro el registro a borrar");
            _unitOfWork.ServicesRepository.Delete(result);

            var exec = await _unitOfWork.SaveChangesAsync();

            if (exec < 1)
                return ServiceResult.ResultError("No se pudo eliminar el registro");
           
            return ServiceResult.ResultSuccess();
        }

        public async Task<ServiceResult<ServiceDto>> GetServiceById(Guid id)
        {
            if (id == null)
                return null;
            
            var result = await _unitOfWork.ServicesRepository.Query().Select(x => new ServiceDto {
            
                Id= x.Id,
                Code = x.Code,
                Category = x.Category,
                Description = x.Description,
                Name = x.Name,
                Price = x.Price,
                State= x.State
                    
            }).FirstOrDefaultAsync(x => x.Id == id);
            
            if (result != null)
                return ServiceResult<ServiceDto>.ResultSuccess(result);

            return null;
        }

        public async Task<ServiceResultList<ServiceDto>> GetServices()
        {
            var result = await _unitOfWork.ServicesRepository.Query().Select(x=> new ServiceDto { 
               Id = x.Id,
               Code = x.Code,
               Description = x.Description,
               Category = x.Category,
               Name = x.Name,
               Price = x.Price,
               State = x.State
            }).ToListAsync();

            return ServiceResultList<ServiceDto>.ResultSuccess(result);
        }

        public async Task<ServiceResultList<ServiceDto>> GetServicesByCategory(Category category)
        {
            var result = await _unitOfWork.ServicesRepository.Query().Where(x => x.Category == category).Select(x => new ServiceDto
            {
                Id = x.Id,
                Code = x.Code,
                Description = x.Description,
                Category = x.Category,
                Name = x.Name,
                Price = x.Price,
                State = x.State
            }).ToListAsync();

            return ServiceResultList<ServiceDto>.ResultSuccess(result);
        }

        public async Task<ServiceResult> UpdateService(ServiceDto service)
        {
            if (service == null)
                return ServiceResult.ResultError("El parametro esta nulo");

            var result = await _unitOfWork.ServicesRepository.Query().FirstOrDefaultAsync(x => x.Id == service.Id);

            if (result == null)
                return ServiceResult.ResultError("No se ha encontrado el registro");

            result.Name = service.Name;
            result.Description = service.Description;
            result.Category = service.Category;
            result.Code = service.Code;
            result.Price = service.Price;
            result.State = service.State;
            result.LastModifiedAt = service.LastModifiedAt;
            result.LastModifiedBy = service.LastModifiedBy;
           
            _unitOfWork.ServicesRepository.Update(result);

            if (await _unitOfWork.SaveChangesAsync() == 0)
                return ServiceResult.ResultError("No se pudo guardar el registro");

            return ServiceResult.ResultSuccess();
        }
    }
}
