using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ReactDemo.Domain.Models.Location
{
    [Owned]
    public class Address : Entity<uint>
    {

        [Column("province")]
        public Province Province { get; private set; }

        [Column("city")]
        public City City { get; private set; }

        [Column("county")]
        public County County { get; private set; }

        [Column("town")]
        public Town Town { get; private set; }

        [Column("detail")]
        public string Detail { get; private set; }

        [Column("lng")]
        public decimal Lng { get; private set; }

        [Column("lat")]
        public decimal Lat { get; private set; }

        [Column("state")]
        public AddressState State { get; private set; }

        public override string ToString()
        {
            return $"{Province.ToString()}{City.ToString()}{County.ToString()}{Town.ToString()}{Detail}";
        }

        public void ChangeState(AddressState state)
        {
            if (State == state)
            {   
                throw new InvalidOperationException(" the address state equals with the arguement");
            }
            State = state;
        }

    }

    public enum AddressState
    {
        OCCUPIED,

        IDLE
    }
}