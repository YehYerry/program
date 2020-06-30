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

namespace buttonchange
{
    public partial class Form1 : Form
    {
        byte cont1, cont2, cont3, cont4, cont5, cont6, cont7, cont8, cont9, cont10, cont11, cont12, cont13, cont14, cont15;

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
        }

        string b1, b2, b3, b4;
        int b1h, b2h, b3h, b4h, b1w, b2w, b3w, b4w;
        int b1bh, b2bh, b3bh, b4bh, b1bw, b2bw, b3bw, b4bw;
        string[] line = new string[1000];
        int ctr = 0;
        public Form1()
        {
            StreamReader str = new StreamReader(Application.StartupPath + @"\buttonchange.ifm");//讀取文字檔                      
            do
            {
                ctr++;
                line[ctr] = str.ReadLine();
                //Console.WriteLine(line[ctr]);
            } while (line[ctr] != null);

            cont1 = Convert.ToByte(line[2].Substring(7));
            Console.WriteLine(cont1);

            b1 = line[19].Substring(10);
            b2 = line[26].Substring(10);
            b3 = line[33].Substring(10);
            b4 = line[40].Substring(10);
            b1w = Convert.ToInt32(line[20].Substring(6));
            b1h = Convert.ToInt32(line[21].Substring(7));
            b1bw = Convert.ToInt32(line[22].Substring(12));
            b1bh = Convert.ToInt32(line[23].Substring(13));
            b2w = Convert.ToInt32(line[27].Substring(6));
            b2h = Convert.ToInt32(line[28].Substring(7));
            b2bw = Convert.ToInt32(line[29].Substring(12));
            b2bh = Convert.ToInt32(line[30].Substring(13));
            b3w = Convert.ToInt32(line[34].Substring(6));
            b3h = Convert.ToInt32(line[35].Substring(7));
            b3bw = Convert.ToInt32(line[36].Substring(12));
            b3bh = Convert.ToInt32(line[37].Substring(13));
            b4w = Convert.ToInt32(line[41].Substring(6));
            b4h = Convert.ToInt32(line[42].Substring(7));
            b4bw = Convert.ToInt32(line[43].Substring(12));
            b4bh = Convert.ToInt32(line[44].Substring(13));
            Console.WriteLine(b2);            
            InitializeComponent();

            button1.Size = new Size(b1w, b1h);
            button1.Location = new Point(b1bw, b1bh);
            button2.Size = new Size(b2w, b2h);
            button2.Location = new Point(b2bw, b2bh);
            button3.Size = new Size(b3w, b3h);
            button3.Location = new Point(b3bw, b3bh);
            button4.Size = new Size(b4w, b4h);
            button4.Location = new Point(b4bw, b4bh);

            this.WindowState = FormWindowState.Maximized;
            if (b1 == "0")
            {
                button1.Visible = false;
            }
            if (b2 == "0")
            {
                button2.Visible = false;
            }
            if (b3 == "0")
            {
                button3.Visible = false;
            }
            if (b4 == "0")
            {
                button4.Visible = false;
            }
        }
    }
}
