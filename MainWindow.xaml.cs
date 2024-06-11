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
            currentUserWorkplace = 0;
            greetingsText = tGreetings;
            greetingsBlock = panelGreetings;
            DataContext = this;
            try
            {
                var sr = new StreamReader("../connect.txt");        // Поскольку используемое облако не предполагает свободного размещения файлов, временно помещаем в папке
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
                // Данный блок кода необходим для упрощения тестирования. К релизу вложенный try-catch удаляется, создание файла переносится во внешний catch
                // Если файл настроек не найден, попытается подключиться с дефолтными настройками.
                try
                {
                    DBControl.Connect("192.168.1.214", "5432", "postgres", "libraryis", "1234");
                    AppFrame.Navigate(PageControl.PageAuth);
                }
                catch
                {
                    // Если и это не удалось - администратору необходимо создать файл настроек.
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
            AppFrame.Navigate(PageControl.PageAddIssue);
            PageControl.pIssuance = null;
            if (greetingsText != null && greetingsBlock != null)
            {
                greetingsText.Text = "";
                greetingsBlock.Visibility = Visibility.Hidden;
            }
            else return;
            currentUserWorkplace = 0;
            currentLibrary = null;
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
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Department\" (code, organization, name) VALUES (3, '1024500521066', 'Отраслевой литературы')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Department\" (code, organization, name) VALUES (4, '1024500521066', 'Краеведческой литературы')");
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

                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Employee\" (\"TIN\", surname, firstname, patronymic, phone, workplace, position, role, login, hashedpass, salt) " +
                                                             "VALUES ('450743882234', 'Грачева', 'Ольга', 'Павловна', '+7(906)673-11-21', 3, 'Руководитель', " +
                                                                     "'Сотрудник библиотечного фонда', 'GrOP21', @pass, 'a30d')");
                try
                {
                    cmdLvL2.Parameters.AddWithValue("@pass", NpgsqlDbType.Varchar, PasswordGeneration.GenerateHash("08xa521N", "a30d"));
                    cmdLvL2.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Ошибочка вышла");
                    return;
                }

                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Employee\" (\"TIN\", surname, firstname, patronymic, phone, workplace, position, role, login, hashedpass, salt) " +
                                                             "VALUES ('450743993345', 'Феоктистова', 'Анна', 'Сергеевна', '+7(909)128-65-65', 4, 'Руководитель', " +
                                                                     "'Сотрудник библиотечного фонда', 'FAnS71', @pass, 'o9x4')");
                try
                {
                    cmdLvL2.Parameters.AddWithValue("@pass", NpgsqlDbType.Varchar, PasswordGeneration.GenerateHash("XArnYeav", "o9x4"));
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
                NpgsqlCommand cmdLvL2 = DBControl.GetCommand("INSERT INTO \"LibraryClassification\" (index, industry) VALUES ('84(2)', 'Литература России')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"LibraryClassification\" (index, industry) VALUES ('84(4)', 'Литература Европы')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"LibraryClassification\" (index, industry) VALUES ('84(6)', 'Литература Африки')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"LibraryClassification\" (index, industry) VALUES ('81.411.2-4', 'Русский язык. Словари')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"LibraryClassification\" (index, industry) VALUES ('65.32', 'Экономика сельского хозяйства')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"LibraryClassification\" (index, industry) VALUES ('84(5)', 'Литература Азии')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"LibraryClassification\" (index, industry) VALUES ('16', 'Информатика и информационные технологии')");
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
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"DecimalClassification\" (index, industry) VALUES ('82-2', 'Драматургия. Пьесы, либретто, сценарии')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"DecimalClassification\" (index, industry) VALUES ('82-4', 'Очерки, эссе')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"DecimalClassification\" (index, industry) VALUES ('82-9', 'Прочие литературные жанры')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"DecimalClassification\" (index, industry) VALUES ('332', 'Территориальная экономика. Аграрный вопрос. Жилищное хозяйство')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"DecimalClassification\" (index, industry) VALUES ('811', 'Языки естественные и искусственные')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"DecimalClassification\" (index, industry) VALUES ('004', 'Информационные технологии. Теория вычислительных машин и систем')");
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
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Publisher\" (\"OGRN\", name) VALUES ('1077761055460', 'ИСТАРИ КОМИКС')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Publisher\" (\"OGRN\", name) VALUES ('1147746296532', 'ПРОСВЕЩЕНИЕ')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Publisher\" (\"OGRN\", name) VALUES ('1157746848478', 'РОСМЭН')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Publisher\" (\"OGRN\", name) VALUES ('1022201769721', 'АЛТАПРЕСС')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Publisher\" (\"OGRN\", name) VALUES ('1147746712651', 'Bubble Comics')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Publisher\" (\"OGRN\", name) VALUES ('1203600007401', 'РЕАНИМЕДИА ПАБЛИШИНГ')");
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
                NpgsqlCommand cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Author\" (code, surname, firstname, patronymic, photo) VALUES (1, 'Лермонтов', 'Михаил', 'Юрьевич', '/pic/no_image.png')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Author\" (code, surname, firstname, patronymic, photo) VALUES (2, 'Акутами', 'Гэгэ', '', '/pic/no_image.png')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Author\" (code, surname, firstname, patronymic, photo) VALUES (3, 'Бархударов', 'Степан', 'Григорьевич', '/pic/no_image.png')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Author\" (code, surname, firstname, patronymic, photo) VALUES (4, 'Кузнецова', 'Надежда', 'Анатольевна', '/pic/no_image.png')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Author\" (code, surname, firstname, patronymic, photo) VALUES (5, 'Сапковский', 'Анджей', '', '/pic/no_image.png')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Author\" (code, surname, firstname, patronymic, photo) VALUES (6, 'Тьюринг', 'Алан', '', '/pic/no_image.png')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Author\" (code, surname, firstname, patronymic, photo) VALUES (7, 'Курцвейл', 'Рэймонд', '', '/pic/no_image.png')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Author\" (code, surname, firstname, patronymic, photo) VALUES (8, 'Олифер', 'Виктор', '', '/pic/no_image.png')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Author\" (code, surname, firstname, patronymic, photo) VALUES (9, 'Эмис', 'Мартин', '', '/pic/no_image.png')");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Author\" (code, surname, firstname, patronymic, photo) VALUES (10, 'Нейман', 'Джон', '', '/pic/no_image.png')");
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
                NpgsqlCommand cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Issue\" (identifier, name, type, \"BBK\", \"UDK\", year, publisher, pagecount, storage, amount, annotation, image, keyword, authorsign) " +
                                                             "VALUES ('978-5-87-107963-8', 'Демон', 'Книжное', '84(2)', '82-3', 2015, '1117746849648', 352, 2, 6, '', '/pic/no_image.png', '', 'Л32')");
                cmdLvL2.ExecuteNonQuery();

                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Issue\" (identifier, name, type, \"BBK\", \"UDK\", year, publisher, pagecount, storage, amount, annotation, image, keyword, authorsign) " +
                                                             "VALUES ('978-5-389-22224-3', 'Таланты умирают молодыми', 'Книжное', '84(5)', '82-9', 2023, '5077746791634', 127, 2, 1, '', '/pic/no_image.png', '', 'А77')");
                cmdLvL2.ExecuteNonQuery();

                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Issue\" (identifier, name, type, \"BBK\", \"UDK\", year, publisher, pagecount, storage, amount, annotation, image, keyword, authorsign) " +
                                                             "VALUES ('978-5-389-24137-4', 'Перелётный гусь', 'Книжное', '84(5)', '82-9', 2023, '5077746791634', 119, 2, 1, '', '/pic/no_image.png', '', 'А77')");
                cmdLvL2.ExecuteNonQuery();

                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Issue\" (identifier, name, type, \"BBK\", \"UDK\", year, publisher, pagecount, storage, amount, annotation, image, keyword, authorsign) " +
                                                             "VALUES ('978-5-389-22224-2', 'В преддверии праздника', 'Книжное', '84(5)', '82-9', 2023, '5077746791634', 122, 2, 2, '', '/pic/no_image.png', '', 'А77')");
                cmdLvL2.ExecuteNonQuery();

                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Issue\" (identifier, name, type, \"BBK\", \"UDK\", year, publisher, pagecount, storage, amount, annotation, image, keyword, authorsign) " +
                                                             "VALUES ('978-5-488-01677-4', 'Орфографический словарь русского языка: 106000 слов', 'Книжное', '81.411.2-4', '811', 2004, '1147746296532', 478, 3, 12, '', '/pic/no_image.png', 'русский язык', 'Б49')");
                cmdLvL2.ExecuteNonQuery();

                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Issue\" (identifier, name, type, \"BBK\", \"UDK\", year, publisher, pagecount, storage, amount, annotation, image, keyword, authorsign) " +
                                                             "VALUES ('978-5-117-06232-4', 'Развитие системы сельскохозяйственных потребительских кооперативов', 'Книжное', '65.32', '332', 2004, '1022201769721', 99, 4, 3, '', '/pic/no_image.png', 'сельское хозяйство', 'К37')");
                cmdLvL2.ExecuteNonQuery();

                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Issue\" (identifier, name, type, \"BBK\", \"UDK\", year, publisher, pagecount, storage, amount, annotation, image, keyword, authorsign) " +
                                                             "VALUES ('978-5-17-105970-5', 'Вычислительные машины и разум', 'Книжное', '16', '004', 2018, '1117746849648', 128, 3, 5, '', '/pic/no_image.png', 'информатика', 'Т82')");
                cmdLvL2.ExecuteNonQuery();

                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Issue\" (identifier, name, type, \"BBK\", \"UDK\", year, publisher, pagecount, storage, amount, annotation, image, keyword, authorsign) " +
                                                             "VALUES ('978-5-9710-2758-4', 'Может ли машина мыслить?', 'Книжное', '16', '004', 2016, '1147746296532', 128, 3, 7, '', '/pic/no_image.png', 'информатика', 'Т82')");
                cmdLvL2.ExecuteNonQuery();

                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Issue\" (identifier, name, type, \"BBK\", \"UDK\", year, publisher, pagecount, storage, amount, annotation, image, keyword, authorsign) " +
                                                             "VALUES ('978-5-04-111270-7', 'Эволюция разума', 'Книжное', '16', '004', 2020, '1022201769721', 448, 3, 4, '', '/pic/no_image.png', 'информатика', 'К46')");
                cmdLvL2.ExecuteNonQuery();

                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Issue\" (identifier, name, type, \"BBK\", \"UDK\", year, publisher, pagecount, storage, amount, annotation, image, keyword, authorsign) " +
                                                             "VALUES ('978-5-4461-1426-9', 'Компьютерные сети', 'Книжное', '16', '004', 2020, '1022201769721', 1008, 3, 9, '', '/pic/no_image.png', 'информатика', 'О127')");
                cmdLvL2.ExecuteNonQuery();

                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Issue\" (identifier, name, type, \"BBK\", \"UDK\", year, publisher, pagecount, storage, amount, annotation, image, keyword, authorsign) " +
                                                             "VALUES ('978-5-699-27868-8', 'Информация', 'Книжное', '16', '004', 2008, '1027739148656', 576, 3, 3, '', '/pic/no_image.png', 'информатика', 'Э22')");
                cmdLvL2.ExecuteNonQuery();

                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Issue\" (identifier, name, type, \"BBK\", \"UDK\", year, publisher, pagecount, storage, amount, annotation, image, keyword, authorsign) " +
                                                             "VALUES ('978-5-17-148015-8', 'Вычислительная машина и мозг', 'Книжное', '16', '004', 2022, '1027739148656', 192, 3, 3, '', '/pic/no_image.png', 'информатика', 'Н91')");
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
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Authorship\" (issue, author) VALUES ('978-5-389-22224-3', 2)");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Authorship\" (issue, author) VALUES ('978-5-389-24137-4', 2)");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Authorship\" (issue, author) VALUES ('978-5-389-22224-2', 2)");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Authorship\" (issue, author) VALUES ('978-5-488-01677-4', 3)");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Authorship\" (issue, author) VALUES ('978-5-117-06232-4', 4)");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Authorship\" (issue, author) VALUES ('978-5-17-105970-5', 6)");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Authorship\" (issue, author) VALUES ('978-5-9710-2758-4', 6)");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Authorship\" (issue, author) VALUES ('978-5-04-111270-7', 7)");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Authorship\" (issue, author) VALUES ('978-5-4461-1426-9', 8)");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Authorship\" (issue, author) VALUES ('978-5-699-27868-8', 9)");
                cmdLvL2.ExecuteNonQuery();
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Authorship\" (issue, author) VALUES ('978-5-17-148015-8', 10)");
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
                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"SocialStatus\" (status) VALUES ('Нетрудоустроенный')");
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

                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Client\" (libcard, surname, firstname, patronymic, phone, socialstatus, debt, birthyear) " +
                                                             "VALUES ('132058', 'Бабаев', 'Анатолий', 'Игоревич', '+7(906)111-82-23', 'Пенсионер', false, 1961)");
                cmdLvL2.ExecuteNonQuery();

                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Client\" (libcard, surname, firstname, patronymic, phone, socialstatus, debt, birthyear) " +
                                                             "VALUES ('132059', 'Файзрахманова', 'Екатерина', 'Васильевна', '+7(900)654-32-74', 'Трудящийся', false, 1994)");
                cmdLvL2.ExecuteNonQuery();

                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Client\" (libcard, surname, firstname, patronymic, phone, socialstatus, debt, birthyear) " +
                                                             "VALUES ('132060', 'Гарина', 'Галина', 'Андреевна', '+7(912)672-10-23', 'Пенсионер', false, 1957)");
                cmdLvL2.ExecuteNonQuery();

                cmdLvL2 = DBControl.GetCommand("INSERT INTO \"Client\" (libcard, surname, firstname, patronymic, phone, socialstatus, debt, birthyear) " +
                                                             "VALUES ('132061', 'Кижичев', 'Олег', 'Витальевич', '+7(902)105-05-62', 'Учащийся', false, 2005)");
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
