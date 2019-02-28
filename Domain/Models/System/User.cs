using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;

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
        
    }
}