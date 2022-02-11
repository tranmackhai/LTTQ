using QuanLyRapChieuPhimCGV.src.DAO;
using QuanLyRapChieuPhimCGV.src.models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuanLyRapChieuPhimCGV.src.views.usercontrols
{
    public partial class ucEmployee : UserControl
    {
        private ucAdministration admin;
        private Employee employee;
        private const string ADD = "Đang thêm"; 
        private const string EDIT = "Đang sửa";
        private string action = "";
        private DAO_Employee dao_e = new DAO_Employee();
        private List<Employee> employees = new List<Employee>();
        public ucEmployee(ucAdministration uc, Employee e)
        {
            admin = uc;
            employee = e;
            InitializeComponent();
            Dock = DockStyle.Fill;
            
            employees = dao_e.getAll();

            employees.ForEach(employee =>
            {
                addToDataGridView(employee);
                cbId.Items.Add(employee.id);
            });

            setEnabled(false);
            cbId.Enabled = false;
        }

        public void addToDataGridView(Employee employee)//Thêm 1 hàng vào datagridview
        {
            dgvEmployee.Rows.Add(new object[]
            {
                employee.id, 
                employee.name, 
                employee.person_id, 
                employee.gender, 
                employee.position, 
                employee.phone, 
                employee.salary, 
                employee.password, 
                (employee.permission == 0)?"USER_TICKET":((employee.permission == 1) ? "USER_FOOD":"ADMIN")
        });
        }

        public void setEnabled(bool status)
        {
            txtName.Enabled = status;
            txtPhone.Enabled = status;
            numSalary.Enabled = status;
            cbPosition.Enabled = status;
            cbPermision.Enabled = status;
            rBtnFemale.Enabled = rBtnMale.Enabled = status;
            txtPersonID.Enabled = status;
            txtPassword.Enabled = status;
        }

        public void resetTextBox()
        {
            txtName.Text = "";
            txtPhone.Text = "";
            numSalary.Value = 0;
            txtPersonID.Text = ""; 
            txtPassword.Text = "";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            resetTextBox();
            if(action != ADD)
            {
                action = ADD;
                cbId.Text = dao_e.generateId();//Tạo id mới
            }
            else
            {
                action = "";
                cbId.Text = "";
            }
            cbId.Enabled = false;
            setEnabled(action == ADD);
        }

        public string validate()
        {
            string error = "";

            if(cbId.Text == "")
            {
                error += "Chưa chọn mã nhân viên";
            }
            if(txtName.Text == "")
            {
                error += "Tên nhân viên không được để trống\n";
            }
            if (txtPersonID.Text.Length != 9 && txtPersonID.Text.Length != 12)
            {
                error += "Số CMND có 9 số, CCCD có 12 số\n";
            }
            else
            {
                try
                {
                    decimal phone = Convert.ToDecimal(txtPersonID.Text);
                    if(action == ADD && employees.Find(em=>em.person_id == txtPersonID.Text) != null)
                    {
                        error += "Không được trùng số CMND/CCCD\n";
                    }
                    if (action == EDIT && cbId.Text != "" && employees.Find(em => em.id != cbId.Text && em.person_id == txtPersonID.Text) != null)
                    {
                        error += "Không được trùng số CMND/CCCD\n";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    error += "Số CMND/CCCD không hợp lệ\n";
                }
            }
            if (txtPhone.Text.Length != 10)
            {
                error += "Số điện thoại phải có 10 chữ số\n";
            }
            else
            {
                try
                {
                    decimal phone = Convert.ToDecimal(txtPhone.Text);

                    if (action == ADD && dao_e.getByPhone(txtPhone.Text) != null)
                    {
                        error += "Số điện thoại đã tồn tại\n";
                    }
                    if(action == EDIT && cbId.Text!=null && employees.Find(employee=>employee.id != cbId.Text && employee.phone == txtPhone.Text)!=null)
                    {
                        error += "Số điện thoại đã tồn tại\n";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    error += "Số điện thoại không hợp lệ\n";
                }
            }

            if (numSalary.Value == 0)
            {
                error += "Lương không được bằng 0\n";
            }

            if(cbPermision.Text == "")
            {
                error += "Chưa chọn quyền\n";
            }

            if (cbPosition.Text == "")
            {
                error += "Chưa chọn vị trí\n";
            }

            return error;
        }

        public Employee getData()//Lấy thông tin nhân viên từ giao diện
        {
            string error = validate();
            if(error == "")
            {
                Employee employee = new Employee();
                employee.id = cbId.Text;
                employee.name = txtName.Text;
                employee.person_id = txtPersonID.Text;
                employee.phone = txtPhone.Text;
                employee.salary = numSalary.Value;
                employee.permission = cbPermision.SelectedIndex;
                employee.gender = (rBtnMale.Checked == true) ? "Nam" : "Nữ";
                employee.password = txtPassword.Text;
                employee.position = cbPosition.Text;
                return employee;
            }
            else
            {
                MessageBox.Show(error, "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public void addEmployee(Employee employee)
        {
            dao_e.insertOne(employee);
            employees.Add(employee);
            cbId.Items.Add(employee.id);
            cbId.Text = dao_e.generateId();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Employee employee = getData();
            if (employee != null)
            {
                if(action == ADD)
                {
                    addEmployee(employee);
                    addToDataGridView(employee);
                }
                if(action == EDIT)
                {
                    editEmployee(employee);
                }
                resetTextBox();
            }
        }
        public void updateDataGridView(Employee employee)//Cập nhật 1 hàng datagridview
        {
            for(int i = 0; i < dgvEmployee.RowCount; i++)
            {
                if(dgvEmployee.Rows[i].Cells[0].Value.ToString() == employee.id)
                {
                    dgvEmployee.Rows[i].Cells[1].Value = employee.name;
                    dgvEmployee.Rows[i].Cells[2].Value = employee.person_id;
                    dgvEmployee.Rows[i].Cells[3].Value = employee.gender;
                    dgvEmployee.Rows[i].Cells[4].Value = employee.position;
                    dgvEmployee.Rows[i].Cells[5].Value = employee.phone;
                    dgvEmployee.Rows[i].Cells[6].Value = employee.salary;
                    dgvEmployee.Rows[i].Cells[7].Value = employee.password;
                    dgvEmployee.Rows[i].Cells[8].Value = (employee.permission == 0)?"USER_TICKET":((employee.permission == 1) ? "USER_FOOD":"ADMIN");
                }
            }
        }

        //Cập nhật nhân viên trong CSDL
        //Tìm vị trí nhân viên vừa cập nhật
        //Sửa datagridview
        public void editEmployee(Employee employee)
        {
            dao_e.updateOne(employee);
            int index = employees.FindIndex(emp => emp.id == employee.id);
            if(index != -1)
            {
                employees[index] = employee;
                updateDataGridView(employee);
            }
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

        //Hiển trị dữ liệu nhân viên lên giao diện
        public void setData(Employee employee)
        {
            txtName.Text = employee.name;
            txtPersonID.Text = employee.person_id;
            txtPhone.Text = employee.phone;
            numSalary.Value = employee.salary;
            cbPermision.SelectedIndex = employee.permission;
            rBtnFemale.Checked = employee.gender == "Nữ";
            rBtnMale.Checked = employee.gender == "Nam";
            txtPassword.Text = employee.password;
            cbPosition.Text = employee.position;
        }

        private void cbId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbId.SelectedIndex != -1)
            {
                Employee employee = employees.Find(emp => emp.id == cbId.Text);
                if(employee != null)
                {
                    setData(employee);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for(int i = dgvEmployee.SelectedRows.Count - 1;i >= 0; i--)
            {
                int index = dgvEmployee.SelectedRows[i].Index;
                if(index != -1)
                {
                    DialogResult answer = MessageBox.Show($"Bạn có chắc chắn xoá <{dgvEmployee.Rows[index].Cells[1].Value}> ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer == DialogResult.Yes)
                    {
                        dao_e.deleteById(dgvEmployee.Rows[index].Cells[0].Value.ToString());
                        int index_employee = employees.FindIndex(emp => emp.id == dgvEmployee.Rows[index].Cells[0].Value.ToString());
                        if (index_employee != -1)
                        {
                            employees.RemoveAt(index_employee);
                            cbId.Items.RemoveAt(index_employee);
                            dgvEmployee.Rows.RemoveAt(index);
                        }
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            resetTextBox();
        }

        private void dgvEmployee_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (action == EDIT)
            {
                Employee employee = employees.Find(emp => emp.id == dgvEmployee.Rows[index].Cells[0].Value.ToString());
                if (employee != null)
                {
                    cbId.Text = dgvEmployee.Rows[index].Cells[0].Value.ToString();
                    setData(employee);
                }
            }
        }

        private void cbId_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cbPermision_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cbPosition_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
