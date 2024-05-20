using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibrISv2
{
    public partial class DataLoad
    {
        public static void LoadAuthors()
        {
            string lastname = " ";
            string patr = " ";
            string pic = "/pic/no_image.png";

            DBControl.Authors.Clear();
            NpgsqlCommand command = DBControl.GetCommand("SELECT code, surname, firstname, patronymic, photo FROM \"Author\" ORDER BY surname");
            NpgsqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DBControl.Authors.Add(new Author(reader.GetInt32(0),
                                                     reader.GetString(1) ?? lastname,
                                                     reader.GetString(2),
                                                     reader.GetString(3) ?? patr,
                                                     reader.GetString(4) ?? pic));
                }
            }
            reader.Close();
        }
        public static void LoadClients()
        {
            DBControl.Clients.Clear();
            NpgsqlCommand command = DBControl.GetCommand("SELECT libcard, surname, firstname, patronymic, phone, socialstatus, debt, birthyear FROM \"Client\" ORDER BY libcard");
            NpgsqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DBControl.Clients.Add(new Client(reader.GetString(0),
                                                     reader.GetString(1),
                                                     reader.GetString(2),
                                                     reader.GetString(3) ?? " ",
                                                     reader.GetString(4),
                                                     reader.GetInt32(7),
                                                     reader.GetString(5),
                                                     reader.GetBoolean(6)));
                }
            }
            reader.Close();
        }
        public static void LoadUDK()
        {
            DBControl.DecClassifications.Clear();
            NpgsqlCommand command = DBControl.GetCommand("SELECT index, industry FROM \"DecimalClassification\" ORDER BY index");
            NpgsqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DBControl.DecClassifications.Add(new DecimalClassification(reader.GetString(0),
                                                                               reader.GetString(1)));
                }
            }
            reader.Close();
        }
        public static void LoadDepartments()
        {
            DBControl.Departments.Clear();
            NpgsqlCommand command = DBControl.GetCommand("SELECT code, organization, name FROM \"Department\" WHERE (organization = @thisLisbrary OR code = 0) ORDER BY name");
            command.Parameters.AddWithValue("@thisLisbrary", NpgsqlDbType.Varchar, MainWindow.currentLibrary);
            NpgsqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DBControl.Departments.Add(new Department(reader.GetInt32(0),
                                                             reader.GetString(1),
                                                             reader.GetString(2)));
                }
            }
            reader.Close();
        }
        public static void LoadIssues()
        {
            DBControl.Issues.Clear();
            NpgsqlCommand command = DBControl.GetCommand("SELECT \"Issue\".identifier, \"Issue\".name, \"Issue\".type, \"Issue\".\"BBK\", \"Issue\".\"UDK\", \"Issue\".year, " +
                                                                "\"Issue\".publisher, \"Issue\".pagecount, \"Issue\".storage, \"Issue\".amount, \"Issue\".annotation, " +
                                                                "\"Issue\".image, \"Issue\".keyword, \"Issue\".authorsign, \"LibraryClassification\".index, " +
                                                                "\"DecimalClassification\".index, \"Publisher\".\"OGRN\", \"IssueType\".type " +
                                                                "FROM \"Issue\", \"LibraryClassification\", \"DecimalClassification\", \"Department\", \"IssueType\", \"Publisher\" " +
                                                                "WHERE (\"Issue\".\"BBK\" = \"LibraryClassification\".index " +
                                                                "AND \"Issue\".\"UDK\" = \"DecimalClassification\".index " +
                                                                "AND \"Issue\".publisher = \"Publisher\".\"OGRN\" " +
                                                                "AND \"Issue\".storage = \"Department\".code " +
                                                                "AND \"Issue\".type = \"IssueType\".type)");
            NpgsqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DBControl.Issues.Add(new Issue(reader.GetString(0),
                                                   reader.GetString(1),
                                                   reader.GetString(2),
                                                   reader.GetString(3),
                                                   reader.GetString(4),
                                                   reader.GetInt32(5),
                                                   reader.GetString(6),
                                                   reader.GetInt32(7),
                                                   reader.GetInt32(8),
                                                   reader.GetInt32(9),
                                                   reader.GetString(10) ?? "",
                                                   reader.GetString(11) ?? "/pic/no_image.png",
                                                   reader.GetString(12) ?? "",
                                                   reader.GetString(13)));
                }
            }
            reader.Close();
        }
        public static void LoadIssueTypes()
        {
            DBControl.IssueTypes.Clear();
            NpgsqlCommand command = DBControl.GetCommand("SELECT type FROM \"IssueType\" ORDER BY type");
            NpgsqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DBControl.IssueTypes.Add(new IssueType(reader.GetString(0)));
                }
            }
            reader.Close();
        }
        public static void LoadBBK()
        {
            DBControl.LibClassifications.Clear();
            NpgsqlCommand command = DBControl.GetCommand("SELECT index, industry FROM \"LibraryClassification\" ORDER BY index");
            NpgsqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DBControl.LibClassifications.Add(new LibraryClassification(reader.GetString(0),
                                                                               reader.GetString(1)));
                }
            }
            reader.Close();
        }
        public static void LoadPublishers()
        {
            DBControl.Publishers.Clear();
            NpgsqlCommand command = DBControl.GetCommand("SELECT \"OGRN\", name FROM \"Publisher\" ORDER BY name");
            NpgsqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DBControl.Publishers.Add(new Publisher(reader.GetString(0),
                                                           reader.GetString(1)));
                }
            }
            reader.Close();
        }
        public static void LoadRoles()
        {
            DBControl.Roles.Clear();
            NpgsqlCommand command = DBControl.GetCommand("SELECT role FROM \"Role\" ORDER BY role");
            NpgsqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DBControl.Roles.Add(new Role(reader.GetString(0)));
                }
            }
            reader.Close();
        }
        public static void LoadSocialStatuses()
        {
            DBControl.SocialStatuses.Clear();

            NpgsqlCommand command = DBControl.GetCommand("SELECT status FROM \"SocialStatus\"");
            NpgsqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DBControl.SocialStatuses.Add(new SocialStatus(reader.GetString(0)));
                }
            }
            reader.Close();
        }


        //public static void LoadCities()
        //{
        //    DBControl.cities.Clear();

        //    NpgsqlCommand command = DBControl.GetCommand("SELECT code, name FROM \"City\"");
        //    NpgsqlDataReader reader = command.ExecuteReader();
        //    if (reader.HasRows)
        //    {
        //        while (reader.Read())
        //        {
        //            DBControl.cities.Add(new City(reader.GetString(0),
        //                                          reader.GetString(1)));
        //        }
        //    }
        //    reader.Close();
        //}



        //public static void LoadLibraries()
        //{
        //    DBControl.libraries.Clear();

        //    NpgsqlCommand command = DBControl.GetCommand("SELECT \"OGRN\", \"TIN\", \"CoR\", name, city, address FROM \"Library\"");
        //    NpgsqlDataReader reader = command.ExecuteReader();
        //    if (reader.HasRows)
        //    {
        //        while (reader.Read())
        //        {
        //            DBControl.libraries.Add(new Library(reader.GetString(0),
        //                                                reader.GetString(1),
        //                                                reader.GetString(2),
        //                                                reader.GetString(3),
        //                                                reader.GetString(4), 
        //                                                reader.GetString(5)));
        //        }
        //    }
        //    reader.Close();
        //}

        //public static void LoadOperations()
        //{
        //    DBControl.operations.Clear();

        //    NpgsqlCommand command = DBControl.GetCommand("SELECT id, client, issue, \"Operation\".status, issuance, returningdate," +
        //                                                 "libcard, identifier, \"OperationStatus\".status " +
        //                                                 "FROM \"Operation\", \"Client\", \"Issue\", \"OperationStatus\" " +
        //                                                 "WHERE (client = libcard " +
        //                                                 "AND issue = identifier " +
        //                                                 "AND \"Operation\".status = \"OperationStatus\".status)");
        //    NpgsqlDataReader reader = command.ExecuteReader();
        //    if (reader.HasRows)
        //    {
        //        while (reader.Read())
        //        {
        //            DBControl.operations.Add(new Operation(reader.GetInt32(0),
        //                                                   reader.GetString(1),
        //                                                   reader.GetString(2),
        //                                                   reader.GetString(3),
        //                                                   reader.GetDateTime(4),
        //                                                   reader.GetDateTime(5)));
        //        }
        //    }
        //    reader.Close();
        //}

        //public static void LoadOperationStatuses()
        //{
        //    DBControl.operationStatuses.Clear();

        //    NpgsqlCommand command = DBControl.GetCommand("SELECT status FROM \"OperationStatus\"");
        //    NpgsqlDataReader reader = command.ExecuteReader();
        //    if (reader.HasRows)
        //    {
        //        while (reader.Read())
        //        {
        //            DBControl.operationStatuses.Add(new OperationStatus(reader.GetString(0)));
        //        }
        //    }
        //    reader.Close();
        //}


    }
}
