using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrISv2
{
    public class Department
    {
        public Department(int code, string organization, string name)
        {
            Code = code;
            Organization = organization;
            Name = name;
        }

        public int Code { get; set; }
        public string Organization { get; set; }
        public string Name { get; set; }
    }
}
