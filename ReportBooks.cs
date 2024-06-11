using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrISv2
{
    public class ReportBooks
    {
        public ReportBooks(string keyword, string[] names, int[] amounts, int coefficient)
        {
            Keyword = keyword;
            Names = names;
            Amounts = amounts;
            Coefficient = coefficient;
        }
        public string Keyword { get; set; }
        public string[] Names { get; set; }
        public int[] Amounts { get; set; }
        public int Coefficient { get; set; }
    }
}
