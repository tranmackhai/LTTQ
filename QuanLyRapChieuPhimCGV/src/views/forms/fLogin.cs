using QuanLyRapChieuPhimCGV.src.DAO;
using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.views.forms;
using System;
using System.Windows.Forms;

namespace QuanLyRapChieuPhimCGV
{
    public partial class fLogin : Form
    {
        private DAO_Employee dao_e = new DAO_Employee();
        public fLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            login();
        }

        private void txtId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                login();
            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)
            {
                login();
            }
        }

        public void login()
        {
            string username = txtId.Text;
            string password = txtPassword.Text;

            //username = "210001";
            //password = "123456";

            string error = dao_e.login(username, password);
            if(error == "")
            {
                txtId.Text = txtPassword.Text = "";
                new fHome(this, dao_e.getById(username)).Visible = true;
                Visible = false;
            }
            else
            {
                MessageBox.Show(error, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
