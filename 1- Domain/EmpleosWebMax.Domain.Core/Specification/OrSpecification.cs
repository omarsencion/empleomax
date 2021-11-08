using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EmpleosWebMax.Domain.Core.Specification
{
    public sealed class OrSpecification<T> : CompositeSpecification<T> where T : BaseEntity
    {
        private ISpecification<T> _rightSideSpecification = null;
        private ISpecification<T> _leftSideSpecification = null;

        public OrSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
        {
            if (leftSide == (ISpecification<T>)null)
                throw new ArgumentNullException("leftSide");
            if (rightSide == (ISpecification<T>)null)
                throw new ArgumentNullException("rightSide");
            _leftSideSpecification = leftSide;
            _rightSideSpecification = rightSide;
        }
        public override ISpecification<T> LeftSideSpecification => _leftSideSpecification;

        public override ISpecification<T> RightSideSpecification => _rightSideSpecification;

        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            Expression<Func<T, bool>> left = _leftSideSpecification.SatisfiedBy();
            Expression<Func<T, bool>> right = _rightSideSpecification.SatisfiedBy();

            return (left.Or(right));
        }
    }
}
