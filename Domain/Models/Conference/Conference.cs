using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models.Location;

namespace ReactDemo.Domain.Models.Conference
{
    [Table("pb_conference")]
    public class Conference : AggregateRoot
    {

        [Column("start_time"), DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }

        [Column("end_time"), DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("hall_id")]
        public int HallID { get; set; }

        [ForeignKey("HallID")]
        public virtual Hall Hall { get; set; }

        [NotMapped]
        public ICollection<ConferenceMember> Members { get; private set; }

        public Conference() {}

        public Conference(ConferenceDto dto, Hall hall)
        {
            if (hall == null || hall.Address.State == AddressState.OCCUPIED)
            {
                throw new Exception("the hall is not exist or the hall is occupied");   
            }
            ID = dto.ConferenceID;
            HallID = hall.ID.Value;
            Hall = hall;
            StartTime = dto.StartTime;
            Content = dto.Content;
            EndTime = StartTime.AddMinutes(dto.Duration);    
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