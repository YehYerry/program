﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            if (textBox1.Text.ToString() == null)
            {
                textBox1.Text = "0000";
            }
            else if (textBox2.Text.ToString() == null)
            {
                textBox2.Text = "0000";
            }
            else if (textBox3.Text.ToString() == null)
            {
                textBox3.Text = "0000";
            }
            else if (textBox4.Text.ToString() == null)
            {
                textBox4.Text = "0000";
            }
            int[] array = { Int32.Parse(textBox1.Text.ToString()), Int32.Parse(textBox2.Text.ToString()), Int32.Parse(textBox3.Text.ToString()) , Int32.Parse(textBox4.Text.ToString())};
            Form1 lForm1 = (Form1)this.Owner;//把Form2的父窗口指針賦給lForm1
            lForm1.NumValue = array;//使用父窗口指針賦值  
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
    }
}