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
            get => _name ?? throw new NullReferenceException("the name of role is empty");

            set 
            { 
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("the name of party role can not be empty");
                }
                _name = value;
            }
        }

        public Role(string name) => Name = name;
    }
}