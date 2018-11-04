using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models;

namespace ReactDemo.Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : AggregateRoot
    {
        void Add(TEntity entity);

        void Delete(TEntity entity);

        void Update(TEntity entity);

        TEntity FindOne(Expression<Func<TEntity, bool>> predicate);

        IList<TEntity> FindList(Expression<Func<TEntity, bool>> predicate);

        IList<TEntity> FindList(Expression<Func<TEntity, bool>> predicate, Page page); 

        int SaveChanges();
    }
}