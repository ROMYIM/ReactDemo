using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ReactDemo.Domain.Models.System;

namespace ReactDemo.Domain.Models.Party
{
    [Table("pb_party_organization")]
    public class Organization : Entity
    {
        [Column("organization_name")]
        public string Name { get; set; }

        [Column("user_id")]
        public int? UserID { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }

        public ICollection<Member> Members { get; set; }

        public int CountMemebers()
        {
            if (Members != null)
            {
                return Members.Count;
            }
            return 0;
        }
    }
}