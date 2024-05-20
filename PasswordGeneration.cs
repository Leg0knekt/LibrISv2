using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace LibrISv2
{
    public class PasswordGeneration
    {
        private static string symbols = "01234567890!@#$%&*ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private static string text = string.Empty;
        public static string GenerateSalt()
        {
            List<string> salts = new List<string>();
            NpgsqlCommand command = DBControl.GetCommand("SELECT salt FROM \"Employee\"");
            NpgsqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    salts.Add(reader.GetString(0));
                }
            }
            reader.Close();

            while (true)
            {
                Random rnd = new Random();
                text = string.Empty;
                for (int i = 0; i < 4; ++i)
                {
                    text += symbols[rnd.Next(symbols.Length)];
                }
                if (!salts.Contains(text)) break;
            }
            return text;
        }
        public static string GeneratePass()
        {
            Random rand = new Random();
            text = string.Empty;
            for (int i = 0; i < 8; ++i)
            {
                text += symbols[rand.Next(symbols.Length)];
            }
            return text;
        }
        public static string GenerateHash(string pass, string salt)
        {
            SHA256 hash = SHA256.Create();
            string result = Encoding.UTF8.GetString(hash.ComputeHash(Encoding.UTF8.GetBytes(pass + salt)));
            return result;
        }
    }
}
