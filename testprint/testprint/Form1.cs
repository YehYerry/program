using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testprint
{
    public partial class Form1 : Form
    {
        string test, test1;
        string[] line = new string[1000];
        string[] line1 = new string[1000];
        int ctr,ctr1 = 0;
        int pageH, pageW, pbw, pbh, photoW, photoH, wordstyle1, wordstyle2, wordstyle3, nums, numbw, numbh, str1s, str1bw, str1bh, str2s, str2bw, str2bh;
        string numfont, str1font, str2font, text;
        FontStyle wdstyle1, wdstyle2, wdstyle3;
        DateTime doubleClickTimer = DateTime.Now;

        public Form1()
        {
            StreamReader str = new StreamReader(Application.StartupPath + @"\config.ifm");//讀取文字檔
            StreamReader str1 = new StreamReader(Application.StartupPath + @"\print_config.txt");//讀取列印文字檔
            do
            {
                ctr++; 
                ctr1++;
                line[ctr] = str.ReadLine();
                line1[ctr1] = str1.ReadLine();
                //Console.WriteLine(line[ctr]);
            } while (line[ctr] != null && line[ctr1] != null);

            test = line[5];
            test1 = line1[2];
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
            InitializeComponent();
            /*String[] pairs = { "Color1=red", "Color2=green", "Color3=blue",
                 "Title=Code Repository" };
            foreach (var pair in pairs)
            {
                int position = pair.IndexOf("=");
                if (position < 0)
                    continue;
                Console.WriteLine("Key: {0}, Value: '{1}'",
                              pair.Substring(0, position),
                               pair.Substring(position + 1));
            }*/
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //宣告一個一表機
            PrintDocument pd = new PrintDocument();
            //設定印表機邊界
            Margins margin = new Margins(0, 0, 0, 0);
            pd.DefaultPageSettings.Margins = margin;
            ////設定紙張大小 'vbPRPSUser'為使用者自訂
            PaperSize pageSize = new PaperSize("vbPRPSUser", pageH, pageW);//256
            pd.DefaultPageSettings.PaperSize = pageSize;
            //印表機事件設定
            pd.PrintPage += new PrintPageEventHandler(this.printCoupon_PrintPage);
            //預覽列印
            PrintPreviewDialog PPD = new PrintPreviewDialog();
            PPD.Document = pd;
            PPD.ShowDialog();
            //預覽列印
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
            Image temp = Image.FromFile(Application.StartupPath + @"\photo\test.bmp");//圖片檔案
            //C:\Users\jerry\github\program\testprint\testprint\bin\Debug\photo
            //GetResultIntoImage(ref temp);//

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
            //列印文字
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
            MyGraphics.DrawString("0017", num, MyBrush, numbw, numbh, new StringFormat()); //70,85
            MyGraphics.DrawString("日期:" + DateTime.Now.ToString("yyyy/MM/dd"), MyFont, MyBrush, str1bw, str1bh, new StringFormat());//30, 145
            MyGraphics.DrawString("時間:" + DateTime.Now.ToString("HH:mm:ss"), MyFont, MyBrush, str1bw, str1bh+30, new StringFormat());//30, 175
            MyGraphics.DrawString( text , MyFont1, MyBrush, str2bw, str2bh, new StringFormat());//80, 210

        }
        private void button2_Click(object sender, EventArgs e)
        {
            string str = System.Windows.Forms.Application.StartupPath;
            string str1 = System.IO.Directory.GetCurrentDirectory();
            string str2 = System.Environment.CurrentDirectory;

            MessageBox.Show(str);
            MessageBox.Show(str1);
            MessageBox.Show(str2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Console.WriteLine(test1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Console.WriteLine(test);
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
