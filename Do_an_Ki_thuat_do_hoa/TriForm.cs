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
    public partial class TriForm : Form
    {
        private Triangle tri;

        internal Triangle Tri
        {
            get
            {
                return tri;
            }

            set
            {
                tri = value;
            }
        }

        public TriForm()
        {
            InitializeComponent();
        }

        private void TriForm_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_X1.Text, out n) && int.TryParse(textBox_Y1.Text, out n) && int.TryParse(textBox_X2.Text, out n) && int.TryParse(textBox_Y2.Text, out n)&&int.TryParse(textBox_X3.Text, out n) && int.TryParse(textBox_Y3.Text, out n))
            {
                tri = new Triangle(new Point(int.Parse(textBox_X1.Text)+50, -int.Parse(textBox_Y1.Text)+50),new Point(int.Parse(textBox_X2.Text)+50, -int.Parse(textBox_Y2.Text)+50),new Point(int.Parse(textBox_X3.Text)+50, -int.Parse(textBox_Y3.Text)+50));
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
