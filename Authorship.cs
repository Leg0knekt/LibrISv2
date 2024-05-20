using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrISv2
{
    public class Authorship
    {
        public Authorship(string issue, int author)
        {
            Issue = issue;
            Author = author;
        }

        public string Issue { get; set; }
        public int Author { get; set; }
    }
}
