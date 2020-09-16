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
    public partial class formlocation : Form
    {
        byte[,] cont = new byte[15, 15];
        string b1, b2, b3, b4;
        int b1h, b2h, b3h, b4h, b1w, b2w, b3w, b4w;
        int L1s, L2s, L3s, L4s;
        string L1f, L2f, L3f, L4f;
        int b1bh, b2bh, b3bh, b4bh, b1bw, b2bw, b3bw, b4bw;

        int L1bh, L2bh, L3bh, L4bh, L1bw, L2bw, L3bw, L4bw;

        byte b1num1, b1num2, b1num3, b1num4, b2num1, b2num2, b2num3, b2num4, b3num1, b3num2, b3num3, b3num4, b4num1, b4num2, b4num3, b4num4;
        byte[,] num;
        byte[] wait1 = { 0, 0, 0, 0 }; //四個按鈕的等待人數(十位數)
        byte[] wait2 = { 0, 0, 0, 0 }; //四個按鈕的等待人數(個位數)      
        int label = 0, label2 = 0, label3 = 0, label4 = 0;
        byte num1, num2, num3, num4, w1, w2, n1, n2, n3, n4;
        string[] line = new string[1000];
        string[] line1 = new string[1000];
        string[] line2 = new string[1000];
        int decimalLength;
        int ctr = 0, ctr1 = 0, ctr2 = 0, note, notej1 = 0, notej2 = 0, notej3 = 0, notej4 = 0, pnum1, pnum2, pnum3, pnum4;
        int pageH, pageW, pbw, pbh, photoW, photoH, wordstyle1, wordstyle2, wordstyle3, nums, numbw, numbh, str1s, str1bw, str1bh, str2s, str2bw, str2bh;
        string numfont, str1font, str2font, text, pnumber1, pnumber2, pnumber3, pnumber4;
        FontStyle wdstyle1, wdstyle2, wdstyle3;
        DateTime doubleClickTimer = DateTime.Now;
        public formlocation()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;

            this.textBox1.KeyPress += new KeyPressEventHandler(CheckEnter);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            TimeSpan t = (TimeSpan)(DateTime.Now - doubleClickTimer); //DoubleClick後又點了一下, 計算時間差 

            if (t.TotalMilliseconds <= 200) //如果小於200豪秒就執行 
            {
                Console.WriteLine("EXIT");
                this.Close();
            }
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            doubleClickTimer = DateTime.Now; //記下DoubleClick的時間 
        }
        private void CheckEnter(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                textBox1.AppendText("X = " + Cursor.Position.X.ToString() + "\r\n");
                textBox1.AppendText("Y = " + Cursor.Position.Y.ToString() + "\r\n");
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {////Timer, 每10ms讀取滑鼠座標值
            label5.Text = "X = " + Cursor.Position.X.ToString();
            label6.Text = "Y = " + Cursor.Position.Y.ToString();

        }
        private void formlocation_Load(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;//timer控制元件的執行頻率
            StreamReader str = new StreamReader(Application.StartupPath + @"\config.txt");//讀取文字檔
            StreamReader str1 = new StreamReader(Application.StartupPath + @"\print_config.txt");
            StreamReader str2 = new StreamReader(Application.StartupPath + @"\Log\exitnum_log.txt");
            do
            {
                ctr++;
                line[ctr] = str.ReadLine();
                //Console.WriteLine(line[ctr]);
            } while (line[ctr] != null);

            for (int i = 2; i <= 16; i++)
            {
                note = Convert.ToInt32(line[i].Substring(6));
                if (note == 1)
                {
                    cont[0, notej1] = Convert.ToByte(line[i].Substring(3, 2));
                    notej1 += 1;
                }
                else if (note == 2)
                {
                    cont[1, notej2] = Convert.ToByte(line[i].Substring(3, 2));
                    notej2 += 1;
                }
                else if (note == 3)
                {
                    cont[2, notej3] = Convert.ToByte(line[i].Substring(3, 2));
                    notej3 += 1;
                }
                else if (note == 4)
                {
                    cont[3, notej4] = Convert.ToByte(line[i].Substring(3, 2));
                    notej4 += 1;
                }
            }//取得控制器對哪個按鈕，存入陣列 cont[0,0] 中 , notej1為控制器1的數量

            //設定背景圖
            BackgroundImage = new Bitmap(Application.StartupPath + @"\background\back.jpg");
            BackgroundImageLayout = ImageLayout.Stretch; //設定圖片填滿
            Image pic1 = new Bitmap(Application.StartupPath + @"\background\button.jpg");
            Image pic2 = new Bitmap(Application.StartupPath + @"\background\button.jpg");
            Image pic3 = new Bitmap(Application.StartupPath + @"\background\button.jpg");
            Image pic4 = new Bitmap(Application.StartupPath + @"\background\button.jpg");
            button1.BackgroundImageLayout = ImageLayout.Stretch; //設定圖片填滿
            button2.BackgroundImageLayout = ImageLayout.Stretch;
            button3.BackgroundImageLayout = ImageLayout.Stretch;
            button4.BackgroundImageLayout = ImageLayout.Stretch;
            button1.BackgroundImage = pic1;
            button2.BackgroundImage = pic2;
            button3.BackgroundImage = pic3;
            button4.BackgroundImage = pic4;
            /*byte[] val = Encoding.UTF8.GetBytes(line[2].Substring(6));
            foreach (byte s1 in val)
            Console.WriteLine(s1);*/
            {
                b1 = line[19].Substring(10);
                b2 = line[27].Substring(10);
                b3 = line[35].Substring(10);
                b4 = line[43].Substring(10);
                b1w = Convert.ToInt32(line[20].Substring(6));
                b1h = Convert.ToInt32(line[21].Substring(7));
                b1bw = Convert.ToInt32(line[22].Substring(12));
                b1bh = Convert.ToInt32(line[23].Substring(13));
                b1num1 = Convert.ToByte(line[24].Substring(9, 1));
                b1num2 = Convert.ToByte(line[24].Substring(10, 1));
                b1num3 = Convert.ToByte(line[24].Substring(11, 1));
                b1num4 = Convert.ToByte(line[24].Substring(12, 1));
                b2w = Convert.ToInt32(line[28].Substring(6));
                b2h = Convert.ToInt32(line[29].Substring(7));
                b2bw = Convert.ToInt32(line[30].Substring(12));
                b2bh = Convert.ToInt32(line[31].Substring(13));
                b2num1 = Convert.ToByte(line[32].Substring(9, 1));
                b2num2 = Convert.ToByte(line[32].Substring(10, 1));
                b2num3 = Convert.ToByte(line[32].Substring(11, 1));
                b2num4 = Convert.ToByte(line[32].Substring(12, 1));
                b3w = Convert.ToInt32(line[36].Substring(6));
                b3h = Convert.ToInt32(line[37].Substring(7));
                b3bw = Convert.ToInt32(line[38].Substring(12));
                b3bh = Convert.ToInt32(line[39].Substring(13));
                b3num1 = Convert.ToByte(line[40].Substring(9, 1));
                b3num2 = Convert.ToByte(line[40].Substring(10, 1));
                b3num3 = Convert.ToByte(line[40].Substring(11, 1));
                b3num4 = Convert.ToByte(line[40].Substring(12, 1));
                b4w = Convert.ToInt32(line[44].Substring(6));
                b4h = Convert.ToInt32(line[45].Substring(7));
                b4bw = Convert.ToInt32(line[46].Substring(12));
                b4bh = Convert.ToInt32(line[47].Substring(13));
                b4num1 = Convert.ToByte(line[48].Substring(9, 1));
                b4num2 = Convert.ToByte(line[48].Substring(10, 1));
                b4num3 = Convert.ToByte(line[48].Substring(11, 1));
                b4num4 = Convert.ToByte(line[48].Substring(12, 1));
                L1s = Convert.ToInt32(line[51].Substring(5));
                L1bw = Convert.ToInt32(line[52].Substring(12));
                L1bh = Convert.ToInt32(line[53].Substring(13));
                L1f = line[54].Substring(5);
                L2s = Convert.ToInt32(line[57].Substring(5));
                L2bw = Convert.ToInt32(line[58].Substring(12));
                L2bh = Convert.ToInt32(line[59].Substring(13));
                L2f = line[60].Substring(5);
                L3s = Convert.ToInt32(line[63].Substring(5));
                L3bw = Convert.ToInt32(line[64].Substring(12));
                L3bh = Convert.ToInt32(line[65].Substring(13));
                L3f = line[66].Substring(5);
                L4s = Convert.ToInt32(line[69].Substring(5));
                L4bw = Convert.ToInt32(line[70].Substring(12));
                L4bh = Convert.ToInt32(line[71].Substring(13));
                L4f = line[72].Substring(5);
            }//config的獲取參數檔

            {
                do
                {
                    ctr1++;
                    line1[ctr1] = str1.ReadLine();
                } while (line1[ctr1] != null);
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
            }//讀取列印文字檔
            button1.Size = new Size(b1w, b1h);
            button1.Location = new Point(b1bw, b1bh);
            button2.Size = new Size(b2w, b2h);
            button2.Location = new Point(b2bw, b2bh);
            button3.Size = new Size(b3w, b3h);
            button3.Location = new Point(b3bw, b3bh);
            button4.Size = new Size(b4w, b4h);
            button4.Location = new Point(b4bw, b4bh);
            button1.FlatStyle = FlatStyle.Flat; button1.FlatAppearance.BorderSize = 0; button1.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button2.FlatStyle = FlatStyle.Flat; button2.FlatAppearance.BorderSize = 0; button2.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button3.FlatStyle = FlatStyle.Flat; button3.FlatAppearance.BorderSize = 0; button3.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button4.FlatStyle = FlatStyle.Flat; button4.FlatAppearance.BorderSize = 0; button4.FlatAppearance.MouseOverBackColor = Color.Transparent;

            label_wait.Font = new Font(L1f, L1s);//Times New Roman、Arial、標楷體
            label_wait.Location = new Point(L1bw, L1bh);
            label_wait2.Font = new Font(L2f, L2s);
            label_wait2.Location = new Point(L2bw, L2bh);
            label_wait3.Font = new Font(L3f, L3s);
            label_wait3.Location = new Point(L3bw, L3bh);
            label_wait4.Font = new Font(L4f, L4s);
            label_wait4.Location = new Point(L4bw, L4bh);

            num = new byte[,] { { b1num1, b1num2, b1num3, b1num4 }, { b2num1, b2num2, b2num3, b2num4 }, { b3num1, b3num2, b3num3, b3num4 }, { b4num1, b4num2, b4num3, b4num4 } };

            this.WindowState = FormWindowState.Maximized;
            //列印號碼
            do
            {
                ctr2++;
                line2[ctr2] = str2.ReadLine();
            } while (line2[ctr2] != null);
            pnum1 = Convert.ToInt32(line2[1].Substring(17, 4));//起始號碼
            pnum2 = Convert.ToInt32(line2[2].Substring(17, 4));
            pnum3 = Convert.ToInt32(line2[3].Substring(17, 4));
            pnum4 = Convert.ToInt32(line2[4].Substring(17, 4));
            wait2[0] = Convert.ToByte(line2[1].Substring(8, 1));
            wait1[0] = Convert.ToByte(line2[1].Substring(9, 1));
            wait2[1] = Convert.ToByte(line2[2].Substring(8, 1));
            wait1[1] = Convert.ToByte(line2[2].Substring(9, 1));
            wait2[2] = Convert.ToByte(line2[3].Substring(8, 1));
            wait1[2] = Convert.ToByte(line2[3].Substring(9, 1));
            wait2[3] = Convert.ToByte(line2[4].Substring(8, 1));
            wait1[3] = Convert.ToByte(line2[4].Substring(9, 1));
            b1num1 = Convert.ToByte(line2[1].Substring(17, 1));
            b1num2 = Convert.ToByte(line2[1].Substring(18, 1));
            b1num3 = Convert.ToByte(line2[1].Substring(19, 1));
            b1num4 = Convert.ToByte(line2[1].Substring(20, 1));
            b2num1 = Convert.ToByte(line2[2].Substring(17, 1));
            b2num2 = Convert.ToByte(line2[2].Substring(18, 1));
            b2num3 = Convert.ToByte(line2[2].Substring(19, 1));
            b2num4 = Convert.ToByte(line2[2].Substring(20, 1));
            b3num1 = Convert.ToByte(line2[3].Substring(17, 1));
            b3num2 = Convert.ToByte(line2[3].Substring(18, 1));
            b3num3 = Convert.ToByte(line2[3].Substring(19, 1));
            b3num4 = Convert.ToByte(line2[3].Substring(20, 1));
            b4num1 = Convert.ToByte(line2[4].Substring(17, 1));
            b4num2 = Convert.ToByte(line2[4].Substring(18, 1));
            b4num3 = Convert.ToByte(line2[4].Substring(19, 1));
            b4num4 = Convert.ToByte(line2[4].Substring(20, 1));
            num = new byte[,] { { b1num1, b1num2, b1num3, b1num4 }, { b2num1, b2num2, b2num3, b2num4 }, { b3num1, b3num2, b3num3, b3num4 }, { b4num1, b4num2, b4num3, b4num4 } };
            //關閉前的等待人數
            label = Convert.ToInt32(line2[1].Substring(8, 1)) * 10 + Convert.ToInt32(line2[1].Substring(9, 1));
            label2 = Convert.ToInt32(line2[2].Substring(8, 1)) * 10 + Convert.ToInt32(line2[2].Substring(9, 1));
            label3 = Convert.ToInt32(line2[3].Substring(8, 1)) * 10 + Convert.ToInt32(line2[3].Substring(9, 1));
            label4 = Convert.ToInt32(line2[4].Substring(8, 1)) * 10 + Convert.ToInt32(line2[4].Substring(9, 1));

            //按鈕是否出現
            if (b1 == "0")
            {
                button1.Visible = false;
                label_wait.Visible = false;
            }
            if (b2 == "0")
            {
                button2.Visible = false;
                label_wait2.Visible = false;
            }
            if (b3 == "0")
            {
                button3.Visible = false;
                label_wait3.Visible = false;
            }
            if (b4 == "0")
            {
                button4.Visible = false;
                label_wait4.Visible = false;
            }
            str.Close();
            str1.Close();
            str2.Close();
        }
    }
}
