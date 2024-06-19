using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace LibrISv2
{
    public class DBControl
    {
        private static NpgsqlConnection Connection;
        public static void Connect(string host, string port, string username, string database, string password)
        {
            string cs = string.Format("Server = {0}; Port = {1}; User Id = {2}; Database = {3}; Password = {4}", host, port, username, database, password);

            Connection = new NpgsqlConnection(cs);
            if (Connection != null)
            {
                Connection.Open();
            }
        }
        public static NpgsqlCommand GetCommand(string sql)
        {
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = Connection;
            command.CommandText = sql;
            return command;
        }

        public static ObservableCollection<Author> Authors { get; set; } = new ObservableCollection<Author>();
        public static ObservableCollection<Catalog> Cat { get; set; } = new ObservableCollection<Catalog>();
        public static ObservableCollection<Client> Clients { get; set; } = new ObservableCollection<Client>();
        public static ObservableCollection<DecimalClassification> DecClassifications { get; set; } = new ObservableCollection<DecimalClassification>();
        public static ObservableCollection<Department> Departments { get; set; } = new ObservableCollection<Department>();
        public static ObservableCollection<Issue> Issues { get; set; } = new ObservableCollection<Issue>();
        public static ObservableCollection<IssueType> IssueTypes { get; set; } = new ObservableCollection<IssueType>();
        public static ObservableCollection<LibraryClassification> LibClassifications { get; set; } = new ObservableCollection<LibraryClassification>();
        public static ObservableCollection<Nums> Numbers { get; set; } = new ObservableCollection<Nums>();
        public static ObservableCollection<Operation> Operations { get; set; } = new ObservableCollection<Operation>();
        public static ObservableCollection<OperationStatus> OperationStatuses { get; set; } = new ObservableCollection<OperationStatus>();
        public static ObservableCollection<Publisher> Publishers { get; set; } = new ObservableCollection<Publisher>();
        public static ObservableCollection<Role> Roles { get; set; } = new ObservableCollection<Role>();
        public static ObservableCollection<SocialStatus> SocialStatuses { get; set; } = new ObservableCollection<SocialStatus>();
        //public static ObservableCollection<City> cities { get; set; } = new ObservableCollection<City>();                 в текущей версии не используется
        //public static ObservableCollection<Library> libraries { get; set; } = new ObservableCollection<Library>();         в текущей версии не используется
    }
}
