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

        async Task IRepository<TEntity>.AddAsync(TEntity entity)
        {
            await _entities.AddAsync(entity);
        }

        async Task IRepository<TEntity>.DeleteAsync(TEntity entity)
        {
            _entities.Remove(entity);
            await _databaseContext.SaveChangesAsync();
        }

        async Task<List<TEntity>> IRepository<TEntity>.FindListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await  _entities.Where(predicate).ToListAsync();
        }

        async Task<TEntity> IRepository<TEntity>.FindOneAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entities.SingleAsync(predicate);
        }

        void IRepository<TEntity>.Update(TEntity entity)
        {
            _entities.Update(entity);
        }

        int IRepository<TEntity>.SaveChanges()
        {
            return _databaseContext.SaveChanges();
        }

        async Task<List<TEntity>> IRepository<TEntity>.FindListAsync(Expression<Func<TEntity, bool>> predicate, Page page)
        {
            if (page != null)
            {
                return await _entities.Where(predicate).OrderByDescending(e => e.CreateTime).Skip(page.Index).Take(page.Count).ToListAsync();
            }
            else
            {
                return await _entities.Where(predicate).OrderByDescending(e => e.CreateTime).ToListAsync();
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

        async Task IRepository<TEntity>.UpdateAsync(TEntity entity)
        {
            _entities.Update(entity);
            await _databaseContext.SaveChangesAsync();
        }
    }
}