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
using MySql.Data.MySqlClient;
using System.Data;

namespace manage_agents
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        MySqlConnection conn;
        public Window1()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            conn = new MySqlConnection("uid = root; pwd = щтекщще; server = localhost; database = manage_agents");
            conn.Open();

            if (string.IsNullOrEmpty(textBox.Text) | string.IsNullOrEmpty(textBox1.Text) | string.IsNullOrEmpty(textBox2.Text))
                MessageBox.Show("Заполните обязательные поля!");
            else
            {
                if (textBox3.Text == "")
                    textBox3.Text = "0";
                MySqlCommand command = new MySqlCommand($"INSERT INTO rialtori(fam, name, otch, deal) values ('{textBox.Text}', '{textBox1.Text}', '{textBox2.Text}', '{textBox3.Text}')", conn);
                MySqlDataReader reader = command.ExecuteReader();
                reader.Close();
                Exit();
            }

        }

        private void Exit()
        {
            MainWindow win = new MainWindow();
            this.Close();
            win.ShowDialog();
            conn.Close();
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Exit();
        }
    }
}
