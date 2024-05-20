using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrISv2
{
    public class Client
    {
        public Client(string libcard, string surname, string firstname, string patronymic, string phone, int birthyear, string socialstatus, bool debt)
        {
            LibCard = libcard;
            Surname = surname;
            FirstName = firstname;
            Patronymic = patronymic;
            Phone = phone;
            BirthYear = birthyear;
            SocialStatus = socialstatus;
            Debt = debt;
        }
        public string LibCard { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string Phone { get; set; }
        public int BirthYear { get; set; }
        public string SocialStatus { get; set; }
        public bool Debt { get; set; }
    }
}
