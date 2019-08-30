using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace ReactDemo.Domain.Models.System
{

    [Table("role")]
    public class Role : AggregateRoot
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

        public Role(string name)
        {
            Name = name;
        }

        public ClaimsIdentity CreateIdentity()
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("role_id", ID.Value.ToString(), ClaimValueTypes.Integer32),
                new Claim("role_name", Name)
            };
            return new ClaimsIdentity(claims);
        }
    }
}