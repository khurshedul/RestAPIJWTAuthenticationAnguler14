using System;
using System.Linq;
using System.Linq.Expressions;
using Core.Utils.Entities;
using Core.Utils.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Utils.Repositories
{
    public class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var query = inputQuery;

            // modify the IQueryable using the specification's criteria expression
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            // Includes all expression-based includes
            query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

            // Include any string-based include statements
            query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

            // Apply ordering if expressions are set
            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.GroupBy != null)
            {
                query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
            }

            // Apply paging if enabled
            if (specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip)
                             .Take(specification.Take);
            }
            return query;
        }
    }

    public static class QuerySpecificationExtensions
    {
        public static IQueryable<T> Specify<T>(this IQueryable<T> query, ISpecification<T> spec) where T : class
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(query,
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return secondaryResult.Where(spec.Criteria);
        }

        public static IQueryable<T> Specify<T>(this IQueryable<T> query, Expression<Func<T, bool>> criteria, params string[] navigations) where T : class
        {
            if (criteria != null)
            {
                query = query.Where(criteria);
            }

            if (navigations == null || navigations.Length == 0) return query;
            return navigations.Aggregate(query, EntityFrameworkQueryableExtensions.Include);
        }
    }

    //public static class ObjectQueryExtensions
    //{
    //    public static ObjectQuery<T> Include<T>(this ObjectQuery<T> query, Expression<Func<T, object>> selector)
    //    {
    //        string path = new PropertyPathVisitor().GetPropertyPath(selector);
    //        return query.Include(path);
    //    }

    //    class PropertyPathVisitor : ExpressionVisitor
    //    {
    //        private Stack<string> _stack;

    //        public string GetPropertyPath(Expression expression)
    //        {
    //            _stack = new Stack<string>();
    //            Visit(expression);
    //            return _stack
    //                .Aggregate(
    //                    new StringBuilder(),
    //                    (sb, name) =>
    //                        (sb.Length > 0 ? sb.Append(".") : sb).Append(name))
    //                .ToString();
    //        }

    //        protected override Expression VisitMember(MemberExpression expression)
    //        {
    //            if (_stack != null)
    //                _stack.Push(expression.Member.Name);
    //            return base.VisitMember(expression);
    //        }

    //        protected override Expression VisitMethodCall(MethodCallExpression expression)
    //        {
    //            if (IsLinqOperator(expression.Method))
    //            {
    //                for (int i = 1; i < expression.Arguments.Count; i++)
    //                {
    //                    Visit(expression.Arguments[i]);
    //                }
    //                Visit(expression.Arguments[0]);
    //                return expression;
    //            }
    //            return base.VisitMethodCall(expression);
    //        }

    //        private static bool IsLinqOperator(MethodInfo method)
    //        {
    //            if (method.DeclaringType != typeof(Queryable) && method.DeclaringType != typeof(Enumerable))
    //                return false;
    //            return Attribute.GetCustomAttribute(method, typeof(ExtensionAttribute)) != null;
    //        }
    //    }
    //}
}
