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

namespace QuanLyRapChieuPhimCGV.src.views.usercontrols
{
    public partial class ucChairType : UserControl
    {
        private ucAdministration admin;
        private Employee employee;
        private const string ADD = "Đang thêm";
        private const string EDIT = "Đang sửa";
        private string action = "";
        private DAO_ChairType dao_ct = new DAO_ChairType();
        private List<ChairType> chairTypes = new List<ChairType>();
        public ucChairType(ucAdministration uc, Employee e)
        {
            admin = uc;
            employee = e;
            InitializeComponent();
            Dock = DockStyle.Fill;

            chairTypes = dao_ct.getAll();

            chairTypes.ForEach(chairType =>
            {
                addToDataGridView(chairType);
                cbId.Items.Add(chairType.id);
            });

            setEnabled(false);
            cbId.Enabled = false;
        }

        public void addToDataGridView(ChairType chairType)//Thêm 1 hàng vào datagridview
        {
            dgvChairType.Rows.Add(new object[]
            {
                chairType.id,
                chairType.name
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
                cbId.Text = dao_ct.generateId();//Tạo id mới
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
                error += "Chưa chọn mã loại";
            }
            if (txtName.Text == "")
            {
                error += "Tên loại không được để trống\n";
            }
            return error;
        }

        public ChairType getData()//Lấy thông tin nhân viên từ giao diện
        {
            string error = validate();
            if (error == "")
            {
                ChairType chairType = new ChairType();
                chairType.id = cbId.Text;
                chairType.name = txtName.Text;
                return chairType;
            }
            else
            {
                MessageBox.Show(error, "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public void addEmployee(ChairType chairType)
        {
            dao_ct.insertOne(chairType);
            chairTypes.Add(chairType);
            cbId.Items.Add(chairType.id);
            cbId.Text = dao_ct.generateId();
        }
        public void setData(ChairType chairType)
        {
            txtName.Text = chairType.name;
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
        public void updateDataGridView(ChairType chairType)//Cập nhật 1 hàng datagridview
        {
            for (int i = 0; i < dgvChairType.RowCount; i++)
            {
                if (dgvChairType.Rows[i].Cells[0].Value.ToString() == chairType.id)
                {
                    dgvChairType.Rows[i].Cells[1].Value = chairType.name;
                }
            }
        }

        //Cập nhật nhân viên trong CSDL
        //Tìm vị trí nhân viên vừa cập nhật
        //Sửa datagridview
        public void editEmployee(ChairType chairType)
        {
            dao_ct.updateOne(chairType);
            int index = chairTypes.FindIndex(emp => emp.id == chairType.id);
            if (index != -1)
            {
                chairTypes[index] = chairType;
                updateDataGridView(chairType);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            ChairType chairType = getData();
            if (chairType != null)
            {
                if (action == ADD)
                {
                    addEmployee(chairType);
                    addToDataGridView(chairType);
                }
                if (action == EDIT)
                {
                    editEmployee(chairType);
                }
                resetTextBox();
            }
        }

        private void cbId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbId.SelectedIndex != -1)
            {
                ChairType chairType = chairTypes.Find(emp => emp.id == cbId.Text);
                if (chairType != null)
                {
                    setData(chairType);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = dgvChairType.SelectedRows.Count - 1; i >= 0; i--)
            {
                int index = dgvChairType.SelectedRows[i].Index;
                if (index != -1)
                {
                    DialogResult answer = MessageBox.Show($"Bạn có chắc chắn xoá <{dgvChairType.Rows[index].Cells[1].Value}> ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer == DialogResult.Yes)
                    {
                        dao_ct.deleteById(dgvChairType.Rows[index].Cells[0].Value.ToString());
                        int index_chairType = chairTypes.FindIndex(emp => emp.id == dgvChairType.Rows[index].Cells[0].Value.ToString());
                        if (index_chairType != -1)
                        {
                            chairTypes.RemoveAt(index_chairType);
                            cbId.Items.RemoveAt(index_chairType);
                            dgvChairType.Rows.RemoveAt(index);
                        }
                    }
                }
            }
        }

        private void dgvChairType_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (action == EDIT)
            {
                ChairType chairType = chairTypes.Find(emp => emp.id == dgvChairType.Rows[index].Cells[0].Value.ToString());
                if (chairType != null)
                {
                    cbId.Text = dgvChairType.Rows[index].Cells[0].Value.ToString();
                    setData(chairType);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            resetTextBox();
        }
    }
}
