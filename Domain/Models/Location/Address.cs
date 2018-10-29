using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ReactDemo.Domain.Models.Location
{
    [Owned]
    public class Address
    {
        [Column("province")]
        public Province Province { get; set; }

        [Column("city")]
        public City City { get; set; }

        [Column("county")]
        public County County { get; set; }

        [Column("town")]
        public Town Town { get; set; }

        [Column("detail")]
        public string Detail { get; set; }

        [Column("lng")]
        public decimal Lng { get; set; }

        [Column("lat")]
        public decimal Lat { get; set; }

        [Column("state")]
        public AddressState State { get; set; }

        public override string ToString()
        {
            return $"{Province.ToString()}{City.ToString()}{County.ToString()}{Town.ToString()}{Detail}";
        }

    }

    public enum AddressState
    {
        OCCUPIED,

        IDLE
    }
}