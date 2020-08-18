using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace clock
{
    public partial class Form1 : Form
    {
        private float X;//當前窗體的寬度
        private float Y;//當前窗體的高度
        bool isLoaded;  // 是否已設定各控制的尺寸資料到Tag屬性
        private SerialPort comport;
        int a, ctr = 0, ctr1 = 0, wordstyle1,size, w1bd, w1bh, w2bd, w2bh, w3bd, w3bh;
        string[] line = new string[1000];
        string[] line1 = new string[1000];
        string port, font, color;
        byte weeks;
        FontStyle wdstyle1;
        DateTime doubleClickTimer = DateTime.Now;
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;

            StreamReader str = new StreamReader(Application.StartupPath + @"\config.txt");//讀取文字檔
            do
            {
                ctr++;
                line[ctr] = str.ReadLine();
            } while (line[ctr] != null);
            isLoaded = false;
            port = line[2];
            font = line[5].Substring(5);
            size = Convert.ToInt32(line[6].Substring(5));
            wordstyle1 = Convert.ToInt32(line[7].Substring(6));
            color = line[8].Substring(6);
            w1bd = Convert.ToInt32(line[11].Substring(12));
            w1bh = Convert.ToInt32(line[12].Substring(12));
            w2bd = Convert.ToInt32(line[15].Substring(12));
            w2bh = Convert.ToInt32(line[16].Substring(12));
            w3bd = Convert.ToInt32(line[19].Substring(12));
            w3bh = Convert.ToInt32(line[20].Substring(12));

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

            label1.Font = new Font(font, size, wdstyle1);//Times New Roman、Arial、標楷體, size
            label1.ForeColor = ColorTranslator.FromHtml(color);
            label1.Location = new Point(w1bd, w1bh);//位址
            label2.Font = new Font(font, size, wdstyle1);//Times New Roman、Arial、標楷體, size
            label2.ForeColor = ColorTranslator.FromHtml(color);
            label2.Location = new Point(w2bd, w2bh);
            label3.Font = new Font(font, size, wdstyle1);//Times New Roman、Arial、標楷體, size
            label3.ForeColor = ColorTranslator.FromHtml(color);
            label3.Location = new Point(w3bd, w3bd);
            str.Close();
            BackgroundImage = new Bitmap(Application.StartupPath + @"\background\back.jpg");
            
            comport = new SerialPort(port, 9600, Parity.None, 8, StopBits.One);
            String[] COMPorts = SerialPort.GetPortNames();
            try
            {
                comport.Open();
            }
            catch
            {               
                MessageBox.Show("COMPORT 不存在! 請點選畫面左上角三下做設定!");
            }
            
            a = DateTime.Now.Minute + 1;
            Console.WriteLine(a);
            timer1.Interval = 100;
            timer1.Enabled = true;
            timer1.Start();
            Console.WriteLine();
        }
        private void FrmF3D_Load(object sender, EventArgs e)
        {
            X = this.Width;//獲取窗體的寬度
            Y = this.Height;//獲取窗體的高度
            isLoaded = true;// 已設定各控制項的尺寸到Tag屬性中
            SetTag(this);//調用方法

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.Year.ToString() + " 年 " + DateTime.Now.Month.ToString() + "月 " + DateTime.Now.Day.ToString() + "日";
            label2.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);
            string week = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);
            label3.Text = DateTime.Now.ToString("HH:mm:ss");
            if (a == 60)
            {
                a = 0;
            }
            //label3.Text = DateTime.Now.Hour.ToString() +":"+ DateTime.Now.Minute.ToString(MM) +":" + DateTime.Now.Second.ToString();
            if (DateTime.Now.Minute.ToString() == a.ToString() && DateTime.Now.Second.ToString() == "0")
            {
                byte month = Convert.ToByte(Convert.ToInt32(DateTime.Now.Month.ToString(), 16));
                byte day = Convert.ToByte(Convert.ToInt32(DateTime.Now.Day.ToString(), 16));
                byte hour = Convert.ToByte(Convert.ToInt32(DateTime.Now.Hour.ToString(), 16));
                byte min = Convert.ToByte(Convert.ToInt32(DateTime.Now.Minute.ToString(), 16));
                byte sec = Convert.ToByte(Convert.ToInt32(DateTime.Now.Second.ToString(), 16));
                byte year = Convert.ToByte(Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2),16));

                switch (week)//星期幾
                {
                    case "星期日":
                        weeks = 0x00;
                        break;
                    case "星期一":
                        weeks = 0x01;
                        break;
                    case "星期二":
                        weeks = 0x02;
                        break;
                    case "星期三":
                        weeks = 0x03;
                        break;
                    case "星期四":
                        weeks = 0x04;
                        break;
                    case "星期五":
                        weeks = 0x05;
                        break;
                    case "星期六":
                        weeks = 0x06;
                        break;
                }

                //每一分鐘傳一次
                byte[] array1 = { 0xED, 0xED, 0xED, 0xFF, 0x00, 0x00, 0xFF, 0xFF, 0xFC, month, day, weeks, hour, min, sec, year, 0x00 };
                try
                {
                    comport.Write(array1, 0, 17);
                }
                catch 
                {
                    MessageBox.Show("COMPORT 不存在! 請點選畫面左上角三下做設定!");
                }
                /*foreach (byte byteValue in array1)//看ARRAY 中的數值
                {
                    Console.WriteLine(byteValue);
                }*/
                a =  a + 1;
                if (a == 60) 
                {
                    a = 0;
                }
            }
        }
        private void SetControls(float newx, float newy, Control cons)
        {
            if (isLoaded)
            {
                //遍歷窗體中的控制項，重新設置控制項的值
                foreach (Control con in cons.Controls)
                {
                    string[] mytag = con.Tag.ToString().Split(new char[] { ':' });//獲取控制項的Tag屬性值，並分割後存儲字元串數組
                    float a = System.Convert.ToSingle(mytag[0]) * newx;//根據窗體縮放比例確定控制項的值，寬度
                    con.Width = (int)a;//寬度
                    a = System.Convert.ToSingle(mytag[1]) * newy;//高度
                    con.Height = (int)(a);
                    a = System.Convert.ToSingle(mytag[2]) * newx;//左邊距離
                    con.Left = (int)(a);
                    a = System.Convert.ToSingle(mytag[3]) * newy;//上邊緣距離
                    con.Top = (int)(a);
                    Single currentSize = System.Convert.ToSingle(mytag[4]) * newy;//字體大小
                    con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0)
                    {
                        SetControls(newx, newy, con);
                    }
                }
            }
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            TimeSpan t = (TimeSpan)(DateTime.Now - doubleClickTimer); //DoubleClick後又點了一下, 計算時間差 
            if (t.TotalMilliseconds <= 200) //如果小於200豪秒就執行 
            {
                this.Close();
            }
        }
        /// <summary>
        /// 將控制項的寬，高，左邊距，頂邊距和字體大小暫存到tag屬性中
        /// </summary>
        /// <param name="cons">遞歸控制項中的控制項</param>
        private void SetTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0)
                    SetTag(con);
            }
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / X; //窗體寬度縮放比例
            float newy = (this.Height) / Y;//窗體高度縮放比例
            SetControls(newx, newy, this);//隨窗體改變控制項大小
        }
        private void FrmF3D_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
            comport.Close();
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
                comportshow f = new comportshow();//產生Form2的物件，才可以使用它所提供的Method
                comport.Close();
                f.ShowDialog(this); //開啟主視窗
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    //若使用者在Form2按下了OK，則進入這個判斷式
                    StreamReader str = new StreamReader(Application.StartupPath + @"\config.txt");//讀取文字檔
                    do
                    {
                        ctr1++;
                        line1[ctr1] = str.ReadLine();
                    } while (line1[ctr1] != null);
                    port = line1[2];
                    comport = new SerialPort(port, 9600, Parity.None, 8, StopBits.One);
                    str.Close();
                    try
                    {
                        comport.Open();
                    }
                    catch 
                    {
                        //MessageBox.Show("沒有此COMPORT請重新選擇");
                    }
                    ctr1 = 0;
                    Console.WriteLine(line1[2]);
                }
                else if (f.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                {
                    MessageBox.Show("請點選確認COMPORT紐做離開");
                }
            }
        }
    }
}
