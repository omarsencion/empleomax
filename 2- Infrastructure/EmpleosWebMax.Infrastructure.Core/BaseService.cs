using EmpleosWebMax.Domain.Core.Interfaces;
using EmpleosWebMax.Infrastructure.Interface.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Infrastructure.Core
{
    public abstract class BaseService : IBaseService
    {
        protected IEmpleoMaxUnitOfWork _unitOfWork;

        public BaseService(IEmpleoMaxUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Dispose()
        {
            // throw new NotImplementedException();

            GC.SuppressFinalize(true);
        }
    }
}
