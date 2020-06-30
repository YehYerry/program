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
using System.Media;
using System.IO;

namespace form
{
    public partial class Form1 : Form
    {
        private SerialPort comport;
        delegate void Display(Byte[] buffer);
        private Int32 totalLength = 0;
        private Boolean receiving;
        private Thread t;
        byte cont1;
        byte cont2;
        byte[,] num = new byte[,] { {0 , 2 }, { 3, 4 }, { 5, 6 }, { 7, 8 } };
        byte t1 = 0, t2 = 0, t3 = 0, t4 = 0;
        byte[] wait1 = { 0, 0 , 0, 0 }; //四個按鈕的等待人數(十位數)
        byte[] wait2 = { 0, 0 , 0, 0 }; //四個按鈕的等待人數(個位數)
        //byte wait1 = 0;
        //byte wait2 = 0;        
        int label = 0;
        int label2 = 0;
        byte num1;
        byte num2;
        byte num3;
        byte num4;
        string[] line = new string[1000];
        int ctr = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StreamReader str = new StreamReader(Application.StartupPath + @"\test.ifm");//讀取文字檔                      
            do
            {
                ctr++;
                line[ctr] = str.ReadLine();
                //Console.WriteLine(line[ctr]);
            } while (line[ctr] != null);

            BackgroundImage = new Bitmap(Application.StartupPath + @"\background\back1.jpg");
            Image pic = new Bitmap(Application.StartupPath + @"\background\button.jpg");
            button1.BackgroundImage = pic;

            cont1 = Convert.ToByte(line[2].Substring(6));
            cont2 = Convert.ToByte(line[3].Substring(6));
            Console.WriteLine(cont1);
            Console.WriteLine(cont2);
            /*byte[] val = Encoding.UTF8.GetBytes(line[2].Substring(6));
            foreach (byte s1 in val)
            Console.WriteLine(s1);*/
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

            wait1[0] += 1;
            //wait1 = (byte)(a + 1);
            if (wait1[0] == 10)
            {
                wait1[0] = 0;
                wait2[0] += 1;
            }
            //array = new byte[] { 0xED, 0xED, wait2, wait1, 0x00, 0x00, 0x03, 0x05, 0x01, 0x01, 0x00, 0x00 };
            //a = wait1;
            //comport.Write(array, 0, 12);
            label += 1;
            label_wait.Text = label.ToString();

            /*foreach (byte byteValue in array)//看ARRAY 中的數值
            {
                Console.WriteLine(byteValue);
            }*/            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            wait1[1] += 1;
            //wait1 = (byte)(a + 1);
            if (wait1[1] == 10)
            {
                wait1[1] = 0;
                wait2[1] += 1;
            }
            label2 += 1;
            label_wait2.Text = label2.ToString();
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

                    SoundPlayer player = new SoundPlayer();
                    SoundPlayer player1 = new SoundPlayer();
                    SoundPlayer player2 = new SoundPlayer();
                    //player.PlayLooping(); //迴圈播放模式
                    //player.PlaySync(); //UI執行緒同步播放

                    try
                    {
                        if (buffer[0] == 165 && buffer[1] == 182 && buffer[2] == 0 && buffer[3] == 0 && buffer[4] == 0 && buffer[5] == 0 && (buffer[6] == 0 || buffer[6] == 1 || buffer[6] == 2 || buffer[6] == 3 || buffer[6] == 4 || buffer[6] == 5 || buffer[6] == 6 || buffer[6] == 7 || buffer[6] == 8 || buffer[6] == 9 || buffer[6] == 10 || buffer[6] == 11 || buffer[6] == 12 || buffer[6] == 13 || buffer[6] == 14) && buffer[7] == 0 && buffer[8] == 0) //無資料時
                        {
                            try
                            {
                                //MessageBox.Show("12213");
                                //byte waits1 = wait1;
                                //waits1 -= 1;
                                byte[] array1 = { 0xED, 0xED, wait2[0], wait1[0], t4, t3, t2, t1, cont1, 0x01, 0x00 };
                                byte[] array2 = { 0xED, 0xED, wait2[1], wait1[1], t4, t3, t2, t1, cont2, 0x01, 0x00 };
                                comport.Write(array1, 0, 11);
                                comport.Write(array2, 0, 11);
                            }
                            catch (TimeoutException timeoutEx)
                            {
                                MessageBox.Show("送出失敗");
                            }
                        }
                        else if (buffer[0] == 165 && buffer[1] == 182 && (buffer[6] == cont1 || buffer[6] == cont2) && buffer[7] == 01) //CALL
                        {
                            try
                            {
                                if (buffer[6] == cont1)
                                {
                                    if (wait2[0] == 0 && wait1[0] != 0)
                                    {
                                        //byte waits1 = wait1;
                                        wait1[0] -= 1;
                                        t1 += 1;
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
                                        byte[] array1 = { 0xED, 0xED, wait2[0], wait1[0], t4, t3, t2, t1, cont1, 0x01, 0x00 };
                                        comport.Write(array1, 0, 11);
                                        label -= 1;
                                    }
                                    else if (wait2[0] != 0 && wait1[0] == 0)
                                    {
                                        //byte waits1 = wait1;
                                        wait2[0] -= 1;
                                        wait1[0] = 9;
                                        t1 += 1;
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
                                        byte[] array1 = { 0xED, 0xED, wait2[0], wait1[0], t4, t3, t2, t1, cont1, 0x01, 0x00 };
                                        comport.Write(array1, 0, 11);
                                        label -= 1;
                                    }
                                    else if (wait2[0] != 0 && wait1[0] != 0)
                                    {
                                        //byte waits1 = wait1;
                                        wait1[0] -= 1;
                                        t1 += 1;
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
                                        byte[] array1 = { 0xED, 0xED, wait2[0], wait1[0], t4, t3, t2, t1, cont1, 0x01, 0x00 };
                                        comport.Write(array1, 0, 11);
                                        label -= 1;
                                    }
                                    else if (wait2[0] == 0 && wait1[0] == 0)
                                    {
                                        MessageBox.Show("no wait");
                                        //byte waits1 = wait1;
                                        //wait1 -= 1;
                                        byte[] array1 = { 0xED, 0xED, wait2[0], wait1[0], t4, t3, t2, t1, cont1, 0x00, 0x00 };
                                        comport.Write(array1, 0, 11);
                                    }
                                    else
                                    {
                                        MessageBox.Show("等待有誤");
                                    }
                                    /*player.SoundLocation = @"C:\Users\bock\github\program\voice\來賓.wav";
                                    player.PlaySync();*/
                                    if (t4 == 0 && t3 == 0 && t2 == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t1 + ".wav";
                                        player.PlaySync();

                                    }
                                    else if (t4 == 0 && t3 == 0 && t2 == 1 && t1 == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                    }
                                    else if (t4 == 0 && t3 == 0 && t2 == 1 && (t1 == 1 || t1 == 2 || t1 == 3 || t1 == 4 || t1 == 5 || t1 == 6 || t1 == 7 || t1 == 8 || t1 == 9))
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t1 + ".wav";
                                        player.PlaySync();
                                    }
                                    else if (t4 == 0 && t3 == 0 && (t2 == 2 || t2 == 3 || t2 == 4 || t2 == 5 || t2 == 6 || t2 == 7 || t2 == 8 || t2 == 9) && t1 == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t2 + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                    }
                                    else if (t4 == 0 && t3 == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t2 + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t1 + ".wav";
                                        player.PlaySync();

                                        Console.WriteLine(t2);
                                        Console.WriteLine(t1);
                                    }
                                    else if (t4 == 0 && (t3 == 1 || t3 == 2 || t3 == 3 || t3 == 4 || t3 == 5 || t3 == 6 || t3 == 7 || t3 == 8 || t3 == 9) && t2 == 0 && t1 == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t3 + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                        player.PlaySync();
                                    }
                                    else if (t4 == 0 && (t3 == 1 || t3 == 2 || t3 == 3 || t3 == 4 || t3 == 5 || t3 == 6 || t3 == 7 || t3 == 8 || t3 == 9) && (t2 == 1 || t2 == 2 || t2 == 3 || t2 == 4 || t2 == 5 || t2 == 6 || t2 == 7 || t2 == 8 || t2 == 9) && t1 == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t3 + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t2 + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                    }
                                    else if (t4 == 0 && (t3 == 1 || t3 == 2 || t3 == 3 || t3 == 4 || t3 == 5 || t3 == 6 || t3 == 7 || t3 == 8 || t3 == 9) && t2 == 0 && (t1 == 1 || t1 == 2 || t1 == 3 || t1 == 4 || t1 == 5 || t1 == 6 || t1 == 7 || t1 == 8 || t1 == 9))
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t3 + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t2 + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t1 + ".wav";
                                        player.PlaySync();
                                    }
                                    else if (t4 == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t3 + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t2 + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t1 + ".wav";
                                        player.PlaySync();
                                    }
                                    else if ((t4 == 1 || t4 == 2 || t4 == 3 || t4 == 4 || t4 == 5 || t4 == 6 || t4 == 7 || t4 == 8 || t4 == 9) && t3 == 0 && t2 == 0 && t1 == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t4 + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                        player.PlaySync();
                                    }
                                    else if ((t4 == 1 || t4 == 2 || t4 == 3 || t4 == 4 || t4 == 5 || t4 == 6 || t4 == 7 || t4 == 8 || t4 == 9) && (t3 == 1 || t3 == 2 || t3 == 3 || t3 == 4 || t3 == 5 || t3 == 6 || t3 == 7 || t3 == 8 || t3 == 9) && t2 == 0 && t1 == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t4 + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t3 + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                        player.PlaySync();
                                    }
                                    else if ((t4 == 1 || t4 == 2 || t4 == 3 || t4 == 4 || t4 == 5 || t4 == 6 || t4 == 7 || t4 == 8 || t4 == 9) && (t3 == 1 || t3 == 2 || t3 == 3 || t3 == 4 || t3 == 5 || t3 == 6 || t3 == 7 || t3 == 8 || t3 == 9) && (t2 == 1 || t2 == 2 || t2 == 3 || t2 == 4 || t2 == 5 || t2 == 6 || t2 == 7 || t2 == 8 || t2 == 9) && t1 == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t4 + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t3 + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t2 + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                    }
                                    else if ((t4 == 1 || t4 == 2 || t4 == 3 || t4 == 4 || t4 == 5 || t4 == 6 || t4 == 7 || t4 == 8 || t4 == 9) && (t3 == 0 || t3 == 1 || t3 == 2 || t3 == 3 || t3 == 4 || t3 == 5 || t3 == 6 || t3 == 7 || t3 == 8 || t3 == 9) && (t2 == 0 || t2 == 1 || t2 == 2 || t2 == 3 || t2 == 4 || t2 == 5 || t2 == 6 || t2 == 7 || t2 == 8 || t2 == 9) && t1 == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t4 + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t3 + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t2 + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                    }
                                    else if ((t4 == 1 || t4 == 2 || t4 == 3 || t4 == 4 || t4 == 5 || t4 == 6 || t4 == 7 || t4 == 8 || t4 == 9) && (t3 == 0 || t3 == 1 || t3 == 2 || t3 == 3 || t3 == 4 || t3 == 5 || t3 == 6 || t3 == 7 || t3 == 8 || t3 == 9) && (t2 == 0 || t2 == 1 || t2 == 2 || t2 == 3 || t2 == 4 || t2 == 5 || t2 == 6 || t2 == 7 || t2 == 8 || t2 == 9) && (t1 == 1 || t1 == 2 || t1 == 3 || t1 == 4 || t1 == 5 || t1 == 6 || t1 == 7 || t1 == 8 || t1 == 9))
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t4 + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t3 + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t2 + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + t1 + ".wav";
                                        player.PlaySync();
                                    }
                                    else
                                    {
                                        MessageBox.Show("人數超過9999號!!");
                                    }
                                    /*player.SoundLocation = @"C:\Users\bock\github\program\voice\號.wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\請到.wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\1.wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\號.wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\櫃台.wav";
                                    player.PlaySync();*/
                                }
                                if (buffer[6] == cont2)
                                {
                                    MessageBox.Show("2");
                                }
                            }
                            catch (TimeoutException timeoutEx)
                            {
                                MessageBox.Show("送出失敗");
                            }
                        }
                        else if (buffer[0] == 165 && buffer[1] == 182 && (buffer[6] == cont1 || buffer[6] == cont2) && buffer[7] == 02)//指定叫號
                        {
                            if (buffer[6] == cont1)
                            {
                                num1 = buffer[2];
                                num2 = buffer[3];
                                num3 = buffer[4];
                                num4 = buffer[5];
                                byte[] array1 = { 0xED, 0xED, wait2[0], wait1[0], num1, num2, num3, num4, cont1, 0x01, 0x00 };
                                comport.Write(array1, 0, 11);
                                /*player.SoundLocation = @"C:\Users\bock\github\program\voice\來賓.wav";
                                    player.PlaySync();*/
                                if (num1 == 0 && num2 == 0 && num3 == 0)
                                {
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num4 + ".wav";
                                    player.PlaySync();

                                }
                                else if (num1 == 0 && num2 == 0 && num3 == 1 && num4 == 0)
                                {
                                    player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                    player.PlaySync();
                                }
                                else if (num1 == 0 && num2 == 0 && num3 == 1 && (num4 == 1 || num4 == 2 || num4 == 3 || num4 == 4 || num4 == 5 || num4 == 6 || num4 == 7 || num4 == 8 || num4 == 9))
                                {
                                    player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num4 + ".wav";
                                    player.PlaySync();
                                }
                                else if (num1 == 0 && num2 == 0 && (num3 == 2 || num3 == 3 || num3 == 4 || num3 == 5 || num3 == 6 || num3 == 7 || num3 == 8 || num3 == 9) && num4 == 0)
                                {
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num3 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                    player.PlaySync();
                                }
                                else if (num1 == 0 && num2 == 0)
                                {
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num3 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num4 + ".wav";
                                    player.PlaySync();

                                    Console.WriteLine(num3);
                                    Console.WriteLine(num4);
                                }
                                else if (num1 == 0 && (num2 == 1 || num2 == 2 || num2 == 3 || num2 == 4 || num2 == 5 || num2 == 6 || num2 == 7 || num2 == 8 || num2 == 9) && num3 == 0 && num4 == 0)
                                {
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num2 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                    player.PlaySync();
                                }
                                else if (num1 == 0 && (num2 == 1 || num2 == 2 || num2 == 3 || num2 == 4 || num2 == 5 || num2 == 6 || num2 == 7 || num2 == 8 || num2 == 9) && (num3 == 1 || num3 == 2 || num3 == 3 || num3 == 4 || num3 == 5 || num3 == 6 || num3 == 7 || num3 == 8 || num3 == 9) && num4 == 0)
                                {
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num2 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num3 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                    player.PlaySync();
                                }
                                else if (num1 == 0 && (num2 == 1 || num2 == 2 || num2 == 3 || num2 == 4 || num2 == 5 || num2 == 6 || num2 == 7 || num2 == 8 || num2 == 9) && num3 == 0 && (num4 == 1 || num4 == 2 || num4 == 3 || num4 == 4 || num4 == 5 || num4 == 6 || num4 == 7 || num4 == 8 || num4 == 9))
                                {
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num2 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num3 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num4 + ".wav";
                                    player.PlaySync();
                                }
                                else if (num1 == 0)
                                {
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num2 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num3 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num4 + ".wav";
                                    player.PlaySync();
                                }
                                else if ((num1 == 1 || num1 == 2 || num1 == 3 || num1 == 4 || num1 == 5 || num1 == 6 || num1 == 7 || num1 == 8 || num1 == 9) && num2 == 0 && num3 == 0 && num4 == 0)
                                {
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num1 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                    player.PlaySync();
                                }
                                else if ((num1 == 1 || num1 == 2 || num1 == 3 || num1 == 4 || num1 == 5 || num1 == 6 || num1 == 7 || num1 == 8 || num1 == 9) && (num2 == 1 || num2 == 2 || num2 == 3 || num2 == 4 || num2 == 5 || num2 == 6 || num2 == 7 || num2 == 8 || num2 == 9) && num3 == 0 && num4 == 0)
                                {
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num1 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num2 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                    player.PlaySync();
                                }
                                else if ((num1 == 1 || num1 == 2 || num1 == 3 || num1 == 4 || num1 == 5 || num1 == 6 || num1 == 7 || num1 == 8 || num1 == 9) && (num2 == 1 || num2 == 2 || num2 == 3 || num2 == 4 || num2 == 5 || num2 == 6 || num2 == 7 || num2 == 8 || num2 == 9) && (num3 == 1 || num3 == 2 || num3 == 3 || num3 == 4 || num3 == 5 || num3 == 6 || num3 == 7 || num3 == 8 || num3 == 9) && num4 == 0)
                                {
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num1 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num2 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num3 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                    player.PlaySync();
                                }
                                else if ((num1 == 1 || num1 == 2 || num1 == 3 || num1 == 4 || num1 == 5 || num1 == 6 || num1 == 7 || num1 == 8 || num1 == 9) && (num2 == 0 || num2 == 1 || num2 == 2 || num2 == 3 || num2 == 4 || num2 == 5 || num2 == 6 || num2 == 7 || num2 == 8 || num2 == 9) && (num3 == 0 || num3 == 1 || num3 == 2 || num3 == 3 || num3 == 4 || num3 == 5 || num3 == 6 || num3 == 7 || num3 == 8 || num3 == 9) && num4 == 0)
                                {
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num1 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num2 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num3 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                    player.PlaySync();
                                }
                                else if ((num1 == 1 || num1 == 2 || num1 == 3 || num1 == 4 || num1 == 5 || num1 == 6 || num1 == 7 || num1 == 8 || num1 == 9) && (num2 == 0 || num2 == 1 || num2 == 2 || num2 == 3 || num2 == 4 || num2 == 5 || num2 == 6 || num2 == 7 || num2 == 8 || num2 == 9) && (num3 == 0 || num3 == 1 || num3 == 2 || num3 == 3 || num3 == 4 || num3 == 5 || num3 == 6 || num3 == 7 || num3 == 8 || num3 == 9) && (num4 == 1 || num4 == 2 || num4 == 3 || num4 == 4 || num4 == 5 || num4 == 6 || num4 == 7 || num4 == 8 || num4 == 9))
                                {
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num1 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num2 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num3 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                    player.PlaySync();
                                    player.SoundLocation = Application.StartupPath + @"\voice\" + num4 + ".wav";
                                    player.PlaySync();
                                }
                                else
                                {
                                    MessageBox.Show("人數超過9999號!!");
                                }

                                /*player.SoundLocation = @"C:\Users\bock\github\program\voice\號.wav";
                                   player.PlaySync();
                                   player.SoundLocation = @"C:\Users\bock\github\program\voice\請到.wav";
                                   player.PlaySync();
                                   player.SoundLocation = @"C:\Users\bock\github\program\voice\1.wav";
                                   player.PlaySync();
                                   player.SoundLocation = @"C:\Users\bock\github\program\voice\號.wav";
                                   player.PlaySync();
                                   player.SoundLocation = @"C:\Users\bock\github\program\voice\櫃台.wav";
                                   player.PlaySync();*/
                            }
                            if (buffer[6] == cont2) 
                            {
                                MessageBox.Show("指定2");
                            }
                            else
                            {
                                byte[] array1 = { 0xED, 0xED, wait2[0], wait1[0], 0x00, 0x00, 0x00, 0x00, cont1, 0x01, 0x00 };
                                comport.Write(array1, 0, 11);
                                Console.WriteLine("陣列外");
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
            //byte waits1 = wait1;
           // waits1 -= 1;
           byte[] clear = { 0xED, 0xED, 0x00, 0x00, 0x00, 0x00, 0x03, 0x05, 0x01, 0x01, 0x00, 0x00 };
            comport.Write(clear, 0, 11);
            foreach (byte byteValue in clear)
           {
                    Console.WriteLine(byteValue);
           }                      
        }       
    }
}
