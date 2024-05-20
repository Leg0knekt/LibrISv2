using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrISv2
{
    public class Publisher
    {
        public Publisher(string ogrn, string name)
        {
            OGRN = ogrn;
            Name = name;
        }

        public string OGRN { get; set; }
        public string Name { get; set; }
    }
}
