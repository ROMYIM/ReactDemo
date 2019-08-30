using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models;
using ReactDemo.Domain.Repositories;

namespace ReactDemo.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : AggregateRoot
    {

        protected readonly DatabaseContext _databaseContext;

        protected DbSet<TEntity> _entities;

        protected readonly HttpContext _httpContext;

        public Repository(DatabaseContext databaseContext, IHttpContextAccessor httpContextAccessor)
        {
            _databaseContext = databaseContext;
            _httpContext = httpContextAccessor.HttpContext;
        }

        Task IRepository<TEntity>.AddAsync(TEntity entity)
        {
            _entities.AddAsync(entity);
            return Task.CompletedTask;
        }

        Task<int> IRepository<TEntity>.DeleteAsync(TEntity entity)
        {
            _entities.Remove(entity);
            return _databaseContext.SaveChangesAsync();
        }

        Task<List<TEntity>> IRepository<TEntity>.FindListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return  _entities.Where(predicate).ToListAsync();
        }

        Task<TEntity> IRepository<TEntity>.FindOneAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.SingleAsync(predicate);
        }

        void IRepository<TEntity>.Update(TEntity entity)
        {
            _entities.Update(entity);
        }

        int IRepository<TEntity>.SaveChanges()
        {
            return _databaseContext.SaveChanges();
        }

        Task<List<TEntity>> IRepository<TEntity>.FindListAsync(Expression<Func<TEntity, bool>> predicate, Page page)
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

        void IRepository<TEntity>.Add(TEntity entity)
        {
            _entities.Add(entity);
        }

        void IRepository<TEntity>.Delete(TEntity entity)
        {
            _entities.Remove(entity);
        }

        Task<int> IRepository<TEntity>.UpdateAsync(TEntity entity)
        {
            _entities.Update(entity);
            return _databaseContext.SaveChangesAsync();
        }
    }
}