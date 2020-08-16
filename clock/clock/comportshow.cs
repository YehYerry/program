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
            this.comboBox1.Items.AddRange(SerialPort.GetPortNames());
            button1.DialogResult = DialogResult.OK;//設定button1為OK
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
            if (port != null)
            {
                serialPort1 = new SerialPort(port, 9600, Parity.None, 8, StopBits.One);
                serialPort1.Open();
                label1.Text = "已開啟COMPORT，完成測試";
                serialPort1.Close();
            }
            else
            {
                MessageBox.Show("請選擇測試的PORT");
            }
        }
    }
}
