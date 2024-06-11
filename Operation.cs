using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrISv2
{
    public class Operation
    {
        public Operation(int id, string client, string issue, string status, DateTime issuance, DateTime returningDate, 
                         string extraClient, string extraPhone, string extraBook, string extraStatus, string extraNumber)
        {
            Id = id;
            Client = client;
            Issue = issue;
            Status = status;
            Issuance = issuance;
            ReturningDate = returningDate;
            ExtraClient = extraClient;
            ExtraPhone = extraPhone;
            ExtraBook = extraBook;
            ExtraStatus = extraStatus;
            ExtraNumber = extraNumber;
        }
        public int Id { get; set; }
        public string Client { get; set; }
        public string Issue { get; set; }
        public string Status { get; set; }
        public DateTime Issuance { get; set; }
        public DateTime ReturningDate { get; set; }
        public string ExtraClient { get; set; }
        public string ExtraPhone {  get; set; }
        public string ExtraBook { get; set; }
        public string ExtraStatus { get; set; }
        public string ExtraNumber { get; set; }
    }
}
