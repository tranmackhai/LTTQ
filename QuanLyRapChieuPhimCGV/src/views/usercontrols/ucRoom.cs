using QuanLyRapChieuPhimCGV.src.DAO;
using QuanLyRapChieuPhimCGV.src.models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace QuanLyRapChieuPhimCGV.src.views.usercontrols
{
    public partial class ucRoom : UserControl
    {
        private ucAdministration admin;
        private Employee employee;
        private const string ADD = "Đang thêm";
        private const string EDIT = "Đang sửa";
        private string action = "";
        private DAO_Room dao_r = new DAO_Room();
        private List<Room> rooms = new List<Room>();
        public ucRoom(ucAdministration uc, Employee e)
        {
            admin = uc;
            employee = e;
            InitializeComponent();
            Dock = DockStyle.Fill;

            rooms = dao_r.getAll();

            rooms.ForEach(room =>
            {
                addToDataGridView(room);
                cbId.Items.Add(room.id);
            });

            setEnabled(false);
            cbId.Enabled = false;
        }

        public void addToDataGridView(Room room)//Thêm 1 hàng vào datagridview
        {
            dgvRoom.Rows.Add(new object[]
            {
                room.id,
                room.name,
                room.totalRows,
                room.totalColumns
            });
        }

        public void setEnabled(bool status)
        {
            txtName.Enabled = status;
            txtTotalRows.Enabled = status;
            txtTotalColumns.Enabled = status;
        }

        public void resetTextBox()
        {
            txtName.Text = txtTotalColumns.Text = txtTotalRows.Text = "";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            resetTextBox();
            if (action != ADD)
            {
                action = ADD;
                cbId.Text = dao_r.generateId();//Tạo id mới
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
                error += "Chưa chọn mã phòng";
            }
            if (txtName.Text == "")
            {
                error += "Tên phòng không được để trống\n";
            }
            try
            {
                int totalRows = Convert.ToInt32(txtTotalRows.Text);
            }catch(Exception ex)
            {
                error += "Tổng số hàng không hợp lệ\n";
                Console.WriteLine(ex);
            }
            try
            {
                int totalColumns = Convert.ToInt32(txtTotalColumns.Text);
            }
            catch (Exception ex)
            {
                error += "Tổng số cột không hợp lệ\n";
                Console.WriteLine(ex);
            }
            return error;
        }

        public Room getData()//Lấy thông tin phòng chiếu từ giao diện
        {
            string error = validate();
            if (error == "")
            {
                Room room = new Room();
                room.id = cbId.Text;
                room.name = txtName.Text;
                room.totalRows = Convert.ToInt32(txtTotalRows.Text);
                room.totalColumns = Convert.ToInt32(txtTotalColumns.Text);
                return room;
            }
            else
            {
                MessageBox.Show(error, "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public void addEmployee(Room room)
        {
            dao_r.insertOne(room);
            rooms.Add(room);
            cbId.Items.Add(room.id);
            cbId.Text = dao_r.generateId();
        }
        public void setData(Room room)
        {
            txtName.Text = room.name;
            txtTotalRows.Text = ""+room.totalRows;
            txtTotalColumns.Text = ""+room.totalColumns;
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
        public void updateDataGridView(Room room)//Cập nhật 1 hàng datagridview
        {
            for (int i = 0; i < dgvRoom.RowCount; i++)
            {
                if (dgvRoom.Rows[i].Cells[0].Value.ToString() == room.id)
                {
                    dgvRoom.Rows[i].Cells[1].Value = room.name;
                    dgvRoom.Rows[i].Cells[2].Value = room.totalRows;
                    dgvRoom.Rows[i].Cells[3].Value = room.totalColumns;
                }
            }
        }

        //Cập nhật phòng chiếu trong CSDL
        //Tìm vị trí phòng chiếu vừa cập nhật
        //Sửa datagridview
        public void editEmployee(Room room)
        {
            if (dao_r.isAllowedUpdate(room))
            {
                dao_r.updateOne(room);
                int index = rooms.FindIndex(emp => emp.id == room.id);
                if (index != -1)
                {
                    rooms[index] = room;
                    updateDataGridView(room);
                }
            }
            else
            {
                MessageBox.Show("Thay đổi tổng số hàng và cột sẽ thừa ghế", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Room room = getData();
            if (room != null)
            {
                if (action == ADD)
                {
                    addEmployee(room);
                    addToDataGridView(room);
                }
                if (action == EDIT)
                {
                    editEmployee(room);
                }
                resetTextBox();
            }
        }

        private void cbId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbId.SelectedIndex != -1)
            {
                Room room = rooms.Find(emp => emp.id == cbId.Text);
                if (room != null)
                {
                    setData(room);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = dgvRoom.SelectedRows.Count - 1; i >= 0; i--)
            {
                int index = dgvRoom.SelectedRows[i].Index;
                if (index != -1)
                {
                    DialogResult answer = MessageBox.Show($"Bạn có chắc chắn xoá <{dgvRoom.Rows[index].Cells[1].Value}> ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer == DialogResult.Yes)
                    {
                        dao_r.deleteById(dgvRoom.Rows[index].Cells[0].Value.ToString());
                        int index_room = rooms.FindIndex(emp => emp.id == dgvRoom.Rows[index].Cells[0].Value.ToString());
                        if (index_room != -1)
                        {
                            cbId.Items.Remove(rooms[index_room].id);
                            rooms.RemoveAt(index_room);
                            dgvRoom.Rows.RemoveAt(index);
                        }
                    }
                }
            }
        }

        private void dgvRoom_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
            if (action == EDIT)
            {
                int index = e.RowIndex;
                if (index != -1)
                {
                    Room room = rooms.Find(emp => emp.id == dgvRoom.Rows[index].Cells[0].Value.ToString());
                    if (room != null)
                    {
                        cbId.Text = dgvRoom.Rows[index].Cells[0].Value.ToString();
                        setData(room);
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            resetTextBox();
        }

        private void cbId_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
