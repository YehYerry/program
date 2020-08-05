using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace finalprogram
{
    public partial class Form2 : Form
    {
        private float X;//當前窗體的寬度
        private float Y;//當前窗體的高度
        bool isLoaded;
        string shut = "設定關機時間:";
        int checkbox = 0;
        public Form2()
        {
            InitializeComponent();
            label2.Text = shut;
            timer1_Tick(this, null);
            timer1.Interval = 100; // 設定每秒觸發一次
            timer1.Enabled = true; // 啟動 Timer
            //button1.DialogResult = System.Windows.Forms.DialogResult.OK;//設定button1為OK
            //button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;//設定button為Cancel  
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy/MM/dd  HH:mm:ss";
            StreamReader str = new StreamReader(Application.StartupPath + @"\SetupTime\SetTime.txt");
            string[] s = new string[1000];
            int ctr = 0;
            do
            {
                ctr++;
                s[ctr] = str.ReadLine();
                //Console.WriteLine(s[ctr]);
            } while (s[ctr] != null);
            label2.Text = s[1].Substring(23);
            //Console.WriteLine(s[1].Substring(22));
            str.Close();
            if (label2.Text.ToString() == "設定關機時間:")
            {            
                button3.Visible = false;
            }
            else 
            {
                button1.Visible = false;
                dateTimePicker1.Visible = false;
            }
            //判斷是否隔日歸零
            StreamReader ischeck = new StreamReader(Application.StartupPath + @"\is_check.txt");
            string a = ischeck.ReadLine();
            if (Convert.ToInt32(a) == 0)
            {
                checkBox1.Checked = false;
            }
            if (Convert.ToInt32(a) == 1)
            {
                checkBox1.Checked = true;
            }
            ischeck.Close();
        }
        private string string1;
        public string String1
        {
            set
            {
                string1 = value;
            }
        }
        private int check1;
        public int Check1
        {
            set
            {
                check1 = value;
            }
        }

        public void SetValue()
        {
            this.label2.Text = string1;
        }
        public void SetCheck()
        {
            check1 = checkbox;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime start = DateTime.Now;
            DateTime end = dateTimePicker1.Value;
            //C#的日期型態可直接相減並傳回TimeSpan物件
            TimeSpan ts = end - start;

            //時間差換算成秒
            String s1 = Convert.ToInt32(ts.TotalSeconds).ToString();
            System.Diagnostics.Process.Start("shutdown.exe", "-s -t " + s1.ToString());
            label2.Text = "已設定關機時間:" + dateTimePicker1.Value.ToString();            
            shut = "已設定關機時間:" + dateTimePicker1.Value.ToString();
            button1.Visible = false;
            button3.Visible = true;
            dateTimePicker1.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("shutdown.exe", "-a");
            label2.Text = "設定關機時間:";
            button3.Visible = false;
            button1.Visible = true;
            dateTimePicker1.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 lForm1 = (Form1)this.Owner;//把Form2的父窗口指針賦給lForm1
            shut = label2.Text;
            lForm1.StrValue = shut;//使用父窗口指針賦值
            lForm1.Check = checkbox;//使用父窗口指針賦值  
            // 將字串寫入TXT檔
            StreamWriter timestr = new StreamWriter(Application.StartupPath + @"\SetupTime\SetTime.txt");
            string wr = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " => " + label2.Text;
            timestr.WriteLine(wr);
            timestr.Close();
            StreamWriter is_check = new StreamWriter(Application.StartupPath + @"\is_check.txt");
            if (checkBox1.Checked == true)
            {
                is_check.WriteLine("1");
            }
            else 
            {
                is_check.WriteLine("0");
            }
            is_check.Close();
            this.Close();
        }
        private void setControls(float newx, float newy, Control cons)
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
                        setControls(newx, newy, con);
                    }
                }
            }
        }
        private void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0)
                    setTag(con);
            }
        }
        //根據窗體大小調整控制項大小
        private void Form1_Load(object sender, EventArgs e)
        {
            X = this.Width;//獲取窗體的寬度
            Y = this.Height;//獲取窗體的高度
            isLoaded = true;
            setTag(this);//調用方法
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / X; //窗體寬度縮放比例
            float newy = (this.Height) / Y;//窗體高度縮放比例
            setControls(newx, newy, this);//隨窗體改變控制項大小
        }
        private void Frm_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)
            {
                checkbox = 0;
            }
            if (checkBox1.Checked == true)
            {
                checkbox = 1;
            }
        }
    }
}
