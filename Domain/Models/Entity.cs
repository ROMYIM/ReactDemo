using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ReactDemo.Domain.Models
{
    public class Entity : IEntity
    {

        protected int? _id;
        [Key, Column("id")]
        public int? ID
        {
            get 
            { 
                if (_id == null)
                {
                    throw new NullReferenceException("id can not bu null");
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
            Entity entity = obj as Entity;
            if (entity != null)
            {
                return entity.ID == this.ID;
            }
            else
            {
                return false;
            }
        }
        
        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            if (this.ID != null)
            {
                return this.ID.GetHashCode();
            }
            return base.GetHashCode();
        }
    }
}