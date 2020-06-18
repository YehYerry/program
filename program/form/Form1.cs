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
using System.Drawing.Printing;

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
        byte wait1 = 0;
        byte wait2 = 0;        
        int label = 0;
        int print = 0;
        int i = 3;
        int decimalLength;
        byte num1;
        byte num2;
        byte num3;
        byte num4;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BackgroundImage = new Bitmap(Application.StartupPath + @"\background\back.jpg");
            //BackgroundImageLayout = ImageLayout.Stretch;
            //按鈕圖
            Image pic = new Bitmap(Application.StartupPath + @"\background\button.png");
            button1.BackgroundImage = pic;

            comport = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
            comport.ReadTimeout = 2000;
            comport.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            /*if (!comport.IsOpen)
            {
                comport.Open();
                receiving = true;
                t = new Thread(DoReceive);
                t.IsBackground = true;
                t.Start();               
            }*/
            MessageBox.Show("開啟");
        }

        public void button1_Click(object sender, EventArgs e)
        {
            print += 1;
            wait1 += 1;
            //wait1 = (byte)(a + 1);
            if (wait1 == 10)
            {
                wait1 = 0;
                wait2 += 1;
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
                        if (buffer[0] == 165 && buffer[1] == 182 && buffer[2] == 0 && buffer[3] == 0 && buffer[4] == 0 && buffer[5] == 0 && buffer[6] == 127)
                        {
                            try
                            {
                                //MessageBox.Show("12213");
                                //byte waits1 = wait1;
                                //waits1 -= 1;
                                byte[] array1 = { 0xED, 0xED, wait2, wait1, t4, t3, t2, t1, 0x01, 0x01, 0x00 };
                                comport.Write(array1, 0, 11);
                            }
                            catch (TimeoutException timeoutEx)
                            {
                                MessageBox.Show("送出失敗");
                            }
                        }
                        else if (buffer[0] == 165 && buffer[1] == 182 && buffer[6] == 01 && buffer[7] == 01)
                        {                            
                            try
                            {
                                if (wait2 == 0 && wait1 != 0)
                                {
                                    //byte waits1 = wait1;
                                    wait1 -= 1;
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
                                    byte[] array1 = { 0xED, 0xED, wait2, wait1, t4, t3, t2, t1, 0x01, 0x01, 0x00 };
                                    comport.Write(array1, 0, 11);
                                    label -= 1;
                                }
                                else if (wait2 != 0 && wait1 == 0)
                                {
                                    //byte waits1 = wait1;
                                    wait2 -= 1;
                                    wait1 = 9;
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
                                    byte[] array1 = { 0xED, 0xED, wait2, wait1, t4, t3, t2, t1, 0x01, 0x01, 0x00 };
                                    comport.Write(array1, 0, 11);
                                    label -= 1;
                                }
                                else if (wait2 != 0 && wait1 != 0)
                                {
                                    //byte waits1 = wait1;
                                    wait1 -= 1;
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
                                    byte[] array1 = { 0xED, 0xED, wait2, wait1, t4, t3, t2, t1, 0x01, 0x01, 0x00 };
                                    comport.Write(array1, 0, 11);
                                    label -= 1;
                                }
                                else if (wait2 == 0 && wait1 == 0)
                                {
                                    MessageBox.Show("no wait");
                                    //byte waits1 = wait1;
                                    //wait1 -= 1;
                                    byte[] array1 = { 0xED, 0xED, wait2, wait1, t4, t3, t2, t1, 0x01, 0x00, 0x00 };
                                    comport.Write(array1, 0, 11);
                                }
                                else
                                {
                                    MessageBox.Show("等待有誤");
                                }
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\來賓.wav";
                                player.PlaySync();
                                if (t4 == 0 && t3 == 0 && t2 == 0)
                                {
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\" + t1 + ".wav";
                                    player.PlaySync();

                                }
                                else if (t4 == 0 && t3 == 0 && t2 == 1 && t1 == 0)
                                {
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\拾.wav";
                                    player.PlaySync();
                                }
                                else if (t4 == 0 && t3 == 0 && t2 == 1 && (t1 == 1 || t1 == 2 || t1 == 3 || t1 == 4 || t1 == 5 || t1 == 6 || t1 == 7 || t1 == 8 || t1 == 9))
                                {
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\拾.wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\" + t1 + ".wav";
                                    player.PlaySync();
                                }
                                else if (t4 == 0 && t3 == 0 && (t2 == 2 || t2 == 3 || t2 == 4 || t2 == 5 || t2 == 6 || t2 == 7 || t2 == 8 || t2 == 9) && t1 == 0)
                                {
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\" + t2 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\拾.wav";
                                    player.PlaySync();
                                }
                                else if (t4 == 0 && t3 == 0)
                                {
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\" + t2 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\拾.wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\" + t1 + ".wav";
                                    player.PlaySync();
                                }
                                else if (t4 == 0 && (t3 == 1 || t3 == 2 || t3 == 3 || t3 == 4 || t3 == 5 || t3 == 6 || t3 == 7 || t3 == 8 || t3 == 9) && t2 == 0 && t1 == 0)
                                {
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\" + t3 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\佰_.wav";
                                    player.PlaySync();
                                }
                                else if (t4 == 0 && (t3 == 1 || t3 == 2 || t3 == 3 || t3 == 4 || t3 == 5 || t3 == 6 || t3 == 7 || t3 == 8 || t3 == 9) && (t2 == 1 || t2 == 2 || t2 == 3 || t2 == 4 || t2 == 5 || t2 == 6 || t2 == 7 || t2 == 8 || t2 == 9) && t1 == 0)
                                {
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\" + t3 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\佰_.wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\" + t2 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\拾.wav";
                                    player.PlaySync();
                                }
                                else if (t4 == 0)
                                {
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\" + t3 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\佰_.wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\" + t2 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\拾.wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\" + t1 + ".wav";
                                    player.PlaySync();
                                }
                                else if ((t4 == 1 || t4 == 2 || t4 == 3 || t4 == 4 || t4 == 5 || t4 == 6 || t4 == 7 || t4 == 8 || t4 == 9) && t3 == 0 && t2 == 0 && t1 == 0)
                                {
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\" + t4 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\仟.wav";
                                    player.PlaySync();
                                }
                                else if ((t4 == 1 || t4 == 2 || t4 == 3 || t4 == 4 || t4 == 5 || t4 == 6 || t4 == 7 || t4 == 8 || t4 == 9) && (t3 == 1 || t3 == 2 || t3 == 3 || t3 == 4 || t3 == 5 || t3 == 6 || t3 == 7 || t3 == 8 || t3 == 9) && t2 == 0 && t1 == 0)
                                {
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\" + t4 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\仟.wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\" + t3 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\佰_.wav";
                                    player.PlaySync();
                                }
                                else if ((t4 == 1 || t4 == 2 || t4 == 3 || t4 == 4 || t4 == 5 || t4 == 6 || t4 == 7 || t4 == 8 || t4 == 9) && (t3 == 1 || t3 == 2 || t3 == 3 || t3 == 4 || t3 == 5 || t3 == 6 || t3 == 7 || t3 == 8 || t3 == 9) && (t2 == 1 || t2 == 2 || t2 == 3 || t2 == 4 || t2 == 5 || t2 == 6 || t2 == 7 || t2 == 8 || t2 == 9) && t1 == 0)
                                {
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\" + t4 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\仟.wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\" + t3 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\佰_.wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\" + t2 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\拾.wav";
                                    player.PlaySync();
                                }
                                else
                                {
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\" + t4 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\仟.wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\" + t3 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\佰_.wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\" + t2 + ".wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\拾.wav";
                                    player.PlaySync();
                                    player.SoundLocation = @"C:\Users\bock\github\program\voice\" + t1 + ".wav";
                                    player.PlaySync();
                                }                             
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\號.wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\請到.wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\1.wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\號.wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\櫃台.wav";
                                player.PlaySync();
                            }
                            catch (TimeoutException timeoutEx)
                            {
                                MessageBox.Show("送出失敗");
                            }
                        }
                        else if (buffer[0] == 165 && buffer[1] == 182 && (buffer[2] == 0 || buffer[2] == 1 || buffer[2] == 2 || buffer[2] == 3 || buffer[2] == 4 || buffer[2] == 5 || buffer[2] == 6 || buffer[2] == 7 || buffer[2] == 8 || buffer[2] == 9) && (buffer[3] == 0 || buffer[3] == 1 || buffer[3] == 2 || buffer[3] == 3 || buffer[3] == 4 || buffer[3] == 5 || buffer[3] == 6 || buffer[3] == 7 || buffer[3] == 8 || buffer[3] == 9) && (buffer[4] == 0 || buffer[4] == 1 || buffer[4] == 2 || buffer[4] == 3 || buffer[4] == 4 || buffer[4] == 5 || buffer[4] == 6 || buffer[4] == 7 || buffer[4] == 8 || buffer[4] == 9) && (buffer[5] == 0 || buffer[5] == 1 || buffer[5] == 2 || buffer[5] == 3 || buffer[5] == 4 || buffer[5] == 5 || buffer[5] == 6 || buffer[5] == 7 || buffer[5] == 8 || buffer[5] == 9) && buffer[6] == 127 && buffer[6] == 127 && buffer[6] == 127)//指定叫號
                        {
                            num1 = buffer[2];
                            num2 = buffer[3];
                            num3 = buffer[4];
                            num4 = buffer[5];
                            byte[] array1 = { 0xED, 0xED, wait2, wait1, num1, num2, num3, num4, 0x01, 0x02, 0x00 };
                            comport.Write(array1, 0, 11);
                            player.SoundLocation = @"C:\Users\bock\github\program\voice\來賓.wav";
                                player.PlaySync();
                            if (num1 == 0 && num2 == 0 && num3 == 0)
                            {
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\" + num4 + ".wav";
                                player.PlaySync();

                            }
                            else if (num1 == 0 && num2 == 0 && num3 == 1 && num4 == 0)
                            {
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\拾.wav";
                                player.PlaySync();
                            }
                            else if (num1 == 0 && num2 == 0 && num3 == 1 && (num4 == 1 || num4 == 2 || num4 == 3 || num4 == 4 || num4 == 5 || num4 == 6 || num4 == 7 || num4 == 8 || num4 == 9))
                            {
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\拾.wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\" + num4 + ".wav";
                                player.PlaySync();
                            }
                            else if (num1 == 0 && num2 == 0 && (num3 == 2 || num3 == 3 || num3 == 4 || num3 == 5 || num3 == 6 || num3 == 7 || num3 == 8 || num3 == 9) && num4 == 0)
                            {
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\" + num3 + ".wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\拾.wav";
                                player.PlaySync();
                            }
                            else if (num1 == 0 && num2 == 0)
                            {
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\" + num3 + ".wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\拾.wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\" + num4 + ".wav";
                                player.PlaySync();
                            }
                            else if (num1 == 0 && (num2 == 1 || num2 == 2 || num2 == 3 || num2 == 4 || num2 == 5 || num2 == 6 || num2 == 7 || num2 == 8 || num2 == 9) && num3 == 0 && num4 == 0)
                            {
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\" + num2 + ".wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\佰_.wav";
                                player.PlaySync();
                            }
                            else if (num1 == 0 && (num2 == 1 || num2 == 2 || num2 == 3 || num2 == 4 || num2 == 5 || num2 == 6 || num2 == 7 || num2 == 8 || num2 == 9) && (num3 == 1 || num3 == 2 || num3 == 3 || num3 == 4 || num3 == 5 || num3 == 6 || num3 == 7 || num3 == 8 || num3 == 9) && num4 == 0)
                            {
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\" + num2 + ".wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\佰_.wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\" + num3 + ".wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\拾.wav";
                                player.PlaySync();
                            }
                            else if (num1 == 0)
                            {
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\" + num2 + ".wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\佰_.wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\" + num3 + ".wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\拾.wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\" + num4 + ".wav";
                                player.PlaySync();
                            }
                            else if ((num1 == 1 || num1 == 2 || num1 == 3 || num1 == 4 || num1 == 5 || num1 == 6 || num1 == 7 || num1 == 8 || num1 == 9) && num2 == 0 && num3 == 0 && num4 == 0)
                            {
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\" + num1 + ".wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\仟.wav";
                                player.PlaySync();
                            }
                            else if ((num1 == 1 || num1 == 2 || num1 == 3 || num1 == 4 || num1 == 5 || num1 == 6 || num1 == 7 || num1 == 8 || num1 == 9) && (num2 == 1 || num2 == 2 || num2 == 3 || num2 == 4 || num2 == 5 || num2 == 6 || num2 == 7 || num2 == 8 || num2 == 9) && num3 == 0 && num4 == 0)
                            {
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\" + num1 + ".wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\仟.wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\" + num2 + ".wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\佰_.wav";
                                player.PlaySync();
                            }
                            else if ((num1 == 1 || num1 == 2 || num1 == 3 || num1 == 4 || num1 == 5 || num1 == 6 || num1 == 7 || num1 == 8 || num1 == 9) && (num2 == 1 || num2 == 2 || num2 == 3 || num2 == 4 || num2 == 5 || num2 == 6 || num2 == 7 || num2 == 8 || num2 == 9) && (num3 == 1 || num3 == 2 || num3 == 3 || num3 == 4 || num3 == 5 || num3 == 6 || num3 == 7 || num3 == 8 || num3 == 9) && num4 == 0)
                            {
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\" + num1 + ".wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\仟.wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\" + num2 + ".wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\佰_.wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\" + num3 + ".wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\拾.wav";
                                player.PlaySync();
                            }
                            else if ((num1 == 1 || num1 == 2 || num1 == 3 || num1 == 4 || num1 == 5 || num1 == 6 || num1 == 7 || num1 == 8 || num1 == 9) && (num2 == 1 || num2 == 2 || num2 == 3 || num2 == 4 || num2 == 5 || num2 == 6 || num2 == 7 || num2 == 8 || num2 == 9) && (num3 == 1 || num3 == 2 || num3 == 3 || num3 == 4 || num3 == 5 || num3 == 6 || num3 == 7 || num3 == 8 || num3 == 9) && (num4 == 1 || num4 == 2 || num4 == 3 || num4 == 4 || num4 == 5 || num4 == 6 || num4 == 7 || num4 == 8 || num4 == 9))
                            {
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\" + num1 + ".wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\仟.wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\" + num2 + ".wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\佰_.wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\" + num3 + ".wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\拾.wav";
                                player.PlaySync();
                                player.SoundLocation = @"C:\Users\bock\github\program\voice\" + num4 + ".wav";
                                player.PlaySync();
                            }
                            player.SoundLocation = @"C:\Users\bock\github\program\voice\號.wav";
                               player.PlaySync();
                               player.SoundLocation = @"C:\Users\bock\github\program\voice\請到.wav";
                               player.PlaySync();
                               player.SoundLocation = @"C:\Users\bock\github\program\voice\1.wav";
                               player.PlaySync();
                               player.SoundLocation = @"C:\Users\bock\github\program\voice\號.wav";
                               player.PlaySync();
                               player.SoundLocation = @"C:\Users\bock\github\program\voice\櫃台.wav";
                               player.PlaySync();
                        }
                        else
                        {
                            byte[] array1 = { 0xED, 0xED, wait2, wait1, 0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00 };
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
