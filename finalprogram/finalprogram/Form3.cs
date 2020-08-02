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
    public partial class Form3 : Form
    {
        private float X;//當前窗體的寬度
        private float Y;//當前窗體的高度
        bool isLoaded;
        public Form3()
        {
            InitializeComponent();
            StreamReader str = new StreamReader(Application.StartupPath + @"\config.txt");
            string[] s = new string[1000];
            int ctr = 0;
            do
            {
                ctr++;
                s[ctr] = str.ReadLine();
            } while (s[ctr] != null);
            textBox1.Text = s[2].Substring(6);
            textBox2.Text = s[3].Substring(6);
            textBox3.Text = s[4].Substring(6);
            textBox4.Text = s[5].Substring(6);
            textBox5.Text = s[6].Substring(6);
            textBox6.Text = s[7].Substring(6);
            textBox7.Text = s[8].Substring(6);
            textBox8.Text = s[9].Substring(6);
            textBox9.Text = s[10].Substring(6);
            textBox10.Text = s[11].Substring(6);
            textBox11.Text = s[12].Substring(6);
            textBox12.Text = s[13].Substring(6);
            textBox13.Text = s[14].Substring(6);
            textBox14.Text = s[15].Substring(6);
            textBox15.Text = s[16].Substring(6);
            str.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
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
        private void button1_Click(object sender, EventArgs e)
        {
            int[] array = { Int32.Parse(textBox1.Text.ToString()), Int32.Parse(textBox2.Text.ToString()) };
            Form1 lForm1 = (Form1)this.Owner;//把Form2的父窗口指針賦給lForm1
            lForm1.IntValue = array;//使用父窗口指針賦值  
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
                    case 2:
                        str1.WriteLine("控制器01=" + textBox1.Text);
                        break;
                    case 3:
                        str1.WriteLine("控制器02=" + textBox2.Text);
                        break;
                    case 4:
                        str1.WriteLine("控制器03=" + textBox3.Text);
                        break;
                    case 5:
                        str1.WriteLine("控制器04=" + textBox4.Text);
                        break;
                    case 6:
                        str1.WriteLine("控制器05=" + textBox5.Text);
                        break;
                    case 7:
                        str1.WriteLine("控制器06=" + textBox6.Text);
                        break;
                    case 8:
                        str1.WriteLine("控制器07=" + textBox7.Text);
                        break;
                    case 9:
                        str1.WriteLine("控制器08=" + textBox8.Text);
                        break;
                    case 10:
                        str1.WriteLine("控制器09=" + textBox9.Text);
                        break;
                    case 11:
                        str1.WriteLine("控制器10=" + textBox10.Text);
                        break;
                    case 12:
                        str1.WriteLine("控制器11=" + textBox11.Text);
                        break;
                    case 13:
                        str1.WriteLine("控制器12=" + textBox12.Text);
                        break;
                    case 14:
                        str1.WriteLine("控制器13=" + textBox13.Text);
                        break;
                    case 15:
                        str1.WriteLine("控制器14=" + textBox14.Text);
                        break;
                    case 16:
                        str1.WriteLine("控制器15=" + textBox15.Text);
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

        private void button3_Click(object sender, EventArgs e)
        {
            comportshow f = new comportshow();//產生Form2的物件，才可以使用它所提供的Method
            f.ShowDialog(this);
        }
    }
}
