using System;
using System.ComponentModel.DataAnnotations;
using ReactDemo.Domain.Models.Meeting;

namespace ReactDemo.Presentation.ViewObject
{
    public class ConferenceVo
    {
        public int ConferenceID { get; set; }

        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "yyyy-MM-dd HH:mm:ss")]
        public DateTime StartTime { get; set; }

        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "yyyy-MM-dd HH:mm:ss")]
        public DateTime EndTime { get; set; }

        public int Duration { get; set; }

        public int HallID { get; set; }

        public string Content { get; set; }

        public ConferenceVo(Conference conference)
        {
            ConferenceID = conference.ID.Value;
            StartTime = conference.StartTime;
            EndTime = conference.EndTime;
            TimeSpan timeSpan = StartTime.Subtract(EndTime).Duration();
            Duration = (int) timeSpan.TotalMinutes;
            HallID = conference.HallID;
            Content = conference.Content;
        }

    }

    public class ConferenceItemVo
    {
        public int ConferenceID { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string HallName { get; set; }

        public ConferenceItemVo(Conference conference)
        {
            ConferenceID = conference.ID.Value;
            StartTime = conference.StartTime;
            EndTime = conference.EndTime;
            HallName = conference.Hall.Name;
        }
    }
}