using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactDemo.Domain.Models.System
{
    [Table("resource")]
    public class Resource : Entity
    {
        private string _name;

        [Column("name")]
        public string Name
        {
            get { return _name ?? throw new NullReferenceException(nameof(_name));}

            private set 
            { 
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(nameof(value));
                }
                _name = value;
            }
        }
   
        public Resource(string name)
        {
            Name = name;
        }
    }
}