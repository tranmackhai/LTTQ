using QuanLyRapChieuPhimCGV.src.DAO;
using QuanLyRapChieuPhimCGV.src.models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuanLyRapChieuPhimCGV.src.views.usercontrols
{
    public partial class ucScreen : UserControl
    {
        private ucAdministration admin;
        private Employee employee;
        private const string ADD = "Đang thêm";
        private const string EDIT = "Đang sửa";
        private string action = "";
        private DAO_Screen dao_s = new DAO_Screen();
        private List<_Screen> screens = new List<_Screen>();
        public ucScreen(ucAdministration uc, Employee e)
        {
            admin = uc;
            employee = e;
            InitializeComponent();
            Dock = DockStyle.Fill;

            screens = dao_s.getAll();

            screens.ForEach(screen =>
            {
                addToDataGridView(screen);
                cbId.Items.Add(screen.id);
            });

            setEnabled(false);
            cbId.Enabled = false;
        }

        public void addToDataGridView(_Screen screen)//Thêm 1 hàng vào datagridview
        {
            dgvScreen.Rows.Add(new object[]
            {
                screen.id,
                screen.name
            });
        }

        public void setEnabled(bool status)
        {
            txtName.Enabled = status;
        }

        public void resetTextBox()
        {
            txtName.Text = "";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            resetTextBox();
            if (action != ADD)
            {
                action = ADD;
                cbId.Text = dao_s.generateId();//Tạo id mới
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

            if (cbId.Text == "")
            {
                error += "Chưa chọn mã màn hình";
            }
            if (txtName.Text == "")
            {
                error += "Tên màn hình không được để trống\n";
            }
            return error;
        }

        public _Screen getData()//Lấy thông tin nhân viên từ giao diện
        {
            string error = validate();
            if (error == "")
            {
                _Screen screen = new _Screen();
                screen.id = cbId.Text;
                screen.name = txtName.Text;
                return screen;
            }
            else
            {
                MessageBox.Show(error, "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public void addEmployee(_Screen screen)
        {
            dao_s.insertOne(screen);
            screens.Add(screen);
            cbId.Items.Add(screen.id);
            cbId.Text = dao_s.generateId();
        }
        public void setData(_Screen screen)
        {
            txtName.Text = screen.name;
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
        public void updateDataGridView(_Screen screen)//Cập nhật 1 hàng datagridview
        {
            for (int i = 0; i < dgvScreen.RowCount; i++)
            {
                if (dgvScreen.Rows[i].Cells[0].Value.ToString() == screen.id)
                {
                    dgvScreen.Rows[i].Cells[1].Value = screen.name;
                }
            }
        }

        //Cập nhật nhân viên trong CSDL
        //Tìm vị trí nhân viên vừa cập nhật
        //Sửa datagridview
        public void editEmployee(_Screen screen)
        {
            dao_s.updateOne(screen);
            int index = screens.FindIndex(emp => emp.id == screen.id);
            if (index != -1)
            {
                screens[index] = screen;
                updateDataGridView(screen);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            _Screen screen = getData();
            if (screen != null)
            {
                if (action == ADD)
                {
                    addEmployee(screen);
                    addToDataGridView(screen);
                }
                if (action == EDIT)
                {
                    editEmployee(screen);
                }
                resetTextBox();
            }
        }

        private void cbId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbId.SelectedIndex != -1)
            {
                _Screen screen = screens.Find(emp => emp.id == cbId.Text);
                if (screen != null)
                {
                    setData(screen);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = dgvScreen.SelectedRows.Count - 1; i >= 0; i--)
            {
                int index = dgvScreen.SelectedRows[i].Index;
                if (index != -1)
                {
                    DialogResult answer = MessageBox.Show($"Bạn có chắc chắn xoá <{dgvScreen.Rows[index].Cells[1].Value}> ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer == DialogResult.Yes)
                    {
                        dao_s.deleteById(dgvScreen.Rows[index].Cells[0].Value.ToString());
                        int index_screen = screens.FindIndex(emp => emp.id == dgvScreen.Rows[index].Cells[0].Value.ToString());
                        if (index_screen != -1)
                        {
                            screens.RemoveAt(index_screen);
                            cbId.Items.RemoveAt(index_screen);
                            dgvScreen.Rows.RemoveAt(index);
                        }
                    }
                }
            }
        }

        private void dgvScreen_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (action == EDIT)
            {
                _Screen screen = screens.Find(emp => emp.id == dgvScreen.Rows[index].Cells[0].Value.ToString());
                if (screen != null)
                {
                    cbId.Text = dgvScreen.Rows[index].Cells[0].Value.ToString();
                    setData(screen);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            resetTextBox();
        }

        private void ucScreen_Load(object sender, EventArgs e)
        {

        }
    }
}
