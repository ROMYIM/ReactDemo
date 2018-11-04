using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models.System;

namespace ReactDemo.Domain.Models.Party
{
    [Table("pb_party_organization")]
    public class Organization : AggregateRoot
    {
        [Column("organization_name")]
        private string _name;
        public string Name 
        { 
            get 
            {
                if (string.IsNullOrWhiteSpace(this._name))
                {
                    throw new NullReferenceException("the organization name is null");
                }
                return this._name;
            }
            private set
            { 
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new NullReferenceException("the organization name can not be empty");
                }
                this._name = value;
            }
        }

        public virtual Contact Contact { get; private set; }

        [Column("user_id")]
        public int? UserID { get; private set; }

        [ForeignKey("UserID")]
        public virtual User User { get; private set; }

        public virtual ICollection<Member> Members { get; private set; }

        private Organization() {}

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

        public void AddMember(MemberDto dto)
        {
            if (dto.OrganizationID != this.ID)
            {
                throw new Exception();
            }
            var member = new Member(dto);
            if (Members.Contains(member))
            {
                throw new Exception();
            }
            Members.Add(member);
        }

    }
}