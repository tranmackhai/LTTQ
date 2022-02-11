using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QuanLyRapChieuPhimCGV.src.DAO;
using QuanLyRapChieuPhimCGV.src.models;

namespace QuanLyRapChieuPhimCGV.src.views.usercontrols
{
    public partial class ucCustomer : UserControl
    {
        private ucAdministration admin;
        private Employee employee;
        private const string ADD = "Đang thêm";
        private const string EDIT = "Đang sửa";
        private string action = "";
        private DAO_Card dao_c = new DAO_Card();
        private DAO_Customer dao_cus = new DAO_Customer();
        private List<Card> cards = new List<Card>();
        private List<Customer> customers = new List<Customer>();
        public ucCustomer(ucAdministration uc, Employee e)
        {
            admin = uc;
            employee = e;
            InitializeComponent();
            Dock = DockStyle.Fill;

            cards = dao_c.getAll();

            cards.ForEach(card =>
            {
                cbCard.Items.Add(card.name);
            });

            customers = dao_cus.getAll();

            customers.ForEach(customer =>
            {
                cbId.Items.Add(customer.id);
                addToDataGridView(customer);
            });

            setEnabled(false);
            cbId.Enabled = false;
        }

        public void addToDataGridView(Customer customer)
        {
            dgvCustomer.Rows.Add(new object[]
            {
                customer.id,
                customer.phone,
                customer.email,
                customer.dayOfBirth.ToString("dd-MM-yyyy"),
                customer.gender,
                customer.card.name,
                customer.totalPoint
            });
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
            
            
            if (cbId.Text == "")
            {
                error += "Chưa chọn mã thẻ\n";
            }
            if (IsValidEmail(txtEmail.Text) == false)
            {
                error += "Email không hợp lệ\n";
            }
            else if (action == ADD && dao_cus.getByEmail(txtEmail.Text) != null)
            {
                error += "Email đã tồn tại\n";
            }
            else if (action == EDIT && cbId.Text != "" && customers.Find(cus => cus.id != cbId.Text && cus.email == txtEmail.Text) != null)
            {
                error += "Số điện thoại đã tồn tại\n";
            }
            if (cbCard.Text == "")
            {
                error += "Chưa chọn thẻ\n";
            }
            if(txtPhone.Text.Length != 10)
            {
                error += "Số điện thoại có 10 chữ số\n";
            }
            else
            {
                try
                {
                    decimal phone = Convert.ToDecimal(txtPhone.Text);
                    if(action == ADD && dao_cus.getByPhone(txtPhone.Text) != null)
                    {
                        error += "Số điện thoại đã tồn tại\n";
                    }
                    if(action == EDIT && cbId.Text!="" && customers.Find(cus=>cus.id != cbId.Text &&cus.phone == txtPhone.Text) != null)
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
            if(age < 18)
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
                customer.id = cbId.Text;
                customer.phone = txtPhone.Text;
                customer.email = txtEmail.Text;
                customer.dayOfBirth = dateTime.Value;
                customer.gender = rBtnFemale.Checked == true ? "Nữ" : "Nam";
                customer.card = cards.Find(card => card.name == cbCard.Text);
                return customer;
            }
            else
            {
                MessageBox.Show(error, "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        public void setEnabled(bool status)
        {
            txtEmail.Enabled = txtPhone.Enabled = cbCard.Enabled = dateTime.Enabled = rBtnFemale.Enabled = rBtnMale.Enabled = status;
        }

        public void resetTextBox()
        {
            txtPhone.Text = txtEmail.Text = "";
        }

        public void addCustomer(Customer customer)
        {
            dao_cus.insertOne(customer);
            customers.Add(customer);
            cbId.Items.Add(customer.id);
            cbId.Text = dao_cus.generateId();
        }
        public void setData(Customer customer)
        {
            txtEmail.Text = customer.email;
            txtPhone.Text = customer.phone;
            dateTime.Value = customer.dayOfBirth;
            rBtnFemale.Checked = customer.gender == "Nữ";
            rBtnMale.Checked = customer.gender == "Nam";
            cbCard.Text = customer.card.name;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            resetTextBox();
            if (action != ADD)
            {
                action = ADD;
                cbId.Text = dao_cus.generateId();//Tạo id mới
            }
            else
            {
                action = "";
                cbId.Text = "";
            }
            cbId.Enabled = false;
            setEnabled(action == ADD);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            resetTextBox();
            if (action != EDIT)
            {
                action = EDIT;
            }
            else
            {
                action = "";
            }
            cbId.Text = "";
            cbId.Enabled = (action == EDIT);
            setEnabled(action == EDIT);
        } 
        public void editCustomer(Customer customer)
        {
            dao_cus.updateOne(customer);
            int index = customers.FindIndex(cus => cus.id == customer.id);
            if (index != -1)
            {
                customers[index] = customer;
                updateDataGridView(customer);
            }
        }
        public void updateDataGridView(Customer customer)//Cập nhật 1 hàng datagridview
        {
            for (int i = 0; i < dgvCustomer.RowCount; i++)
            {
                if (dgvCustomer.Rows[i].Cells[0].Value.ToString() == customer.id)
                {
                    dgvCustomer.Rows[i].Cells[1].Value = customer.phone;
                    dgvCustomer.Rows[i].Cells[2].Value = customer.email;
                    dgvCustomer.Rows[i].Cells[3].Value = customer.dayOfBirth.ToString("dd-MM-yyyy");
                    dgvCustomer.Rows[i].Cells[4].Value = customer.gender;
                    dgvCustomer.Rows[i].Cells[5].Value = customer.card.name;
                    dgvCustomer.Rows[i].Cells[6].Value = 0;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = dgvCustomer.SelectedRows.Count - 1; i >= 0; i--)
            {
                int index = dgvCustomer.SelectedRows[i].Index;
                if (index != -1)
                {
                    DialogResult answer = MessageBox.Show($"Bạn có chắc chắn xoá <{dgvCustomer.Rows[index].Cells[0].Value} - {dgvCustomer.Rows[index].Cells[1].Value}> ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer == DialogResult.Yes)
                    {
                        dao_cus.deleteById(dgvCustomer.Rows[index].Cells[0].Value.ToString());
                        int index_customer = customers.FindIndex(cus => cus.id == dgvCustomer.Rows[index].Cells[0].Value.ToString());
                        if (index_customer != -1)
                        {
                            customers.RemoveAt(index_customer);
                            cbId.Items.RemoveAt(index_customer);
                            dgvCustomer.Rows.RemoveAt(index);
                        }
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            resetTextBox();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Customer customer = getData();
            if (customer != null)
            {
                if (action == ADD)
                {
                    addCustomer(customer);
                    addToDataGridView(customer);
                }
                if (action == EDIT)
                {
                    editCustomer(customer);
                }
                resetTextBox();
            }
        }

        private void cbId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbId.SelectedIndex != -1)
            {
                Customer customer = customers.Find(cus => cus.id == cbId.Text);
                if (customer != null)
                {
                    setData(customer);
                }
            }
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (action == EDIT)
            {
                Customer customer = customers.Find(cus => cus.id == dgvCustomer.Rows[index].Cells[0].Value.ToString());
                if (customer != null)
                {
                    cbId.Text = dgvCustomer.Rows[index].Cells[0].Value.ToString();
                    setData(customer);
                }
            }
        }

        private void cbCard_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cbId_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
