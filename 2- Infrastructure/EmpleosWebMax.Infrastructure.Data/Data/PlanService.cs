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
    public class PlanService : BaseService, IPlanService
    {
        public PlanService(IEmpleoMaxUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<ServiceResult<PlanDto>> AddPlan(PlanDto plan)
        {
            if (plan == null)
                return ServiceResult<PlanDto>.ResultError("El parametro es nulo");


            var addService = new Plan
            {
                Name = plan.Name,
                Code = plan.Code,
                Description = plan.Description,
                Price = plan.Price,
                State = Common.Enum.State.Active,
                CanditateMessageNumber = plan.CanditateMessageNumber,
                IsCanditateSearch = plan.IsCanditateSearch,
                IsDefaultPlan = plan.IsDefaultPlan,
                JobVacancyNumber = plan.JobVacancyNumber,
                PriceOfJobVacancyBonus = plan.PriceOfJobVacancyBonus,
                PriorityJobNumber = plan.PriorityJobNumber,
                Category = plan.Category,
                CreatedBy = plan.CreatedBy,
                CreatedAt = plan.CreatedAt
            };

            _unitOfWork.PlansRepository.Add(addService);
            
            var result = await _unitOfWork.SaveChangesAsync();
            if (result == 0)
                return ServiceResult<PlanDto>.ResultError("No se ha podido guardar el registro");

            var planDto = new PlanDto { Id = addService.Id };

            return ServiceResult<PlanDto>.ResultSuccess(planDto);

        }

        public async  Task<ServiceResult> DeletePlan(Guid planId)
        {
            if (planId == null)
                return null;
            var result = await _unitOfWork.PlansRepository.FirstOrDefaultAsync(x => x.Id == planId);

            if (result == null)
                return ServiceResult.ResultError("No Se encontro el registro a borrar");
            _unitOfWork.PlansRepository.Delete(result);

            var exec = await _unitOfWork.SaveChangesAsync();

            if (exec < 1)
                return ServiceResult.ResultError("No se pudo eliminar el registro");

            return ServiceResult.ResultSuccess();
        }

        public async Task<ServiceResult<PlanDto>> GetPlanById(Guid id)
        {
            if (id == null)
                return null;

            var result = await _unitOfWork.PlansRepository.Query().Select(plan => new PlanDto
            {
                Id = plan.Id,
                Name = plan.Name,
                Code = plan.Code,
                Description = plan.Description,
                Price = plan.Price,
                State = plan.State,
                CanditateMessageNumber = plan.CanditateMessageNumber,
                IsCanditateSearch = plan.IsCanditateSearch,
                IsDefaultPlan = plan.IsDefaultPlan,
                JobVacancyNumber = plan.JobVacancyNumber,
                PriceOfJobVacancyBonus = plan.PriceOfJobVacancyBonus,
                PriorityJobNumber = plan.PriorityJobNumber,
               Category = plan.Category

            }).FirstOrDefaultAsync(x => x.Id == id);

            if (result != null)
                return ServiceResult<PlanDto>.ResultSuccess(result);

            return null;
        }

        public async Task<ServiceResult<PlanDto>> GetPlanDefaultByCategory(Category category)
        {
            var result = await _unitOfWork.PlansRepository.FirstOrDefaultAsync(x => x.Category == category && x.IsDefaultPlan && x.State == State.Active);
            if (result != null)
            {
                return ServiceResult<PlanDto>.ResultSuccess(new PlanDto
                {
                    Id = result.Id,
                    Name = result.Name,
                    Price = result.Price,
                    Category = result.Category,
                    Code = result.Code,
                    State = result.State,
                    IsDefaultPlan = result.IsDefaultPlan,
                });
            }
            return ServiceResult<PlanDto>.ResultError("No Existe ningun plan Default");
        }

        public async Task<ServiceResultList<PlanDto>> GetPlans()
        {
            var result = await _unitOfWork.PlansRepository.Query().Select(plan => new PlanDto
            {
                Id = plan.Id,
                Name = plan.Name,
                Code = plan.Code,
                Description = plan.Description,
                Price = plan.Price,
                State = plan.State,
                CanditateMessageNumber = plan.CanditateMessageNumber,
                IsCanditateSearch = plan.IsCanditateSearch,
                IsDefaultPlan = plan.IsDefaultPlan,
                JobVacancyNumber = plan.JobVacancyNumber,
                PriceOfJobVacancyBonus = plan.PriceOfJobVacancyBonus,
                PriorityJobNumber = plan.PriorityJobNumber,
                Category = plan.Category

            }).ToListAsync();
            
            return ServiceResultList<PlanDto>.ResultSuccess(result);
        }

        public async Task<ServiceResult> UpdatePlan(PlanDto plan)
        {
            if (plan == null)
                return ServiceResult.ResultError("El parametro es nulo");

            var result = await _unitOfWork.PlansRepository.Query().FirstOrDefaultAsync(x => x.Id == plan.Id);

            if (result == null)
               return ServiceResult.ResultError("No se ha encontrado el registro");

            result.Name = plan.Name;
            result.Code = plan.Code;
            result.Description = plan.Description;
            result.Price = plan.Price;
            result.State = plan.State;
            result.CanditateMessageNumber = plan.CanditateMessageNumber;
            result.IsCanditateSearch = plan.IsCanditateSearch;
            result.IsDefaultPlan = plan.IsDefaultPlan;
            result.JobVacancyNumber = plan.JobVacancyNumber;
            result.PriceOfJobVacancyBonus = plan.PriceOfJobVacancyBonus;
            result.PriorityJobNumber = plan.PriorityJobNumber;
            result.LastModifiedAt = plan.LastModifiedAt;
            result.LastModifiedBy = plan.LastModifiedBy;

            _unitOfWork.PlansRepository.Update(result);
            if (await _unitOfWork.SaveChangesAsync() == 0)
              return  ServiceResult.ResultError("No se ha podido guardar el registro");

            return ServiceResult.ResultSuccess();
        }
    }
}
