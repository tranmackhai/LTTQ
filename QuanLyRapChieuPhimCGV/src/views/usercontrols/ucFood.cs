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
    public partial class ucFood : UserControl
    {
        private ucAdministration admin;
        private Employee employee;
        private const string ADD = "Đang thêm";
        private const string EDIT = "Đang sửa";
        private string action = "";
        private DAO_Food dao_f = new DAO_Food();
        private DAO_GroupFood dao_g = new DAO_GroupFood();
        private List<Food> foods = new List<Food>();
        public ucFood(ucAdministration uc, Employee e)
        {
            admin = uc;
            employee = e;
            InitializeComponent();
            Dock = DockStyle.Fill;

            dao_g.getAll().ForEach(screen =>
            {
                cbGroup.Items.Add(screen.name);
            });

            foods = dao_f.getAll();

            foods.ForEach(food =>
            {
                addToDataGridView(food);
                cbId.Items.Add(food.id);
            });

            setEnabled(false);
            cbId.Enabled = false;
        }

        public void addToDataGridView(Food food)//Thêm 1 hàng vào datagridview
        {
            dgvFood.Rows.Add(new object[]
            {
                food.id,
                food.name,
                food.price,
                food.groupFood.name,
            });
        }

        public void setEnabled(bool status)
        {
            txtName.Enabled = status;
            numPrice.Enabled = status;
            cbGroup.Enabled = status;
        }

        public void resetTextBox()
        {
            txtName.Text = "";
            numPrice.Value = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            resetTextBox();
            if (action != ADD)
            {
                action = ADD;
                cbId.Text = dao_f.generateId();//Tạo id mới
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
                error += "Chưa chọn mã đồ ăn";
            }
            if (txtName.Text == "")
            {
                error += "Tên đồ ăn không được để trống\n";
            }
            if(numPrice.Value == 0)
            {
                error += "Giá bán phải lớn hơn 0\n";
            }
            if (cbGroup.Text == "")
            {
                error += "Chưa chọn nhóm đồ ăn";
            }
            return error;
        }

        public Food getData()//Lấy thông tin phòng chiếu từ giao diện
        {
            string error = validate();
            if (error == "")
            {
                Food food = new Food();
                food.id = cbId.Text;
                food.name = txtName.Text;
                food.price = numPrice.Value;
                food.groupFood = dao_g.getByName(cbGroup.Text);
                return food;
            }
            else
            {
                MessageBox.Show(error, "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public void addEmployee(Food food)
        {
            dao_f.insertOne(food);
            foods.Add(food);
            cbId.Items.Add(food.id);
            cbId.Text = dao_f.generateId();
        }
        public void setData(Food food)
        {
            txtName.Text = food.name;
            numPrice.Value = food.price;
            cbGroup.Text = food.groupFood.name;
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
        public void updateDataGridView(Food food)//Cập nhật 1 hàng datagridview
        {
            for (int i = 0; i < dgvFood.RowCount; i++)
            {
                if (dgvFood.Rows[i].Cells[0].Value.ToString() == food.id)
                {
                    dgvFood.Rows[i].Cells[1].Value = food.name;
                    dgvFood.Rows[i].Cells[2].Value = food.price;
                    dgvFood.Rows[i].Cells[3].Value = food.groupFood.name;
                }
            }
        }

        //Cập nhật phòng chiếu trong CSDL
        //Tìm vị trí phòng chiếu vừa cập nhật
        //Sửa datagridview
        public void editEmployee(Food food)
        {
            dao_f.updateOne(food);
            int index = foods.FindIndex(emp => emp.id == food.id);
            if (index != -1)
            {
                foods[index] = food;
                updateDataGridView(food);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Food food = getData();
            if (food != null)
            {
                if (action == ADD)
                {
                    addEmployee(food);
                    addToDataGridView(food);
                }
                if (action == EDIT)
                {
                    editEmployee(food);
                }
                resetTextBox();
            }
        }

        private void cbId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbId.SelectedIndex != -1)
            {
                Food food = foods.Find(emp => emp.id == cbId.Text);
                if (food != null)
                {
                    setData(food);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = dgvFood.SelectedRows.Count - 1; i >= 0; i--)
            {
                int index = dgvFood.SelectedRows[i].Index;
                if (index != -1)
                {
                    DialogResult answer = MessageBox.Show($"Bạn có chắc chắn xoá <{dgvFood.Rows[index].Cells[1].Value}> ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer == DialogResult.Yes)
                    {
                        dao_f.deleteById(dgvFood.Rows[index].Cells[0].Value.ToString());
                        int index_food = foods.FindIndex(emp => emp.id == dgvFood.Rows[index].Cells[0].Value.ToString());
                        if (index_food != -1)
                        {
                            cbId.Items.Remove(foods[index_food].id);
                            foods.RemoveAt(index_food);
                            dgvFood.Rows.RemoveAt(index);
                        }
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            resetTextBox();
        }

        private void cbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = cbId.Text;
            cbId.Items.Clear();
            if (cbGroup.SelectedIndex != -1)
            {
                List<Food> newFoods = foods.FindAll(r => r.groupFood.name == cbGroup.Text);
                newFoods.ForEach(ro =>
                {
                    cbId.Items.Add(ro.id);
                });
            }
            cbId.Text = id;
        }

        private void dgvFood_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (action == EDIT)
            {
                Food food = foods.Find(emp => emp.id == dgvFood.Rows[index].Cells[0].Value.ToString());
                if (food != null)
                {
                    cbId.Text = dgvFood.Rows[index].Cells[0].Value.ToString();
                    setData(food);
                }
            }
        }
    }
}
