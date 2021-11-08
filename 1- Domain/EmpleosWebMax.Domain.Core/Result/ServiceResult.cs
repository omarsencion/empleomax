using EmpleosWebMax.Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Domain.Core.Result
{
    public class ServiceResultList<T> : ServiceResult where T : BaseDto
    {
        public IReadOnlyList<T> Data { get; set; }
        public static ServiceResultList<T> ResultSuccess(List<T> data)
        {
            return new ServiceResultList<T>
            {
                Data = data,
                ResultKind = ResultKind.Success
            };
        }
        public static new ServiceResultList<T> ResultError(string error)
        {
            return new ServiceResultList<T>
            {
                Data = default(List<T>),
                ResultKind = ResultKind.Error,
                Error = error
            };
        }

    }
    public  class ServiceResult<T>:ServiceResult where T :BaseDto
    {
    
        public T Data { get; set; }
        

        public static ServiceResult<T> ResultSuccess(T data) 
        {
            return new ServiceResult<T>
            {
                Data = data,
               ResultKind = ResultKind.Success
            };
        }

        public static new ServiceResult<T> ResultError(string error) 
        {
            return new ServiceResult<T>
            {
                Data = default(T),
                ResultKind= ResultKind.Error,
                Error =error
            };
        }
    }

    public class ServiceResult 
    {
        public ResultKind ResultKind { get; set; }
        public string Error { get; set; }
        
        public static ServiceResult ResultSuccess()
        {
            return new ServiceResult
            {
                ResultKind = ResultKind.Success
            };
        }
        
        public static  ServiceResult ResultError(string error)
        {
            return new ServiceResult
            {
                ResultKind = ResultKind.Error,
                Error = error
            };
        }
    }
}
