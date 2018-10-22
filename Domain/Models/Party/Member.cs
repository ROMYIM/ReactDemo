using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ReactDemo.Application.Dtos;

namespace ReactDemo.Domain.Models.Party
{
    [Table("pb_party_member")]
    public class Member : Entity
    {
        [Column("organization_id")]
        public int OrganizationID { get; set; }

        [ForeignKey("OrganizationID")]
        public Organization Organization { get; set; }

        [Column("role")]
        public PartyRole Role { get; set; }

        public PersonalInformation PersonalInformation { get; set; }

        [Column("position")]
        public string Position { get; set; }

        [Column("join_time"), DataType(DataType.Date)]
        public DateTime JoinTime { get; set; }

        public Member(MemberDto dto)
        {
            this.ID = dto.MemberID;
            this.OrganizationID = dto.OrganizationID;
            this.Role = dto.Role;
            this.JoinTime = dto.JoinTime;
            this.Position = dto.Position;
            this.PersonalInformation = new PersonalInformation
            {
                Name = dto.Name,
                ID = dto.CardNo,
                Sex = dto.Sex,
                NativePlace = dto.NativePlace,
                Nation = dto.Nation,
                Mobile = dto.Mobile,
                BirthTime = dto.BirthTime
            };
        }
    }

    public enum PartyRole
    {
        党书记, 党委, 党总支, 党支书
    }
}