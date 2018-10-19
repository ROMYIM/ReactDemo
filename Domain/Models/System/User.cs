using System.ComponentModel.DataAnnotations.Schema;

namespace ReactDemo.Domain.Models.System
{
    [Table("sys_user")]
    public class User : Entity
    {
        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("image_url")]
        public string ImageUrl { get; set; }
    }
}