using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace finalprogram
{
    public partial class Form4 : Form
    {
        private float X;//當前窗體的寬度
        private float Y;//當前窗體的高度
        bool isLoaded;
        public Form4()
        {
            InitializeComponent();
            textBox1.MaxLength = 4;
            textBox2.MaxLength = 4;
            textBox3.MaxLength = 4;
            textBox4.MaxLength = 4;
            StreamReader str1 = new StreamReader(Application.StartupPath + @"\config.txt");
            StreamReader str = new StreamReader(Application.StartupPath + @"\SetupNum\SetNum.txt");
            string[] s = new string[1000];
            string[] s1 = new string[1000];
            int ctr = 0, ctr1 = 0;
            do
            {
                ctr++;
                s[ctr] = str.ReadLine();
            } while (s[ctr] != null);
            do
            {
                ctr1++;
                s1[ctr1] = str1.ReadLine();
            } while (s1[ctr1] != null);
            Console.WriteLine(s1[24]);
            textBox1.Text = s1[24].Substring(9);
            textBox2.Text = s1[32].Substring(9);
            textBox3.Text = s1[40].Substring(9);
            textBox4.Text = s1[48].Substring(9);
            textBox5.Text = s[6];
            textBox6.Text = s[7];
            textBox7.Text = s[8];
            textBox8.Text = s[9];
            str.Close();
            str1.Close();
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.ToString() == "")
            {
                textBox1.Text = "0000";
            }
            if (textBox2.Text.ToString() == "")
            {
                textBox2.Text = "0000";
            }
            if (textBox3.Text.ToString() == "")
            {
                textBox3.Text = "0000";
            }
            if (textBox4.Text.ToString() == "")
            {
                textBox4.Text = "0000";
            }
            if (textBox5.Text.ToString() == "")
            {
                textBox5.Text = "業務一";
            }
            if (textBox6.Text.ToString() == "")
            {
                textBox6.Text = "業務二";
            }
            if (textBox7.Text.ToString() == "")
            {
                textBox7.Text = "業務三";
            }
            if (textBox8.Text.ToString() == "")
            {
                textBox8.Text = "業務四";
            }
            string[] array = { textBox1.Text.ToString(), textBox2.Text.ToString(), textBox3.Text.ToString(), textBox4.Text.ToString(), textBox5.Text.ToString(), textBox6.Text.ToString(), textBox7.Text.ToString(), textBox8.Text.ToString()};
            Form1 lForm1 = (Form1)this.Owner;//把Form2的父窗口指針賦給lForm1
            lForm1.NumValue = array;//使用父窗口指針賦值  
            StreamWriter num = new StreamWriter(Application.StartupPath + @"\SetupNum\SetNum.txt");
            string numValue1 = string.Format("{0:0000}", Convert.ToInt16(textBox1.Text.ToString()));
            string numValue2 = string.Format("{0:0000}", Convert.ToInt16(textBox2.Text.ToString()));
            string numValue3 = string.Format("{0:0000}", Convert.ToInt16(textBox3.Text.ToString()));
            string numValue4 = string.Format("{0:0000}", Convert.ToInt16(textBox4.Text.ToString()));
            string numValue5 = textBox5.Text.ToString();
            string numValue6 = textBox6.Text.ToString();
            string numValue7 = textBox7.Text.ToString();
            string numValue8 = textBox8.Text.ToString();
            num.WriteLine(DateTime.Now.ToString());
            num.WriteLine(numValue1);
            num.WriteLine(numValue2);
            num.WriteLine(numValue3);
            num.WriteLine(numValue4);
            num.WriteLine(numValue5);
            num.WriteLine(numValue6);
            num.WriteLine(numValue7);
            num.WriteLine(numValue8);
            num.Close();
            //更改記事本內容，先讀取在寫入 (,Encoding.Default)UTF-8
            StreamReader str = new StreamReader(Application.StartupPath + @"\config.txt");
            string[] s = new string[1000];
            int ctr = 0;
            do
            {
                ctr++;
                s[ctr] = str.ReadLine();
            } while (s[ctr] != null);
            str.Close();
            StreamWriter str1 = new StreamWriter(Application.StartupPath + @"\config.txt", false);
            for (int ctr1 = 1; ctr1 <= ctr; ctr1++)
            {
                switch (ctr1)
                {
                    case 24:
                        str1.WriteLine("StartNum=" + textBox1.Text);
                        break;
                    case 32:
                        str1.WriteLine("StartNum=" + textBox2.Text);
                        break;
                    case 40:
                        str1.WriteLine("StartNum=" + textBox3.Text);
                        break;
                    case 48:
                        str1.WriteLine("StartNum=" + textBox4.Text);
                        break;
                    default:
                        str1.WriteLine(s[ctr1]);
                        break;
                }
            }
            str1.Close();
            MessageBox.Show("設定完成!");
            this.Close();
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar < 48 | (int)e.KeyChar > 57) & (int)e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }
        private string[] string1;
        public string[] String1
        {
            set
            {
                string1 = value;
            }
        }

        public void SetValue()
        {
            this.textBox1.Text = string1[0];
            this.textBox2.Text = string1[1];
            this.textBox3.Text = string1[2];
            this.textBox4.Text = string1[3];
            this.textBox5.Text = string1[4];
            this.textBox6.Text = string1[5];
            this.textBox7.Text = string1[6];
            this.textBox8.Text = string1[7];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StreamReader str2 = new StreamReader(Application.StartupPath + @"\SetupNum\SetNum.txt");
            string[] s2 = new string[1000];
            int ctr = 0, ctr2 = 0;
            do
            {
                ctr2++;
                s2[ctr2] = str2.ReadLine();
            } while (s2[ctr2] != null);
            str2.Close();
            StreamReader str = new StreamReader(Application.StartupPath + @"\Log\exitnum_log.txt");
            string[] s = new string[1000];
            do
            {
                ctr++;
                s[ctr] = str.ReadLine();
            } while (s[ctr] != null);
            str.Close();
            StreamWriter str1 = new StreamWriter(Application.StartupPath + @"\Log\exitnum_log.txt", false);
            for (int ctr1 = 1; ctr1 <= ctr; ctr1++)
            {
                switch (ctr1)
                {
                    case 1:
                        str1.WriteLine("業務一等待人數:00 => 叫號:" + s2[2] + " => " + DateTime.Now.ToString());
                        break;
                    case 2:
                        str1.WriteLine("業務二等待人數:00 => 叫號:" + s2[3] + " => " + DateTime.Now.ToString());
                        break;
                    case 3:
                        str1.WriteLine("業務三等待人數:00 => 叫號:" + s2[4] + " => " + DateTime.Now.ToString());
                        break;
                    case 4:
                        str1.WriteLine("業務四等待人數:00 => 叫號:" + s2[5] + " => " + DateTime.Now.ToString());
                        break;
                    default:
                        str1.WriteLine(s[ctr1]);
                        break;
                }
            }
            str1.Close();
            MessageBox.Show("歸零成功!");
        }
    }
}
