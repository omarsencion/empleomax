using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EmpleosWebMax.Domain.Core.Specification
{
    public sealed class TrueSpecification<TEntity> : Specification<TEntity> where TEntity : BaseEntity
    {
        public override Expression<Func<TEntity, bool>> SatisfiedBy()
        {
            bool result = true;

            Expression<Func<TEntity, bool>> trueExpression = t => result;

            return trueExpression;
        }
    }
}
