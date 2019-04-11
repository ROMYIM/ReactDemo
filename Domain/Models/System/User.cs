using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace ReactDemo.Domain.Models.System
{
    [Table("user")]
    public class User : AggregateRoot, IIdentity
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

        private Role _role;

        [ForeignKey("RoleID")]
        public Role Role
        {
            get { return _lazyLoader.Load(this, ref _role);}
            set { _role = value;}
        }

        public string AuthenticationType => throw new NotImplementedException();

        public bool IsAuthenticated => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();


        // [Column("member_id")]
        // public int? MemberID { get; private set; }

        // private Member _member;
        // [ForeignKey("MemberID")]
        // public Member Member
        // {
        //     get { return _lazyLoader.Load(this, ref _member); }
        //     private set { _member = value;}
        // }

        private User(ILazyLoader lazyLoader) : base(lazyLoader) {}

        public ClaimsPrincipal CreateClaimsPrincipal()
        {
            var claims = new List<Claim>
            {
                new Claim("username", Username, ClaimValueTypes.String),
                new Claim("role", Role.Name, ClaimValueTypes.String)
            };

            var identity = new ClaimsIdentity(claims, Startup.SchemeName);

            return new ClaimsPrincipal(identity);
        }
        
    }
}