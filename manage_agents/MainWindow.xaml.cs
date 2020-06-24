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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;

namespace manage_agents
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MySqlConnection conn;
        DataRowView dat;
        int del;
        string f, m, l, d;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void dataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            conn = new MySqlConnection("uid = root; pwd = щтекщще; server = localhost; database = manage_agents");
            conn.Open();
            Load_dat();
        }
        private void Load_dat()
        {

            MySqlCommand command = new MySqlCommand($"select fam, name, otch, idrialtori, CONCAT(fam, ' ', name, ' ', otch) as FIO, deal from rialtori", conn);
            MySqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            dataGrid.ItemsSource = table.DefaultView;
            reader.Close();

            MySqlCommand command1 = new MySqlCommand($"SELECT spros.type, CONCAT(client.fam, ' ', client.name, ' ', client.otchestvo) as client, CONCAT(rialtori.fam, ' ', rialtori.name, ' ', rialtori.otch) as rialtor, spros.address, spros.maxprice FROM spros JOIN rialtori ON spros.rialtor = rialtori.idrialtori JOIN client ON spros.client = client.idclient WHERE rialtor = " + table.Rows[0]["idrialtori"].ToString(), conn);
            MySqlDataReader reader1 = command1.ExecuteReader();
            DataTable table1 = new DataTable();
            table1.Load(reader1);
            dataGrid_Copy1.ItemsSource = table1.DefaultView;
            reader1.Close();

            MySqlCommand command2 = new MySqlCommand($"SELECT CONCAT(supples.Type, ' ', supples.Address) as RealEstate, supples.Price, CONCAT(client.fam, ' ', client.name, ' ', client.otchestvo) as client, CONCAT(rialtori.fam, ' ', rialtori.name, ' ', rialtori.otch) as rialtor FROM supples JOIN rialtori ON supples.Rialtor = rialtori.idrialtori JOIN client ON supples.Client = client.idclient WHERE rialtor = " + table.Rows[0]["idrialtori"].ToString(), conn);
            MySqlDataReader reader2 = command2.ExecuteReader();
            DataTable table2 = new DataTable();
            table2.Load(reader2);
            dataGrid_Copy.ItemsSource = table2.DefaultView;
            reader2.Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Window1 win = new Window1();
            this.Close();
            win.ShowDialog();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dat = dataGrid.SelectedItem as DataRowView;
            if (dat != null)
            {
                MySqlCommand command1 = new MySqlCommand($"SELECT spros.type, CONCAT(client.fam, ' ', client.name, ' ', client.otchestvo) as client, CONCAT(rialtori.fam, ' ', rialtori.name, ' ', rialtori.otch) as rialtor, spros.address, spros.maxprice FROM spros JOIN rialtori ON spros.rialtor = rialtori.idrialtori JOIN client ON spros.client = client.idclient WHERE rialtor = " + dat["idrialtori"].ToString(), conn);
                MySqlDataReader reader1 = command1.ExecuteReader();
                DataTable table1 = new DataTable();
                table1.Load(reader1);
                dataGrid_Copy1.ItemsSource = table1.DefaultView;
                reader1.Close();

                MySqlCommand command2 = new MySqlCommand($"SELECT CONCAT(supples.Type, ' ', supples.Address) as RealEstate, supples.Price, CONCAT(client.fam, ' ', client.name, ' ', client.otchestvo) as client, CONCAT(rialtori.fam, ' ', rialtori.name, ' ', rialtori.otch) as rialtor FROM supples JOIN rialtori ON supples.Rialtor = rialtori.idrialtori JOIN client ON supples.Client = client.idclient WHERE rialtor = " + dat["idrialtori"].ToString(), conn);
                MySqlDataReader reader2 = command2.ExecuteReader();
                DataTable table2 = new DataTable();
                table2.Load(reader2);
                dataGrid_Copy.ItemsSource = table2.DefaultView;
                reader2.Close();

                textBox.Text = dat["fam"].ToString();
                textBox1.Text = dat["name"].ToString();
                textBox2.Text = dat["otch"].ToString();
                textBox3.Text = dat["deal"].ToString();

                if (table1.Rows.Count > 0 & table2.Rows.Count > 0)
                    del = 1;
                else del = 0;
            }
            else Load_dat();
            f = textBox.Text;
            m = textBox1.Text;
            l = textBox2.Text;
            d = textBox3.Text;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (dat != null)
            {
                if (del == 0)
                {
                    MySqlCommand command2 = new MySqlCommand($"delete from rialtori where idrialtori = " + dat["idrialtori"].ToString(), conn);
                    MySqlDataReader reader2 = command2.ExecuteReader();
                    DataTable table2 = new DataTable();
                    table2.Load(reader2);

                    reader2.Close();

                    Load_dat();
                }
                else MessageBox.Show("Нельзя удалить риэлтора, т.к. он состоит в спросе и предложениях!");
            }
            else MessageBox.Show("Выберите риэлтора!");
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (f != textBox.Text | m != textBox1.Text | l != textBox2.Text | d != textBox3.Text)
            {
                MySqlCommand command = new MySqlCommand($"UPDATE rialtori SET fam = '{textBox.Text}', name = '{textBox1.Text}', otch = '{textBox2.Text}', deal = '{textBox3.Text}' WHERE idrialtori = " + dat["idrialtori"], conn);
                MySqlDataReader read = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(read);
                read.Close();
                Load_dat();
            }
        }
    }
}
