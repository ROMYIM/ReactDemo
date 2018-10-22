using System;
using System.ComponentModel.DataAnnotations;
using ReactDemo.Domain.Models.Party;

namespace ReactDemo.Application.Dtos
{
    public class MemberDto
    {
        public int? MemberID { get; set; }

        [Required]
        public int OrganizationID { get; set; }

        [Required]
        public PartyRole Role { get; set; }

        public string Position { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, StringLength(21)]
        public string CardNo { get; set; }

        [Required]
        public Sex Sex { get; set; }

        [Required]
        public string NativePlace { get; set; }

        [Required]
        public Nation Nation { get; set; }

        [Required, Phone]
        public string Mobile { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthTime { get; set; }

        [DataType(DataType.Date)]
        public DateTime JoinTime { get; set; }
    }
}