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
    public partial class ConeForm : Form
    {
        private Cone cone;

        internal Cone Cone { get => cone; set => cone = value; }

        public ConeForm()
        {
            InitializeComponent();
        }

        private void ConeForm_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!(int.TryParse(textBox_X.Text, out int n) && int.TryParse(textBox_Y.Text, out n) && int.TryParse(textBox_Z.Text, out n) && int.TryParse(textBox_R.Text, out n) && int.TryParse(textBox_H.Text, out n)))
            {
                MessageBox.Show("Nhap sai. Hay nhap so", "Canh bao!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                return;
            }
            cone = new Cone(new Point3D(int.Parse(textBox_X.Text), int.Parse(textBox_Y.Text), int.Parse(textBox_Z.Text)), int.Parse(textBox_R.Text), int.Parse(textBox_H.Text));
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
