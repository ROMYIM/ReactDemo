using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models;

namespace ReactDemo.Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : IAggregateRoot
    {
        Task AddAsync(TEntity entity);

        void Add(TEntity entity);

        Task DeleteAsync(TEntity entity);

        void Delete(TEntity entity);

        Task UpdateAsync(TEntity entity);

        void Update(TEntity entity);

        Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate);

        Task<List<TEntity>> FindListAsync(Expression<Func<TEntity, bool>> predicate);

        Task<List<TEntity>> FindListAsync(Expression<Func<TEntity, bool>> predicate, Page page); 

        int SaveChanges();
    }
}