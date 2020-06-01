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
using System.Threading;

namespace form
{
    public partial class Form1 : Form
    {
        private SerialPort comport;
        delegate void Display(Byte[] buffer);
        private Int32 totalLength = 0;
        private Boolean receiving;
        private Thread t;
        byte[] array;
        byte a = 0;
        byte t1 = 0;
        byte t2 = 0;
        byte t3 = 0;
        byte t4 = 0;
        byte wait1 = 1;
        byte wait2 = 0;        
        int label = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {            
            comport = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
            comport.ReadTimeout = 2000;
            comport.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            if (!comport.IsOpen)
            {
                comport.Open();
                receiving = true;
                t = new Thread(DoReceive);
                t.IsBackground = true;
                t.Start();               
            }
            MessageBox.Show("開啟");
        }

        public void button1_Click(object sender, EventArgs e)
        {
            //byte wait1 = (byte)(a + 1);
            if (wait1 == 10)
            {
                wait1 = 0;
                wait2 = (byte)(wait2 + 1);
            }
            //array = new byte[] { 0xED, 0xED, wait2, wait1, 0x00, 0x00, 0x03, 0x05, 0x01, 0x01, 0x00, 0x00 };
            a = wait1;
            wait1 += 1;
            //comport.Write(array, 0, 12);
            label += 1;
            label_wait.Text = label.ToString();

            /*foreach (byte byteValue in array)//看ARRAY 中的數值
            {
                Console.WriteLine(byteValue);
            }*/
        }

        private void DoReceive()
        {
            Byte[] buffer = new Byte[1024];            
            while (receiving)
            {
                /*收到100個BYTE才寫入
                while (comport.BytesToRead < 100)
                {
                    Thread.Sleep(16);
                }*/
                if (comport.BytesToRead > 0)
                {
                    Int32 length = comport.Read(buffer, 0, buffer.Length);
                    Array.Resize(ref buffer, length);
                    Display d = new Display(DisplayText);
                    this.Invoke(d, new Object[] { buffer });
                    
                    try
                    {
                        if (buffer[0] == 165 && buffer[1] == 182 && buffer[6] == 127)
                        {
                            try
                            {
                                //MessageBox.Show("12213");
                                byte waits1 = wait1;
                                waits1 -= 1;
                                byte[] array1 = { 0xED, 0xED, wait2, waits1, t4, t3, t2, t1, 0x01, 0x01, 0x00 };
                                comport.Write(array1, 0, 11);
                            }
                            catch (TimeoutException timeoutEx)
                            {
                                MessageBox.Show("送出失敗");
                            }
                        }
                        else if (buffer[0] == 165 && buffer[1] == 182 && buffer[6] == 01 && buffer[7] == 01 )
                        {
                            try
                            {
                                if (wait2 == 0 && (wait1 - 1) != 0)
                                {
                                    //byte waits1 = wait1;
                                    wait1 -= 1;
                                    if (t1 == 10)
                                    {
                                        t1 = 0;
                                        t2 = (byte)(t2 + 1);
                                    }
                                    else if (t2 == 10)
                                    {
                                        t2 = 0;
                                        t3 = (byte)(t3 + 1);
                                    }
                                    else if (t3 == 10)
                                    {
                                        t3 = 0;
                                        t3 = (byte)(t3 + 1);
                                    }
                                    t1 += 1;
                                    byte[] array1 = { 0xED, 0xED, wait2, wait1, t4, t3, t2, t1, 0x01, 0x01, 0x00 };
                                    comport.Write(array1, 0, 11);
                                    label -= 1;
                                }
                                else if (wait2 != 0 && (wait1 - 1) == 0)
                                {
                                    //byte waits1 = wait1;
                                    wait2 -= 1;
                                    wait1 = 9;
                                    if (t1 == 10)
                                    {
                                        t1 = 0;
                                        t2 = (byte)(t2 + 1);
                                    }
                                    else if (t2 == 10)
                                    {
                                        t2 = 0;
                                        t3 = (byte)(t3 + 1);
                                    }
                                    else if (t3 == 10)
                                    {
                                        t3 = 0;
                                        t3 = (byte)(t3 + 1);
                                    }
                                    t1 += 1;
                                    byte[] array1 = { 0xED, 0xED, wait2, wait1, t4, t3, t2, t1, 0x01, 0x01, 0x00 };
                                    comport.Write(array1, 0, 11);
                                    label -= 1;
                                }
                                else if (wait2 != 0 && (wait1 - 1) != 0)
                                {
                                    //byte waits1 = wait1;
                                    wait1 -= 1;
                                    if (t1 == 10)
                                    {
                                        t1 = 0;
                                        t2 = (byte)(t2 + 1);
                                    }
                                    else if (t2 == 10)
                                    {
                                        t2 = 0;
                                        t3 = (byte)(t3 + 1);
                                    }
                                    else if (t3 == 10)
                                    {
                                        t3 = 0;
                                        t3 = (byte)(t3 + 1);
                                    }
                                    t1 += 1;
                                    byte[] array1 = { 0xED, 0xED, wait2, wait1, t4, t3, t2, t1, 0x01, 0x01, 0x00 };
                                    comport.Write(array1, 0, 11);
                                    label -= 1;
                                }
                                else if (wait2 == 0 && (wait1 - 1) == 0)
                                {
                                    MessageBox.Show("no wait");
                                    //byte waits1 = wait1;
                                    wait1 -= 1;
                                    byte[] array1 = { 0xED, 0xED, wait2, wait1, t4, t3, t2, t1, 0x01, 0x00, 0x00 };
                                    comport.Write(array1, 0, 11);
                                }
                                else 
                                {
                                    MessageBox.Show("等待有誤");
                                }
                            }
                            catch (TimeoutException timeoutEx)
                            {
                                MessageBox.Show("送出失敗");
                            }
                        }
                    }
                    catch (TimeoutException timeoutEx)
                    {
                        MessageBox.Show("陣列過長");
                    }
                }
                Thread.Sleep(160);
            }
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            /*int len = comport.BytesToRead;  //測試收到的byte資料
            Byte[] buff = new Byte[len];
            comport.Read(buff, 0, buff.Length); 
            foreach (byte byteValue in buff)//看ARRAY 中的數值
            {
                Console.WriteLine(byteValue);
            }*/
            Thread.Sleep(16);
            if ((sender as SerialPort).BytesToRead > 0)
            {
                try
                {/*
                    Byte[] buffer = new Byte[1024];
                    Int32 length = (sender as SerialPort).Read(buffer, 0, buffer.Length);
                    Array.Resize(ref buffer, length);
                    Display d = new Display(DisplayText);
                    this.Invoke(d, new Object[] { buffer });*/
                }
                catch (TimeoutException timeoutEx)
                {
                    MessageBox.Show(timeoutEx.ToString());
                    //以下這邊請自行撰寫你想要的例外處理
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    //以下這邊請自行撰寫你想要的例外處理
                }
            }
            

            byte[] receive = { 0xA5, 0xB6, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00 };
            /*if (buffer == receive)
            {
                try
                {
                    //MessageBox.Show("12213");
                    byte waits1 = wait1;
                    waits1 -= 1;
                    byte[] array1 = { 0xED, 0xED, wait2, waits1, 0x00, 0x00, 0x03, 0x05, 0x01, 0x01, 0x00, 0x00 };
                    comport.Write(array1, 0, 12);
                }
                catch (TimeoutException timeoutEx)
                {
                    MessageBox.Show("送出失敗");
                }
            }
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
            //richTextBox1.Text += String.Format("{0}{1}", Encoding.ASCII.GetString(buffer), Environment.NewLine); //顯示的是字元的內容而非 Byte 值。
            richTextBox1.Text += String.Format("{0}{1}", BitConverter.ToString(buffer), Environment.NewLine);
            totalLength = totalLength + buffer.Length;
            label1.Text = totalLength.ToString();
            label_wait.Text = label.ToString();
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
           byte[] array1 = { 0xED, 0xED, wait2, waits1, 0x00, 0x00, 0x03, 0x05, 0x01, 0x01, 0x00, 0x00 };
           foreach (byte byteValue in array1)
           {
                    Console.WriteLine(byteValue);
           }                      
        }
    }
}
