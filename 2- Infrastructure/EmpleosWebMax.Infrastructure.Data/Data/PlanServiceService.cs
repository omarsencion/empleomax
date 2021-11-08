using EmpleosWebMax.Common.Enum;
using EmpleosWebMax.Domain.Core.Result;
using EmpleosWebMax.Domain.Dtos;
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
    public class PlanServiceService : BaseService, IPlanServiceService
    {
        public PlanServiceService(IEmpleoMaxUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public async Task<ServiceResult> AddPlanService(PlanServiceDto serviceDto)
        {
            if (serviceDto == null)
                return ServiceResult.ResultError("El parametro recibido es nulo");
            var isExist = await _unitOfWork.PlanServicesRepository.AnyAsync(x => x.PlanId == serviceDto.PlanId && x.ServiceId == serviceDto.ServicesId);
            if (isExist)
                return ServiceResult.ResultError("El servicio Existe en el plan");
            var PlanService = new EmpleosWebMax.Domain.Entity.PlanService
            {
                PlanId = serviceDto.PlanId,
                ServiceId = serviceDto.ServicesId,
                CreatedBy = serviceDto.CreatedBy,
                CreatedAt = serviceDto.CreatedAt
            };

            _unitOfWork.PlanServicesRepository.Add(PlanService);

            var result = await _unitOfWork.SaveChangesAsync();
            
            if (result == 1) 
            {
                return  ServiceResult.ResultSuccess();
            }
            return  ServiceResult.ResultError("No se pudo guardar el registro"); 
            
        }

        public async Task<ServiceResult> DeletePlanService(PlanServiceDto serviceDto)
        {
            if (serviceDto == null)
                return  ServiceResult.ResultError("El parametro recibido es nulo"); ;
            var result = await _unitOfWork.PlanServicesRepository.FirstOrDefaultAsync(x => x.Id == serviceDto.Id);
            
            if (result == null)
                return ServiceResult.ResultError("No se encontro el registro"); ;

            _unitOfWork.PlanServicesRepository.Delete(result);

            if (await _unitOfWork.SaveChangesAsync() == 1)
            {
                return ServiceResult.ResultSuccess();
            }

            return ServiceResult.ResultError("No se pudo guardar el registro");
        }

        public async Task<ServiceResultList<PlanServiceSubscriptionDto>> GetAll(Category category)
        {
            var result = await _unitOfWork.PlanServicesRepository.Query().Include(x => x.Plans).Include(x => x.Services).Where(x => x.Plans.Category == category).ToListAsync();
            var resultDto=result
                .GroupBy(x => x.PlanId, x => x)
                .Select(x => new PlanServiceSubscriptionDto
            {
                PlanId = x.Key,
                PlanName = x.Select(t => t.Plans.Name).FirstOrDefault(),
                PlanPrice = x.Select(t => t.Plans.Price).FirstOrDefault(),
                IsDefaultPlan = x.Select(t =>t.Plans.IsDefaultPlan).FirstOrDefault(),
                ServicesPlan = x.Where(t=> t.Services.State == Common.Enum.State.Active).Select(t => new ServiceDto { 
                Name = t.Services.Name, 
                }).ToList()
                
            }).ToList();

            return ServiceResultList<PlanServiceSubscriptionDto>.ResultSuccess(resultDto);
        }

        public async Task<ServiceResultList<PlanServiceDto>> GetByPlanId(Guid PlanId)
        {
            if (PlanId == null)
                throw new ArgumentNullException();

            var result = await  _unitOfWork.PlanServicesRepository.Query().Include(x => x.Services).Where(x => x.PlanId == PlanId).Select(x => new PlanServiceDto {
                Id = x.Id,
                PlanId = x.PlanId,
                ServiceCode = x.Services.Code,
                ServiceName = x.Services.Name,
                ServiceDescription = x.Services.Description
            }).ToListAsync();

            return ServiceResultList<PlanServiceDto>.ResultSuccess(result);
        }
    }
}
