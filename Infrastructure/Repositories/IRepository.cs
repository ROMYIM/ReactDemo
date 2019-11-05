using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReactDemo.Application.Dtos;
using ReactDemo.Infrastructure.Domain;

namespace ReactDemo.Infrastructure.Repositories
{
    public interface IRepository<TEntity, TKey> where TEntity : class, IAggregateRoot<TKey>
    {
        Task<int> AddAsync(TEntity entity);

        void Add(TEntity entity);

        Task<int> DeleteAsync(TEntity entity);

        void Delete(TEntity entity);

        Task<int> UpdateAsync(TEntity entity);

        void Update(TEntity entity);

        Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate);

        Task<List<TEntity>> FindListAsync(Expression<Func<TEntity, bool>> predicate);

        Task<List<TEntity>> FindListAsync(Expression<Func<TEntity, bool>> predicate, Page page); 

        int SaveChanges();

        DbSet<TEntity> DbSet { get; }
    }
}