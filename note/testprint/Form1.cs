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

namespace testprint
{
    public partial class Form1 : Form
    {
        int timeLeft;
        public Form1()
        {
            InitializeComponent();
            Console.WriteLine(DateTime.Now.Year.ToString());
            //FileStream fs = File.Create(Application.StartupPath + @"\NOTE\2.txt");
            FileStream fs = File.Create(Application.StartupPath + @"\NOTE\" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + ".txt");
            fs.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // 讀取TXT檔內文串
            /*
                StreamReader str = new StreamReader(@"E:\pixnet\20160614\Lab2_TXT_Read_Write\Read.TXT");
                StreamReader str = new StreamReader(讀取TXT檔路徑)    
                str.ReadLine(); (一行一行讀取)
                str.ReadToEnd();(一次讀取全部)
                str.Close(); (關閉str)
            */
            StreamReader str = new StreamReader(@"C:\Users\jerry\github\program\note\test.ifm");
            string ReadLine1, ReadAll;
            string[] s = new string[1000];
            int ctr = 0;
            do
            {
                ctr++;
                s[ctr] = str.ReadLine();
                Console.WriteLine(s[ctr]);
            } while (s[ctr] != null);

            ReadLine1 = str.ReadLine();
            ReadAll = str.ReadToEnd();

            //MessageBox.Show("ReadLine1 = " + ReadLine1);
            //MessageBox.Show("ReadAll = " + ReadAll);           
            //string[] info = { ReadLine1, ReadLine2, ReadLine3, ReadLine4, ReadLine5};
            //Console.WriteLine(info);
            
            str.Close();
            label1.Text = s[2].Substring(6);
            Console.WriteLine(Encoding.UTF8.GetBytes(s[2].Substring(6)));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 將字串寫入TXT檔
            StreamWriter str = new StreamWriter(@"C:\Users\jerry\github\program\note\1.txt",true);
            string WriteWord = "aaaaa";
            str.WriteLine(WriteWord);
            str.WriteLine("bbb");
            for (int n = 1; n <= 10; n++) 
            {
                Console.WriteLine(DateTime.Now.AddDays(7*n).ToShortDateString());
            }
            str.Close();
            //清空檔案
            if (DateTime.Now.Hour.ToString() == "11" & DateTime.Now.Minute.ToString() == "32") 
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(@"C:\Users\jerry\github\program\note\1.txt", FileMode.Truncate, FileAccess.ReadWrite);

                }
                catch (Exception ex)
                {
                    Trace.Write("清空日誌檔案失敗：" + ex.Message);
                }
                finally
                {
                    fs.Close();
                }
                Console.WriteLine("清空");
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft = timeLeft - 1;
                label2.Text = timeLeft + " seconds";
            }
            else
            {
                /* 倒數時間到執行 */

                timeLeft = 5;
                label2.Text = "5 seconds";
            }
            if (timeLeft == 0) 
            {
                timer1.Stop();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timeLeft = 5;
            label2.Text = "5 seconds";
            /* Timer 啟動 */
            timer1.Start();
        }
    }
}
