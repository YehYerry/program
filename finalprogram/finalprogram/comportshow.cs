using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace finalprogram
{
    public partial class comportshow : Form
    {
        string port;
        public comportshow()
        {
            InitializeComponent();
            this.comboBox1.Items.AddRange(SerialPort.GetPortNames());
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            port = comboBox1.Text;
            label1.Text = "已選取" + port;
        }
    }
}
