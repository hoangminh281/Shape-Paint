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
    public partial class TransForm : Form
    {
        private List<double[,]> transList;
        private TransClass transclass = new TransClass();

        internal List<double[,]> TransList
        {
            get
            {
                return transList;
            }

            set
            {
                transList = value;
            }
        }

        public TransForm()
        {
            InitializeComponent();
        }

        private void TransForm_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            transList = new List<double[,]>();
            int n;
            if (tabControl_Trans.SelectedIndex == 0)
            {
                if (!(int.TryParse(textBox_moveX.Text, out n) && int.TryParse(textBox_moveY.Text, out n)))
                {
                    MessageBox.Show("Nhap sai. Hay nhap so", "Canh bao!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    return;
                }
                transList.Add(transclass.tinhtien(double.Parse(textBox_moveX.Text), -double.Parse(textBox_moveY.Text)));
                this.Close();
            }
            else if (tabControl_Trans.SelectedIndex == 1)
            {
                if (!(int.TryParse(textBox_tamX.Text, out n) && int.TryParse(textBox_tamY.Text, out n) && int.TryParse(textBox_goc.Text, out n)))
                {
                    MessageBox.Show("Nhap sai. Hay nhap so", "Canh bao!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    return;
                }
                transList.Add(transclass.tinhtien(-int.Parse(textBox_tamX.Text) - 50, int.Parse(textBox_tamY.Text) - 50));
                transList.Add(transclass.quay(-double.Parse(textBox_goc.Text) * (Math.PI / 180)));
                transList.Add(transclass.tinhtien(int.Parse(textBox_tamX.Text) + 50, -int.Parse(textBox_tamY.Text) + 50));
                this.Close();
            }
            else if (tabControl_Trans.SelectedIndex == 2)
            {
                if (tabControl1.SelectedIndex == 0)
                {
                    if (radioButton1.Checked == false && radioButton2.Checked == false && radioButton3.Checked == false)
                    {
                        MessageBox.Show("Chưa chọn trục", "Canh bao!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                        return;
                    }
                    if (radioButton1.Checked)
                    {
                        transList.Add(transclass.tinhtien(-50, -50));
                        TransList.Add(transclass.doixungOy());
                        transList.Add(transclass.tinhtien(50, 50));
                    }
                    if (radioButton2.Checked)
                    {
                        transList.Add(transclass.tinhtien(-50, -50));
                        TransList.Add(transclass.doixungOx());
                        transList.Add(transclass.tinhtien(50, 50));
                    }
                    if (radioButton3.Checked)
                    {
                        transList.Add(transclass.tinhtien(-50, -50));
                        TransList.Add(transclass.doixungO());
                        transList.Add(transclass.tinhtien(50, 50));
                    }
                    this.Close();
                }
                else if (tabControl1.SelectedIndex == 1)
                {
                    if (!(int.TryParse(textBoxX.Text, out n) && int.TryParse(textBoxY.Text, out n)))
                    {
                        MessageBox.Show("Nhap sai. Hay nhap so", "Canh bao!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                        return;
                    }
                    transList.Add(transclass.tinhtien(-int.Parse(textBoxX.Text) - 50, int.Parse(textBoxY.Text) - 50));
                    transList.Add(transclass.doixungO());
                    transList.Add(transclass.tinhtien(int.Parse(textBoxX.Text) + 50, -int.Parse(textBoxY.Text) + 50));
                    this.Close();
                }
                else if (tabControl1.SelectedIndex == 2)
                {
                    if (!(int.TryParse(textBoxX1.Text, out n) && int.TryParse(textBoxY1.Text, out n) && int.TryParse(textBoxX2.Text, out n) && int.TryParse(textBoxY2.Text, out n)))
                    {
                        MessageBox.Show("Nhap sai. Hay nhap so", "Canh bao!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                        return;
                    }
                    Point p1 = new Point(int.Parse(textBoxX1.Text), int.Parse(textBoxY1.Text));
                    Point p2 = new Point(int.Parse(textBoxX2.Text), int.Parse(textBoxY2.Text));
                    if (p1.X == 0 && p2.X == 0)
                    {
                        transList.Add(transclass.tinhtien(-50, -50));
                        TransList.Add(transclass.doixungOy());
                        transList.Add(transclass.tinhtien(50, 50));
                        this.Close();
                        return;
                    }
                    else if (p1.Y == 0 && p2.Y == 0)
                    {
                        transList.Add(transclass.tinhtien(-50, -50));
                        TransList.Add(transclass.doixungOx());
                        transList.Add(transclass.tinhtien(50, 50));
                        this.Close();
                        return;
                    }
                    double alpha = (double)Math.Atan2(p2.X-p1.X , p2.Y-p1.Y);
                    transList.Add(transclass.tinhtien(-50-p1.X, -50-p1.Y));
                    TransList.Add(transclass.quay(-alpha));
                    TransList.Add(transclass.doixungOy());
                    TransList.Add(transclass.quay(alpha));
                    transList.Add(transclass.tinhtien(50+p1.X, 50+p1.Y));
                    this.Close();
                }
            }
            else if (tabControl_Trans.SelectedIndex == 3)
            {
                double d;
                if (!(double.TryParse(textBox_zoomX.Text, out d) && double.TryParse(textBox_zoomY.Text, out d)))
                {
                    MessageBox.Show("Nhap sai. Hay nhap so", "Canh bao!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    return;
                }
                transList.Add(transclass.tinhtien(-50, -50));
                TransList.Add(transclass.tile(double.Parse(textBox_zoomX.Text), double.Parse(textBox_zoomY.Text)));
                transList.Add(transclass.tinhtien(50, 50));
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
