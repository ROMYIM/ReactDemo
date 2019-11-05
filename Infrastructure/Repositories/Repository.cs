using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ReactDemo.Application.Dtos;
using ReactDemo.Infrastructure.Domain;

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

        Task<int> IRepository<TEntity, TKey>.AddAsync(TEntity entity)
        {
            _entities.Add(entity);
            return _databaseContext.SaveChangesAsync();
        }

        Task<int> IRepository<TEntity, TKey>.DeleteAsync(TEntity entity)
        {
            _entities.Remove(entity);
            return _databaseContext.SaveChangesAsync();
        }

        Task<List<TEntity>> IRepository<TEntity, TKey>.FindListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return  _entities.Where(predicate).ToListAsync();
        }

        Task<TEntity> IRepository<TEntity, TKey>.FindOneAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.SingleAsync(predicate);
        }

        void IRepository<TEntity, TKey>.Update(TEntity entity)
        {
            _entities.Update(entity);
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

        void IRepository<TEntity, TKey>.Add(TEntity entity)
        {
            _entities.Add(entity);
        }

        void IRepository<TEntity, TKey>.Delete(TEntity entity)
        {
            _entities.Remove(entity);
        }

        Task<int> IRepository<TEntity, TKey>.UpdateAsync(TEntity entity)
        {
            _entities.Update(entity);
            return _databaseContext.SaveChangesAsync();
        }
    }
}