using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReactDemo.Application.Dtos;
using ReactDemo.Infrastructure.Entities;

namespace ReactDemo.Infrastructure.Repositories
{
    public interface IRepository<TEntity, TKey> where TEntity : class, IAggregateRoot<TKey>
    {
        Task<TEntity> AddAsync(TEntity entity);

        TEntity Add(TEntity entity);

        Task<TEntity> DeleteAsync(TEntity entity);

        TEntity Delete(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        TEntity Update(TEntity entity);

        Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate);

        Task<List<TEntity>> FindListAsync(Expression<Func<TEntity, bool>> predicate);

        Task<List<TEntity>> FindListAsync(Expression<Func<TEntity, bool>> predicate, Page page); 

        int SaveChanges();

        DbSet<TEntity> DbSet { get; }
    }
}