using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrISv2
{
    public class ReportBooks
    {
        public ReportBooks(string keyword, string id, string name, int amount)
        {
            Keyword = keyword;
            Id = id;
            Name = name;
            Amount = amount;
        }
        public string Keyword { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
    }
}
