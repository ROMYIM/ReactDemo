using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ReactDemo.Infrastructure.Domain;

namespace ReactDemo.Domain.Models.User
{

    [Table("role")]
    public class Role : Entity<int>
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

        private List<UserRole> _userRoles;
        public List<UserRole> UserRoles
        {
            get { return _lazyLoader.Load(this, ref _userRoles); }
            set { _userRoles = value; }
        }
        
        public Role(string name)
        {
            Name = name;
        }

        public IEnumerable<Claim> GetClaims()
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("role_id", ID.ToString(), ClaimValueTypes.Integer32),
                new Claim("role_name", Name)
            };
            return claims;
        }
    }
}