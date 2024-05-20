using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrISv2
{
    public class Operation
    {
        public Operation(int id, string client, string issue, string status, DateTime issuance, DateTime returningDate)
        {
            Id = id;
            Client = client;
            Issue = issue;
            Status = status;
            Issuance = issuance;
            ReturningDate = returningDate;
        }
        public int Id { get; set; }
        public string Client { get; set; }
        public string Issue { get; set; }
        public string Status { get; set; }
        public DateTime Issuance { get; set; }
        public DateTime? ReturningDate { get; set; }
    }
}
