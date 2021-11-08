using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EmpleosWebMax.Domain.Core.Specification
{
    public sealed class NotSpecification<T> : Specification<T> where T : BaseEntity
    {
       Expression<Func<T, bool>> _originalCriteria;

        public NotSpecification(ISpecification<T> originalSpecification)
        {
            if(originalSpecification == (ISpecification<T>)null)
            throw new ArgumentNullException("OriginalSpecification");
            _originalCriteria = originalSpecification.SatisfiedBy();
        }

        public NotSpecification(Expression<Func<T, bool>> originalCriteria)
        {
            if (originalCriteria == (Expression<Func<T, bool>>)null)
                throw new ArgumentNullException("originalSpecification");
            _originalCriteria = originalCriteria;
        }

        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            return Expression.Lambda<Func<T, bool>>(Expression.Not(_originalCriteria.Body), _originalCriteria.Parameters.Single());
        }
    }
}
