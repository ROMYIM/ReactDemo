using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ReactDemo.Application.Dtos;
using ReactDemo.Infrastructure.Entities;

namespace ReactDemo.Infrastructure.Repositories
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : AggregateRoot<TKey>
    {

        protected readonly DatabaseContext _databaseContext;

        protected DbSet<TEntity> _entities;

        protected readonly HttpContext _httpContext;

        DbSet<TEntity> IRepository<TEntity, TKey>.DbSet => _entities;

        public Repository(DatabaseContext databaseContext, IHttpContextAccessor httpContextAccessor)
        {
            _databaseContext = databaseContext;
            _httpContext = httpContextAccessor.HttpContext;
        }

        Task<TEntity> IRepository<TEntity, TKey>.AddAsync(TEntity entity)
        {
            var entry = _entities.Add(entity);
            
            return Task.FromResult(entry.Entity);
        }

        Task<TEntity> IRepository<TEntity, TKey>.DeleteAsync(TEntity entity)
        {
            var entry = _entities.Remove(entity);
            return Task.FromResult(entry.Entity);
        }

        Task<List<TEntity>> IRepository<TEntity, TKey>.FindListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return  _entities.Where(predicate).ToListAsync();
        }

        Task<TEntity> IRepository<TEntity, TKey>.FindOneAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.SingleAsync(predicate);
        }

        TEntity IRepository<TEntity, TKey>.Update(TEntity entity)
        {
            return _entities.Update(entity).Entity;
        }

        int IRepository<TEntity, TKey>.SaveChanges()
        {
            return _databaseContext.SaveChanges();
        }

        Task<List<TEntity>> IRepository<TEntity, TKey>.FindListAsync(Expression<Func<TEntity, bool>> predicate, Page page)
        {
            if (page != null)
            {
                return _entities.Where(predicate).OrderByDescending(e => e.CreateTime).Skip(page.Index).Take(page.Count).ToListAsync();
            }
            else
            {
                return _entities.Where(predicate).OrderByDescending(e => e.CreateTime).ToListAsync();
            }
        }

        TEntity IRepository<TEntity, TKey>.Add(TEntity entity)
        {
            return _entities.Add(entity).Entity;
        }

        TEntity IRepository<TEntity, TKey>.Delete(TEntity entity)
        {
            return _entities.Remove(entity).Entity;
        }

        Task<TEntity> IRepository<TEntity, TKey>.UpdateAsync(TEntity entity)
        {
            var entry = _entities.Update(entity);
            return Task.FromResult(entry.Entity);
        }
    }
}