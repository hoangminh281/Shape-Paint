using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Do_an_Ki_thuat_do_hoa
{
    public partial class LineForm : Form
    {
        private Line line;
        private int mode = -1;

        internal Line Line
        {
            get
            {
                return line;
            }

            set
            {
                line = value;
            }
        }

        public int Mode { get => mode; set => mode = value; }

        public LineForm()
        {
            InitializeComponent();
        }

        private void LineForm_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_X1.Text, out n)&&int.TryParse(textBox_Y1.Text,out n)&&int.TryParse(textBox_X2.Text,out n)&&int.TryParse(textBox_Y2.Text,out n))
            {
                if (radioButton1_bre.Checked) Mode = (int)MainForm.ModeStyle.bresenham;
                else if (radioButton1_mid.Checked) Mode = (int)MainForm.ModeStyle.midpoint;
                else
                {
                    MessageBox.Show("Chua chon kieu", "Canh bao!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    return;
                }
                Line = new Line(new Point(int.Parse(textBox_X1.Text)+50,-int.Parse(textBox_Y1.Text)+50),new Point(int.Parse(textBox_X2.Text)+50,-int.Parse(textBox_Y2.Text)+50));
                this.Close();
            }
            else MessageBox.Show("Nhap sai. Hay nhap so", "Canh bao!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
