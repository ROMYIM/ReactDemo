using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactDemo.Domain.Models.Location
{
    public class Address
    {
        [Column("province")]
        public Province Province { get; set; }

        [Column("city")]
        public City City { get; set; }

        [Column("town")]
        public Town Town { get; set; }

        [Column("county")]
        public County County { get; set; }

        [Column("detail")]
        public string Detail { get; set; }

        [Column("lng")]
        public decimal Lng { get; set; }

        [Column("lat")]
        public decimal Lat { get; set; }

        public override string ToString()
        {
            return $"{Province.ToString()}{City.ToString()}{Town.ToString()}{County.ToString()}{Detail}";
        }

    }
}