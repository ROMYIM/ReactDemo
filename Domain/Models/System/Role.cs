using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactDemo.Domain.Models.System
{

    [Table("role")]
    public class Role : Entity
    {
        private string _name;

        [Column("name")]
        public string Name
        {
            get => _name ?? throw new NullReferenceException(nameof(_name));

            set 
            { 
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(nameof(value));
                }
                _name = value;
            }
        }

        public Role(string name) => Name = name;
    }
}