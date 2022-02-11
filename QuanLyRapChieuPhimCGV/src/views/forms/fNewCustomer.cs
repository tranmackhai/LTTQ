using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QuanLyRapChieuPhimCGV.src.DAO;
using QuanLyRapChieuPhimCGV.src.models;

namespace QuanLyRapChieuPhimCGV.src.views.forms
{
    public partial class fNewCustomer : Form
    {
        private fSelectCustomer preComponent;
        private DAO_Customer dao_cus = new DAO_Customer();
        private DAO_Card dao_c = new DAO_Card();
        public fNewCustomer(fSelectCustomer f)
        {
            preComponent = f;
            InitializeComponent();
            txtId.Text = dao_cus.generateId();
            dao_c.getAll().ForEach(card =>
            {
                cbCard.Items.Add(card.name);
            });
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; 
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
        public string validate()
        {
            string error = "";

            DateTime today = DateTime.Today;
            int age = today.Year - dateTime.Value.Year;
            if (dateTime.Value > today.AddYears(-age))
                age--;
            if (IsValidEmail(txtEmail.Text) == false)
            {
                error += "Email không hợp lệ\n";
            }
            else if (dao_cus.getByEmail(txtEmail.Text) != null)
            {
                error += "Email đã tồn tại\n";
            }
            else if (dao_cus.getAll().Find(cus => cus.email == txtEmail.Text) != null)
            {
                error += "Số điện thoại đã tồn tại\n";
            }
            if (cbCard.Text == "")
            {
                error += "Chưa chọn thẻ\n";
            }
            if (txtPhone.Text.Length != 10)
            {
                error += "Số điện thoại có 10 chữ số\n";
            }
            else
            {
                try
                {
                    decimal phone = Convert.ToDecimal(txtPhone.Text);
                    if (dao_cus.getByPhone(txtPhone.Text) != null)
                    {
                        error += "Số điện thoại đã tồn tại\n";
                    }
                    if (dao_cus.getAll().Find(cus => cus.phone == txtPhone.Text) != null)
                    {
                        error += "Số điện thoại đã tồn tại\n";
                    }
                }
                catch (Exception ex)
                {
                    error += "Số điện thoại không hợp lệ\n";
                    Console.WriteLine(ex);
                }
            }
            if (age < 18)
            {
                error += "Tuổi không được nhỏ hơn 18\n";
            }
            return error;
        }
        public Customer getData()//Lấy thông tin nhân viên từ giao diện
        {
            string error = validate();
            if (error == "")
            {
                Customer customer = new Customer();
                customer.id = txtId.Text;
                customer.phone = txtPhone.Text;
                customer.email = txtEmail.Text;
                customer.dayOfBirth = dateTime.Value;
                customer.gender = rBtnFemale.Checked == true ? "Nữ" : "Nam";
                customer.card = dao_c.getAll().Find(card => card.name == cbCard.Text);
                return customer;
            }
            else
            {
                MessageBox.Show(error, "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Customer customer = getData();
            if (customer != null)
            {
                preComponent.getNewCustomer(customer);
                Close();
            }
        }
    }
}
