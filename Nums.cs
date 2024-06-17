using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrISv2
{
    public class Nums
    {
        public Nums(int id, string book, string num)
        {
            Id = id;
            Book = book;
            Num = num;
        }
        public int Id { get; set; }
        public string Book { get; set; }
        public string Num { get; set; }
    }
}
