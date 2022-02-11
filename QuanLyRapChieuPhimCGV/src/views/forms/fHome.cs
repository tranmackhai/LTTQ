using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.utils;
using QuanLyRapChieuPhimCGV.src.views.usercontrols;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyRapChieuPhimCGV.src.views.forms
{
    public partial class fHome : Form
    {
        private fLogin preComponent;
        private Employee employee;
        private Button btn_Clicked;
        private Methods methods = new Methods();
        private bool logOut = false;
        public fHome(fLogin f, Employee emp)
        {
            InitializeComponent();
            preComponent = f;
            employee = emp;
            pnlView.Controls.Add(new ucHome());
            btn_Clicked = btnHome;
            pictureBox2.Image = Image.FromFile($@"..\..\public\img\{(employee.gender == "Nam" ? "profile" : "profile-female")}.png");
            label1.Text = $"{employee.name}";
            label2.Text = $"{employee.position.ToUpper()}";
            if (employee.permission == 0)
            {
                flowLayoutPanel1.Controls.Remove(btnAdministration);
                flowLayoutPanel1.Controls.Remove(btnSellFood);
                flowLayoutPanel1.Controls.Remove(btnStatistics);
            }
            else if (employee.permission == 1)
            {
                flowLayoutPanel1.Controls.Remove(btnAdministration);
                flowLayoutPanel1.Controls.Remove(btnBookTicket);
                flowLayoutPanel1.Controls.Remove(btnStatistics);
            }
            else if (employee.permission == 2)
            {
                flowLayoutPanel1.Controls.Remove(btnBookTicket);
                flowLayoutPanel1.Controls.Remove(btnSellFood);
            }
        }

        private void fNewHome_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(logOut == false)
            {
                DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn thoát ?", "Câu hỏi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                e.Cancel = dialogResult == DialogResult.No;
                if (dialogResult == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    preComponent.Close();
                }
            }
            preComponent.Visible = logOut == true;
        }

        public void activeBtn(object sender)
        {
            btn_Clicked.BackColor = Color.FromArgb(192, 192, 255);
            Button thisBtn = sender as Button;
            btn_Clicked = thisBtn;
            thisBtn.BackColor = Color.White;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            activeBtn(sender);
            pnlView.Controls.Clear();
            pnlView.Controls.Add(new ucHome());
        }

        private void btnBookTicket_Click(object sender, EventArgs e)
        {
            activeBtn(sender);
            pnlView.Controls.Clear();
            pnlView.Controls.Add(new ucBookTicket(null, employee));
        }

        private void btnAdministration_Click(object sender, EventArgs e)
        {
            activeBtn(sender);
            pnlView.Controls.Clear();
            pnlView.Controls.Add(new ucAdministration(employee));
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            new fChangePassword(this, employee).Visible = true;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            logOut = true;
            Close();
        }

        public void getNewPassword(Employee emp)
        {
            employee = emp;
        }

        private void btnSellFood_Click(object sender, EventArgs e)
        {
            activeBtn(sender);
            pnlView.Controls.Clear();
            pnlView.Controls.Add(new ucBill(employee));
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            activeBtn(sender);
            pnlView.Controls.Clear();
            pnlView.Controls.Add(new ucReportStatistic());
        }
    }
}
