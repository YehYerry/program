using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using System.Data.SqlClient;

namespace TestReport
{
    public partial class Form1 : Form
    {
        int id = 2;
        int i = 0;
        string a1, a2, a3, a4;
        /*string[] a1 = new string[100];
        string[] a2 = new string[100];
        string[] a3 = new string[100];
        string[] a4 = new string[100];*/
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var fileName = "C:/Users/jerry/github/program/TestReport/testDB.db";
           // SQLiteConnection.CreateFile(fileName);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string databaseFileName = "C:/Users/jerry/github/program/TestReport/testDB.db";
            string connectionString = "data source = " + databaseFileName;
            SQLiteConnection dbConnection = new SQLiteConnection(connectionString);
            dbConnection.Open();
            string[] colNames = new string[] { "ID", "Name", "Age", "Email" };
            string[] colTypes = new string[] { "INTEGER", "TEXT", "INTEGER", "TEXT" };

            string tableName = "table1";
            string str1 = "test", str2 = "@gmail";
            int b1 = 4, a2 = 15;

            string queryString = "CREATE TABLE IF NOT EXISTS " + tableName + "( " + colNames[0] + " " + colTypes[0];
            //string queryString1 = "INSERT INTO table1(ID,name,age,email)VALUES(@param1,@param2,@param3,@param4)";
            for (int i = 1; i < colNames.Length; i++)
            {
                queryString += ", " + colNames[i] + " " + colTypes[i];
            }
            queryString += "  ) ";
            SQLiteCommand dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandText = queryString;
            dbCommand.CommandText = "INSERT INTO table1(ID,name,age,email)VALUES(@param1,@param2,@param3,@param4)";
            dbCommand.Parameters.AddWithValue("@param1", b1);
            dbCommand.Parameters.AddWithValue("@param2", str1);
            dbCommand.Parameters.AddWithValue("@param3", a2);
            dbCommand.Parameters.AddWithValue("@param4", str2);
            dbCommand.ExecuteNonQuery();
            id += 1;
        }
        private void HasRows(SQLiteConnection connection)
        {
            using (connection)
            {
                SQLiteCommand command = new SQLiteCommand(
                  "SELECT * FROM table1;",
                  connection);

                SQLiteDataReader reader = command.ExecuteReader();
                //使用 NextResult 來取出多個結果集
                if (reader.HasRows)
                {
                    Console.WriteLine("\t{0}\t{1}\t{2}\t{3}", reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3));
                    // Create an unbound DataGridView by declaring a column count.
                    dataGridView1.ColumnCount = 4;
                    dataGridView1.ColumnHeadersVisible = true;
                    // Set the column header style.
                    DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
                    columnHeaderStyle.BackColor = Color.Beige;
                    columnHeaderStyle.Font = new Font("Verdana", 10, FontStyle.Bold);
                    dataGridView1.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
                    dataGridView1.Columns[0].Name = reader.GetName(0);
                    dataGridView1.Columns[1].Name = reader.GetName(1);
                    dataGridView1.Columns[2].Name = reader.GetName(2);
                    dataGridView1.Columns[3].Name = reader.GetName(3);
                    dataGridView1.Visible = true;
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    MessageBox.Show(dt.ToString());
                    dataGridView1.DataSource = dt;

                    string[] row5 = new string[] { a1, a2, a3, a4 };
                    object[] rows = new object[] { row5 };
                    while (reader.Read())
                    {
                        /*for (i=0; i <= 3; i++)
                        {
                            //Console.WriteLine("\t{0}\t{1}\t{2}\t{3}", reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetString(3));
                            //Console.WriteLine(reader.GetInt32(0).ToString());
                            a1 = reader.GetInt32(0).ToString();
                            //Console.WriteLine(reader.GetString(1));
                            a2 = reader.GetString(1);
                            a3 = reader.GetInt32(2).ToString();
                            a4 = reader.GetString(3);
                        }*/
                        if (!reader[0].Equals(DBNull.Value))
                        {
                            listBox1.Items.Add(reader[0].ToString() + "\t" + reader[1].ToString() + "\t" + reader[2].ToString() + "\t" + reader[3].ToString());
                        }
                    }
                    reader.NextResult();
                    /*string[] row1 = new string[] { a1[0], a2[0], a3[0], a4[0] };
                    string[] row2 = new string[] { a1[1], a2[1], a3[1], a4[1] };
                    string[] row3 = new string[] { a1[2], a2[2], a3[2], a4[2] };
                    string[] row4 = new string[] { a1[3], a2[3], a3[3], a4[3] };*/
                    /*string[] row5 = new string[] { a1, a2, a3, a4 };
                    object[] rows = new object[] { row5 };
                    foreach (string[] rowArray in rows)
                    {
                        dataGridView1.Rows.Add(rowArray);
                    }*/

                    /*while ((reader.Read()))

                    {

                        if (!reader[0].Equals(DBNull.Value))

                        {

                            listBox2.Items.Add(reader[0].ToString() + "\t" + reader[1].ToString() + "\t" + reader[2].ToString() + "\t" + reader[3].ToString());

                        }

                    }*/

                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                //取得架構資訊
                /*DataTable schemaTable = reader.GetSchemaTable();

                foreach (DataRow row in schemaTable.Rows)
                {
                    foreach (DataColumn column in schemaTable.Columns)
                    {
                        Console.WriteLine(String.Format("{0} = {1}", column.ColumnName, row[column]));
                    }
                }*/
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string databaseFileName = "C:/Users/jerry/github/program/TestReport/testDB.db";
            string connectionString = "data source = " + databaseFileName;
            SQLiteConnection dbConnection = new SQLiteConnection(connectionString);
            dbConnection.Open();
            //HasRows(dbConnection);
            
            SQLiteCommand command = new SQLiteCommand(
                 "SELECT * FROM callnum_log;",
                 dbConnection);

            SQLiteDataReader reader = command.ExecuteReader();
            //使用 NextResult 來取出多個結果集
            if (reader.HasRows)
            {
                Console.WriteLine("\t{0}\t{1}\t{2}\t{3}", reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3));
                // Create an unbound DataGridView by declaring a column count.
                /*dataGridView1.ColumnCount = 4;
                dataGridView1.ColumnHeadersVisible = true;*/
                // Set the column header style.
                DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
                columnHeaderStyle.BackColor = Color.Beige;
                columnHeaderStyle.Font = new Font("Verdana", 10, FontStyle.Bold);
                dataGridView1.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
                /*dataGridView1.Columns[0].Name = reader.GetName(0);
                dataGridView1.Columns[1].Name = reader.GetName(1);
                dataGridView1.Columns[2].Name = reader.GetName(2);
                dataGridView1.Columns[3].Name = reader.GetName(3);*/
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
