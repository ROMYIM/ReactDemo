using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models.System;

namespace ReactDemo.Domain.Models.Party
{
    [Table("pb_party_organization")]
    public class Organization : Entity
    {
        [Column("organization_name")]
        public string Name { get; set; }

        public virtual Contact Contact { get; set; }

        [Column("user_id")]
        public int? UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public virtual ICollection<Member> Members { get; set; }

        public Organization() {}

        public Organization(OrganizationDto dto)
        {
            ID = dto.OrganizationID;
            Name = dto.OrganizationName;
            Contact = new Contact
            {
                Phone = dto.Phone,
                QQ = dto.QQ,
                Email = dto.Email
            };
        }

    }
}