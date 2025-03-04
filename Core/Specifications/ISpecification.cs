using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>>? Criteria { get; }  // Cho phép null
        List<Expression<Func<T, object>>> Includes { get; }
    }


}