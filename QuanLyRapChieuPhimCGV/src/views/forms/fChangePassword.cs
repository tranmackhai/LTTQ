using QuanLyRapChieuPhimCGV.src.DAO;
using QuanLyRapChieuPhimCGV.src.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyRapChieuPhimCGV.src.views.forms
{
    public partial class fChangePassword : Form
    {
        private DAO_Employee dao_e = new DAO_Employee();
        private Employee employee;
        private fHome preComponent;
        public fChangePassword(fHome f, Employee emp)
        {
            preComponent = f;
            employee = emp; //Lấy thông tin nhân viên được truyền từ form trước
            InitializeComponent();
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            string error = dao_e.changePassword(employee, txtOldPassword.Text, txtNewPassword.Text, txtConfirmNewPassword.Text);
            if(error == "")
            {
                MessageBox.Show("Đổi mật khẩu thành công", "Thành công");
                employee.password = txtNewPassword.Text;
                preComponent.getNewPassword(employee);
            }
            else
            {
                MessageBox.Show(error, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
