using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ReactDemo.Infrastructure.Entities
{
    public abstract class Entity<TKey> : IEntity<TKey>
    {

        protected TKey _id;
        [Key, Column("id")]
        public TKey ID
        {
            get 
            { 
                if (_id == null)
                {
                    throw new NullReferenceException("主键不能为空");
                }
                return _id;
            }
            protected set => _id = value;
        }

        protected readonly ILazyLoader _lazyLoader;    

        [Column("create_time"), DataType(DataType.DateTime)]
        public DateTime CreateTime { get; protected set; }

        protected Entity()
        {
            CreateTime = DateTime.Now;
        }

        protected Entity(ILazyLoader lazyLoader)
        {
            this._lazyLoader = lazyLoader;
            CreateTime = DateTime.Now;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //
            
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            // TODO: write your implementation of Equals() here
            Entity<TKey> entity = obj as Entity<TKey>;
            return ID.Equals(entity.ID);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id);
        }
    }
}