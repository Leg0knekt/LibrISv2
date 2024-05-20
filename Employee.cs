using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrISv2
{
    public class Employee
    {
        public Employee(string tin, string surname, string firstname, string patronymic, string phone, int workplace,
                        string position, string role, string login, string hashedpass, string salt)
        {
            TIN = tin;
            Surname = surname;
            FirstName = firstname;
            Patronymic = patronymic;
            Phone = phone;
            Workplace = workplace;
            Position = position;
            Role = role;
            Login = login;
            HashedPass = hashedpass;
            Salt = salt;
        }

        public string TIN { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string Phone { get; set; }
        public int Workplace { get; set; }
        public string Position { get; set; }
        public string Role { get; set; }
        public string Login { get; set; }
        public string HashedPass { get; set; }
        public string Salt { get; set; }
    }
}
