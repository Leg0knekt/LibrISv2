using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrISv2
{
    public class Library
    {
        public Library(string ogrn, string tin, string cor, string name, string city, string address)
        {
            OGRN = ogrn;
            TIN = tin;
            CoR = cor;
            Name = name;
            City = city;
            Address = address;
        }

        public string OGRN { get; set; }
        public string TIN { get; set; }
        public string CoR { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}
