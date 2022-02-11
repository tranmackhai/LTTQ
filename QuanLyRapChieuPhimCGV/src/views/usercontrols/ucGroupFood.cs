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
    public partial class ucGroupFood : UserControl
    {
        private ucAdministration admin;
        private Employee employee;
        private const string ADD = "Đang thêm";
        private const string EDIT = "Đang sửa";
        private string action = "";
        private DAO_GroupFood dao_g = new DAO_GroupFood();
        private List<GroupFood> groupFoods = new List<GroupFood>();
        public ucGroupFood(ucAdministration uc, Employee e)
        {
            admin = uc;
            employee = e;
            InitializeComponent();
            Dock = DockStyle.Fill;

            groupFoods = dao_g.getAll();

            groupFoods.ForEach(room =>
            {
                addToDataGridView(room);
                cbId.Items.Add(room.id);
            });

            setEnabled(false);
            cbId.Enabled = false;
        }

        public void addToDataGridView(GroupFood groupFood)//Thêm 1 hàng vào datagridview
        {
            dgvGroupFood.Rows.Add(new object[]
            {
                groupFood.id,
                groupFood.name
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
                cbId.Text = dao_g.generateId();//Tạo id mới
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
                error += "Chưa chọn mã nhóm";
            }
            if (txtName.Text == "")
            {
                error += "Tên nhóm không được để trống\n";
            }
            return error;
        }

        public GroupFood getData()//Lấy thông tin nhân viên từ giao diện
        {
            string error = validate();
            if (error == "")
            {
                GroupFood groupFood = new GroupFood();
                groupFood.id = cbId.Text;
                groupFood.name = txtName.Text;
                return groupFood;
            }
            else
            {
                MessageBox.Show(error, "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public void addGroupFood(GroupFood groupFood)
        {
            dao_g.insertOne(groupFood);
            groupFoods.Add(groupFood);
            cbId.Items.Add(groupFood.id);
            cbId.Text = dao_g.generateId();
        }
        public void setData(GroupFood groupFood)
        {
            txtName.Text = groupFood.name;
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
        public void updateDataGridView(GroupFood groupFood)//Cập nhật 1 hàng datagridview
        {
            for (int i = 0; i < dgvGroupFood.RowCount; i++)
            {
                if (dgvGroupFood.Rows[i].Cells[0].Value.ToString() == groupFood.id)
                {
                    dgvGroupFood.Rows[i].Cells[1].Value = groupFood.name;
                }
            }
        }

        //Cập nhật nhân viên trong CSDL
        //Tìm vị trí nhân viên vừa cập nhật
        //Sửa datagridview
        public void editGroupFood(GroupFood groupFood)
        {
            dao_g.updateOne(groupFood);
            int index = groupFoods.FindIndex(gf => gf.id == groupFood.id);
            if (index != -1)
            {
                groupFoods[index] = groupFood;
                updateDataGridView(groupFood);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            GroupFood groupFood = getData();
            if (groupFood != null)
            {
                if (action == ADD)
                {
                    addGroupFood(groupFood);
                    addToDataGridView(groupFood);
                }
                if (action == EDIT)
                {
                    editGroupFood(groupFood);
                }
                resetTextBox();
            }
        }

        private void cbId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbId.SelectedIndex != -1)
            {
                GroupFood groupFood = groupFoods.Find(gf => gf.id == cbId.Text);
                if (groupFood != null)
                {
                    setData(groupFood);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = dgvGroupFood.SelectedRows.Count - 1; i >= 0; i--)
            {
                int index = dgvGroupFood.SelectedRows[i].Index;
                if (index != -1)
                {
                    DialogResult answer = MessageBox.Show($"Bạn có chắc chắn xoá <{dgvGroupFood.Rows[index].Cells[1].Value}> ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer == DialogResult.Yes)
                    {
                        dao_g.deleteById(dgvGroupFood.Rows[index].Cells[0].Value.ToString());
                        int index_groupFood = groupFoods.FindIndex(gf => gf.id == dgvGroupFood.Rows[index].Cells[0].Value.ToString());
                        if (index_groupFood != -1)
                        {
                            groupFoods.RemoveAt(index_groupFood);
                            cbId.Items.RemoveAt(index_groupFood);
                            dgvGroupFood.Rows.RemoveAt(index);
                        }
                    }
                }
            }
        }

        private void dgvGroupFood_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (action == EDIT)
            {
                GroupFood groupFood = groupFoods.Find(gf => gf.id == dgvGroupFood.Rows[index].Cells[0].Value.ToString());
                if (groupFood != null)
                {
                    cbId.Text = dgvGroupFood.Rows[index].Cells[0].Value.ToString();
                    setData(groupFood);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            resetTextBox();
        }
    }
}
