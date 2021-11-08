
using EmpleosWebMax.Domain.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Infrastructure.Interface.Module
{
    public interface IEmpleoMaxUnitOfWork : IUnitOfWork, ISuscriptionAndPlanModule
    {
    }
}
