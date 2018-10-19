using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactDemo.Domain.Models
{
    public class Entity
    {
        [Key, Column("id")]
        public int? ID { get; set; }

        [Column("create_time")]
        public DateTime CreateTime { get; set; }

        public Entity()
        {
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
                return (int)this.ID;
            }
            return base.GetHashCode();
        }
    }
}