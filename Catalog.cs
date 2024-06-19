using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrISv2
{
    public class Catalog
    {
        public Catalog(string id, string name, string author, string num, int authorCode)
        {
            Id = id;
            Name = name;
            Author = author;
            Num = num;
            AuthorCode = authorCode;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Num { get; set; }
        public int AuthorCode { get; set; }
    }
}
