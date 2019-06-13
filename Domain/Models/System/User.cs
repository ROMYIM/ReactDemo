using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace ReactDemo.Domain.Models.System
{
    [Table("user")]
    public class User : AggregateRoot
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

        [Column("role_id")]
        public int RoleID { get; set; }

        // private Role _role;

        private User(ILazyLoader lazyLoader) : base(lazyLoader) {}

        public ClaimsIdentity CreateIdentity()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, ID.Value.ToString(), ClaimValueTypes.Integer32),
                new Claim(ClaimTypes.Name, Username, ClaimValueTypes.String)
            };

            return new ClaimsIdentity(claims);
        }

        public bool verifyPassword(string passwordInput)
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

        // public AuthenticationProperties CreateAuthenticationProperties()
        // {
        //     var propperties = new AuthenticationProperties();
        //     propperties.IsPersistent = true;
        //     propperties.IssuedUtc = DateTimeOffset.UtcNow;
        //     return propperties;
        // }
        
    }
}