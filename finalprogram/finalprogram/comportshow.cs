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
        public comportshow()
        {
            InitializeComponent();
            this.comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.Open();
            label1.Text = "已開啟COMPORT";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            label1.Text = "已關閉COMPORT";
        }
    }
}
