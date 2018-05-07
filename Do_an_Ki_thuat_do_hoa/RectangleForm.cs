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
    public partial class RectangleForm : Form
    {
        private Rectangle rec;

        internal Rectangle Rec
        {
            get
            {
                return rec;
            }

            set
            {
                rec = value;
            }
        }

        public RectangleForm()
        {
            InitializeComponent();
        }

        private void RectangleForm_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int n;
            if (tabControl1.SelectedIndex == 0)
            {
                if (!(int.TryParse(textBox_X.Text, out n) && int.TryParse(textBox_Y.Text, out n) && int.TryParse(textBox_W.Text, out n) && int.TryParse(textBox_H.Text, out n)))
                {
                    MessageBox.Show("Nhap sai. Hay nhap so", "Canh bao!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    return;
                }
                rec = new Rectangle(new Point(int.Parse(textBox_X.Text)+50, -int.Parse(textBox_Y.Text)+50), int.Parse(textBox_W.Text), int.Parse(textBox_H.Text));
                this.Close();
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                if (!(int.TryParse(textBox_X1.Text, out n) && int.TryParse(textBox_Y1.Text, out n) && int.TryParse(textBox_X2.Text, out n) && int.TryParse(textBox_Y2.Text, out n)))
                {
                    MessageBox.Show("Nhap sai. Hay nhap so", "Canh bao!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    return;
                }
                rec = new Rectangle(new Point(int.Parse(textBox_X1.Text)+50, -int.Parse(textBox_Y1.Text)+50), new Point(int.Parse(textBox_X2.Text)+50, -int.Parse(textBox_Y2.Text)+50));
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
