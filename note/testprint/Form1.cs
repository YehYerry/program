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

namespace testprint
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            String[] pairs = { "Color1=red", "Color2=green", "Color3=blue",
                 "Title=Code Repository" };
            foreach (var pair in pairs)
            {
                int position = pair.IndexOf("=");
                if (position < 0)
                    continue;
                Console.WriteLine("Key: {0}, Value: '{1}'",
                              pair.Substring(0, position),
                               pair.Substring(position + 1));
            }
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
            string ReadLine1, ReadLine2, ReadLine3, ReadLine4, ReadLine5, ReadAll;
            ReadLine1 = str.ReadLine();
            ReadLine2 = str.ReadLine();
            ReadLine3 = str.ReadLine();
            ReadLine4 = str.ReadLine();
            ReadLine5 = str.ReadLine();
            ReadAll = str.ReadToEnd();

            MessageBox.Show("ReadLine1 = " + ReadLine1);
            MessageBox.Show("ReadLine2 = " + ReadLine2);
            MessageBox.Show("ReadAll = " + ReadAll);           
            //string[] info = { ReadLine1, ReadLine2, ReadLine3, ReadLine4, ReadLine5};
            //Console.WriteLine(info);
            
            str.Close();
            label1.Text = ReadLine2;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
                // 將字串寫入TXT檔
                StreamWriter str = new StreamWriter(@"C:\Users\jerry\Desktop\testprint\1.txt");
                string WriteWord = "aaaaa";
                str.WriteLine(WriteWord);
                str.WriteLine("bbb");
                str.Close();           
        }
    }
}
