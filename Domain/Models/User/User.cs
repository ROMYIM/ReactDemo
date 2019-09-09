using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ReactDemo.Domain.Models.Events;
using UserModule = ReactDemo.Domain.Models.User;

namespace ReactDemo.Domain.Models.User
{
    [Table("user")]
    public class User : AggregateRoot<uint>
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

        private List<Role> _roles;
        public List<Role> Roles
        {
            get { return _lazyLoader.Load(this, ref _roles); }
            set { _roles = value; }
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