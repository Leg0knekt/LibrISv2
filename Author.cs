using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrISv2
{
    public class Author
    {
        public Author(int id, string surname, string firstname, string patronymic, string photo)
        {
            Id = id;
            Surname = surname;
            FirstName = firstname;
            Patronymic = patronymic;
            Photo = photo;
        }
        public int Id { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string Photo { get; set; }
    }
}
