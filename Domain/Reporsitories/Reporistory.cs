using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ReactDemo.Domain.Models;

namespace ReactDemo.Domain.Reporsitories
{
    public interface Reporistory<TEntity> where TEntity : Entity
    {
        void Add(TEntity entity);

        void Delete(TEntity entity);

        void Update(TEntity entity);

        TEntity FindOne(int id);

        IList<TEntity> FindList(Expression<Predicate<TEntity>> expression);
    }
}