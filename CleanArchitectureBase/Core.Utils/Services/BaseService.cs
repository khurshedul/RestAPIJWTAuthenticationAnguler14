using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Utils.Entities;
using Core.Utils.Interfaces;

namespace Core.Utils.Services
{
    public abstract class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        private readonly IAsyncRepository<T> _asyncRepository;
        protected BaseService(IAsyncRepository<T> asyncRepository)
        {
            _asyncRepository = asyncRepository;
        }

        public async Task<T> Add(T entity)
        {
            return await _asyncRepository.AddAsync(entity);
        }

        public async Task<T> GetById(long id)
        {
            return await _asyncRepository.GetByIdAsync(id);
        }

        public async Task<IList<T>> GetList(ISpecification<T> spec)
        {
            return await _asyncRepository.ListAsync(spec);
        }

        public async Task<IList<T>> GetList()
        {
            return await _asyncRepository.ListAsync();
        }

        public async Task<int> Count(ISpecification<T> spec)
        {
            return await _asyncRepository.CountAsync(spec);
        }

        public async Task Update(T entity)
        {
            await _asyncRepository.UpdateAsync(entity);
        }

        public async Task Delete(long id)
        {
            var entity = await GetById(id);
            if (entity != null)
            {
                await _asyncRepository.UpdateAsync(entity);
            }
        }

        public async Task PermanentDelete(T entity)
        {
            await _asyncRepository.PermanentDeleteAsync(entity);
        }

        public async Task<IList<T>> ListAsync()
        {
            return await _asyncRepository.ListAsync();
        }

        public async Task<IList<T>> ListAsync(Expression<Func<T, bool>> criteria, params string[] navigations)
        {
            return await _asyncRepository.ListAsync(criteria, navigations);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> criteria, params string[] navigations)
        {
            return await _asyncRepository.FirstOrDefaultAsync(criteria, navigations);
        }

        public async Task<IList<T>> ListAsync(ISpecification<T> spec)
        {
            return await _asyncRepository.ListAsync(spec);
        }

        public async Task<T> FirstOrDefaultAsync(ISpecification<T> spec)
        {
            return await _asyncRepository.FirstOrDefaultAsync(spec);
        }

        public async Task UpdateAll(List<T> entity)
        {
            await _asyncRepository.UpdateListAsync(entity);
        }

        public async Task UpdateAsync(T entity, params Expression<Func<T, object>>[] properties)
        {
            await _asyncRepository.UpdateAsync(entity, properties);
        }

        public async Task AddAll(List<T> entity)
        {
            await _asyncRepository.AddListAsync(entity);
        }
    }
}
