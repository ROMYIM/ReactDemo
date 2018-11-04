using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public Repository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        void IRepository<TEntity>.Add(TEntity entity)
        {
            _entities.Add(entity);
        }

        void IRepository<TEntity>.Delete(TEntity entity)
        {
            _entities.Remove(entity);
        }

        IList<TEntity> IRepository<TEntity>.FindList(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.Where(predicate).ToList();
        }

        TEntity IRepository<TEntity>.FindOne(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.Single(predicate);
        }

        void IRepository<TEntity>.Update(TEntity entity)
        {
            _entities.Update(entity);
        }

        int IRepository<TEntity>.SaveChanges()
        {
            return _databaseContext.SaveChanges();
        }

        IList<TEntity> IRepository<TEntity>.FindList(Expression<Func<TEntity, bool>> predicate, Page page)
        {
            if (page != null)
            {
                return _entities.Where(predicate).OrderByDescending(e => e.CreateTime).Skip(page.Index).Take(page.Count).ToList();
            }
            else
            {
                return _entities.Where(predicate).OrderByDescending(e => e.CreateTime).ToList();
            }
        }
    }
}