using System.ComponentModel;

namespace ReactDemo.Application.Dtos
{
    public class Page
    {
        [DefaultValue(0)]
        public int Index { get; set; }

        [DefaultValue(10)]
        public int Count { get; set; }
    }
}