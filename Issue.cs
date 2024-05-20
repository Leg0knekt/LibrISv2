using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrISv2
{
    public class Issue
    {
        public Issue(string identifier, string name, string type, string bbk, string udk, int year, string publisher,
                     int pagecount, int storage, int amount, string annotation, string image, string keyword, string authorsign)
        {
            Identifier = identifier;
            Name = name;
            Type = type;
            BBK = bbk;
            UDK = udk;
            Publisher = publisher;
            Year = year;
            PageCount = pagecount;
            Storage = storage;
            Amount = amount;
            Annotation = annotation;
            Image = image;
            Keyword = keyword;
            AuthorSign = authorsign;
        }
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string BBK { get; set; }
        public string UDK { get; set; }
        public string Publisher { get; set; }
        public int Year { get; set; }
        public int PageCount { get; set; }
        public int Storage { get; set; }
        public int Amount { get; set; }
        public string Annotation { get; set; }
        public string Image { get; set; }
        public string Keyword { get; set; }
        public string AuthorSign { get; set; }
    }
}
