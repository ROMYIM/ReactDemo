using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models.Meeting;

namespace ReactDemo.Domain.Models.Party
{
    [Table("pb_party_member")]
    public sealed class Member : AggregateRoot
    {

        private int _organizationID;
        [Column("organization_id")]
        public int OrganizationID 
        { 
            get
            {
                if (_organizationID == 0)
                {
                    throw new Exception("organizationID can not be 0");
                }
                return _organizationID;
            }
            private set
            {
                if (value == 0)
                {
                    throw new Exception("organizationID can not be 0");
                }
                _organizationID = value;
            }
        }

        private Organization _organization;

        [ForeignKey("OrganizationID")]
        public Organization Organization 
        { 
            get => this._lazyLoader.Load(this, ref _organization);
            set
            {
                if (value == null)
                {
                    throw new NullReferenceException("organization can not be empty");
                }
                _organization = value;
                OrganizationID = _organization.ID.Value;
            }
        }

        [Column("role")]
        public PartyRole Role { get; set; }

        public PersonalInformation PersonalInformation { get; private set; }


        [Column("join_time"), DataType(DataType.Date)]
        public  DateTime JoinTime { get; private set; }

        private Member(ILazyLoader lazyLoader) : base(lazyLoader) {}

        public Member(MemberDto dto)
        {
            this.ID = dto.MemberID;
            this.OrganizationID = dto.OrganizationID;
            this.Role = dto.Role;
            this.JoinTime = dto.JoinTime;
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
            conference.OrganizerID = OrganizationID;
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