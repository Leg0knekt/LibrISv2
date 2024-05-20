using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrISv2
{
    public class LibraryClassification
    {
        public LibraryClassification(string index, string industry)
        {
            Index = index;
            Industry = industry;
        }

        public string Index { get; set; }
        public string Industry { get; set; }
    }
}
