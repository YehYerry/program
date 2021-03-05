using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestSQL
{
    public partial class Form1 : Form
    {
        string dbHost = "localhost";//資料庫位址
        string dbUser = "root";//資料庫使用者帳號
        string dbPass = "jerry127532";//資料庫使用者密碼
        string dbName = "testbase";//資料庫名稱
        string[] line = new string[1000];
        string USRID = "jerry";
        string[] Call_DT = new string[1000];
        string[] Call_TM = new string[1000];
        string[] Controler = new string[1000];
        string[] Call_num = new string[1000];
        int ctr = 0;


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();
            conn.Open();
            String email,password,address,cellphone,create_date;
            int level;
            for (int i = 0; i < 10; i++)
            {
                email = "123@123";
                address = "台北";
                cellphone = "123456789";
                create_date = DateTime.Now.Year.ToString()+"-"+ DateTime.Now.Month.ToString()+"-" + DateTime.Now.Day.ToString();
                password = "password" + i.ToString();
                level = i * 10;
                command.CommandText = "Insert into memberaccount2019(email,password,address,cellphone,create_date) values('" + email + "','" + password + "','" + address + "'," + cellphone + ",'" + create_date + "')";
                command.ExecuteNonQuery();
            }
            Console.ReadLine();
            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();
            conn.Open();
            command.CommandText = "Delete FROM memberaccount2019 WHERE id=15";
            int n = command.ExecuteNonQuery();

            Console.WriteLine("共刪除 {0} 筆資料", n);
            Console.ReadLine();
            conn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();
            conn.Open();
            command.CommandText = "Update memberaccount2019 SET password='1234' WHERE id='1'";
            command.ExecuteNonQuery();


            Console.ReadLine();
            conn.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();
            conn.Open();

            StreamReader str = new StreamReader(Application.StartupPath + @"\Log\callnum_log\2020920.txt");//讀取文字檔
            do
            {
                ctr++;
                line[ctr] = str.ReadLine();
                Console.WriteLine(line[ctr]);
            } while (line[ctr] != null);

            Call_DT[1] = line[1];

            for (int i = 1; i < ctr; i++)
            {
                Call_DT[i] = line[i].Substring(0,8);
                Call_TM[i] = line[i].Substring(10,11);
                Controler[i] = line[i].Substring(29, 1);
                Call_num[i] = line[i].Substring(37, 4);

                Console.WriteLine("Insert into callnum_log(RowID,USRID,Call_DT,Call_TM,Controler,Call_num) values(" + i + ",'" + USRID + "','" + Call_DT[i] + "','" + Call_TM[i] + "'," + Controler[i] + ",'" + Call_num[i] + "')");
                command.CommandText = "Insert into callnum_log(RowID,USRID,Call_DT,Call_TM,Controler,Call_num) values(" + i + ",'" + USRID + "','" + Call_DT[i] + "','" + Call_TM[i] + "'," + Controler[i] + ",'" + Call_num[i] + "')";
                command.ExecuteNonQuery();
                Console.WriteLine(Call_DT[i]);
                Console.WriteLine(Call_TM[i]);
                Console.WriteLine(Controler[i]);
                Console.WriteLine(Call_num[i]);
            }
            Console.ReadLine();
            conn.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();
            conn.Open();

            var commandStr = "CREATE TABLE IF NOT EXISTS callnum_log (RowID INTEGER ,USRID nvarchar(50), Call_DT nvarchar(50), Call_TM nvarchar(50), Controler int(32), Call_num nvarchar(50));";
            command.CommandText = commandStr;
            command.ExecuteNonQuery();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();
            conn.Open();
            command.CommandText = "DROP TABLE callnum_log;";
            int n = command.ExecuteNonQuery();

            Console.WriteLine("共刪除 {0} 筆資料", n);
            Console.ReadLine();
            conn.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

            MySqlCommand command = new MySqlCommand(
                 "SELECT * FROM callnum_log;",
                 conn);

            MySqlDataReader reader = command.ExecuteReader();
            //使用 NextResult 來取出多個結果集
            if (reader.HasRows)
            {
                //Console.WriteLine("\t{0}\t{1}\t{2}\t{3}", reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3));
                DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
                columnHeaderStyle.BackColor = Color.Beige;
                columnHeaderStyle.Font = new Font("Verdana", 10, FontStyle.Bold);
                dataGridView1.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
                if (reader.HasRows)
                {
                    dataGridView1.Visible = true;
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    MessageBox.Show(dt.ToString());
                    dataGridView1.DataSource = dt;
                }
            }
        }
    }
}
