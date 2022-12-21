using DotnetCoreTemplate.Application.Shared.Specification;
using DotnetCoreTemplate.Application.Shared.Specification.Enums;
using DotnetCoreTemplate.Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Services;

public class EFIncludeSpecificationEvaluator : ISpecificationEvaluator
{
    public IQueryable<TEntity> Evaluate<TEntity>(IQueryable<TEntity> query, SpecificationBase<TEntity> specification)
        where TEntity : class
    {
        const string errorMsg = "'{0}' cannot be empty when '{1}' is set to '{2}'";

        if (specification.IncludeExpressions.Count > 0)
        {
            foreach (var include in specification.IncludeExpressions)
            {
                switch (include.Type)
                {
                    case IncludeType.String:
                        if (string.IsNullOrEmpty(include.StringExpression))
                            throw new ArgumentNullException(
                                string.Format(errorMsg, include.StringExpression, include.Type, IncludeType.String));

                        query = query.Include(include.StringExpression);
                        break;

                    case IncludeType.LINQ:
                        if (include.LinqExpression == null)
                            throw new ArgumentNullException(
                                string.Format(errorMsg, include.LinqExpression, include.Type, IncludeType.LINQ));

                        query = query.Include(include.LinqExpression);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        return query;
    }
}