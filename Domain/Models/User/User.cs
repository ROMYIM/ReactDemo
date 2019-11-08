using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ReactDemo.Infrastructure.Entities;

namespace ReactDemo.Domain.Models.User
{
    [Table("user")]
    public class User : AggregateRoot<int>
    {

        private string _username;

        [Column("username")]
        public string Username
        {
            get => _username ?? throw new NullReferenceException("username is null");
            set 
            { 
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("username can not be empty");
                }
                _username = value;
            }
        }
        
        private string _password;

        [Column("password")]
        public string Password
        {
            get => _password ?? throw new NullReferenceException("password is null");
            set 
            { 
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("username can not be empty");
                }
                _password = value;
            }
        }
        

        [Column("image_url")]
        public string ImageUrl { get; set; }

        // public List<UserRole> UserRoles { get; set; }

        private List<Role> _roles;

        [NotMapped]
        public List<Role> Roles
        {
            get 
            { 
                if (_roles == null)
                {
                    _roles = UserRoles.Select(ur => ur.Role).ToList();
                }
                return _roles;
            }
            set { _roles = value; }
        }

        private List<UserRole> _userRoles;
        public List<UserRole> UserRoles
        {
            get { return _lazyLoader.Load(this, ref _userRoles); }
            private set { _userRoles = value; }
        }
        
        private string LatestLoginIp { get; set; }

        private DateTime LatestLoginTime { get; set; }

        private User(ILazyLoader lazyLoader) : base(lazyLoader) {}

        public ClaimsIdentity CreateIdentity()
        {
            var claims = new List<Claim>
            {
                new Claim("user_id", ID.ToString(), ClaimValueTypes.Integer32),
                new Claim("username", Username, ClaimValueTypes.String)
            };

            var roles = Roles;
            roles.ForEach(r => claims.AddRange(r.GetClaims()));

            return new ClaimsIdentity(claims);
        }

        public bool VerifyPassword(string passwordInput)
        {
            if (passwordInput == null)
            {
                throw new ArgumentNullException(nameof(passwordInput));
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                throw new NullReferenceException(nameof(Password));
            }

            return Password == passwordInput;
        }

    }
}