using EmpleosWebMax.Domain.Core.Interfaces;
using EmpleosWebMax.Domain.Core.Result;
using EmpleosWebMax.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmpleosWebMax.Infrastructure.Interface.InterfaceService
{
    public interface ISubscriptionService: IBaseService
    {
        Task<ServiceResult<SubscriptionDto>> GetSubscriptioByUser(string userId);
        Task<ServiceResult<SubscriptionDto>> AddSubscription(SubscriptionDto subscriptionDto);
        
    }
}
