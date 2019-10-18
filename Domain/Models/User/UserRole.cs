using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ReactDemo.Domain.Models.User
{
    [Table("user_role")]
    public class UserRole
    {

        private readonly ILazyLoader _lazyLoader;

        [Column("user_id")]
        public int UserID { get; set; }

        private User _user;
        public User User
        {
            get { return _lazyLoader.Load(this, ref _user); }
            set { _user = value; }
        }
                
        [Column("role_id")]
        public int RoleID { get; set; }

        private Role _role;
        public Role Role
        {
            get { return _lazyLoader.Load(this, ref _role); }
            set { _role = value; }
        }

        public UserRole(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }
    }
}