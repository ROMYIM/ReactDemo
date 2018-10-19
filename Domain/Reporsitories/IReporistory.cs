using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ReactDemo.Domain.Models;

namespace ReactDemo.Domain.Reporsitories
{
    public interface IReporistory<TEntity> where TEntity : Entity
    {
        int Add(TEntity entity);

        int Delete(TEntity entity);

        int Update(TEntity entity);

        TEntity FindOne(int id);

        IList<TEntity> FindList(Expression<Predicate<TEntity>> expression);
    }
}