using System.ComponentModel.DataAnnotations;

namespace ReactDemo.Application.Dtos
{
    public class OrganizationDto
    {
        public int? OrganizationID { get; set; }

        [Required]
        public string OrganizationName { get; set; }

        [Required, Phone]
        public string Phone { get; set; }

        [Required]
        public string QQ { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }
}