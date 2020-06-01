using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;//延遲

namespace form
{
    public partial class Form1 : Form
    {
        private SerialPort comport;
        delegate void Display(Byte[] buffer);
        private Int32 totalLength = 0;
        byte[] array;
        byte a = 0;
        byte wait1 = 1;
        byte wait2 = 0;        
        int label = 0;
        int countarray = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            comport = new SerialPort("COM6", 9600, Parity.None, 8, StopBits.One);
            comport.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            if (!comport.IsOpen)
            {
                comport.Open();
            }
            MessageBox.Show("開啟");*/
        }

        public void button1_Click(object sender, EventArgs e)
        {
            //byte wait1 = (byte)(a + 1);
            if (wait1 == 10)
            {
                wait1 = 0;
                wait2 = (byte)(wait2 + 1);
            }
            array = new byte[] { 0xED, 0xED, wait2, wait1, 0x00, 0x00, 0x03, 0x05, 0x01, 0x01, 0x00, 0x00 };
            a = wait1;
            wait1 += 1;
            //comport.Write(array, 0, 12);
            label += 1;
            label_wait.Text = label.ToString();

            foreach (byte byteValue in array)
            {
                Console.WriteLine(byteValue);
            }
        }
       
        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Byte[] buffer = new Byte[1024];
            Int32 length = (sender as SerialPort).Read(buffer, 0, buffer.Length);
            Array.Resize(ref buffer, length);
            Display d = new Display(DisplayText);
            this.Invoke(d, new Object[] { buffer });
            comport.Write(array, 0, 12);

            /*
            Thread.Sleep(50);  //（毫秒）等待一定時間，確保資料的完整性 int len        
            int len = comport.BytesToRead;
            string receivedata = string.Empty;
            if (len != 0)
            {
                byte[] buff = new byte[len];
                comport.Read(buff, 0, len);
                receivedata = Encoding.Default.GetString(buff);
            }
            MessageBox.Show(receivedata);
            richTextBox1.AppendText(receivedata + "\r\n");
            */
        }

        private void DisplayText(Byte[] buffer)
        {
            richTextBox1.Text += String.Format("{0}{1}", BitConverter.ToString(buffer), Environment.NewLine);
            totalLength = totalLength + buffer.Length;
            label1.Text = totalLength.ToString();
        }

        private void Form1_Leave(object sender, EventArgs e)
        {
            comport.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            //byte[] array1 = { 0xED, 0xED, wait2, wait1, 0x00, 0x00, 0x03, 0x05, 0x01, 0x01, 0x00, 0x00 };
            byte waits1 = wait1;
            waits1 -= 1;
           byte[] array1 = {  wait2, waits1 };
           foreach (byte byteValue in array1)
           {
                    Console.WriteLine(byteValue);
           }                      
        }
    }
}
