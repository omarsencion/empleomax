using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EmpleosWebMax.Domain.Core.Specification
{
    public sealed class DirectSpecification<TEntity> : Specification<TEntity> where TEntity : BaseEntity
    {
        Expression<Func<TEntity, bool>> _matchingCriteria;
        public DirectSpecification(Expression<Func<TEntity, bool>> matchingCriteria)
        {
            if (matchingCriteria == (Expression<Func<TEntity, bool>>)null)
                throw new ArgumentNullException("matchingCriteria");

            _matchingCriteria = matchingCriteria;
        }
        public override Expression<Func<TEntity, bool>> SatisfiedBy()
        {
            return _matchingCriteria;
        }
    }
}
