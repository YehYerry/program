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
    public partial class Form1 : Form
    {
        private float X;//當前窗體的寬度
        private float Y;//當前窗體的高度
        bool isLoaded;
        private string strValue;
        private int[] intValue;
        private string[] numValue;
        private int check , checkbox=0;
        string shutstr = "設定關機時間:";
        string shutstr1;
        public Form1()
        {
            InitializeComponent();
            //this.WindowState = FormWindowState.Maximized;
            timer1.Interval = 100;
            timer1.Enabled = true;
            StreamReader str = new StreamReader(Application.StartupPath + @"\SetupTime\SetTime.txt");
            string[] s = new string[1000];
            string[] str1 = new string[1000];
            int ctr = 0,ctr1 = 0;
            do
            {
                ctr++;
                s[ctr] = str.ReadLine();
                //Console.WriteLine(s[ctr]);
            } while (s[ctr] != null);
            shutstr = s[1].Substring(23);
            str.Close();
            if (shutstr == "設定關機時間:")
            {
                MessageBox.Show("未設定關機時間");
            }
            else
            {
                shutstr1 = shutstr.Substring(8);
                Console.WriteLine(shutstr1);
                DateTime start = DateTime.Now;
                DateTime end = DateTime.Parse(shutstr1);
                //C#的日期型態可直接相減並傳回TimeSpan物件
                TimeSpan ts = end - start;
                String s1 = Convert.ToInt32(ts.TotalSeconds).ToString();
                System.Diagnostics.Process.Start("shutdown.exe", "-s -t " + s1.ToString());
                Console.WriteLine(s1);
            }
            //判斷是否隔日歸零
            StreamReader ischeck = new StreamReader(Application.StartupPath + @"\is_check.txt");
            string a = ischeck.ReadLine();
            if (Convert.ToInt32(a) == 1)
            {
                StreamReader compare = new StreamReader(Application.StartupPath + @"\Log\exitnum_log.txt");
                do
                {
                    ctr1++;
                    str1[ctr1] = compare.ReadLine();
                    //Console.WriteLine(s[ctr]);
                } while (str1[ctr1] != null);
                string comparedate = str1[5];
                string now = DateTime.Now.Year.ToString()+ "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString();
                compare.Close();
                if (comparedate == now)
                {
                    Console.WriteLine("同天");
                }
                else 
                {
                    StreamReader str2 = new StreamReader(Application.StartupPath + @"\SetupNum\SetNum.txt");
                    string[] s2 = new string[1000];
                    int ctr3 = 0, ctr2 = 0;
                    do
                    {
                        ctr2++;
                        s2[ctr2] = str2.ReadLine();
                    } while (s2[ctr2] != null);
                    str2.Close();
                    StreamReader str3 = new StreamReader(Application.StartupPath + @"\Log\exitnum_log.txt");
                    string[] s3 = new string[1000];
                    do
                    {
                        ctr3++;
                        s3[ctr3] = str3.ReadLine();
                    } while (s3[ctr3] != null);
                    str3.Close();
                    Console.WriteLine(ctr3);
                    StreamWriter str4 = new StreamWriter(Application.StartupPath + @"\Log\exitnum_log.txt", false);
                    for (int ctr4 = 1; ctr4 <= ctr3; ctr4++)
                    {
                        switch (ctr4)
                        {
                            case 1:
                                str4.WriteLine("業務一等待人數:00 => 叫號:" + s2[2] + " => " + DateTime.Now.ToString());
                                break;
                            case 2:
                                str4.WriteLine("業務二等待人數:00 => 叫號:" + s2[3] + " => " + DateTime.Now.ToString());
                                break;
                            case 3:
                                str4.WriteLine("業務三等待人數:00 => 叫號:" + s2[4] + " => " + DateTime.Now.ToString());
                                break;
                            case 4:
                                str4.WriteLine("業務四等待人數:00 => 叫號:" + s2[5] + " => " + DateTime.Now.ToString());
                                break;
                            default:
                                str4.WriteLine(s3[ctr4]);
                                break;
                        }
                    }
                    str4.Close();
                    //將暫存的號碼歸零
                    StreamWriter str5 = new StreamWriter(Application.StartupPath + @"\Log\num_log\num_log.txt");
                    str5.WriteLine(s2[2]);
                    str5.WriteLine(s2[3]);
                    str5.WriteLine(s2[4]);
                    str5.WriteLine(s2[5]);
                    str5.WriteLine(DateTime.Now.ToString());
                    str5.Close();
                }
                compare.Close();
            }
            ischeck.Close();
        }
        public int[] IntValue
        {
            set
            {
                intValue = value;
            }
        }
        public string[] NumValue
        {
            set
            {
                numValue = value;
            }
        }
        public string StrValue
        {
            set
            {              
                strValue = value;
            }
        }
        public int Check
        {
            set
            {
                check = value;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();//產生Form2的物件，才可以使用它所提供的Method
            f.String1 = shutstr;//設置Form2中string1的值 
            f.Check1 = check;
            f.SetValue();//設置Form2中Label1的 
            //f.SetCheck();//設置Form2中Checkbox 
            f.ShowDialog(this);//設定Form2為Form1的上層，並開啟Form2視窗。由於在Form1的程式碼內使用this，所以this為Form1的物件本身
            //MessageBox.Show(strValue);//顯示返回的值   
            f.SetCheck();//設置Form2中Checkbox 
            shutstr = strValue;
            check = checkbox;
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
        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToString();
        }
        /// <summary>
        /// 將控制項的寬，高，左邊距，頂邊距和字體大小暫存到tag屬性中
        /// </summary>
        /// <param name="cons">遞歸控制項中的控制項</param>
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

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3();//產生Form2的物件，才可以使用它所提供的Method
            f.ShowDialog(this);
            //MessageBox.Show(intValue[0].ToString());回傳的值
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form4 f = new Form4();//產生Form2的物件，才可以使用它所提供的Method
            f.ShowDialog(this);            
            /*StreamWriter num = new StreamWriter(Application.StartupPath + @"\SetupNum\SetNum.txt");
            string numValue1 = string.Format("{0:0000}", Convert.ToInt16(numValue[0]));
            string numValue2 = string.Format("{0:0000}", Convert.ToInt16(numValue[1]));
            string numValue3 = string.Format("{0:0000}", Convert.ToInt16(numValue[2]));
            string numValue4 = string.Format("{0:0000}", Convert.ToInt16(numValue[3]));
            string numValue5 = numValue[4];
            string numValue6 = numValue[5];
            string numValue7 = numValue[6];
            string numValue8 = numValue[7];
            num.WriteLine(DateTime.Now.ToString());
            num.WriteLine(numValue1);
            num.WriteLine(numValue2);
            num.WriteLine(numValue3);
            num.WriteLine(numValue4);
            num.WriteLine(numValue5);
            num.WriteLine(numValue6);
            num.WriteLine(numValue7);
            num.WriteLine(numValue8);
            num.Close();*/
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Main f = new Main();//產生Form2的物件，才可以使用它所提供的Method
            f.ShowDialog(this);
        }
    }
}
