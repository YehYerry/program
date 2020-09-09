using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace finalprogram
{
    public partial class Main : Form
    {
        private SerialPort comport;
        delegate void Display(Byte[] buffer);
        private Int32 totalLength = 0;
        private Boolean receiving;
        private Thread t;
        byte[,] cont = new byte[15, 15];
        string b1, b2, b3, b4, port;
        int b1h, b2h, b3h, b4h, b1w, b2w, b3w, b4w;
        int L1s, L2s, L3s, L4s;
        string L1f, L2f, L3f, L4f;
        int b1bh, b2bh, b3bh, b4bh, b1bw, b2bw, b3bw, b4bw;
        int L1bh, L2bh, L3bh, L4bh, L1bw, L2bw, L3bw, L4bw;

        byte b1num1, b1num2, b1num3, b1num4, b2num1, b2num2, b2num3, b2num4, b3num1, b3num2, b3num3, b3num4, b4num1, b4num2, b4num3, b4num4;
        byte[,] num;
        byte[] wait1 = { 0, 0, 0, 0 }; //四個按鈕的等待人數(十位數)
        byte[] wait2 = { 0, 0, 0, 0 }; //四個按鈕的等待人數(個位數)      
        int label = 0, label2 = 0, label3 = 0, label4 = 0;
        byte num1, num2, num3, num4, w1, w2, n1, n2, n3, n4;
        string[] line = new string[1000];
        string[] line1 = new string[1000];
        string[] line2 = new string[1000];
        int decimalLength;
        int ctr = 0, ctr1 = 0, ctr2 = 0, note, notej1 = 0, notej2 = 0, notej3 = 0, notej4 = 0, pnum1, pnum2, pnum3, pnum4;
        int pageH, pageW, pbw, pbh, photoW, photoH, wordstyle1, wordstyle2, wordstyle3, nums, numbw, numbh, str1s, str1bw, str1bh, str2s, str2bw, str2bh;
        string numfont, str1font, str2font, text, pnumber1, pnumber2, pnumber3, pnumber4;
        FontStyle wdstyle1, wdstyle2, wdstyle3;
        DateTime doubleClickTimer = DateTime.Now;
        public Main()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StreamReader str = new StreamReader(Application.StartupPath + @"\config.txt");//讀取文字檔
            StreamReader str1 = new StreamReader(Application.StartupPath + @"\print_config.txt");
            StreamReader str2 = new StreamReader(Application.StartupPath + @"\Log\exitnum_log.txt");
            do
            {
                ctr++;
                line[ctr] = str.ReadLine();
                //Console.WriteLine(line[ctr]);
            } while (line[ctr] != null);
            port = line[75];

            for (int i = 2; i <= 16; i++)
            {
                note = Convert.ToInt32(line[i].Substring(6));
                if (note == 1)
                {
                    cont[0, notej1] = Convert.ToByte(line[i].Substring(3, 2));
                    notej1 += 1;
                }
                else if (note == 2)
                {
                    cont[1, notej2] = Convert.ToByte(line[i].Substring(3, 2));
                    notej2 += 1;
                }
                else if (note == 3)
                {
                    cont[2, notej3] = Convert.ToByte(line[i].Substring(3, 2));
                    notej3 += 1;
                }
                else if (note == 4)
                {
                    cont[3, notej4] = Convert.ToByte(line[i].Substring(3, 2));
                    notej4 += 1;
                }
            }//取得控制器對哪個按鈕，存入陣列 cont[0,0] 中 , notej1為控制器1的數量

            //設定背景圖
            BackgroundImage = new Bitmap(Application.StartupPath + @"\background\back.jpg");
            BackgroundImageLayout = ImageLayout.Stretch; //設定圖片填滿
            Image pic1 = new Bitmap(Application.StartupPath + @"\background\button.jpg");
            Image pic2 = new Bitmap(Application.StartupPath + @"\background\button.jpg");
            Image pic3 = new Bitmap(Application.StartupPath + @"\background\button.jpg");
            Image pic4 = new Bitmap(Application.StartupPath + @"\background\button.jpg");
            button1.BackgroundImageLayout = ImageLayout.Stretch; //設定圖片填滿
            button2.BackgroundImageLayout = ImageLayout.Stretch;
            button3.BackgroundImageLayout = ImageLayout.Stretch;
            button4.BackgroundImageLayout = ImageLayout.Stretch;
            button1.BackgroundImage = pic1;
            button2.BackgroundImage = pic2;
            button3.BackgroundImage = pic1;
            button4.BackgroundImage = pic2;
            /*byte[] val = Encoding.UTF8.GetBytes(line[2].Substring(6));
            foreach (byte s1 in val)
            Console.WriteLine(s1);*/
            comport = new SerialPort(port, 9600, Parity.None, 8, StopBits.One);
            comport.ReadTimeout = 2000;
            comport.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            try
            {
                if (!comport.IsOpen)
                {
                    comport.Open();
                    receiving = true;
                    t = new Thread(DoReceive);
                    t.IsBackground = true;
                    t.Start();
                }
            }
            catch 
            {
                MessageBox.Show("COMPORT不存在!請點選右上角離開系統做COMPORT設定!");
            }
            {
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
            }//config的獲取參數檔
            
            {
                do
                {
                    ctr1++;
                    line1[ctr1] = str1.ReadLine();
                } while (line1[ctr1] != null);
                pageH = Convert.ToInt32(line1[2].Substring(6));
                pageW = Convert.ToInt32(line1[3].Substring(7));
                pbw = Convert.ToInt32(line1[6].Substring(12));
                pbh = Convert.ToInt32(line1[7].Substring(13));
                photoW = Convert.ToInt32(line1[8].Substring(6));
                photoH = Convert.ToInt32(line1[9].Substring(7));
                numfont = line1[12].Substring(7);
                nums = Convert.ToInt32(line1[13].Substring(5));
                wordstyle1 = Convert.ToInt32(line1[14].Substring(6));
                numbw = Convert.ToInt32(line1[15].Substring(12));
                numbh = Convert.ToInt32(line1[16].Substring(13));
                str1font = line1[19].Substring(7);
                str1s = Convert.ToInt32(line1[20].Substring(5));
                wordstyle2 = Convert.ToInt32(line1[21].Substring(6));
                str1bw = Convert.ToInt32(line1[22].Substring(12));
                str1bh = Convert.ToInt32(line1[23].Substring(13));
                str2font = line1[26].Substring(7);
                str2s = Convert.ToInt32(line1[27].Substring(5));
                wordstyle3 = Convert.ToInt32(line1[28].Substring(6));
                str2bw = Convert.ToInt32(line1[29].Substring(12));
                str2bh = Convert.ToInt32(line1[30].Substring(13));
                text = line1[31].Substring(5);
            }//讀取列印文字檔
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

            num = new byte[,] { { b1num1, b1num2, b1num3, b1num4 }, { b2num1, b2num2, b2num3, b2num4 }, { b3num1, b3num2, b3num3, b3num4 }, { b4num1, b4num2, b4num3, b4num4 } };

            this.WindowState = FormWindowState.Maximized;
            //列印號碼
            do
            {
                ctr2++;
                line2[ctr2] = str2.ReadLine();
            } while (line2[ctr2] != null);
            pnum1 = Convert.ToInt32(line2[1].Substring(17, 4));//起始號碼
            pnum2 = Convert.ToInt32(line2[2].Substring(17, 4));
            pnum3 = Convert.ToInt32(line2[3].Substring(17, 4));
            pnum4 = Convert.ToInt32(line2[4].Substring(17, 4));
            wait2[0] = Convert.ToByte(line2[1].Substring(8, 1));
            wait1[0] = Convert.ToByte(line2[1].Substring(9, 1));
            wait2[1] = Convert.ToByte(line2[2].Substring(8, 1));
            wait1[1] = Convert.ToByte(line2[2].Substring(9, 1));
            wait2[2] = Convert.ToByte(line2[3].Substring(8, 1));
            wait1[2] = Convert.ToByte(line2[3].Substring(9, 1));
            wait2[3] = Convert.ToByte(line2[4].Substring(8, 1));
            wait1[3] = Convert.ToByte(line2[4].Substring(9, 1));
            b1num1 = Convert.ToByte(line2[1].Substring(17, 1));
            b1num2 = Convert.ToByte(line2[1].Substring(18, 1));
            b1num3 = Convert.ToByte(line2[1].Substring(19, 1));
            b1num4 = Convert.ToByte(line2[1].Substring(20, 1));
            b2num1 = Convert.ToByte(line2[2].Substring(17, 1));
            b2num2 = Convert.ToByte(line2[2].Substring(18, 1));
            b2num3 = Convert.ToByte(line2[2].Substring(19, 1));
            b2num4 = Convert.ToByte(line2[2].Substring(20, 1));
            b3num1 = Convert.ToByte(line2[3].Substring(17, 1));
            b3num2 = Convert.ToByte(line2[3].Substring(18, 1));
            b3num3 = Convert.ToByte(line2[3].Substring(19, 1));
            b3num4 = Convert.ToByte(line2[3].Substring(20, 1));
            b4num1 = Convert.ToByte(line2[4].Substring(17, 1));
            b4num2 = Convert.ToByte(line2[4].Substring(18, 1));
            b4num3 = Convert.ToByte(line2[4].Substring(19, 1));
            b4num4 = Convert.ToByte(line2[4].Substring(20, 1));
            num = new byte[,] { { b1num1, b1num2, b1num3, b1num4 }, { b2num1, b2num2, b2num3, b2num4 }, { b3num1, b3num2, b3num3, b3num4 }, { b4num1, b4num2, b4num3, b4num4 } };
            //關閉前的等待人數
            label = Convert.ToInt32(line2[1].Substring(8, 1)) * 10 + Convert.ToInt32(line2[1].Substring(9, 1));
            label2 = Convert.ToInt32(line2[2].Substring(8, 1)) * 10 + Convert.ToInt32(line2[2].Substring(9, 1));
            label3 = Convert.ToInt32(line2[2].Substring(8, 1)) * 10 + Convert.ToInt32(line2[2].Substring(9, 1));
            label4 = Convert.ToInt32(line2[2].Substring(8, 1)) * 10 + Convert.ToInt32(line2[2].Substring(9, 1));

            //按鈕是否出現
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
            str.Close();
            str1.Close();
            str2.Close();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            pnum1 += 1;//列印的號碼加1
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
             
            //宣告一個印表機
            PrintDocument pd = new PrintDocument();
            //設定印表機邊界
            Margins margin = new Margins(0, 0, 0, 0);
            pd.DefaultPageSettings.Margins = margin;
            ////設定紙張大小 'vbPRPSUser'為使用者自訂
            PaperSize pageSize = new PaperSize("vbPRPSUser", pageH, pageW);//256
            pd.DefaultPageSettings.PaperSize = pageSize;
            //印表機事件設定
            pd.PrintPage += new PrintPageEventHandler(this.printCoupon_PrintPage);
            /*//預覽列印
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
        private void button2_Click(object sender, EventArgs e)
        {
            pnum2 += 1;
            wait1[1] += 1;
            //wait1 = (byte)(a + 1);
            if (wait1[1] == 10)
            {
                wait1[1] = 0;
                wait2[1] += 1;
            }
            label2 += 1;
            label_wait2.Text = label2.ToString();

            PrintDocument pd = new PrintDocument();
            //設定印表機邊界
            Margins margin = new Margins(0, 0, 0, 0);
            pd.DefaultPageSettings.Margins = margin;
            ////設定紙張大小 'vbPRPSUser'為使用者自訂
            PaperSize pageSize = new PaperSize("vbPRPSUser", pageH, pageW);//256
            pd.DefaultPageSettings.PaperSize = pageSize;
            //印表機事件設定
            pd.PrintPage += new PrintPageEventHandler(this.printCoupon_PrintPage2);
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
            //列印圖片
            Image temp = Image.FromFile(Application.StartupPath + @"\print_photo\test.bmp");//圖片檔案
            //設定圖片列印的x,y座標
            int x = pbw;   //e.MarginBounds.X; 20
            int y = pbh;  //e.MarginBounds.Y; 0
            //圖片列印的大小
            int width = photoW;//temp.Width; 220
            int height = photoH;//temp.Height; 80
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
            switch (wordstyle1)
            {
                case 0:
                    wdstyle1 = FontStyle.Regular;
                    break;
                case 1:
                    wdstyle1 = FontStyle.Bold;
                    break;
                case 2:
                    wdstyle1 = FontStyle.Italic;
                    break;
                case 3:
                    wdstyle1 = FontStyle.Strikeout;
                    break;
                case 4:
                    wdstyle1 = FontStyle.Underline;
                    break;
            }
            switch (wordstyle2)
            {
                case 0:
                    wdstyle2 = FontStyle.Regular;
                    break;
                case 1:
                    wdstyle2 = FontStyle.Bold;
                    break;
                case 2:
                    wdstyle2 = FontStyle.Italic;
                    break;
                case 3:
                    wdstyle2 = FontStyle.Strikeout;
                    break;
                case 4:
                    wdstyle2 = FontStyle.Underline;
                    break;
            }
            switch (wordstyle3)
            {
                case 0:
                    wdstyle3 = FontStyle.Regular;
                    break;
                case 1:
                    wdstyle3 = FontStyle.Bold;
                    break;
                case 2:
                    wdstyle3 = FontStyle.Italic;
                    break;
                case 3:
                    wdstyle3 = FontStyle.Strikeout;
                    break;
                case 4:
                    wdstyle3 = FontStyle.Underline;
                    break;
            }
            Graphics MyGraphics = e.Graphics;
            SolidBrush MyBrush = new SolidBrush(Color.Black);
            Font num = new Font(numfont, nums, wdstyle1);//40
            Font MyFont = new Font(str1font, str1s, wdstyle2);//設定字型與大小; 20
            Font MyFont1 = new Font(str2font, str2s, wdstyle3);//10            
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
            pnumber1 = string.Format("{0:0000}", pnum1);
            MyGraphics.DrawString(pnumber1, num, MyBrush, numbw, numbh, new StringFormat()); //70,85
            MyGraphics.DrawString("日期:" + DateTime.Now.ToString("yyyy/MM/dd"), MyFont, MyBrush, str1bw, str1bh, new StringFormat());//30, 145
            MyGraphics.DrawString("時間:" + DateTime.Now.ToString("HH:mm:ss"), MyFont, MyBrush, str1bw, str1bh + 30, new StringFormat());//30, 175
            MyGraphics.DrawString(text, MyFont1, MyBrush, str2bw, str2bh, new StringFormat());//80, 210
        }
        private void printCoupon_PrintPage2(object sender, PrintPageEventArgs e)
        {
            //列印圖片
            Image temp = Image.FromFile(Application.StartupPath + @"\print_photo\test.bmp");//圖片檔案
            //設定圖片列印的x,y座標
            int x = pbw;   //e.MarginBounds.X; 20
            int y = pbh;  //e.MarginBounds.Y; 0
            //圖片列印的大小
            int width = photoW;//temp.Width; 220
            int height = photoH;//temp.Height; 80
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
            switch (wordstyle1)
            {
                case 0:
                    wdstyle1 = FontStyle.Regular;
                    break;
                case 1:
                    wdstyle1 = FontStyle.Bold;
                    break;
                case 2:
                    wdstyle1 = FontStyle.Italic;
                    break;
                case 3:
                    wdstyle1 = FontStyle.Strikeout;
                    break;
                case 4:
                    wdstyle1 = FontStyle.Underline;
                    break;
            }
            switch (wordstyle2)
            {
                case 0:
                    wdstyle2 = FontStyle.Regular;
                    break;
                case 1:
                    wdstyle2 = FontStyle.Bold;
                    break;
                case 2:
                    wdstyle2 = FontStyle.Italic;
                    break;
                case 3:
                    wdstyle2 = FontStyle.Strikeout;
                    break;
                case 4:
                    wdstyle2 = FontStyle.Underline;
                    break;
            }
            switch (wordstyle3)
            {
                case 0:
                    wdstyle3 = FontStyle.Regular;
                    break;
                case 1:
                    wdstyle3 = FontStyle.Bold;
                    break;
                case 2:
                    wdstyle3 = FontStyle.Italic;
                    break;
                case 3:
                    wdstyle3 = FontStyle.Strikeout;
                    break;
                case 4:
                    wdstyle3 = FontStyle.Underline;
                    break;
            }
            Graphics MyGraphics = e.Graphics;
            SolidBrush MyBrush = new SolidBrush(Color.Black);
            Font num = new Font(numfont, nums, wdstyle1);//40
            Font MyFont = new Font(str1font, str1s, wdstyle2);//設定字型與大小; 20
            Font MyFont1 = new Font(str2font, str2s, wdstyle3);//10            
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
            pnumber2 = string.Format("{0:0000}", pnum2);
            MyGraphics.DrawString(pnumber2, num, MyBrush, numbw, numbh, new StringFormat()); //70,85
            MyGraphics.DrawString("日期:" + DateTime.Now.ToString("yyyy/MM/dd"), MyFont, MyBrush, str1bw, str1bh, new StringFormat());//30, 145
            MyGraphics.DrawString("時間:" + DateTime.Now.ToString("HH:mm:ss"), MyFont, MyBrush, str1bw, str1bh + 30, new StringFormat());//30, 175
            MyGraphics.DrawString(text, MyFont1, MyBrush, str2bw, str2bh, new StringFormat());//80, 210
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
                if (comport.IsOpen)
                {
                    if (comport.BytesToRead > 0)
                    {
                        Int32 length = comport.Read(buffer, 0, buffer.Length);
                        Array.Resize(ref buffer, length);
                        Display d = new Display(DisplayText);
                        if (this.IsHandleCreated)
                            this.Invoke(d, new Object[] { buffer });

                        SoundPlayer player = new SoundPlayer();
                        SoundPlayer player1 = new SoundPlayer();
                        SoundPlayer player2 = new SoundPlayer();
                        //player.PlayLooping(); //迴圈播放模式
                        //player.PlaySync(); //UI執行緒同步播放

                        if (buffer.Length == 9)
                        {
                            try
                            {
                                //無資料時，送總等待人數
                                if (buffer[0] == 165 && buffer[1] == 182 && buffer[2] == 0 && buffer[3] == 0 && buffer[4] == 0 && buffer[5] == 0 && buffer[6] == 126 && buffer[7] == 0 && buffer[8] == 0)
                                {
                                    try
                                    {
                                        //MessageBox.Show("12213");
                                        //byte waits1 = wait1;
                                        //waits1 -= 1;
                                        int total1 = wait2[0] + wait2[1] + wait2[2] + wait2[3];
                                        int total2 = wait1[0] + wait1[1] + wait1[2] + wait1[3];
                                        byte[] array1 = { 0xED, 0xED, Convert.ToByte(total1), Convert.ToByte(total2), 0x00, 0x00, 0x00, 0x00, 0x7E, 0x00, 0x00 };
                                        comport.Write(array1, 0, 11);
                                    }
                                    catch (TimeoutException timeoutEx)
                                    {
                                        MessageBox.Show("送出失敗");
                                    }
                                }
                                //無資料時，送個別等待人數
                                else if (buffer[0] == 165 && buffer[1] == 182 && buffer[2] == 0 && buffer[3] == 0 && buffer[4] == 0 && buffer[5] == 0 && (buffer[6] == 0 || buffer[6] == 1 || buffer[6] == 2 || buffer[6] == 3 || buffer[6] == 4 || buffer[6] == 5 || buffer[6] == 6 || buffer[6] == 7 || buffer[6] == 8 || buffer[6] == 9 || buffer[6] == 10 || buffer[6] == 11 || buffer[6] == 12 || buffer[6] == 13 || buffer[6] == 14 || buffer[6] == 15) && buffer[7] == 03 && buffer[8] == 0)
                                {
                                    for (int i = 0; i <= cont.GetUpperBound(0); i++)
                                    {
                                        for (int j = 0; j <= cont.GetUpperBound(1); j++)/*第二維是2(有幾「戶」)，因arr1[1,0],arr1[1,1]，故第二維上限是1*/
                                        {
                                            if (cont[i, j].ToString() == buffer[6].ToString())
                                            {
                                                w2 = wait2[i];
                                                w1 = wait1[i];
                                            }
                                        }
                                    }
                                    byte[] array1 = { 0xED, 0xED, w2, w1, 0x00, 0x00, 0x00, 0x00, buffer[6], 0x00, 0x00 };
                                    comport.Write(array1, 0, 11);
                                }
                                //CALL 叫號
                                else if (buffer[0] == 165 && buffer[1] == 182 && (buffer[6] == 0x00 || buffer[6] == 0x01 || buffer[6] == 0x02 || buffer[6] == 0x03 || buffer[6] == 0x04 || buffer[6] == 0x05 || buffer[6] == 0x06 || buffer[6] == 0x07 || buffer[6] == 0x08 || buffer[6] == 0x09 || buffer[6] == 0x10 || buffer[6] == 0x11 || buffer[6] == 0x12 || buffer[6] == 0x13 || buffer[6] == 0x14 || buffer[6] == 0x15) && buffer[7] == 01)
                                {
                                    int i, icount = 0;
                                    for (i = 0; i <= cont.GetUpperBound(0); i++)//找出控制器 ID 屬於哪個群組， i 為第幾群 有 0,1,2,3 四個群
                                    {
                                        for (int j = 0; j <= cont.GetUpperBound(1); j++)
                                        {
                                            if (cont[i, j].ToString() == buffer[6].ToString())
                                            {
                                                w2 = wait2[i];
                                                w1 = wait1[i];
                                                n1 = num[i, 0];
                                                n2 = num[i, 1];
                                                n3 = num[i, 2];
                                                n4 = num[i, 3];
                                                icount = i;
                                            }
                                        }
                                    }
                                    try
                                    {
                                        if (w2 == 0 && w1 != 0)
                                        {
                                            w1 -= 1;
                                            n4 += 1;
                                            if (n4 == 10)
                                            {
                                                n4 = 0;
                                                n3 = (byte)(n3 + 1);
                                            }
                                            else if (n3 == 10)
                                            {
                                                n3 = 0;
                                                n2 = (byte)(n2 + 1);
                                            }
                                            else if (n2 == 10)
                                            {
                                                n2 = 0;
                                                n2 = (byte)(n2 + 1);
                                            }
                                            wait2[icount] = w2; wait1[icount] = w1; num[icount, 0] = n1; num[icount, 1] = n2; num[icount, 2] = n3; num[icount, 3] = n4;
                                            byte[] array1 = { 0xED, 0xED, w2, w1, n1, n2, n3, n4, buffer[6], 0x01, 0x00 };
                                            comport.Write(array1, 0, 11);
                                            //label 看哪個要 -1  
                                            switch (icount)
                                            {
                                                case 0:
                                                    label -= 1;
                                                    break;
                                                case 1:
                                                    label2 -= 1;
                                                    break;
                                                case 2:
                                                    label -= 1;
                                                    break;
                                                case 3:
                                                    label -= 1;
                                                    break;
                                                default:
                                                    Console.WriteLine("叫號沒扣1");
                                                    break;
                                            }
                                        }
                                        else if (w2 != 0 && w1 == 0)
                                        {
                                            w2 -= 1;
                                            w1 = 9;
                                            n4 += 1;
                                            if (n4 == 10)
                                            {
                                                n4 = 0;
                                                n3 = (byte)(n3 + 1);
                                            }
                                            else if (n3 == 10)
                                            {
                                                n3 = 0;
                                                n2 = (byte)(n2 + 1);
                                            }
                                            else if (n2 == 10)
                                            {
                                                n2 = 0;
                                                n2 = (byte)(n2 + 1);
                                            }
                                            wait2[icount] = w2; wait1[icount] = w1; num[icount, 0] = n1; num[icount, 1] = n2; num[icount, 2] = n3; num[icount, 3] = n4;
                                            byte[] array1 = { 0xED, 0xED, w2, w1, n1, n2, n3, n4, buffer[6], 0x01, 0x00 };
                                            comport.Write(array1, 0, 11);
                                            switch (icount)
                                            {
                                                case 0:
                                                    label -= 1;
                                                    break;
                                                case 1:
                                                    label2 -= 1;
                                                    break;
                                                case 2:
                                                    label -= 1;
                                                    break;
                                                case 3:
                                                    label -= 1;
                                                    break;
                                                default:
                                                    Console.WriteLine("叫號沒扣1");
                                                    break;
                                            }
                                        }
                                        else if (w2 != 0 && w1 != 0)
                                        {
                                            //byte waits1 = wait1;
                                            w1 -= 1;
                                            n4 += 1;
                                            if (n4 == 10)
                                            {
                                                n4 = 0;
                                                n3 = (byte)(n3 + 1);
                                            }
                                            else if (n3 == 10)
                                            {
                                                n3 = 0;
                                                n2 = (byte)(n2 + 1);
                                            }
                                            else if (n2 == 10)
                                            {
                                                n2 = 0;
                                                n2 = (byte)(n2 + 1);
                                            }
                                            wait2[icount] = w2; wait1[icount] = w1; num[icount, 0] = n1; num[icount, 1] = n2; num[icount, 2] = n3; num[icount, 3] = n4;
                                            byte[] array1 = { 0xED, 0xED, w2, w1, n1, n2, n3, n4, buffer[6], 0x01, 0x00 };
                                            comport.Write(array1, 0, 11);
                                            switch (icount)
                                            {
                                                case 0:
                                                    label -= 1;
                                                    break;
                                                case 1:
                                                    label2 -= 1;
                                                    break;
                                                case 2:
                                                    label -= 1;
                                                    break;
                                                case 3:
                                                    label -= 1;
                                                    break;
                                                default:
                                                    Console.WriteLine("叫號沒扣1");
                                                    break;
                                            }
                                        }
                                        else if (w2 == 0 && w1 == 0)
                                        {
                                            MessageBox.Show("no wait");
                                            //byte waits1 = wait1;
                                            //wainum[0, 3]-= 1;
                                            byte[] array1 = { 0xED, 0xED, w2, w1, n1, n2, n3, n4, buffer[6], 0x00, 0x00 };
                                            comport.Write(array1, 0, 11);
                                        }
                                        else
                                        {
                                            MessageBox.Show("等待有誤");
                                        }
                                        /*player.SoundLocation = @"C:\Users\bock\github\program\voice\來賓.wav";
                                        player.PlaySync();*/
                                        if (n1 == 0 && n2 == 0 && n3 == 0)
                                        {
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n4 + ".wav";
                                            player.PlaySync();

                                        }
                                        else if (n1 == 0 && n2 == 0 && n3 == 1 && n4 == 0)
                                        {
                                            player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                            player.PlaySync();
                                        }
                                        else if (n1 == 0 && n2 == 0 && n3 == 1 && (n4 == 1 || n4 == 2 || n4 == 3 || n4 == 4 || n4 == 5 || n4 == 6 || n4 == 7 || n4 == 8 || n4 == 9))
                                        {
                                            player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n4 + ".wav";
                                            player.PlaySync();
                                        }
                                        else if (n1 == 0 && n2 == 0 && (n3 == 2 || n3 == 3 || n3 == 4 || n3 == 5 || n3 == 6 || n3 == 7 || n3 == 8 || n3 == 9) && n4 == 0)
                                        {
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n3 + ".wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                            player.PlaySync();
                                        }
                                        else if (n1 == 0 && n2 == 0)
                                        {
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n3 + ".wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n4 + ".wav";
                                            player.PlaySync();
                                        }
                                        else if (n1 == 0 && (n2 == 1 || n2 == 2 || n2 == 3 || n2 == 4 || n2 == 5 || n2 == 6 || n2 == 7 || n2 == 8 || n2 == 9) && n3 == 0 && n4 == 0)
                                        {
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n2 + ".wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                            player.PlaySync();
                                        }
                                        else if (n1 == 0 && (n2 == 1 || n2 == 2 || n2 == 3 || n2 == 4 || n2 == 5 || n2 == 6 || n2 == 7 || n2 == 8 || n2 == 9) && (n3 == 1 || n3 == 2 || n3 == 3 || n3 == 4 || n3 == 5 || n3 == 6 || n3 == 7 || n3 == 8 || n3 == 9) && n4 == 0)
                                        {
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n2 + ".wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n3 + ".wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                            player.PlaySync();
                                        }
                                        else if (n1 == 0 && (n2 == 1 || n2 == 2 || n2 == 3 || n2 == 4 || n2 == 5 || n2 == 6 || n2 == 7 || n2 == 8 || n2 == 9) && n3 == 0 && (n4 == 1 || n4 == 2 || n4 == 3 || n4 == 4 || n4 == 5 || n4 == 6 || n4 == 7 || n4 == 8 || n4 == 9))
                                        {
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n2 + ".wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n3 + ".wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n4 + ".wav";
                                            player.PlaySync();
                                        }
                                        else if (n1 == 0)
                                        {
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n2 + ".wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n3 + ".wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n4 + ".wav";
                                            player.PlaySync();
                                        }
                                        else if ((n1 == 1 || n1 == 2 || n1 == 3 || n1 == 4 || n1 == 5 || n1 == 6 || n1 == 7 || n1 == 8 || n1 == 9) && n2 == 0 && n3 == 0 && n4 == 0)
                                        {
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n1 + ".wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                            player.PlaySync();
                                        }
                                        else if ((n1 == 1 || n1 == 2 || n1 == 3 || n1 == 4 || n1 == 5 || n1 == 6 || n1 == 7 || n1 == 8 || n1 == 9) && (n2 == 1 || n2 == 2 || n2 == 3 || n2 == 4 || n2 == 5 || n2 == 6 || n2 == 7 || n2 == 8 || n2 == 9) && n3 == 0 && n4 == 0)
                                        {
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n1 + ".wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n2 + ".wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                            player.PlaySync();
                                        }
                                        else if ((n1 == 1 || n1 == 2 || n1 == 3 || n1 == 4 || n1 == 5 || n1 == 6 || n1 == 7 || n1 == 8 || n1 == 9) && (n2 == 1 || n2 == 2 || n2 == 3 || n2 == 4 || n2 == 5 || n2 == 6 || n2 == 7 || n2 == 8 || n2 == 9) && (n3 == 1 || n3 == 2 || n3 == 3 || n3 == 4 || n3 == 5 || n3 == 6 || n3 == 7 || n3 == 8 || n3 == 9) && n4 == 0)
                                        {
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n1 + ".wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n2 + ".wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n3 + ".wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                            player.PlaySync();
                                        }
                                        else if ((n1 == 1 || n1 == 2 || n1 == 3 || n1 == 4 || n1 == 5 || n1 == 6 || n1 == 7 || n1 == 8 || n1 == 9) && (n2 == 0 || n2 == 1 || n2 == 2 || n2 == 3 || n2 == 4 || n2 == 5 || n2 == 6 || n2 == 7 || n2 == 8 || n2 == 9) && (n3 == 0 || n3 == 1 || n3 == 2 || n3 == 3 || n3 == 4 || n3 == 5 || n3 == 6 || n3 == 7 || n3 == 8 || n3 == 9) && n4 == 0)
                                        {
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n1 + ".wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n2 + ".wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n3 + ".wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                            player.PlaySync();
                                        }
                                        else if ((n1 == 1 || n1 == 2 || n1 == 3 || n1 == 4 || n1 == 5 || n1 == 6 || n1 == 7 || n1 == 8 || n1 == 9) && (n2 == 0 || n2 == 1 || n2 == 2 || n2 == 3 || n2 == 4 || n2 == 5 || n2 == 6 || n2 == 7 || n2 == 8 || n2 == 9) && (n3 == 0 || n3 == 1 || n3 == 2 || n3 == 3 || n3 == 4 || n3 == 5 || n3 == 6 || n3 == 7 || n3 == 8 || n3 == 9) && (n4 == 1 || n4 == 2 || n4 == 3 || n4 == 4 || n4 == 5 || n4 == 6 || n4 == 7 || n4 == 8 || n4 == 9))
                                        {
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n1 + ".wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n2 + ".wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n3 + ".wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                                            player.PlaySync();
                                            player.SoundLocation = Application.StartupPath + @"\voice\" + n4 + ".wav";
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
                                        FileStream fs = new FileStream(Application.StartupPath + @"\Log\callnum_log\" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + ".txt", FileMode.Append);//若沒有此檔名，建立一個記事本
                                        fs.Close();
                                        StreamWriter str = new StreamWriter(Application.StartupPath + @"\Log\callnum_log\" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + ".txt", true);
                                        str.WriteLine(DateTime.Now.ToString() + " => 控制器:" + buffer[6] + " => 叫號:" + n1 + n2 + n3 + n4 );
                                        str.Close();                                        
                                    }
                                    catch (TimeoutException timeoutEx)
                                    {
                                        MessageBox.Show("送出失敗");
                                    }
                                }
                                //指定叫號
                                else if (buffer[0] == 165 && buffer[1] == 182 && (buffer[6] == 0x00 || buffer[6] == 0x01 || buffer[6] == 0x02 || buffer[6] == 0x03 || buffer[6] == 0x04 || buffer[6] == 0x05 || buffer[6] == 0x06 || buffer[6] == 0x07 || buffer[6] == 0x08 || buffer[6] == 0x09 || buffer[6] == 0x10 || buffer[6] == 0x11 || buffer[6] == 0x12 || buffer[6] == 0x13 || buffer[6] == 0x14 || buffer[6] == 0x15) && buffer[7] == 02)
                                {
                                    for (int i = 0; i <= cont.GetUpperBound(0); i++)//找出控制器 ID 屬於哪個群組， i 為第幾群 有 0,1,2,3 四個群
                                    {
                                        for (int j = 0; j <= cont.GetUpperBound(1); j++)
                                        {
                                            if (cont[i, j].ToString() == buffer[6].ToString())
                                            {
                                                w2 = wait2[i];
                                                w1 = wait1[i];
                                                n1 = num[i, 0];
                                                n2 = num[i, 1];
                                                n3 = num[i, 2];
                                                n4 = num[i, 3];
                                            }
                                        }
                                    }
                                    num1 = buffer[2];
                                    num2 = buffer[3];
                                    num3 = buffer[4];
                                    num4 = buffer[5];
                                    byte[] array1 = { 0xED, 0xED, w2, w1, num1, num2, num3, num4, buffer[6], 0x01, 0x00 };
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
                                    //指定叫號，存記事本
                                    FileStream fs = new FileStream(Application.StartupPath + @"\Log\pickcall_log\" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + ".txt", FileMode.Append);
                                    fs.Close();
                                    StreamWriter str = new StreamWriter(Application.StartupPath + @"\Log\pickcall_log\" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + ".txt", true);
                                    //第二個參數設定為true表示不覆蓋原本的內容，把新內容直接添加進去
                                    str.WriteLine(DateTime.Now.ToString() + " => 控制器:" + buffer[6] + " => 目前號碼:" + n1 + n2 + n3 + n4 + " => 指定叫號:" + num1 + num2 + num3 + num4);
                                    str.Close();
                                }
                                else
                                {
                                    byte[] array1 = { 0xED, 0xED, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                                    comport.Write(array1, 0, 11);
                                    Console.WriteLine("陣列外");
                                }
                            }
                            catch (TimeoutException timeoutEx)
                            {
                                MessageBox.Show("陣列過長");
                            }
                        }
                        else
                        {
                            Console.WriteLine(buffer.Length);
                            byte[] array1 = { 0xED, 0xED, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                            comport.Write(array1, 0, 11);
                        }
                    }
                    Thread.Sleep(100);
                }
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
            }
            Thread.Sleep(16);
            if ((sender as SerialPort).BytesToRead > 0)
            {
                try
                {/*
                    Byte[] buffer = new Byte[1024];
                    Int32 length = (sender as SerialPort).Read(buffer, 0, buffer.Length);
                    Array.Resize(ref buffer, length);
                    Display d = new Display(DisplayText);
                    this.Invoke(d, new Object[] { buffer });
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
            label_wait2.Text = label2.ToString();
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
                Console.WriteLine("EXIT");
                comport.Close();
                this.Close();
            }
        }
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Console.WriteLine("關");
            //關閉後，存記事本
            StreamWriter str = new StreamWriter(Application.StartupPath + @"\Log\exitnum_log.txt");
            //第二個參數設定為true表示不覆蓋原本的內容，把新內容直接添加進去
            str.WriteLine("業務一等待人數:" + wait2[0] + wait1[0] + " => 叫號:" + num[0, 0] + num[0, 1] + num[0, 2] + num[0, 3] + " => " + DateTime.Now.ToString());
            str.WriteLine("業務二等待人數:" + wait2[1] + wait1[1] + " => 叫號:" + num[1, 0] + num[1, 1] + num[1, 2] + num[1, 3] + " => " + DateTime.Now.ToString());
            str.WriteLine("業務三等待人數:" + wait2[2] + wait1[2] + " => 叫號:" + num[2, 0] + num[2, 1] + num[2, 2] + num[2, 3] + " => " + DateTime.Now.ToString());
            str.WriteLine("業務四等待人數:" + wait2[3] + wait1[3] + " => 叫號:" + num[3, 0] + num[3, 1] + num[3, 2] + num[3, 3] + " => " + DateTime.Now.ToString());
            str.WriteLine(DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString());
            str.Close();
            comport.Close();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            TimeSpan t = (TimeSpan)(DateTime.Now - doubleClickTimer); //DoubleClick後又點了一下, 計算時間差 

            if (t.TotalMilliseconds <= 200) //如果小於200豪秒就執行 
            {
                formlocation f = new formlocation();//產生Form2的物件，才可以使用它所提供的Method
                f.ShowDialog(this); //開啟主視窗
            }
        }
    }
}
