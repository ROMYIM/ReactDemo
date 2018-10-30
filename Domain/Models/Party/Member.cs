using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models.Meeting;

namespace ReactDemo.Domain.Models.Party
{
    [Table("pb_party_member")]
    public class Member : Entity
    {
        [Column("organization_id")]
        public int OrganizationID { get; set; }

        [ForeignKey("OrganizationID")]
        public virtual Organization Organization { get; set; }

        [Column("role")]
        public PartyRole Role { get; set; }

        public virtual PersonalInformation PersonalInformation { get; set; }

        [Column("position")]
        public  string Position { get; set; }

        [Column("join_time"), DataType(DataType.Date)]
        public  DateTime JoinTime { get; set; }

        public Member() {}

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

        public Conference CreateConference(ConferenceDto dto, Hall hall)
        {
            var conference = new Conference(dto, hall);
            conference.OrganizerID = ID.Value;
            return conference;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //
            
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            // TODO: write your implementation of Equals() here
            var member = obj as Member;
            if (member != null && member.PersonalInformation.ID == this.PersonalInformation.ID)
                return true;
            return base.Equals (obj);
        }
        
        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            if (this.ID == null)
            {
                return int.Parse(this.PersonalInformation.ID);
            }
            return base.GetHashCode();
        }
    }

    public enum PartyRole
    {
        党书记, 党委, 党总支, 党支书
    }
}