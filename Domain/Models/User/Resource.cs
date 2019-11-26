using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ReactDemo.Infrastructure.Domain;

namespace ReactDemo.Domain.Models.System
{
    [Table("resource")]
    public class Resource : AggregateRoot<uint>
    {
        public Resource(ILazyLoader lazyLoader) : base(lazyLoader)
        {
        }

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

        private string _value;

        [Column("value")]
        public string Value
        {
            get { return _value ?? throw new NullReferenceException(nameof(_value));}
            set 
            { 
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(nameof(value));
                }
                _value = value;
            }
        }

    }
}