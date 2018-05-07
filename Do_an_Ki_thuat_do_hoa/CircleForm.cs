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
    public partial class CircleForm : Form
    {
        private Circle cir;
        private int mode = -1;

        public int Mode
        {
            get
            {
                return mode;
            }

            set
            {
                mode = value;
            }
        }

        internal Circle Cir
        {
            get
            {
                return cir;
            }

            set
            {
                cir = value;
            }
        }

        public CircleForm()
        {
            InitializeComponent();
        }

        private void CircleForm_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            int n;
            if (tabControl1.SelectedIndex == 0)
            {
                if (!(int.TryParse(textBox_X.Text, out n) && int.TryParse(textBox_Y.Text, out n) && int.TryParse(textBox_r.Text, out n)))
                {
                    MessageBox.Show("Nhap sai. Hay nhap so", "Canh bao!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    return;
                }
                if (radioButton1_bre.Checked) Mode = (int)MainForm.ModeStyle.bresenham;
                else if (radioButton1_mid.Checked) Mode = (int)MainForm.ModeStyle.midpoint;
                else
                {
                    MessageBox.Show("Chua chon kieu", "Canh bao!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    return;
                }
                cir = new Circle(new Point(int.Parse(textBox_X.Text) + 50, -int.Parse(textBox_Y.Text) + 50), int.Parse(textBox_r.Text));
                this.Close();
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                if (!(int.TryParse(textBox_X1.Text, out n) && int.TryParse(textBox_Y1.Text, out n) && int.TryParse(textBox_X2.Text, out n) && int.TryParse(textBox_Y2.Text, out n)))
                {
                    MessageBox.Show("Nhap sai. Hay nhap so", "Canh bao!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    return;
                }
                if (radioButton1_bre.Checked) Mode = (int)MainForm.ModeStyle.bresenham;
                else if (radioButton1_mid.Checked) Mode = (int)MainForm.ModeStyle.midpoint;
                else
                {
                    MessageBox.Show("Chua chon kieu", "Canh bao!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    return;
                }
                cir = new Circle(new Point(int.Parse(textBox_X1.Text) + 50, -int.Parse(textBox_Y1.Text) + 50), new Point(int.Parse(textBox_X2.Text) + 50, -int.Parse(textBox_Y2.Text) + 50));
                this.Close();
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
