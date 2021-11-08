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
using System.Text;
using System.Threading.Tasks;

namespace EmpleosWebMax.Infrastructure.Data.Data
{
    public class SubscriptionService : BaseService, ISubscriptionService
    {
        public SubscriptionService(IEmpleoMaxUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<ServiceResult<SubscriptionDto>> AddSubscription(SubscriptionDto subscriptionDto)
        {
            if (subscriptionDto == null)
                return ServiceResult<SubscriptionDto>.ResultError("");

            var suscription = new Subscription
            {
                ApplicationUserId = subscriptionDto.ApplicationUserId,
                CreatedDate = DateTime.Now,
                DateFrom = DateTime.Now,
                DateTo = DateTime.Now.AddYears(100),
                PlanId = subscriptionDto.PlanId,
                PlanPrice = subscriptionDto.PlanPrice,
                IsActive = true,
                TotalPrice = subscriptionDto.TotalPrice,
                SubscriptionType = SubscriptionType.Annual,
                IsPaid = true
            };

            _unitOfWork.SubscriptionRepository.Add(suscription);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 1)
            {
                subscriptionDto.Id = suscription.Id;
                return ServiceResult<SubscriptionDto>.ResultSuccess(subscriptionDto);
            }

            return ServiceResult<SubscriptionDto>.ResultError("");
        }
        
        public async Task<ServiceResult<SubscriptionDto>> GetSubscriptioByUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return ServiceResult<SubscriptionDto>.ResultError("El UsuarioId esta vacio");
          
            var result = await _unitOfWork.SubscriptionRepository.Query().Include(x => x.Plan).FirstOrDefaultAsync(x => x.ApplicationUserId == userId && x.IsActive);
           
            if(result == null)
                return ServiceResult<SubscriptionDto>.ResultError("Este usuario no tiene suscripcion activa");

            return ServiceResult<SubscriptionDto>.ResultSuccess(new SubscriptionDto {
            
                Id = result.Id,
                ApplicationUserId = result.ApplicationUserId,
                DateFrom = result.DateFrom,
                DateTo = result.DateTo,
                PlanName = result.Plan.Name,
                SubscriptionType = result.SubscriptionType,
                TotalPrice = result.TotalPrice,
                CreatedDate = result.CreatedDate,
                IsActive = result.IsActive,
                IsAutoRenewel = result.IsAutoRenewel,
                IsPaid = result.IsPaid,
                PlanPrice = result.PlanPrice,
                PlanId = result.PlanId
            });
        }
    }
}
