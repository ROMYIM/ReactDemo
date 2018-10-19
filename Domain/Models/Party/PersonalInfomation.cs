using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactDemo.Domain.Models.Party
{
    public class PersonalInformation
    {
        [Column("card_no")]
        public string ID { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("sex")]
        public Sex Sex { get; set; }

        [Column("native_place")]
        public string NativePlace { get; set; }

        [Column("nation")]
        public Nation Nation { get; set; }

        [Column("mobile")]
        public string Mobile { get; set; }

        [Column("birth_time"), DataType(DataType.Date)]
        public DateTime BirthTime { get; set; }

    }

    public enum Sex
    {
        MALE, FEMALE
    }

    public enum Nation
    {
        汉族, 回族, 苗族
    }
}