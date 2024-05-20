using Npgsql;
using NpgsqlTypes;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibrISv2
{
    public partial class MainWindow : Window
    {
        public static TextBlock greetingsText;
        public static StackPanel greetingsBlock;
        public static int currentUserWorkplace;
        public static string currentLibrary;

        public static string server = "";
        public static string port = "";
        public static string user = "";
        public static string database = "";
        public static string password = "";

        public MainWindow()
        {
            InitializeComponent();
            greetingsText = tGreetings;
            greetingsBlock = panelGreetings;
            DataContext = this;
            try
            {
                var sr = new StreamReader("../connect.txt");
                string str = sr.ReadToEnd();
                string[] config = str.Split('|');
                server = config[0];
                port = config[1];
                user = config[2];
                database = config[3];
                password = config[4];
                DBControl.Connect(server, port, user, database, password);
                sr.Close();
                AppFrame.Navigate(PageControl.PageAuth);
            }
            catch
            {
                // Если файл настроек не найден, попытается подключиться с дефолтными настройками
                try
                {
                    DBControl.Connect("localhost", "5432", "postgres", "LibraryIS", "1234");
                    AppFrame.Navigate(PageControl.PageAuth);
                }
                catch
                {
                    // Если и это не удалось - администратору необходимо создать файл настроек
                    MessageBox.Show("Не удалось установить соединение, поскольку отсутствует файл настроек.\n " +
                                    "Необходимо ввести данные для подключения вручную.\n " +
                                    "Если эти данные вам не известны, обратитесь к администратору");
                    AppFrame.Navigate(PageControl.PageConnectionProperties);
                    return;
                }
            }
            AddCity();
            AddLibrary();
            AddDepartment();
            AddRole();
            AddEmployee();
            AddIssueType();
            AddBBK();
            AddUDK();
            AddPublisher();
            AddAuthor();
            AddIssue();
            AddAuthorship();
            AddSocialStatus();
            AddClient();
            AddOperationStatus();
        }
        public static void DisplayGreetings(string name, string patronymic)
        {
            if (greetingsText != null && greetingsBlock != null)
            {
                greetingsBlock.Visibility = Visibility.Visible;
                greetingsText.Text = "Здравствуйте, " + name + " " + patronymic + "!    ";
            }
            else return;
        }
        private void bLogOut_Click(object sender, RoutedEventArgs e)
        {
            if (greetingsText != null && greetingsBlock != null)
            {
                greetingsText.Text = "";
                greetingsBlock.Visibility = Visibility.Hidden;
            }
            else return;
            currentLibrary = null;
            currentUserWorkplace = 0;
            PageControl.PageAuth.tbLogin.Text = "Логин";
            PageControl.PageAuth.tbLogin.Foreground = Brushes.LightSlateGray;
            PageControl.PageAuth.tbPassword.Password = string.Empty;
            PageControl.PageAuth.tbPassTip.Visibility = Visibility.Visible;
            AppFrame.Navigate(PageControl.PageAuth);
        }

        // Заполнение тестовыми данными
        private void AddCity()
        {
            NpgsqlCommand cmdLvL1 = DBControl.GetCommand("SELECT * FROM \"City\"");
            NpgsqlDataReader reader = cmdLvL1.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                NpgsqlCommand cmdLvL2 = DBControl.GetCommand("INSERT INTO \"City\" (code, name) VALUES ('3522', 'Курган')");
                cmdLvL2.ExecuteNonQuery();
            }
            else reader.Close();
        }
        private void AddLibrary()
        {
            NpgsqlCommand cmdLvL1 = DBControl.GetCommand("SELECT * FROM \"Library\"");
            NpgsqlDataReader reader = cmdLvL1.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                NpgsqlCommand cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Library\" (\"OGRN\", \"TIN\", \"CoR\", name, city, address) " +
                                                             "VALUES ('1024500521066', '4501009308', '450101001', 'ЦГБ им. Маяковского', '3522', 'Пролетарская, 41')");
                cmdLvL2.ExecuteNonQuery();
            }
            else reader.Close();
        }
        private void AddDepartment()
        {
            NpgsqlCommand cmdLvL1 = DBControl.GetCommand("SELECT * FROM \"Department\"");
            NpgsqlDataReader reader = cmdLvL1.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                NpgsqlCommand cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Department\" (code, organization, name) VALUES (1, '1024500521066', 'Администрирования')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Department\" (code, organization, name) VALUES (2, '1024500521066', 'Художественной литературы')");
                cmdLvL2.ExecuteNonQuery();
            }
            else reader.Close();
        }
        private void AddRole()
        {
            NpgsqlCommand cmdLvL1 = DBControl.GetCommand("SELECT * FROM \"Role\"");
            NpgsqlDataReader reader = cmdLvL1.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                NpgsqlCommand cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Role\" (role) VALUES ('Администратор')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Role\" (role) VALUES ('Сотрудник библиотечного фонда')");
                cmdLvL2.ExecuteNonQuery();
            }
            else reader.Close();
        }
        private void AddEmployee()
        {
            NpgsqlCommand cmdLvL1 = DBControl.GetCommand("SELECT * FROM \"Employee\"");
            NpgsqlDataReader reader = cmdLvL1.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                NpgsqlCommand cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Employee\" (\"TIN\", surname, firstname, patronymic, phone, workplace, position, role, login, hashedpass, salt) " +
                                                             "VALUES ('450743982027', 'Хорина', 'Наталья', 'Игоревна', '+7(908)892-11-74', 1, 'Администратор', " +
                                                                     "'Администратор', 'HorNI27', @pass, 'nSM@')");
                try
                {
                    cmdLvL2.Parameters.AddWithValue("@pass", NpgsqlDbType.Varchar, PasswordGeneration.GenerateHash("#8hHYUB6", "nSM@"));
                    cmdLvL2.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Ошибочка вышла");
                    return;
                }

                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Employee\" (\"TIN\", surname, firstname, patronymic, phone, workplace, position, role, login, hashedpass, salt) " +
                                                             "VALUES ('450743771123', 'Иванова', 'Любовь', 'Фёдоровна', '+7(919)582-11-47', 2, 'Руководитель', " +
                                                                     "'Сотрудник библиотечного фонда', 'IvaLF23', @pass, 'viF&')");
                try
                {
                    cmdLvL2.Parameters.AddWithValue("@pass", NpgsqlDbType.Varchar, PasswordGeneration.GenerateHash("R3Nj$wj8", "viF&"));
                    cmdLvL2.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Ошибочка вышла");
                    return;
                }
            }
            else reader.Close();
        }
        private void AddIssueType()
        {
            NpgsqlCommand cmdLvL1 = DBControl.GetCommand("SELECT * FROM \"IssueType\"");
            NpgsqlDataReader reader = cmdLvL1.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                NpgsqlCommand cmdLvL2 = DBControl.GetCommand("INSERT INTO \"IssueType\" (type) VALUES ('Книжное')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"IssueType\" (type) VALUES ('Периодическое')");
                cmdLvL2.ExecuteNonQuery();
            }
            else reader.Close();
        }
        private void AddBBK()
        {
            NpgsqlCommand cmdLvL1 = DBControl.GetCommand("SELECT * FROM \"LibraryClassification\"");
            NpgsqlDataReader reader = cmdLvL1.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                NpgsqlCommand cmdLvL2 = DBControl.GetCommand("INSERT INTO \"LibraryClassification\" (index, industry) VALUES ('84(2)4', 'Древнерусская (IX - XVIII вв.)')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"LibraryClassification\" (index, industry) VALUES ('84(2)5', 'Новое время (XVIII в. - 1917 г.)')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"LibraryClassification\" (index, industry) VALUES ('84(2)6', 'Новейшее время (с октября 1917 г.)')");
                cmdLvL2.ExecuteNonQuery();
            }
            else reader.Close();
        }
        private void AddUDK()
        {
            NpgsqlCommand cmdLvL1 = DBControl.GetCommand("SELECT * FROM \"DecimalClassification\"");
            NpgsqlDataReader reader = cmdLvL1.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                NpgsqlCommand cmdLvL2 = DBControl.GetCommand("INSERT INTO \"DecimalClassification\" (index, industry) VALUES ('82-1', 'Поэзия. Стихи, оды, поэмы, баллады и т.д.')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"DecimalClassification\" (index, industry) VALUES ('82-3', 'Художественная проза. Роман, новелла, рассказ и т.д.')");
                cmdLvL2.ExecuteNonQuery();
            }
            else reader.Close();
        }
        private void AddPublisher()
        {
            NpgsqlCommand cmdLvL1 = DBControl.GetCommand("SELECT * FROM \"Publisher\"");
            NpgsqlDataReader reader = cmdLvL1.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                NpgsqlCommand cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Publisher\" (\"OGRN\", name) VALUES ('1117746849648', 'ACT')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Publisher\" (\"OGRN\", name) VALUES ('5077746791634', 'Азбука-аттикус')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Publisher\" (\"OGRN\", name) VALUES ('1027739148656', 'Эксмо')");
                cmdLvL2.ExecuteNonQuery();
            }
            else reader.Close();
        }
        private void AddAuthor()
        {
            NpgsqlCommand cmdLvL1 = DBControl.GetCommand("SELECT * FROM \"Author\"");
            NpgsqlDataReader reader = cmdLvL1.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                NpgsqlCommand cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Author\" (code, surname, firstname, patronymic, photo) " +
                                                             "VALUES (1, 'Лермонтов', 'Михаил', 'Юрьевич', '/pic/no_image.png')");
                cmdLvL2.ExecuteNonQuery();
            }
            else reader.Close();
        }
        private void AddIssue()
        {
            NpgsqlCommand cmdLvL1 = DBControl.GetCommand("SELECT * FROM \"Issue\"");
            NpgsqlDataReader reader = cmdLvL1.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                NpgsqlCommand cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Issue\" (identifier, name, type, \"BBK\", \"UDK\", year, publisher, pagecount, storage, " +
                                                                                    "amount, annotation, image, keyword, authorsign) " +
                                                             "VALUES ('978-5-87-107963-8', 'Демон', 'Книжное', '84(2)5', '82-3', 2015, '5077746791634', 352, 2, " +
                                                                      "6, '', '/pic/no_image.png', '', 'Л32')");
                cmdLvL2.ExecuteNonQuery();
            }
            else reader.Close();
        }
        private void AddAuthorship()
        {
            NpgsqlCommand cmdLvL1 = DBControl.GetCommand("SELECT * FROM \"Authorship\"");
            NpgsqlDataReader reader = cmdLvL1.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                NpgsqlCommand cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Authorship\" (issue, author) VALUES ('978-5-87-107963-8', 1)");
                cmdLvL2.ExecuteNonQuery();
            }
            else reader.Close();
        }
        private void AddSocialStatus()
        {
            NpgsqlCommand cmdLvL1 = DBControl.GetCommand("SELECT * FROM \"SocialStatus\"");
            NpgsqlDataReader reader = cmdLvL1.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                NpgsqlCommand cmdLvL2 = DBControl.GetCommand("INSERT INTO \"SocialStatus\" (status) VALUES ('Учащийся')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"SocialStatus\" (status) VALUES ('Трудящийся')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"SocialStatus\" (status) VALUES ('Пенсионер')");
                cmdLvL2.ExecuteNonQuery();
            }
            else reader.Close();
        }
        private void AddClient()
        {
            NpgsqlCommand cmdLvL1 = DBControl.GetCommand("SELECT * FROM \"Client\"");
            NpgsqlDataReader reader = cmdLvL1.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                NpgsqlCommand cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Client\" (libcard, surname, firstname, patronymic, phone, socialstatus, debt, birthyear) " +
                                                             "VALUES ('132057', 'Иванчук', 'Софья', 'Андреевна', '+7(908)941-07-07', 'Учащийся', false, 2000)");
                cmdLvL2.ExecuteNonQuery();
            }
            else reader.Close();
        }
        private void AddOperationStatus()
        {
            NpgsqlCommand cmdLvL1 = DBControl.GetCommand("SELECT * FROM \"OperationStatus\"");
            NpgsqlDataReader reader = cmdLvL1.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                NpgsqlCommand cmdLvL2 = DBControl.GetCommand("INSERT INTO \"OperationStatus\" (status) VALUES ('Возвращено')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"OperationStatus\" (status) VALUES ('Выдано')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"OperationStatus\" (status) VALUES ('Забронировано')");
                cmdLvL2.ExecuteNonQuery();
            }
            else reader.Close();
        }
    }
}
