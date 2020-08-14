﻿using System;
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
        int a, ctr = 0, wordstyle1,size;
        string[] line = new string[1000];
        string port, font, color;
        FontStyle wdstyle1;
        public Form1()
        {
            InitializeComponent();

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
            label2.Font = new Font(font, size, wdstyle1);//Times New Roman、Arial、標楷體, size
            label2.ForeColor = ColorTranslator.FromHtml(color);
            label3.Font = new Font(font, size, wdstyle1);//Times New Roman、Arial、標楷體, size
            label3.ForeColor = ColorTranslator.FromHtml(color);
            str.Close();

            BackgroundImage = new Bitmap(Application.StartupPath + @"\background\back.jpg");

            comport = new SerialPort(port, 9600, Parity.None, 8, StopBits.One);
            /*comport.Open();
            byte[] array1 = { 0xED, 0xED, 0xED, 0xFF, 0x00, 0x00, 0xFF, 0xFF, 0xFC, 0x08, 0x14, 0x00, 0x00, 0x44, 0x00, 0x00, 0x00 };
            comport.Write(array1, 0, 17);
            */
            a = DateTime.Now.Minute + 1;
            Console.WriteLine(a);
            timer1.Interval = 100;
            timer1.Enabled = true;
            timer1.Start();
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
            label1.Text = DateTime.Now.Year.ToString() + "年" + DateTime.Now.Month.ToString() + "月" + DateTime.Now.Day.ToString() + "日";
            label2.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);
            label3.Text = DateTime.Now.ToString("HH:mm:ss");
            if (a == 60)
            {
                a = 0;
            }
            //label3.Text = DateTime.Now.Hour.ToString() +":"+ DateTime.Now.Minute.ToString(MM) +":" + DateTime.Now.Second.ToString();
            if (DateTime.Now.Minute.ToString() == a.ToString() && DateTime.Now.Second.ToString() == "0")
            {
                byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(DateTime.Now.Minute.ToString());
                byte month = Convert.ToByte(DateTime.Now.Month.ToString());
                byte day = Convert.ToByte(DateTime.Now.Day.ToString());
                byte hour = Convert.ToByte(DateTime.Now.Hour.ToString());
                byte min = Convert.ToByte(DateTime.Now.Minute.ToString());
                byte sec = Convert.ToByte(DateTime.Now.Second.ToString());
                //每一分鐘傳一次
                byte[] array1 = { 0xED, 0xED, 0xED, 0xFF, 0x00, 0x00, 0xFF, 0xFF, 0xFC, month, day, 0x00, hour, min, sec, 0x00, 0x00 };
                comport.Write(array1, 0, 17);
                foreach (byte byteValue in array1)//看ARRAY 中的數值
                {
                    Console.WriteLine(byteValue);
                }
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
    }
}