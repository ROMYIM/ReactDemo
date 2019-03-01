using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models.Location;

namespace ReactDemo.Domain.Models.Meeting
{
    [Table("pb_conference")]
    public sealed class Conference : AggregateRoot
    {

        [Column("start_time"), DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }

        [Column("end_time"), DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("organizer_id")]
        public int OrganizerID { get; set; }

        [Column("hall_id")]
        public int HallID { get; set; }


        private Hall _hall;
        [ForeignKey("HallID")]
        public Hall Hall 
        { 
            get => this._lazyLoader.Load(this, ref this._hall);
            private set => this._hall = value;
        }

        [NotMapped]
        public ICollection<ConferenceMember> Members { get; private set; }

        // private Conference(ILazyLoader layloader) : base(layloader) {}

        public Conference(ConferenceDto dto, Hall hall)
        {
            ID = dto.ConferenceID;
            BookHall(hall);
            StartTime = dto.StartTime;
            Content = dto.Content;
            EndTime = StartTime.AddMinutes(dto.Duration);    
        }

        public void BookHall(Hall hall)
        {
            if (hall == null || hall.Address.State == AddressState.OCCUPIED)
            {
                throw new Exception("the hall is not exist or the hall is occupied");   
            }
            HallID = hall.ID.Value;
            Hall = hall;
            Hall.Address.ChangeState(AddressState.OCCUPIED);
        }

        public int AddConferencetMembers(ICollection<int> memberIds)
        {   
            if (memberIds == null || memberIds.Count == 0)
            {
                throw new Exception("添加的成员不能为空");
            }
            if (Members == null)
            {
                Members = new HashSet<ConferenceMember>(memberIds.Count);
            }
            foreach (var memberId in memberIds)
            {
                Members.Add(new ConferenceMember(memberId));
            }
            return Members.Count;
        }
    }
}