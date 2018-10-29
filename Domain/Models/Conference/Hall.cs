using System.ComponentModel.DataAnnotations.Schema;
using ReactDemo.Domain.Models.Location;

namespace ReactDemo.Domain.Models.Conference
{

    [Table("pb_hall")]
    public class Hall : Entity
    {

        [Column("name")]
        public string Name { get; set; }

        public virtual Address Address { get; set; }
    }
}