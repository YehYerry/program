using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace clock
{
    public partial class comportshow : Form
    {
        string port;
        public comportshow()
        {
            InitializeComponent();
            StreamReader str = new StreamReader(Application.StartupPath + @"\config.txt");
            string[] s = new string[1000];
            int ctr = 0;
            do
            {
                ctr++;
                s[ctr] = str.ReadLine();
            } while (s[ctr] != null);
            str.Close();
            label1.Text = "已選擇" + s[2];
            comboBox1.Text = s[2];
            comboBox1.Items.AddRange(SerialPort.GetPortNames());
            button1.DialogResult = DialogResult.OK;//設定button1為OK
            button2.DialogResult = DialogResult.Cancel;//設定button2
        }

        private void button1_Click(object sender, EventArgs e)
        {
            port = comboBox1.Text;
            if (port != "")
            {
                StreamReader str = new StreamReader(Application.StartupPath + @"\config.txt");
                string[] s = new string[1000];
                int ctr = 0;
                do
                {
                    ctr++;
                    s[ctr] = str.ReadLine();
                } while (s[ctr] != null);
                str.Close();
                StreamWriter str1 = new StreamWriter(Application.StartupPath + @"\config.txt", false);
                for (int ctr1 = 1; ctr1 <= ctr; ctr1++)
                {
                    if (ctr1 == 2)
                    {
                        str1.WriteLine(port);
                    }
                    else
                    {
                        str1.WriteLine(s[ctr1]);
                    }
                }
                str1.Close();
            }
            else MessageBox.Show("輸入為空值，請重新設定");
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            port = comboBox1.Text;
            label1.Text = "已選取" + port;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
