using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Utils.Entities;

namespace Core.Utils.Interfaces
{
    public interface IAsyncRepository<T> where T : IAggregateRoot
    {
        IDatabaseManager DatabaseManager { get; }

        Task<T> GetByIdAsync(long id);
        Task<IList<T>> ListAsync();
        Task<IList<T>> ListAsync(Expression<Func<T, bool>> criteria, params string[] navigations);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> criteria, params string[] navigations);
        Task<T> AddAsync(T entity);
        Task<IList<T>> AddListAsync(IList<T> entityList);
        Task UpdateAsync(T entity);
        Task UpdateAsync(T entity, params Expression<Func<T, object>>[] properties);
        Task UpdateListAsync(IList<T> entityList);
        Task DeleteAsync(T entity);
        Task PermanentDeleteAsync(T entity);

        Task<int> CountAsync(ISpecification<T> spec);
        Task<IList<T>> ListAsync(ISpecification<T> spec);
        Task<T> FirstOrDefaultAsync();
        Task<T> FirstOrDefaultAsync(ISpecification<T> spec);
        Task<IList<T>> ListAsyncSpecify(ISpecification<T> spec);

        Task PermanentDeleteRangeAsync(IList<T> entity);
        Task<IList<T>> GetListFromStoredProcedure(string procedureName, params SqlParameter[] listParameters);
        Task<IList<T>> GetListFromStoredProcedure(string procedureName, object viewModel);
        Task<bool> AnyAsync(ISpecification<T> spec);
        Task<string> MaxAsync(Expression<Func<T, string>> maxBy, ISpecification<T> spec);
        Task<string> MaxAsync(Expression<Func<T, string>> maxBy, Expression<Func<T, bool>> criteria, params string[] navigations);
        Task<bool> AnyAsync(Expression<Func<T, bool>> criteria, params string[] navigations);
        Task<int> CountAsync(Expression<Func<T, bool>> criteria, params string[] navigations);
        Task<bool> IsDuplicateAsync(long? id, Expression<Func<T, bool>> criteria, params string[] navigations);
        Task<string> NextAsync(Expression<Func<T, string>> itemName, int itemLength, Expression<Func<T, bool>> criteria, params string[] navigations);

        Task UpdateExceptAsync(T entity, params Expression<Func<T, object>>[] properties);
        Task UpdateExceptOneAsync(T entity, params Expression<Func<T, object>>[] properties);
        Task<T> LastOrDefaultAsync();
        Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> criteria, params string[] navigations);
    }
}
