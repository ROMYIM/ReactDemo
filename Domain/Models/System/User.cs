using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using ReactDemo.Domain.Models.Party;

namespace ReactDemo.Domain.Models.System
{
    [Table("sys_user")]
    public class User : AggregateRoot
    {


        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("image_url")]
        public string ImageUrl { get; set; }

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
        
    }
}