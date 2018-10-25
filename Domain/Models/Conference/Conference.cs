using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactDemo.Domain.Models.Conference
{
    [Table("pb_conference")]
    public class Conference : AggregateRoot
    {

        [Column("start_time")]
        public DateTime StartTime { get; set; }

        [Column("end_time")]
        public DateTime EndTime { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("hall_id")]
        public int HallID { get; set; }

        [ForeignKey("HallID")]
        public virtual Hall Hall { get; set; }

        public ICollection<ConferenceMember> Members { get; private set; }

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