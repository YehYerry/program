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
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //宣告一個一表機
            PrintDocument pd = new PrintDocument();
            //設定印表機邊界
            Margins margin = new Margins(0, 0, 0, 0);
            pd.DefaultPageSettings.Margins = margin;
            ////設定紙張大小 'vbPRPSUser'為使用者自訂
            PaperSize pageSize = new PaperSize("vbPRPSUser", 256, 256);
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
            MyGraphics.DrawString("0017", num, MyBrush, 70, 85, new StringFormat());
            MyGraphics.DrawString("日期:" + DateTime.Now.ToString("yyyy/MM/dd"), MyFont, MyBrush, 30, 145, new StringFormat());
            MyGraphics.DrawString("時間:" + DateTime.Now.ToString("HH:mm:ss"), MyFont, MyBrush, 30, 175, new StringFormat());
            MyGraphics.DrawString("請依叫號 等候辦理", MyFont1, MyBrush, 80, 210, new StringFormat());

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
    }    
}
