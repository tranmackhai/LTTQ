using System;
using System.Collections.Generic;
using QuanLyRapChieuPhimCGV.src.models;
using System.Windows.Forms;
using QuanLyRapChieuPhimCGV.src.DAO;

namespace QuanLyRapChieuPhimCGV.src.views.usercontrols
{
    public partial class ucSchedule : UserControl
    {
        private const string ADD = "Đang thêm";
        private const string EDIT = "Đang sửa";
        private string action = "";
        private ucAdministration admin;
        private Employee employee;
        private DAO_Room dao_r = new DAO_Room();
        private DAO_Movie dao_m = new DAO_Movie();
        private DAO_Schedule dao_s = new DAO_Schedule();
        private List<Room> rooms = new List<Room>();
        private List<Movie> movies = new List<Movie>();
        private List<Schedule> schedules = new List<Schedule>();
        public ucSchedule(ucAdministration uc, Employee e)
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

            movies = dao_m.getAll();

            movies.ForEach(movie =>
            {
                cbMovie.Items.Add(movie.name);
            });

            schedules = dao_s.getAll();

            schedules.ForEach(schedule =>
            {
                cbId.Items.Add(schedule.id);
                addToDGV(schedule);
            });

            setEnabled(false);
            cbId.Enabled = false;
        }

        public void addToDGV(Schedule schedule)
        {
            dgvSchedule.Rows.Add(new object[]
            {
                schedule.id,
                schedule.dateTime.ToString("dd-MM-yyyy HH:mm"),
                schedule.movie.name,
                schedule.room.name
            });
        }

        public void setEnabled(bool status)
        {
            dtDate.Enabled = dtTime.Enabled = cbMovie.Enabled = cbRoom.Enabled = status;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (action != ADD)
            {
                action = ADD;
                cbId.Text = dao_s.generateId();
            }
            else
            {
                cbId.Text = "";
                action = "";
            }
            setEnabled(action == ADD);
            cbId.Enabled = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (action != EDIT)
            {
                action = EDIT;
            }
            else
            {
                action = "";
            }
            setEnabled(action == EDIT);
            cbId.Enabled = (action == EDIT);
            cbId.Text = "";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = dgvSchedule.SelectedRows.Count - 1; i >= 0; i--)
            {
                int index = dgvSchedule.SelectedRows[i].Index;//Lấy vị trí hàng đang chọn thứ i
                if (index != -1)
                {
                    DialogResult answer = MessageBox.Show($"Bạn có chắc chắn xoá <{dgvSchedule.Rows[index].Cells[1].Value}> ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer == DialogResult.Yes)
                    {
                        dao_s.deleteById(dgvSchedule.Rows[index].Cells[0].Value.ToString());
                        int index_schedule = schedules.FindIndex(s => s.id == dgvSchedule.Rows[index].Cells[0].Value.ToString());
                        if (index_schedule != -1)
                        {
                            schedules.RemoveAt(index_schedule);
                            if (cbId.Items.Contains(dgvSchedule.Rows[index].Cells[0].Value.ToString()))
                            {
                                cbId.Items.Remove(dgvSchedule.Rows[index].Cells[0].Value.ToString());
                            }
                            dgvSchedule.Rows.RemoveAt(index);
                        }
                    }
                }
            }
        }


        public string validate()
        {
            string error = "";
            if (cbId.Text == "")
            {
                error += "Chưa chọn mã phim\n";
            }
            if (cbMovie.Text == "")
            {
                error += "Chưa chọn phim\n";
            }
            if (cbRoom.Text == "")
            {
                error += "Chưa chọn rạp\n";
            }

            return error;
        }

        public Schedule getData()
        {
            string error = validate();

            if (error == "")
            {

                Schedule schedule = new Schedule()
                {
                    id = cbId.Text,
                    movie = movies.Find(m=>m.name == cbMovie.Text),
                    dateTime =new DateTime(dtDate.Value.Year, dtDate.Value.Month, dtDate.Value.Day, dtTime.Value.Hour, dtTime.Value.Minute, 0),
                    room = rooms.Find(r=>r.name == cbRoom.Text)
                };
                return schedule;
            }
            else
            {
                MessageBox.Show(error, "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        public void addSchedule(Schedule schedule)
        {
            if (dao_s.canInsertSchedule(schedule))
            {
                dao_s.insertOne(schedule);
                schedules.Add(schedule);
                cbId.Items.Add(schedule.id);
                addToDGV(schedule);
                cbId.Text = dao_s.generateId();
            }
            else
            {
                MessageBox.Show("Không thể đặt lịch này", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void updateDGV(Schedule schedule)
        {
            for (int i = 0; i < dgvSchedule.RowCount; i++)
            {
                if (dgvSchedule.Rows[i].Cells[0].Value.ToString() == schedule.id)
                {
                    dgvSchedule.Rows[i].Cells[1].Value = schedule.dateTime.ToString("dd-MM-yyyy HH:mm");
                    dgvSchedule.Rows[i].Cells[2].Value = schedule.movie.name;
                    dgvSchedule.Rows[i].Cells[3].Value = schedule.room.name;
                    break;
                }
            }
        }
        public void editSchedule(Schedule schedule)
        {
            if (dao_s.canInsertSchedule(schedule))
            {
                dao_s.updateOne(schedule);
                int index = schedules.FindIndex(s => s.id == schedule.id);
                if (index != -1)
                {
                    schedules[index] = schedule;
                }
                updateDGV(schedule);
            }
            else
            {
                MessageBox.Show("Không thể đặt lịch này", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Schedule schedule = getData();
            if (schedule != null)
            {
                if (action == ADD)
                {
                    addSchedule(schedule);
                }
                if (action == EDIT)
                {
                    editSchedule(schedule);
                }
            }
        }

        private void cbId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbId.SelectedIndex != -1)
            {
                Schedule schedule = schedules.Find(s => s.id == cbId.Text);
                if (schedule != null)
                {
                    setData(schedule);
                }
            }
        }
        public void setData(Schedule schedule)
        {
            cbId.Text = schedule.id;
            dtDate.Value = schedule.dateTime;
            dtTime.Value = schedule.dateTime;
            cbRoom.Text = schedule.room.name;
            cbMovie.Text = schedule.movie.name;
        }

        private void dgvSchedule_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(action == EDIT)
            {
                int index = e.RowIndex;
                if (index != -1)
                {
                    Schedule schedule = schedules.Find(s => s.id == dgvSchedule.Rows[index].Cells[0].Value.ToString());
                    if (schedule != null)
                    {
                        setData(schedule);
                        cbId.Text = schedule.id;
                    }
                }
            }
        }
    }
}
