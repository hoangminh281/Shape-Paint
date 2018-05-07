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
    public partial class RectangularForm : Form
    {
        Rectangular rtg;

        internal Rectangular Rtg { get => rtg; set => rtg = value; }

        public RectangularForm()
        {
            InitializeComponent();
        }

        private void RectangularForm_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!(int.TryParse(textBox_X.Text, out int n) && int.TryParse(textBox_Y.Text, out n) && int.TryParse(textBox_Z.Text, out n) && int.TryParse(textBox_D.Text, out n) && int.TryParse(textBox_R.Text, out n) && int.TryParse(textBox_C.Text, out n)))
            {
                MessageBox.Show("Nhap sai. Hay nhap so", "Canh bao!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                return;
            }
            rtg = new Rectangular(new Point3D(int.Parse(textBox_X.Text), int.Parse(textBox_Y.Text), int.Parse(textBox_Z.Text)), int.Parse(textBox_D.Text), int.Parse(textBox_C.Text), int.Parse(textBox_R.Text));
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
