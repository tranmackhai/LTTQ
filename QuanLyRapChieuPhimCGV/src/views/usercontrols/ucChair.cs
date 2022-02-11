using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QuanLyRapChieuPhimCGV.src.DAO;
using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.views.forms;

namespace QuanLyRapChieuPhimCGV.src.views.usercontrols
{
    public partial class ucChair : UserControl
    {
        private ucAdministration admin;
        private Employee employee;
        private const string ADD = "Đang thêm";
        private const string EDIT = "Đang sửa";
        private string action = "";
        private DAO_Chair dao_ch = new DAO_Chair();
        private List<Chair> chairs = new List<Chair>();
        private DAO_ChairType dao_ct = new DAO_ChairType();
        private List<ChairType> chairTypes = new List<ChairType>();
        private DAO_Room dao_r = new DAO_Room();
        private List<Room> rooms = new List<Room>();

        public ucChair(ucAdministration uc, Employee e)
        {
            admin = uc;
            employee = e;
            InitializeComponent();
            Dock = DockStyle.Fill;

            rooms = dao_r.getAll();

            rooms.ForEach(room =>
            {
                cbRoom.Items.Add(room.name);
            });

            chairTypes = dao_ct.getAll();

            chairTypes.ForEach(chairType =>
            {
                cbType.Items.Add(chairType.name);
            });

            chairs = dao_ch.getAll();
            cbId.Items.Clear();

            chairs.ForEach(chair =>
            {
                cbId.Items.Add(chair.id);
                addToDGV(chair);
            });

            setEnabled(false);
            cbId.Enabled = false;
        }
        public void addToDGV(Chair chair)
        {
            dgvChair.Rows.Add(new object[]
            {
                chair.id,
                chair.row,
                chair.column,
                chair.type.name,
                chair.room.name
            });
        }
        public void setEnabled(bool status)
        {
            cbRoom.Enabled = cbType.Enabled = numRow.Enabled = numColumn.Enabled = status;
        }

        public void resetTextBox()
        {
            numRow.Value = numColumn.Value = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            resetTextBox();
            if(action != ADD)
            {
                action = ADD;
                cbId.Text = dao_ch.generateId();
            }
            else
            {
                action = "";
                cbId.Text = "";
            }
            setEnabled(action == ADD);
            cbId.Enabled = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            resetTextBox();
            if(action != EDIT)
            {
                action = EDIT;
            }
            else
            {
                action = "";
            }
            setEnabled(action == EDIT);
            cbId.Enabled = action == EDIT;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            resetTextBox();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for(int i = dgvChair.SelectedRows.Count - 1; i >= 0; i--)
            {
                int index = dgvChair.SelectedRows[i].Index;//Lấy vị trí hàng đang chọn thứ i
                if (index != -1)
                {
                    DialogResult answer = MessageBox.Show($"Bạn có chắc chắn xoá <{dgvChair.Rows[index].Cells[0].Value} - {dgvChair.Rows[index].Cells[1].Value} - {dgvChair.Rows[index].Cells[2].Value}> ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if(answer == DialogResult.Yes)
                    {
                        dao_ch.deleteById(dgvChair.Rows[index].Cells[0].Value.ToString());
                        int index_chair = chairs.FindIndex(chair => chair.id == dgvChair.Rows[index].Cells[0].Value.ToString());
                        if (index_chair != -1)
                        {
                            chairs.RemoveAt(index_chair);
                            if (cbId.Items.Contains(dgvChair.Rows[index].Cells[0].Value.ToString()))
                            {
                                cbId.Items.Remove(dgvChair.Rows[index].Cells[0].Value.ToString());
                            }
                            dgvChair.Rows.RemoveAt(index);
                        }
                    }
                }
            }
        }

        public string validate()
        {
            string error = "";

            if(cbRoom.Text == "")
            {
                error += "Chưa chọn rạp\n";
            }
            if(cbType.Text == "")
            {
                error += "Chưa chọn loại ghế\n";
            }
            if(cbId.Text == "")
            {
                error += "Chưa chọn mã ghế\n";
            }
            Room r = rooms.Find(room => room.name == cbRoom.Text);
            if (r != null)
            {
                int indexRow = Convert.ToInt32(numRow.Value);
                int indexColumn = Convert.ToInt32(numColumn.Value);

                if(indexRow > r.totalRows)
                {
                    error += $"Rạp này chỉ có {r.totalRows} hàng\n";
                }

                if (indexColumn > r.totalColumns)
                {
                    error += $"Rạp này chỉ có {r.totalColumns} cột\n";
                }
            }

            return error;
        }

        public Chair getData()
        {
            string error = validate();

            if(error == "")
            {
                Chair chair = new Chair()
                {
                    id = cbId.Text,
                    row = Convert.ToInt32(numRow.Value),
                    column = Convert.ToInt32(numColumn.Value),
                    room = rooms.Find(room => room.name == cbRoom.Text),
                    type = chairTypes.Find(ct=>ct.name == cbType.Text)
                };
                return chair;
            }
            else
            {
                MessageBox.Show(error, "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        public void addChair(Chair chair)
        {
            dao_ch.insertOne(chair);
            chairs.Add(chair);
            cbId.Items.Add(chair.id);
            addToDGV(chair);
            cbId.Text = dao_ch.generateId();
        }
        public void updateDGV(Chair chair)
        {
            for(int i = 0; i < dgvChair.RowCount; i++)
            {
                if(dgvChair.Rows[i].Cells[0].Value.ToString() == chair.id)
                {
                    dgvChair.Rows[i].Cells[1].Value = chair.row;
                    dgvChair.Rows[i].Cells[2].Value = chair.column;
                    dgvChair.Rows[i].Cells[3].Value = chair.type.name;
                    dgvChair.Rows[i].Cells[4].Value = chair.room.name;
                    break;
                }
            }
        }
        public void editChair(Chair chair)
        {
            dao_ch.updateOne(chair);
            int index = chairs.FindIndex(c => c.id == chair.id);
            if (index != -1)
            {
                chairs[index] = chair;
            }
            updateDGV(chair);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Chair chair = getData();
            if (chair != null)
            {
                if(action == ADD)
                {
                    addChair(chair);
                }
                if(action == EDIT)
                {
                    editChair(chair);
                    resetTextBox();
                }
                //resetTextBox();
            }
        }
        public void setData(Chair chair)
        {
            cbRoom.Text = chair.room.name;
            cbType.Text = chair.type.name;
            numRow.Value = chair.row;
            numColumn.Value = chair.column;
        }
        private void cbId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbId.SelectedIndex != -1)
            {
                Chair chair = chairs.Find(c => c.id == cbId.Text);
                if (chair != null)
                {
                    setData(chair);
                }
            }
        }

        private void cbRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbRoom.SelectedIndex != -1)
            {
                Room room = rooms.Find(r => r.name == cbRoom.Text);
                ChairType chairType = chairTypes.Find(ct => ct.name == cbType.Text);
                if(room != null && chairType != null)
                {
                    cbId.Items.Clear();
                    chairs.FindAll(c => c.room.id == room.id && chairType.id == c.type.id).ForEach(chair =>
                    {
                        cbId.Items.Add(chair.id);
                    });
                }
                else if (room != null && !(chairType != null))
                {
                    cbId.Items.Clear();
                    chairs.FindAll(c => c.room.id == room.id).ForEach(chair =>
                    {
                        cbId.Items.Add(chair.id);
                    });
                }
                else if (chairType != null && !(room != null))
                {
                    cbId.Items.Clear();
                    chairs.FindAll(c => chairType.id == c.type.id).ForEach(chair =>
                    {
                        cbId.Items.Add(chair.id);
                    });
                }
                numRow.Maximum = room.totalRows;
                numColumn.Maximum = room.totalColumns;
            }
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbType.SelectedIndex != -1)
            {
                Room room = rooms.Find(r => r.name == cbRoom.Text);
                ChairType chairType = chairTypes.Find(ct => ct.name == cbType.Text);
                if (room != null && chairType != null)
                {
                    cbId.Items.Clear();
                    chairs.FindAll(c => c.room.id == room.id && chairType.id == c.type.id).ForEach(chair =>
                    {
                        cbId.Items.Add(chair.id);
                    });
                }
                else if (room != null && !(chairType != null))
                {
                    cbId.Items.Clear();
                    chairs.FindAll(c => c.room.id == room.id).ForEach(chair =>
                    {
                        cbId.Items.Add(chair.id);
                    });
                }
                else if (chairType != null && !(room != null))
                {
                    cbId.Items.Clear();
                    chairs.FindAll(c => chairType.id == c.type.id).ForEach(chair =>
                    {
                        cbId.Items.Add(chair.id);
                    });
                }
            }
        }

        private void cbRoom_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cbType_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cbId_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void dgvChair_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (action == EDIT)
            {
                int index = e.RowIndex;
                if (index != -1)
                {
                    Chair chair = chairs.Find(s => s.id == dgvChair.Rows[index].Cells[0].Value.ToString());
                    if (chair != null)
                    {
                        setData(chair);
                        cbId.Text = chair.id;
                    }
                }
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if(cbRoom.Text != "")
            {
                Room room = rooms.Find(r => r.name == cbRoom.Text);
                if(room != null)
                {
                    new fSelectChair(room).Visible = true;
                }
            }
            else
            {
                MessageBox.Show("Chưa chọn rạp", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
