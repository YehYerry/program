using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace voiceTest
{
    public partial class Form1 : Form
    {
        int t1 = 0, t2, t3, t4;
        int ctr = 0, note, notej1 = 0, notej2 = 0, notej3 = 0, notej4 = 0;
        string[] line = new string[1000];
        byte[,] cont = new byte[15, 15];
        public Form1()
        {
            InitializeComponent();
            StreamReader str = new StreamReader(Application.StartupPath + @"\config.ifm");//讀取文字檔                      
            do
            {
                ctr++;
                line[ctr] = str.ReadLine();
                //Console.WriteLine(line[ctr]);
            } while (line[ctr] != null);
            for (int i = 2; i <= 16; i++)
            {
                note = Convert.ToInt32(line[i].Substring(7));
                if (note == 1)
                {
                    cont[0, notej1] = Convert.ToByte(line[i].Substring(4, 2));
                    notej1 += 1;
                }
                else if (note == 2)
                {
                    cont[1, notej2] = Convert.ToByte(line[i].Substring(4, 2));
                    notej2 += 1;
                }
                else if (note == 3)
                {
                    cont[2, notej3] = Convert.ToByte(line[i].Substring(4, 2));
                    notej3 += 1;
                }
                else if (note == 4)
                {
                    cont[3, notej4] = Convert.ToByte(line[i].Substring(4, 2));
                    notej4 += 1;
                }
            }//取得控制器對哪個按鈕，存入陣列 cont[0,0] 中 , notej1為控制器1的數量
             //int[,] arr1 = new int[,] {{1,3,4 } , {2,5,7},{ 8, 10 , 5 }, { 6, 5, 7 } };
            for (int i = 0; i <= cont.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= cont.GetUpperBound(1); j++)/*第二維是2(有幾「戶」)，因arr1[1,0],arr1[1,1]，故第二維上限是1*/
                {
                    if (cont[i, j].ToString() == "13") 
                    {
                        Console.WriteLine(i.ToString());
                        Console.WriteLine(cont[i, j].ToString());
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] str = new string[textBox1.Lines.Length];
            for (int i = 0; i < textBox1.Lines.Length; i++)
            {
                str[i] = textBox1.Lines[i];
                Console.WriteLine(str[i]);
            }

            t4 = int.Parse(str[0]);
            t3 = int.Parse(str[1]);
            t2 = int.Parse(str[2]);
            t1 = int.Parse(str[3]);

            SoundPlayer player = new SoundPlayer();
            if (t4 == 0 && t3 == 0 && t2 == 0)
            {
                player.SoundLocation = Application.StartupPath + @"\voice\" + t1 + ".wav";
                player.PlaySync();

            }
            else if (t4 == 0 && t3 == 0 && t2 == 1 && t1 == 0)
            {
                player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                player.PlaySync();
            }
            else if (t4 == 0 && t3 == 0 && t2 == 1 && (t1 == 1 || t1 == 2 || t1 == 3 || t1 == 4 || t1 == 5 || t1 == 6 || t1 == 7 || t1 == 8 || t1 == 9))
            {
                player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\" + t1 + ".wav";
                player.PlaySync();
            }
            else if (t4 == 0 && t3 == 0 && (t2 == 2 || t2 == 3 || t2 == 4 || t2 == 5 || t2 == 6 || t2 == 7 || t2 == 8 || t2 == 9) && t1 == 0)
            {
                player.SoundLocation = Application.StartupPath + @"\voice\" + t2 + ".wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                player.PlaySync();
            }
            else if (t4 == 0 && t3 == 0)
            {
                player.SoundLocation = Application.StartupPath + @"\voice\" + t2 + ".wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\" + t1 + ".wav";
                player.PlaySync();

                Console.WriteLine(t2);
                Console.WriteLine(t1);
            }
            else if (t4 == 0 && (t3 == 1 || t3 == 2 || t3 == 3 || t3 == 4 || t3 == 5 || t3 == 6 || t3 == 7 || t3 == 8 || t3 == 9) && t2 == 0 && t1 == 0)
            {
                player.SoundLocation = Application.StartupPath + @"\voice\" + t3 + ".wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                player.PlaySync();
            }
            else if (t4 == 0 && (t3 == 1 || t3 == 2 || t3 == 3 || t3 == 4 || t3 == 5 || t3 == 6 || t3 == 7 || t3 == 8 || t3 == 9) && (t2 == 1 || t2 == 2 || t2 == 3 || t2 == 4 || t2 == 5 || t2 == 6 || t2 == 7 || t2 == 8 || t2 == 9) && t1 == 0)
            {
                player.SoundLocation = Application.StartupPath + @"\voice\" + t3 + ".wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\" + t2 + ".wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                player.PlaySync();
            }
            else if (t4 == 0 && (t3 == 1 || t3 == 2 || t3 == 3 || t3 == 4 || t3 == 5 || t3 == 6 || t3 == 7 || t3 == 8 || t3 == 9) && t2 == 0 && (t1 == 1 || t1 == 2 || t1 == 3 || t1 == 4 || t1 == 5 || t1 == 6 || t1 == 7 || t1 == 8 || t1 == 9))
            {
                player.SoundLocation = Application.StartupPath + @"\voice\" + t3 + ".wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\" + t2 + ".wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\" + t1 + ".wav";
                player.PlaySync();
            }
            else if (t4 == 0)
            {
                player.SoundLocation = Application.StartupPath + @"\voice\" + t3 + ".wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\" + t2 + ".wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\" + t1 + ".wav";
                player.PlaySync();
            }
            else if ((t4 == 1 || t4 == 2 || t4 == 3 || t4 == 4 || t4 == 5 || t4 == 6 || t4 == 7 || t4 == 8 || t4 == 9) && t3 == 0 && t2 == 0 && t1 == 0)
            {
                player.SoundLocation = Application.StartupPath + @"\voice\" + t4 + ".wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                player.PlaySync();
            }
            else if ((t4 == 1 || t4 == 2 || t4 == 3 || t4 == 4 || t4 == 5 || t4 == 6 || t4 == 7 || t4 == 8 || t4 == 9) && (t3 == 1 || t3 == 2 || t3 == 3 || t3 == 4 || t3 == 5 || t3 == 6 || t3 == 7 || t3 == 8 || t3 == 9) && t2 == 0 && t1 == 0)
            {
                player.SoundLocation = Application.StartupPath + @"\voice\" + t4 + ".wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\" + t3 + ".wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                player.PlaySync();
            }
            else if ((t4 == 1 || t4 == 2 || t4 == 3 || t4 == 4 || t4 == 5 || t4 == 6 || t4 == 7 || t4 == 8 || t4 == 9) && (t3 == 1 || t3 == 2 || t3 == 3 || t3 == 4 || t3 == 5 || t3 == 6 || t3 == 7 || t3 == 8 || t3 == 9) && (t2 == 1 || t2 == 2 || t2 == 3 || t2 == 4 || t2 == 5 || t2 == 6 || t2 == 7 || t2 == 8 || t2 == 9) && t1 == 0)
            {
                player.SoundLocation = Application.StartupPath + @"\voice\" + t4 + ".wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\" + t3 + ".wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\" + t2 + ".wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                player.PlaySync();
            }
            else if ((t4 == 1 || t4 == 2 || t4 == 3 || t4 == 4 || t4 == 5 || t4 == 6 || t4 == 7 || t4 == 8 || t4 == 9) && (t3 == 0 || t3 == 1 || t3 == 2 || t3 == 3 || t3 == 4 || t3 == 5 || t3 == 6 || t3 == 7 || t3 == 8 || t3 == 9) && (t2 == 0 || t2 == 1 || t2 == 2 || t2 == 3 || t2 == 4 || t2 == 5 || t2 == 6 || t2 == 7 || t2 == 8 || t2 == 9) && t1 == 0 )
            {
                player.SoundLocation = Application.StartupPath + @"\voice\" + t4 + ".wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\" + t3 + ".wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\" + t2 + ".wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                player.PlaySync();
            }
            else if ((t4 == 1 || t4 == 2 || t4 == 3 || t4 == 4 || t4 == 5 || t4 == 6 || t4 == 7 || t4 == 8 || t4 == 9) && (t3 == 0 || t3 == 1 || t3 == 2 || t3 == 3 || t3 == 4 || t3 == 5 || t3 == 6 || t3 == 7 || t3 == 8 || t3 == 9) && (t2 == 0 || t2 == 1 || t2 == 2 || t2 == 3 || t2 == 4 || t2 == 5 || t2 == 6 || t2 == 7 || t2 == 8 || t2 == 9) && ( t1 == 1 || t1 == 2 || t1 == 3 || t1 == 4 || t1 == 5 || t1 == 6 || t1 == 7 || t1 == 8 || t1 == 9))
            {
                player.SoundLocation = Application.StartupPath + @"\voice\" + t4 + ".wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\仟.wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\" + t3 + ".wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\佰_.wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\" + t2 + ".wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\拾.wav";
                player.PlaySync();
                player.SoundLocation = Application.StartupPath + @"\voice\" + t1 + ".wav";
                player.PlaySync();
            }
            else
            {
                MessageBox.Show("人數超過9999號!!");
            }
            textBox1.Clear();
        }
    }
}
