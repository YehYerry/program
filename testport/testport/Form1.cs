using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testport
{

    public partial class Form1 : Form
    {
        SerialPort serialPort1 = new SerialPort();
        Byte receivedata ;
        delegate void Display(Byte[] buffer);
        public delegate void ShowData(string s);
        // event object
        public event ShowData ShowDataHandler;
        public Form1()
        {
            InitializeComponent();
        }
        public void opencom()
        {
            try
            {
                //波特率
                serialPort1.BaudRate = 9600;
                //資料位
                serialPort1.DataBits = 8;
                serialPort1.PortName = "COM1";
                //兩個停止位
                serialPort1.StopBits = System.IO.Ports.StopBits.One;
                //無奇偶校驗位
                serialPort1.Parity = System.IO.Ports.Parity.None;
                serialPort1.ReadTimeout = 100;
                serialPort1.Open();
                if (!serialPort1.IsOpen)
                {
                    MessageBox.Show("埠開啟失敗");
                    return;
                }
                else
                {
                    richTextBox1.AppendText("埠COM1開啟成功\r\n");
                }
                serialPort1.DataReceived += serialPort1_DataReceived;
            }
            catch (Exception ex)
            {
                serialPort1.Dispose();
                richTextBox1.AppendText(ex.Message);
            }
        }
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(50);  //（毫秒）等待一定時間，確保資料的完整性 int len        
            int len = serialPort1.BytesToRead;
            
            if (len != 0)
            {
                byte[] buff = new byte[len];
                serialPort1.Read(buff, 0, len);
                //receivedata = Encoding.Default.GetBytes(buff);
                Display d = new Display(DisplayText);
                if (this.IsHandleCreated)
                    this.Invoke(d, new Object[] { buff });
            }
           
            MethodInvoker mi = new MethodInvoker(this.UpdateUI);
            this.BeginInvoke(mi, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.Write(textBox1.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            opencom();
        }
        private void UpdateUI()
        {
            //richTextBox1.AppendText(receivedata + "\r\n");
        }
        private void DisplayText(Byte[] buffer)
        {
           // richTextBox1.AppendText(receivedata + "\r\n");
            richTextBox1.Text += String.Format("{0}{1}", BitConverter.ToString(buffer), Environment.NewLine);
        }
    }
}
