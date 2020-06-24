using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        public Form1()
        {
            InitializeComponent();
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
            else if ((t4 == 1 || t4 == 2 || t4 == 3 || t4 == 4 || t4 == 5 || t4 == 6 || t4 == 7 || t4 == 8 || t4 == 9) && (t3 == 1 || t3 == 2 || t3 == 3 || t3 == 4 || t3 == 5 || t3 == 6 || t3 == 7 || t3 == 8 || t3 == 9) && (t2 == 1 || t2 == 2 || t2 == 3 || t2 == 4 || t2 == 5 || t2 == 6 || t2 == 7 || t2 == 8 || t2 == 9) && (t1 == 1 || t1 == 2 || t1 == 3 || t1 == 4 || t1 == 5 || t1 == 6 || t1 == 7 || t1 == 8 || t1 == 9))
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
