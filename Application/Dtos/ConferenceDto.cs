using System;
using System.ComponentModel.DataAnnotations;

namespace ReactDemo.Application.Dtos
{
    public class ConferenceDto
    {
        [Required, DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }

        [Required]
        public int HallID { get; set; }

        [Required, DataType(DataType.Duration)]
        public int Duration { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int OrganizationID { get; set; }
    }
}