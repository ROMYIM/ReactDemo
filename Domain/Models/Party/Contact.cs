using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ReactDemo.Domain.Models.Party
{
    [Owned]
    public class Contact
    {

        [Column("phone")]
        public string Phone { get; set; }

        [Column("qq")]
        public string QQ { get; set; }

        [Column("e_mail")]
        public string Email { get; set; }
    }
}