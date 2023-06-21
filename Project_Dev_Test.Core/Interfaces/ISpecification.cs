using System;
using System.Linq.Expressions;

namespace Project_Dev_Test.Core.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
    }
}
