using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
//using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace LibrISv2
{
    public partial class WindowOperationSelector : Window
    {
        private static DateTime todayDate;
        private static DateTime returningDate;
        private static string now;
        private static string ret;
        List<string> NumCheck = new List<string>();

        public WindowOperationSelector()
        {
            InitializeComponent();
            DataContext = this;
            todayDate = DateTime.Today.Date;
            returningDate = todayDate.AddDays(14);
            now = todayDate.ToShortDateString();
            ret = returningDate.ToShortDateString();
            LoadNums();
            cbNum.SetBinding(ItemsControl.ItemsSourceProperty, new Binding() { Source = DBControl.Numbers });
        }

        // Кнопки
        private void bReturning_Click(object sender, RoutedEventArgs e)
        {
            if (PageControl.PageIssuance == null) return;

            Client client = (Client)PageControl.PageIssuance.lvClients.SelectedItem;
            Issue issue = (Issue)PageControl.PageIssuance.lvIssues.SelectedItem;

            if (client != null && issue != null)
            {
                if (cbNum.SelectedIndex != -1)
                {
                    int amount = OperationCheck(issue, client);
                    if (amount < 0)
                    {
                        MessageBox.Show("Обнаружена серьёзная ошибка. Пожалуйста, обратитесь к разработчику или системному администритору");
                        return;
                    }
                    if (amount >= 0)
                    {
                        if (UpdateOperation(issue, client))
                        {
                            if (IssueAmountUpdate(issue, (amount + 1)))
                            {
                                MessageBox.Show(now + " читателем " + client.Surname + " " + client.FirstName + " " + client.Patronymic +
                                               " возвращено издание " + issue.Name);
                                Close();
                            }
                            else
                            {
                                MessageBox.Show("Не удалось завершить операцию");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Не удалось обновить запись");
                            return;
                        }
                    }
                }
                else { MessageBox.Show("Инвентарный номер должен быть указан"); }
            }
        }
        private void bIssuance_Click(object sender, RoutedEventArgs e)
        {
            if (PageControl.PageIssuance == null) return;
            DataLoad.LoadOperations();
            NumsCheck();
            if (NumCheck.Contains(((Nums)cbNum.SelectedItem as Nums).Num)) return;
            
            Client client = (Client)PageControl.PageIssuance.lvClients.SelectedItem;
            Issue issue = (Issue)PageControl.PageIssuance.lvIssues.SelectedItem;
            if (client != null && issue != null)
            {
                if (cbNum.SelectedIndex != -1)
                {
                    int amount = AvailabilityCheck(issue);
                    if (amount == 0)
                    {
                        MessageBox.Show("В настоящий момент данного издания нет в наличии");
                        return;
                    }
                    if (amount < 0)
                    {
                        MessageBox.Show("Произошла непредвиденная ошибка. Обратитесь к разработчику или системному администратору");
                    }
                    if (amount > 0)
                    {
                        if (IssueAmountUpdate(issue, (amount - 1)))
                        {
                            if (CreateOperation(issue, client))
                            {
                                MessageBox.Show(now + " произведена операция выдачи издания " + issue.Name + " читателю " + client.Surname + " "
                                                + client.FirstName + " " + client.Patronymic + "\nСрок возврата составляет 14 дней и истекает " + ret);
                                Close();
                            }
                            else
                            {
                                MessageBox.Show("Не удалось завершить операцию");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Не удалось обновить запись");
                            return;
                        }
                    }
                }
                else { MessageBox.Show("Инвентарный номер должен быть указан"); }
            }
        }
        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Операции с БД
        private int AvailabilityCheck(Issue issue)
        {
            NpgsqlCommand command = DBControl.GetCommand("SELECT \"Issue\".\"amount\" FROM \"Issue\", \"Nums\" WHERE (\"identifier\" = @id AND \"amount\" > 0 AND \"storage\" = @thisDepartment AND \"Nums\".\"book\" = @id)");
            command.Parameters.AddWithValue("@id", NpgsqlDbType.Varchar, issue.Identifier);
            command.Parameters.AddWithValue("@thisDepartment", NpgsqlDbType.Integer, MainWindow.currentUserWorkplace);
            NpgsqlDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                return 0;
            }
            else
            {
                reader.Read();
                int r = reader.GetInt32(0);
                reader.Close();
                return r;
            }
        }
        private bool IssueAmountUpdate(Issue issue, int amount)
        {
            NpgsqlCommand command = DBControl.GetCommand("UPDATE \"Issue\" SET \"amount\" = @amount WHERE \"identifier\" = @id AND \"storage\" = @thisDepartment");
            try
            {
                command.Parameters.AddWithValue("@id", NpgsqlDbType.Varchar, issue.Identifier);
                command.Parameters.AddWithValue("@thisDepartment", NpgsqlDbType.Integer, MainWindow.currentUserWorkplace);
                command.Parameters.AddWithValue("@amount", NpgsqlDbType.Integer, amount);
                int result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    return true;
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }
        private bool CreateOperation(Issue issue, Client client)
        {
            NpgsqlCommand command = DBControl.GetCommand("INSERT INTO \"Operation\" (client, issue, status, issuance, returningdate, num) " +
                                                         "VALUES (@client, @issue, @status, @issuance, @returning, @num)");
            try
            {
                command.Parameters.AddWithValue("@client", NpgsqlDbType.Varchar, client.LibCard);
                command.Parameters.AddWithValue("@issue", NpgsqlDbType.Varchar, issue.Identifier);
                command.Parameters.AddWithValue("@status", NpgsqlDbType.Varchar, "Выдано");
                command.Parameters.AddWithValue("@issuance", NpgsqlDbType.Date, todayDate);
                command.Parameters.AddWithValue("@returning", NpgsqlDbType.Date, returningDate);
                command.Parameters.AddWithValue("@num", NpgsqlDbType.Varchar, ((Nums)cbNum.SelectedItem as Nums).Num);
                int result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    return true;
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }
        private int OperationCheck(Issue issue, Client client)
        {
            NpgsqlCommand command = DBControl.GetCommand("SELECT \"Operation\".\"client\", \"Operation\".\"issue\", \"Issue\".\"amount\", \"Issue\".\"identifier\", \"Operation\".\"num\" " +
                                                         "FROM \"Operation\", \"Issue\" " +
                                                         "WHERE (\"Operation\".\"client\" = @client " +
                                                         "AND \"Operation\".\"issue\" = \"Issue\".\"identifier\" " +
                                                         "AND \"Operation\".\"issue\" = @issue " +
                                                         "AND \"Issue\".\"identifier\" = @issue " +
                                                         "AND \"Operation\".\"status\" != @status " +
                                                         "AND \"Issue\".\"storage\" = @thisDepartment " +
                                                         "AND \"Operation\".\"num\" = @num)");
            command.Parameters.AddWithValue("@issue", NpgsqlDbType.Varchar, issue.Identifier);
            command.Parameters.AddWithValue("@client", NpgsqlDbType.Varchar, client.LibCard);
            command.Parameters.AddWithValue("@status", NpgsqlDbType.Varchar, "Возвращено");
            command.Parameters.AddWithValue("@thisDepartment", NpgsqlDbType.Integer, MainWindow.currentUserWorkplace);
            command.Parameters.AddWithValue("@num", NpgsqlDbType.Varchar, ((Nums)cbNum.SelectedItem as Nums).Num);
            NpgsqlDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                return 0;
            }
            else
            {
                reader.Read();
                int r = reader.GetInt32(2);
                reader.Close();
                return r;
            }
        }
        private bool UpdateOperation(Issue issue, Client client)
        {
            //NpgsqlCommand command = DBControl.GetCommand("UPDATE \"Operation\" SET status = @status WHERE client = @client AND issue = @issue AND num = @num");
            //try
            //{
            //    command.Parameters.AddWithValue("@client", NpgsqlDbType.Varchar, client.LibCard);
            //    command.Parameters.AddWithValue("@issue", NpgsqlDbType.Varchar, issue.Identifier);
            //    command.Parameters.AddWithValue("@status", NpgsqlDbType.Varchar, "Возвращено");
            //    command.Parameters.AddWithValue("@num", NpgsqlDbType.Varchar, tbNum.Text.Trim());
            //    int result = command.ExecuteNonQuery();
            //    if (result == 1)
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //catch
            //{
            //    return false;
            //}
            NpgsqlCommand command = DBControl.GetCommand("DELETE FROM \"Operation\" WHERE client = @client AND issue = @issue AND num = @num");
            try
            {
                command.Parameters.AddWithValue("@client", NpgsqlDbType.Varchar, client.LibCard);
                command.Parameters.AddWithValue("@issue", NpgsqlDbType.Varchar, issue.Identifier);
                command.Parameters.AddWithValue("@num", NpgsqlDbType.Varchar, ((Nums)cbNum.SelectedItem as Nums).Num);
                int result = command.ExecuteNonQuery();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        private void LoadNums()
        {
            Issue issue = (Issue)PageControl.PageIssuance.lvIssues.SelectedItem;
            DBControl.Numbers.Clear();

            NpgsqlCommand command = DBControl.GetCommand("SELECT \"Nums\".id, \"Nums\".book, \"Nums\".num FROM \"Nums\" WHERE \"Nums\".book = @book");
            command.Parameters.AddWithValue("@book", NpgsqlDbType.Varchar, issue.Identifier);
            NpgsqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DBControl.Numbers.Add(new Nums(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
                }
            }
            reader.Close();
        }
        private void NumsCheck()
        {
            NumCheck.Clear();
            foreach (var op in DBControl.Operations)
            {
                NumCheck.Add(op.ExtraNumber);
            }
        }
    }
}
