﻿using System;
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
using System.Drawing.Printing;
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
        byte cont1, cont2, cont3, cont4, cont5, cont6, cont7, cont8, cont9, cont10, cont11, cont12, cont13, cont14, cont15;
        string b1, b2, b3, b4;
        int b1h, b2h, b3h, b4h, b1w, b2w, b3w, b4w;
        int L1s, L2s, L3s, L4s;
        string L1f, L2f, L3f, L4f;
        int b1bh, b2bh, b3bh, b4bh, b1bw, b2bw, b3bw, b4bw;
        int L1bh, L2bh, L3bh, L4bh, L1bw, L2bw, L3bw, L4bw;
        byte b1num1, b1num2, b1num3, b1num4, b2num1, b2num2, b2num3, b2num4, b3num1, b3num2, b3num3, b3num4, b4num1, b4num2, b4num3, b4num4;
        byte[,] num;
        byte[] wait1 = { 0, 0, 0, 0 }; //四個按鈕的等待人數(十位數)
        byte[] wait2 = { 0, 0, 0, 0 }; //四個按鈕的等待人數(個位數)      
        int label = 0;
        int label2 = 0;
        byte num1, num2, num3, num4;
        string[] line = new string[1000];
        int ctr = 0;
        int print = 0;
        int i = 3;
        int decimalLength;
        DateTime doubleClickTimer = DateTime.Now;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StreamReader str = new StreamReader(Application.StartupPath + @"\config.ifm");//讀取文字檔                      
            do
            {
                ctr++;
                line[ctr] = str.ReadLine();
                //Console.WriteLine(line[ctr]);
            } while (line[ctr] != null);

            BackgroundImage = new Bitmap(Application.StartupPath + @"\background\back.jpg");
            Image pic = new Bitmap(Application.StartupPath + @"\background\button.jpg");
            button1.BackgroundImage = pic;

            cont1 = Convert.ToByte(line[3].Substring(5, 1));
            cont2 = Convert.ToByte(line[5].Substring(5, 1));
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

            b1 = line[19].Substring(10);
            b2 = line[27].Substring(10);
            b3 = line[35].Substring(10);
            b4 = line[43].Substring(10);
            b1w = Convert.ToInt32(line[20].Substring(6));
            b1h = Convert.ToInt32(line[21].Substring(7));
            b1bw = Convert.ToInt32(line[22].Substring(12));
            b1bh = Convert.ToInt32(line[23].Substring(13));
            b1num1 = Convert.ToByte(line[24].Substring(9, 1));
            b1num2 = Convert.ToByte(line[24].Substring(10, 1));
            b1num3 = Convert.ToByte(line[24].Substring(11, 1));
            b1num4 = Convert.ToByte(line[24].Substring(12, 1));
            b2w = Convert.ToInt32(line[28].Substring(6));
            b2h = Convert.ToInt32(line[29].Substring(7));
            b2bw = Convert.ToInt32(line[30].Substring(12));
            b2bh = Convert.ToInt32(line[31].Substring(13));
            b2num1 = Convert.ToByte(line[32].Substring(9, 1));
            b2num2 = Convert.ToByte(line[32].Substring(10, 1));
            b2num3 = Convert.ToByte(line[32].Substring(11, 1));
            b2num4 = Convert.ToByte(line[32].Substring(12, 1));
            b3w = Convert.ToInt32(line[36].Substring(6));
            b3h = Convert.ToInt32(line[37].Substring(7));
            b3bw = Convert.ToInt32(line[38].Substring(12));
            b3bh = Convert.ToInt32(line[39].Substring(13));
            b3num1 = Convert.ToByte(line[40].Substring(9, 1));
            b3num2 = Convert.ToByte(line[40].Substring(10, 1));
            b3num3 = Convert.ToByte(line[40].Substring(11, 1));
            b3num4 = Convert.ToByte(line[40].Substring(12, 1));
            b4w = Convert.ToInt32(line[44].Substring(6));
            b4h = Convert.ToInt32(line[45].Substring(7));
            b4bw = Convert.ToInt32(line[46].Substring(12));
            b4bh = Convert.ToInt32(line[47].Substring(13));
            b4num1 = Convert.ToByte(line[48].Substring(9, 1));
            b4num2 = Convert.ToByte(line[48].Substring(10, 1));
            b4num3 = Convert.ToByte(line[48].Substring(11, 1));
            b4num4 = Convert.ToByte(line[48].Substring(12, 1));
            L1s = Convert.ToInt32(line[51].Substring(5));
            L1bw = Convert.ToInt32(line[52].Substring(12));
            L1bh = Convert.ToInt32(line[53].Substring(13));
            L1f = line[54].Substring(5);
            L2s = Convert.ToInt32(line[57].Substring(5));
            L2bw = Convert.ToInt32(line[58].Substring(12));
            L2bh = Convert.ToInt32(line[59].Substring(13));
            L2f = line[60].Substring(5);
            L3s = Convert.ToInt32(line[63].Substring(5));
            L3bw = Convert.ToInt32(line[64].Substring(12));
            L3bh = Convert.ToInt32(line[65].Substring(13));
            L3f = line[66].Substring(5);
            L4s = Convert.ToInt32(line[69].Substring(5));
            L4bw = Convert.ToInt32(line[70].Substring(12));
            L4bh = Convert.ToInt32(line[71].Substring(13));
            L4f = line[72].Substring(5);

            button1.Size = new Size(b1w, b1h);
            button1.Location = new Point(b1bw, b1bh);
            button2.Size = new Size(b2w, b2h);
            button2.Location = new Point(b2bw, b2bh);
            button3.Size = new Size(b3w, b3h);
            button3.Location = new Point(b3bw, b3bh);
            button4.Size = new Size(b4w, b4h);
            button4.Location = new Point(b4bw, b4bh);
            button1.FlatStyle = FlatStyle.Flat; button1.FlatAppearance.BorderSize = 0; button1.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button2.FlatStyle = FlatStyle.Flat; button2.FlatAppearance.BorderSize = 0; button2.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button3.FlatStyle = FlatStyle.Flat; button3.FlatAppearance.BorderSize = 0; button3.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button4.FlatStyle = FlatStyle.Flat; button4.FlatAppearance.BorderSize = 0; button4.FlatAppearance.MouseOverBackColor = Color.Transparent;

            label_wait.Font = new Font(L1f, L1s);//Times New Roman、Arial、標楷體
            label_wait.Location = new Point(L1bw, L1bh);
            label_wait2.Font = new Font(L2f, L2s);
            label_wait2.Location = new Point(L2bw, L2bh);
            Console.WriteLine(L1f);
            Console.WriteLine(L2f);

            num = new byte[,] { { b1num1, b1num2, b1num3, b1num4 }, { b2num1, b2num2, b2num3, b2num4 }, { b3num1, b3num2, b3num3, b3num4 }, { b4num1, b4num2, b4num3, b4num4 } };
            Console.WriteLine(num[0, 0]);
            Console.WriteLine(num[0, 1]);
            Console.WriteLine(num[0, 2]);
            Console.WriteLine(num[0, 3]);

            this.WindowState = FormWindowState.Maximized;
            if (b1 == "0")
            {
                button1.Visible = false;
                label_wait.Visible = false;
            }
            if (b2 == "0")
            {
                button2.Visible = false;
                label_wait2.Visible = false;
            }
            if (b3 == "0")
            {
                button3.Visible = false;
                label_wait3.Visible = false;
            }
            if (b4 == "0")
            {
                button4.Visible = false;
                label_wait4.Visible = false;
            }
            MessageBox.Show("開啟");
        }

        public void button1_Click(object sender, EventArgs e)
        {
            print += 1;
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
            //宣告一個印表機
            PrintDocument pd = new PrintDocument();
            //設定印表機邊界
            Margins margin = new Margins(0, 0, 0, 0);
            pd.DefaultPageSettings.Margins = margin;
            ////設定紙張大小 'vbPRPSUser'為使用者自訂
            PaperSize pageSize = new PaperSize("vbPRPSUser", 256, 256);
            pd.DefaultPageSettings.PaperSize = pageSize;
            //印表機事件設定
            pd.PrintPage += new PrintPageEventHandler(this.printCoupon_PrintPage);
            /*預覽列印
            PrintPreviewDialog PPD = new PrintPreviewDialog();
            PPD.Document = pd;
            PPD.ShowDialog();*/
            //列印
            try
            {
                pd.Print();//列印
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "列印失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pd.PrintController.OnEndPrint(pd, new PrintEventArgs());
            }
        }
        private void printCoupon_PrintPage(object sender, PrintPageEventArgs e)
        {
            string str = System.Windows.Forms.Application.StartupPath;
            //列印圖片
            Image temp = Image.FromFile(@"C:\Users\bock\github\program\testprint\testprint\bin\Debug\photo\test.bmp");//圖片檔案
            //C:\Users\jerry\github\program\testprint\testprint\bin\Debug\photo
            //GetResultIntoImage(ref temp);//

            //設定圖片列印的x,y座標
            int x = 20;   //e.MarginBounds.X
            int y = 0;  //e.MarginBounds.Y
            //圖片列印的大小
            int width = 220;//temp.Width;
            int height = 80;//temp.Height;
            Rectangle destRect = new Rectangle(x, y, width, height);

            // public void DrawImage (
            // Image image, 要繪製的 Image。
            // Rectangle destRect, Rectangle 結構，指定繪製影像的位置和大小。縮放影像來符合矩形
            // int srcX, 要繪製之來源影像部分左上角的 X 座標
            // int srcY, 要繪製之來源影像部分左上角的 Y 座標。
            // int srcWidth, 要繪製之來源影像部分的寬度。
            // int srcHeight, 要繪製之來源影像部分的高度。
            // GraphicsUnit srcUnit, GraphicsUnit 列舉型別的成員，指定用來判斷來源矩形的測量單位。
            // ImageAttributes imageAttr ImageAttributes，指定 image 物件的重新著色和 Gamma 資訊。
            //)
            //將圖片放入要列印的文件中
            e.Graphics.DrawImage(temp, destRect, 0, 0, temp.Width, temp.Height, System.Drawing.GraphicsUnit.Pixel);

            //列印文字
            Graphics MyGraphics = e.Graphics;
            SolidBrush MyBrush = new SolidBrush(Color.Black);
            Font MyFont = new Font("標楷體", 20);//設定字型與大小
            Font MyFont1 = new Font("標楷體", 10);
            Font num = new Font("標楷體", 40);
            float leftMargin = e.MarginBounds.Left;//取得文件左邊界
            float topMargin = e.MarginBounds.Top;//取得文件上邊界
            int count = 10;//起始列印的行數
            float yPos = 0f;//收集成列印起始點的參數
            yPos = topMargin + count * MyFont.GetHeight(e.Graphics);

            // public void DrawString (
            // string s, 要繪製的字串。
            // Font font, Font，定義字串的文字格式。
            // Brush brush, Brush，決定所繪製文字的色彩和紋理。
            // float x, 繪製文字左上角的 X 座標。
            // float y, 繪製文字左上角的 Y 座標。
            // StringFormat format StringFormat，指定套用到所繪製文字的格式化屬性，例如，行距和對齊。
            //)
            //將要列印的文字放入要列印的文件中
            if (print >= 10)
            {
                i = 2;
            }
            else if (print >= 100)
            {
                i = 1;
            }
            else if (print >= 1000)
            {
                i = 0;
            }
            decimalLength = print.ToString("D").Length + i;
            string printnum = print.ToString("D" + decimalLength.ToString());
            MyGraphics.DrawString(printnum, num, MyBrush, 70, 85, new StringFormat());
            MyGraphics.DrawString("日期:" + DateTime.Now.ToString("yyyy/MM/dd"), MyFont, MyBrush, 30, 145, new StringFormat());
            MyGraphics.DrawString("時間:" + DateTime.Now.ToString("HH:mm:ss"), MyFont, MyBrush, 30, 175, new StringFormat());
            MyGraphics.DrawString("請依叫號 等候辦理", MyFont1, MyBrush, 80, 210, new StringFormat());
            Console.WriteLine(print.ToString("D" + decimalLength.ToString()));
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
                                byte[] array1 = { 0xED, 0xED, wait2[0], wait1[0], num[0, 0], num[0, 1], num[0, 2], num[0, 3], cont1, 0x01, 0x00 };
                                byte[] array2 = { 0xED, 0xED, wait2[1], wait1[1], num[0, 0], num[0, 1], num[0, 2], num[0, 3], cont2, 0x01, 0x00 };
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
                                        num[0, 3] += 1;
                                        if (num[0, 3] == 10)
                                        {
                                            num[0, 3] = 0;
                                            num[0, 2] = (byte)(num[0, 2] + 1);
                                        }
                                        else if (num[0, 2] == 10)
                                        {
                                            num[0, 2] = 0;
                                            num[0, 1] = (byte)(num[0, 1] + 1);
                                        }
                                        else if (num[0, 1] == 10)
                                        {
                                            num[0, 1] = 0;
                                            num[0, 1] = (byte)(num[0, 1] + 1);
                                        }
                                        byte[] array1 = { 0xED, 0xED, wait2[0], wait1[0], num[0, 0], num[0, 1], num[0, 2], num[0, 3], cont1, 0x01, 0x00 };
                                        comport.Write(array1, 0, 11);
                                        label -= 1;
                                    }
                                    else if (wait2[0] != 0 && wait1[0] == 0)
                                    {
                                        //byte waits1 = wait1;
                                        wait2[0] -= 1;
                                        wait1[0] = 9;
                                        num[0, 3] += 1;
                                        if (num[0, 3] == 10)
                                        {
                                            num[0, 3] = 0;
                                            num[0, 2] = (byte)(num[0, 2] + 1);
                                        }
                                        else if (num[0, 2] == 10)
                                        {
                                            num[0, 2] = 0;
                                            num[0, 1] = (byte)(num[0, 1] + 1);
                                        }
                                        else if (num[0, 1] == 10)
                                        {
                                            num[0, 1] = 0;
                                            num[0, 1] = (byte)(num[0, 1] + 1);
                                        }
                                        byte[] array1 = { 0xED, 0xED, wait2[0], wait1[0], num[0, 0], num[0, 1], num[0, 2], num[0, 3], cont1, 0x01, 0x00 };
                                        comport.Write(array1, 0, 11);
                                        label -= 1;
                                    }
                                    else if (wait2[0] != 0 && wait1[0] != 0)
                                    {
                                        //byte waits1 = wait1;
                                        wait1[0] -= 1;
                                        num[0, 3] += 1;
                                        if (num[0, 3] == 10)
                                        {
                                            num[0, 3] = 0;
                                            num[0, 2] = (byte)(num[0, 2] + 1);
                                        }
                                        else if (num[0, 2] == 10)
                                        {
                                            num[0, 2] = 0;
                                            num[0, 1] = (byte)(num[0, 1] + 1);
                                        }
                                        else if (num[0, 1] == 10)
                                        {
                                            num[0, 1] = 0;
                                            num[0, 1] = (byte)(num[0, 1] + 1);
                                        }
                                        byte[] array1 = { 0xED, 0xED, wait2[0], wait1[0], num[0, 0], num[0, 1], num[0, 2], num[0, 3], cont1, 0x01, 0x00 };
                                        comport.Write(array1, 0, 11);
                                        label -= 1;
                                    }
                                    else if (wait2[0] == 0 && wait1[0] == 0)
                                    {
                                        MessageBox.Show("no wait");
                                        //byte waits1 = wait1;
                                        //wainum[0, 3]-= 1;
                                        byte[] array1 = { 0xED, 0xED, wait2[0], wait1[0], num[0, 0], num[0, 1], num[0, 2], num[0, 3], cont1, 0x00, 0x00 };
                                        comport.Write(array1, 0, 11);
                                    }
                                    else
                                    {
                                        MessageBox.Show("等待有誤");
                                    }
                                    /*player.SoundLocation = @"C:\Users\bock\github\program\voice\來賓.wav";
                                    player.PlaySync();*/
                                    if (num[0, 0] == 0 && num[0, 1] == 0 && num[0, 2] == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 3] + ".wav";
                                        player.PlaySync();

                                    }
                                    else if (num[0, 0] == 0 && num[0, 1] == 0 && num[0, 2] == 1 && num[0, 3] == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                    }
                                    else if (num[0, 0] == 0 && num[0, 1] == 0 && num[0, 2] == 1 && (num[0, 3] == 1 || num[0, 3] == 2 || num[0, 3] == 3 || num[0, 3] == 4 || num[0, 3] == 5 || num[0, 3] == 6 || num[0, 3] == 7 || num[0, 3] == 8 || num[0, 3] == 9))
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 3] + ".wav";
                                        player.PlaySync();
                                    }
                                    else if (num[0, 0] == 0 && num[0, 1] == 0 && (num[0, 2] == 2 || num[0, 2] == 3 || num[0, 2] == 4 || num[0, 2] == 5 || num[0, 2] == 6 || num[0, 2] == 7 || num[0, 2] == 8 || num[0, 2] == 9) && num[0, 3] == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 2] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                    }
                                    else if (num[0, 0] == 0 && num[0, 1] == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 2] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 3] + ".wav";
                                        player.PlaySync();

                                        Console.WriteLine(num[0, 2]);
                                        Console.WriteLine(num[0, 3]);
                                    }
                                    else if (num[0, 0] == 0 && (num[0, 1] == 1 || num[0, 1] == 2 || num[0, 1] == 3 || num[0, 1] == 4 || num[0, 1] == 5 || num[0, 1] == 6 || num[0, 1] == 7 || num[0, 1] == 8 || num[0, 1] == 9) && num[0, 2] == 0 && num[0, 3] == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 1] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                        player.PlaySync();
                                    }
                                    else if (num[0, 0] == 0 && (num[0, 1] == 1 || num[0, 1] == 2 || num[0, 1] == 3 || num[0, 1] == 4 || num[0, 1] == 5 || num[0, 1] == 6 || num[0, 1] == 7 || num[0, 1] == 8 || num[0, 1] == 9) && (num[0, 2] == 1 || num[0, 2] == 2 || num[0, 2] == 3 || num[0, 2] == 4 || num[0, 2] == 5 || num[0, 2] == 6 || num[0, 2] == 7 || num[0, 2] == 8 || num[0, 2] == 9) && num[0, 3] == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 1] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 2] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                    }
                                    else if (num[0, 0] == 0 && (num[0, 1] == 1 || num[0, 1] == 2 || num[0, 1] == 3 || num[0, 1] == 4 || num[0, 1] == 5 || num[0, 1] == 6 || num[0, 1] == 7 || num[0, 1] == 8 || num[0, 1] == 9) && num[0, 2] == 0 && (num[0, 3] == 1 || num[0, 3] == 2 || num[0, 3] == 3 || num[0, 3] == 4 || num[0, 3] == 5 || num[0, 3] == 6 || num[0, 3] == 7 || num[0, 3] == 8 || num[0, 3] == 9))
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 1] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 2] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 3] + ".wav";
                                        player.PlaySync();
                                    }
                                    else if (num[0, 0] == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 1] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 2] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 3] + ".wav";
                                        player.PlaySync();
                                    }
                                    else if ((num[0, 0] == 1 || num[0, 0] == 2 || num[0, 0] == 3 || num[0, 0] == 4 || num[0, 0] == 5 || num[0, 0] == 6 || num[0, 0] == 7 || num[0, 0] == 8 || num[0, 0] == 9) && num[0, 1] == 0 && num[0, 2] == 0 && num[0, 3] == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 0] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                        player.PlaySync();
                                    }
                                    else if ((num[0, 0] == 1 || num[0, 0] == 2 || num[0, 0] == 3 || num[0, 0] == 4 || num[0, 0] == 5 || num[0, 0] == 6 || num[0, 0] == 7 || num[0, 0] == 8 || num[0, 0] == 9) && (num[0, 1] == 1 || num[0, 1] == 2 || num[0, 1] == 3 || num[0, 1] == 4 || num[0, 1] == 5 || num[0, 1] == 6 || num[0, 1] == 7 || num[0, 1] == 8 || num[0, 1] == 9) && num[0, 2] == 0 && num[0, 3] == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 0] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 1] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                        player.PlaySync();
                                    }
                                    else if ((num[0, 0] == 1 || num[0, 0] == 2 || num[0, 0] == 3 || num[0, 0] == 4 || num[0, 0] == 5 || num[0, 0] == 6 || num[0, 0] == 7 || num[0, 0] == 8 || num[0, 0] == 9) && (num[0, 1] == 1 || num[0, 1] == 2 || num[0, 1] == 3 || num[0, 1] == 4 || num[0, 1] == 5 || num[0, 1] == 6 || num[0, 1] == 7 || num[0, 1] == 8 || num[0, 1] == 9) && (num[0, 2] == 1 || num[0, 2] == 2 || num[0, 2] == 3 || num[0, 2] == 4 || num[0, 2] == 5 || num[0, 2] == 6 || num[0, 2] == 7 || num[0, 2] == 8 || num[0, 2] == 9) && num[0, 3] == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 0] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 1] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 2] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                    }
                                    else if ((num[0, 0] == 1 || num[0, 0] == 2 || num[0, 0] == 3 || num[0, 0] == 4 || num[0, 0] == 5 || num[0, 0] == 6 || num[0, 0] == 7 || num[0, 0] == 8 || num[0, 0] == 9) && (num[0, 1] == 0 || num[0, 1] == 1 || num[0, 1] == 2 || num[0, 1] == 3 || num[0, 1] == 4 || num[0, 1] == 5 || num[0, 1] == 6 || num[0, 1] == 7 || num[0, 1] == 8 || num[0, 1] == 9) && (num[0, 2] == 0 || num[0, 2] == 1 || num[0, 2] == 2 || num[0, 2] == 3 || num[0, 2] == 4 || num[0, 2] == 5 || num[0, 2] == 6 || num[0, 2] == 7 || num[0, 2] == 8 || num[0, 2] == 9) && num[0, 3] == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 0] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 1] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 2] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                    }
                                    else if ((num[0, 0] == 1 || num[0, 0] == 2 || num[0, 0] == 3 || num[0, 0] == 4 || num[0, 0] == 5 || num[0, 0] == 6 || num[0, 0] == 7 || num[0, 0] == 8 || num[0, 0] == 9) && (num[0, 1] == 0 || num[0, 1] == 1 || num[0, 1] == 2 || num[0, 1] == 3 || num[0, 1] == 4 || num[0, 1] == 5 || num[0, 1] == 6 || num[0, 1] == 7 || num[0, 1] == 8 || num[0, 1] == 9) && (num[0, 2] == 0 || num[0, 2] == 1 || num[0, 2] == 2 || num[0, 2] == 3 || num[0, 2] == 4 || num[0, 2] == 5 || num[0, 2] == 6 || num[0, 2] == 7 || num[0, 2] == 8 || num[0, 2] == 9) && (num[0, 3] == 1 || num[0, 3] == 2 || num[0, 3] == 3 || num[0, 3] == 4 || num[0, 3] == 5 || num[0, 3] == 6 || num[0, 3] == 7 || num[0, 3] == 8 || num[0, 3] == 9))
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 0] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 1] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 2] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 3] + ".wav";
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
                                    if (wait2[0] == 0 && wait1[0] != 0)
                                    {
                                        //byte waits1 = wait1;
                                        wait1[0] -= 1;
                                        num[0, 3] += 1;
                                        if (num[0, 3] == 10)
                                        {
                                            num[0, 3] = 0;
                                            num[0, 2] = (byte)(num[0, 2] + 1);
                                        }
                                        else if (num[0, 2] == 10)
                                        {
                                            num[0, 2] = 0;
                                            num[0, 1] = (byte)(num[0, 1] + 1);
                                        }
                                        else if (num[0, 1] == 10)
                                        {
                                            num[0, 1] = 0;
                                            num[0, 1] = (byte)(num[0, 1] + 1);
                                        }
                                        byte[] array1 = { 0xED, 0xED, wait2[0], wait1[0], num[0, 0], num[0, 1], num[0, 2], num[0, 3], cont1, 0x01, 0x00 };
                                        comport.Write(array1, 0, 11);
                                        label -= 1;
                                    }
                                    else if (wait2[0] != 0 && wait1[0] == 0)
                                    {
                                        //byte waits1 = wait1;
                                        wait2[0] -= 1;
                                        wait1[0] = 9;
                                        num[0, 3] += 1;
                                        if (num[0, 3] == 10)
                                        {
                                            num[0, 3] = 0;
                                            num[0, 2] = (byte)(num[0, 2] + 1);
                                        }
                                        else if (num[0, 2] == 10)
                                        {
                                            num[0, 2] = 0;
                                            num[0, 1] = (byte)(num[0, 1] + 1);
                                        }
                                        else if (num[0, 1] == 10)
                                        {
                                            num[0, 1] = 0;
                                            num[0, 1] = (byte)(num[0, 1] + 1);
                                        }
                                        byte[] array1 = { 0xED, 0xED, wait2[0], wait1[0], num[0, 0], num[0, 1], num[0, 2], num[0, 3], cont1, 0x01, 0x00 };
                                        comport.Write(array1, 0, 11);
                                        label -= 1;
                                    }
                                    else if (wait2[0] != 0 && wait1[0] != 0)
                                    {
                                        //byte waits1 = wait1;
                                        wait1[0] -= 1;
                                        num[0, 3] += 1;
                                        if (num[0, 3] == 10)
                                        {
                                            num[0, 3] = 0;
                                            num[0, 2] = (byte)(num[0, 2] + 1);
                                        }
                                        else if (num[0, 2] == 10)
                                        {
                                            num[0, 2] = 0;
                                            num[0, 1] = (byte)(num[0, 1] + 1);
                                        }
                                        else if (num[0, 1] == 10)
                                        {
                                            num[0, 1] = 0;
                                            num[0, 1] = (byte)(num[0, 1] + 1);
                                        }
                                        byte[] array1 = { 0xED, 0xED, wait2[0], wait1[0], num[0, 0], num[0, 1], num[0, 2], num[0, 3], cont1, 0x01, 0x00 };
                                        comport.Write(array1, 0, 11);
                                        label -= 1;
                                    }
                                    else if (wait2[0] == 0 && wait1[0] == 0)
                                    {
                                        MessageBox.Show("no wait");
                                        //byte waits1 = wait1;
                                        //wainum[0, 3]-= 1;
                                        byte[] array1 = { 0xED, 0xED, wait2[0], wait1[0], num[0, 0], num[0, 1], num[0, 2], num[0, 3], cont1, 0x00, 0x00 };
                                        comport.Write(array1, 0, 11);
                                    }
                                    else
                                    {
                                        MessageBox.Show("等待有誤");
                                    }
                                    /*player.SoundLocation = @"C:\Users\bock\github\program\voice\來賓.wav";
                                    player.PlaySync();*/
                                    if (num[0, 0] == 0 && num[0, 1] == 0 && num[0, 2] == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 3] + ".wav";
                                        player.PlaySync();

                                    }
                                    else if (num[0, 0] == 0 && num[0, 1] == 0 && num[0, 2] == 1 && num[0, 3] == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                    }
                                    else if (num[0, 0] == 0 && num[0, 1] == 0 && num[0, 2] == 1 && (num[0, 3] == 1 || num[0, 3] == 2 || num[0, 3] == 3 || num[0, 3] == 4 || num[0, 3] == 5 || num[0, 3] == 6 || num[0, 3] == 7 || num[0, 3] == 8 || num[0, 3] == 9))
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 3] + ".wav";
                                        player.PlaySync();
                                    }
                                    else if (num[0, 0] == 0 && num[0, 1] == 0 && (num[0, 2] == 2 || num[0, 2] == 3 || num[0, 2] == 4 || num[0, 2] == 5 || num[0, 2] == 6 || num[0, 2] == 7 || num[0, 2] == 8 || num[0, 2] == 9) && num[0, 3] == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 2] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                    }
                                    else if (num[0, 0] == 0 && num[0, 1] == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 2] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 3] + ".wav";
                                        player.PlaySync();

                                        Console.WriteLine(num[0, 2]);
                                        Console.WriteLine(num[0, 3]);
                                    }
                                    else if (num[0, 0] == 0 && (num[0, 1] == 1 || num[0, 1] == 2 || num[0, 1] == 3 || num[0, 1] == 4 || num[0, 1] == 5 || num[0, 1] == 6 || num[0, 1] == 7 || num[0, 1] == 8 || num[0, 1] == 9) && num[0, 2] == 0 && num[0, 3] == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 1] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                        player.PlaySync();
                                    }
                                    else if (num[0, 0] == 0 && (num[0, 1] == 1 || num[0, 1] == 2 || num[0, 1] == 3 || num[0, 1] == 4 || num[0, 1] == 5 || num[0, 1] == 6 || num[0, 1] == 7 || num[0, 1] == 8 || num[0, 1] == 9) && (num[0, 2] == 1 || num[0, 2] == 2 || num[0, 2] == 3 || num[0, 2] == 4 || num[0, 2] == 5 || num[0, 2] == 6 || num[0, 2] == 7 || num[0, 2] == 8 || num[0, 2] == 9) && num[0, 3] == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 1] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 2] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                    }
                                    else if (num[0, 0] == 0 && (num[0, 1] == 1 || num[0, 1] == 2 || num[0, 1] == 3 || num[0, 1] == 4 || num[0, 1] == 5 || num[0, 1] == 6 || num[0, 1] == 7 || num[0, 1] == 8 || num[0, 1] == 9) && num[0, 2] == 0 && (num[0, 3] == 1 || num[0, 3] == 2 || num[0, 3] == 3 || num[0, 3] == 4 || num[0, 3] == 5 || num[0, 3] == 6 || num[0, 3] == 7 || num[0, 3] == 8 || num[0, 3] == 9))
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 1] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 2] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 3] + ".wav";
                                        player.PlaySync();
                                    }
                                    else if (num[0, 0] == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 1] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 2] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 3] + ".wav";
                                        player.PlaySync();
                                    }
                                    else if ((num[0, 0] == 1 || num[0, 0] == 2 || num[0, 0] == 3 || num[0, 0] == 4 || num[0, 0] == 5 || num[0, 0] == 6 || num[0, 0] == 7 || num[0, 0] == 8 || num[0, 0] == 9) && num[0, 1] == 0 && num[0, 2] == 0 && num[0, 3] == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 0] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                        player.PlaySync();
                                    }
                                    else if ((num[0, 0] == 1 || num[0, 0] == 2 || num[0, 0] == 3 || num[0, 0] == 4 || num[0, 0] == 5 || num[0, 0] == 6 || num[0, 0] == 7 || num[0, 0] == 8 || num[0, 0] == 9) && (num[0, 1] == 1 || num[0, 1] == 2 || num[0, 1] == 3 || num[0, 1] == 4 || num[0, 1] == 5 || num[0, 1] == 6 || num[0, 1] == 7 || num[0, 1] == 8 || num[0, 1] == 9) && num[0, 2] == 0 && num[0, 3] == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 0] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 1] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                        player.PlaySync();
                                    }
                                    else if ((num[0, 0] == 1 || num[0, 0] == 2 || num[0, 0] == 3 || num[0, 0] == 4 || num[0, 0] == 5 || num[0, 0] == 6 || num[0, 0] == 7 || num[0, 0] == 8 || num[0, 0] == 9) && (num[0, 1] == 1 || num[0, 1] == 2 || num[0, 1] == 3 || num[0, 1] == 4 || num[0, 1] == 5 || num[0, 1] == 6 || num[0, 1] == 7 || num[0, 1] == 8 || num[0, 1] == 9) && (num[0, 2] == 1 || num[0, 2] == 2 || num[0, 2] == 3 || num[0, 2] == 4 || num[0, 2] == 5 || num[0, 2] == 6 || num[0, 2] == 7 || num[0, 2] == 8 || num[0, 2] == 9) && num[0, 3] == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 0] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 1] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 2] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                    }
                                    else if ((num[0, 0] == 1 || num[0, 0] == 2 || num[0, 0] == 3 || num[0, 0] == 4 || num[0, 0] == 5 || num[0, 0] == 6 || num[0, 0] == 7 || num[0, 0] == 8 || num[0, 0] == 9) && (num[0, 1] == 0 || num[0, 1] == 1 || num[0, 1] == 2 || num[0, 1] == 3 || num[0, 1] == 4 || num[0, 1] == 5 || num[0, 1] == 6 || num[0, 1] == 7 || num[0, 1] == 8 || num[0, 1] == 9) && (num[0, 2] == 0 || num[0, 2] == 1 || num[0, 2] == 2 || num[0, 2] == 3 || num[0, 2] == 4 || num[0, 2] == 5 || num[0, 2] == 6 || num[0, 2] == 7 || num[0, 2] == 8 || num[0, 2] == 9) && num[0, 3] == 0)
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 0] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 1] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 2] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                    }
                                    else if ((num[0, 0] == 1 || num[0, 0] == 2 || num[0, 0] == 3 || num[0, 0] == 4 || num[0, 0] == 5 || num[0, 0] == 6 || num[0, 0] == 7 || num[0, 0] == 8 || num[0, 0] == 9) && (num[0, 1] == 0 || num[0, 1] == 1 || num[0, 1] == 2 || num[0, 1] == 3 || num[0, 1] == 4 || num[0, 1] == 5 || num[0, 1] == 6 || num[0, 1] == 7 || num[0, 1] == 8 || num[0, 1] == 9) && (num[0, 2] == 0 || num[0, 2] == 1 || num[0, 2] == 2 || num[0, 2] == 3 || num[0, 2] == 4 || num[0, 2] == 5 || num[0, 2] == 6 || num[0, 2] == 7 || num[0, 2] == 8 || num[0, 2] == 9) && (num[0, 3] == 1 || num[0, 3] == 2 || num[0, 3] == 3 || num[0, 3] == 4 || num[0, 3] == 5 || num[0, 3] == 6 || num[0, 3] == 7 || num[0, 3] == 8 || num[0, 3] == 9))
                                    {
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 0] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 1] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 2] + ".wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                        player.PlaySync();
                                        player.SoundLocation = Application.StartupPath + @"\voice\" + num[0, 3] + ".wav";
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
                            else if (buffer[6] == cont2)
                            {
                                MessageBox.Show("指定2");
                            }
                        }
                        else
                        {
                            byte[] array1 = { 0xED, 0xED, wait2[0], wait1[0], 0x00, 0x00, 0x00, 0x00, cont1, 0x01, 0x00 };
                            comport.Write(array1, 0, 11);
                            Console.WriteLine("陣列外");
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
            /*richTextBox1.Text += String.Format("{0}{1}", BitConverter.ToString(buffer), Environment.NewLine);
            totalLength = totalLength + buffer.Length;
            label1.Text = totalLength.ToString();*/
            label_wait.Text = label.ToString();
        }

        private void Form1_Leave(object sender, EventArgs e)
        {
            comport.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {
           byte[] array1 = { 0xED, 0xED, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x01, 0x00};
           foreach (byte byteValue in array1)
           {
                    Console.WriteLine(byteValue);
           }                      
        }
        void mouseDoubleClick(object sender, EventArgs e)
        {
            doubleClickTimer = DateTime.Now; //記下DoubleClick的時間 
        }

        private void exit_Click(object sender, EventArgs e)
        {
            TimeSpan t = (TimeSpan)(DateTime.Now - doubleClickTimer); //DoubleClick後又點了一下, 計算時間差 

            if (t.TotalMilliseconds <= 200) //如果小於200豪秒就執行 
            {
                Console.WriteLine("3");
            }
        }
    }
}
