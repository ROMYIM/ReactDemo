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
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {

        protected readonly DatabaseContext _databaseContext;
        public DbSet<TEntity> Entities { get; protected set; }

        public Repository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            Entities = _databaseContext.Set<TEntity>(); 
        }

        void IRepository<TEntity>.Add(TEntity entity)
        {
            Entities.Add(entity);
        }

        void IRepository<TEntity>.Delete(TEntity entity)
        {
            Entities.Remove(entity);
        }

        IList<TEntity> IRepository<TEntity>.FindList(Expression<Func<TEntity, bool>> predicate)
        {
            return Entities.Where(predicate).ToList();
        }

        TEntity IRepository<TEntity>.FindOne(Expression<Func<TEntity, bool>> predicate)
        {
            return Entities.Single(predicate);
        }

        void IRepository<TEntity>.Update(TEntity entity)
        {
            Entities.Update(entity);
        }

        int IRepository<TEntity>.SaveChanges()
        {
            return _databaseContext.SaveChanges();
        }

        IList<TEntity> IRepository<TEntity>.FindList(Expression<Func<TEntity, bool>> predicate, Page page)
        {
            if (page != null)
            {
                return Entities.Where(predicate).Skip(page.Index).Take(page.Count).ToList();
            }
            else
            {
                return Entities.Where(predicate).ToList();
            }
        }
    }
}