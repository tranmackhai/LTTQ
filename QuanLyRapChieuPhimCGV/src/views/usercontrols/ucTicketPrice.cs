using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyRapChieuPhimCGV.src.models;
using System.Windows.Forms;
using QuanLyRapChieuPhimCGV.src.DAO;
using QuanLyRapChieuPhimCGV.src.views.forms;
using QuanLyRapChieuPhimCGV.src.utils;

namespace QuanLyRapChieuPhimCGV.src.views.usercontrols
{
    public partial class ucTicketPrice : UserControl
    {
        private ucAdministration admin;
        private Employee employee;
        private const string ADD = "Đang thêm";
        private const string EDIT = "Đang sửa";
        private string action = "";
        private DAO_TicketPrice dao_tp = new DAO_TicketPrice();
        private List<TicketPrice> ticketPrices = new List<TicketPrice>();
        private Methods myMethod = new Methods();

        public ucTicketPrice(ucAdministration uc, Employee e)
        {
            admin = uc;
            employee = e;
            InitializeComponent();
            Dock = DockStyle.Fill;

            ticketPrices = dao_tp.getAll();
            ticketPrices.ForEach(ticketPrice =>
            {
                cbId.Items.Add(ticketPrice.id);
                addToDGV(ticketPrice);
            });

            setEnabled(false);
            cbId.Enabled = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(action != ADD)
            {
                action = ADD;
                cbId.Text = dao_tp.generateId();
            }
            else
            {
                action = cbId.Text = "";
            }
            setEnabled(action == ADD);
            cbId.Enabled = false;
        }

        public void setEnabled(bool status)
        {
            cbStart.Enabled = cbEnd.Enabled = cbObject.Enabled = numPrice.Enabled = status;
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
            cbId.Text = "";
            cbId.Enabled = (action == EDIT);
            setEnabled(action == EDIT);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = dgvManufacturer.SelectedRows.Count - 1; i >= 0; i--)
            {
                int index = dgvManufacturer.SelectedRows[i].Index;
                if (index != -1)
                {
                    DialogResult answer = MessageBox.Show($"Bạn có chắc chắn xoá <{dgvManufacturer.Rows[index].Cells[1].Value}> ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer == DialogResult.Yes)
                    {
                        dao_tp.deleteById(dgvManufacturer.Rows[index].Cells[0].Value.ToString());
                        int index_tp = ticketPrices.FindIndex(tp => tp.id == dgvManufacturer.Rows[index].Cells[0].Value.ToString());
                        if (index_tp != -1)
                        {
                            ticketPrices.RemoveAt(index_tp);
                            cbId.Items.RemoveAt(index_tp);
                            dgvManufacturer.Rows.RemoveAt(index);
                        }
                    }
                }
            }
        }
        public string validate()
        {
            string error = "";
            if(cbId.Text == "")
            {
                error += "Chưa chọn mã giá\n";
            }
            int start = cbStart.SelectedIndex + 1;
            int end = cbEnd.SelectedIndex + 1;

            if (start == -1 || end == -1)
            {
                error += "Chưa chọn thời gian\n";
            }
            else
            {
                if(start > end)
                {
                    error += "Khoảng thời gian không hợp lệ\n";
                }
            }
            if(cbObject.Text == "")
            {
                error += "Chưa chọn đối tượng mua vé\n";
            }
            if(numPrice.Value == 0)
            {
                error += "Giá vé phải > 0\n";
            }
            return error;
        }

        public TicketPrice getData()
        {
            string error = validate();

            if(error == "")
            {
                TicketPrice ticketPrice = new TicketPrice()
                {
                    id = cbId.Text,
                    startDate = cbStart.SelectedIndex + 2,
                    endDate = cbEnd.SelectedIndex + 2,
                    objectPerson = cbObject.Text,
                    price = numPrice.Value
                };
                return ticketPrice;
            }
            else
            {
                MessageBox.Show(error, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        public void addToDGV(TicketPrice ticketPrice)
        {
            dgvManufacturer.Rows.Add(new object[]
            {
                ticketPrice.id, ticketPrice.startDate, ticketPrice.endDate, ticketPrice.objectPerson, ticketPrice.price.ToString("#,##")
            });
        }
        public void addTicketPrice(TicketPrice ticketPrice)
        {
            dao_tp.insertOne(ticketPrice);
            cbId.Items.Add(ticketPrice.id);
            ticketPrices.Add(ticketPrice);
            addToDGV(ticketPrice);
            cbId.Text = dao_tp.generateId();
        }
        public void editTicketPrice(TicketPrice ticketPrice)
        {
            dao_tp.updateOne(ticketPrice);
            int index = ticketPrices.FindIndex(tp => tp.id == ticketPrice.id);
            if (index != -1)
            {
                ticketPrices[index] = ticketPrice;
                editDGV(ticketPrice);
            }
        }
        public void editDGV(TicketPrice ticketPrice)
        {
            for(int i = 0; i < dgvManufacturer.RowCount; i++)
            {
                if(dgvManufacturer.Rows[i].Cells[0].Value.ToString() == ticketPrice.id)
                {
                    dgvManufacturer.Rows[i].Cells[1].Value = ticketPrice.startDate;
                    dgvManufacturer.Rows[i].Cells[2].Value = ticketPrice.endDate;
                    dgvManufacturer.Rows[i].Cells[3].Value = ticketPrice.objectPerson;
                    dgvManufacturer.Rows[i].Cells[4].Value = ticketPrice.price.ToString("#,##");
                    break;
                }
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            TicketPrice ticketPrice = getData();
            if(ticketPrice != null)
            {
                if(action == ADD)
                {
                    addTicketPrice(ticketPrice);
                }
                if(action == EDIT)
                {
                    editTicketPrice(ticketPrice);
                }
            }
        }
        public void setData(TicketPrice ticketPrice)
        {
            cbId.Text = ticketPrice.id;
            cbStart.Text = ""+ticketPrice.startDate;
            cbEnd.Text = ""+ticketPrice.endDate;
            cbObject.Text = ticketPrice.objectPerson;
            numPrice.Value = ticketPrice.price;
        }
        private void cbId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (action == EDIT && cbId.SelectedIndex != -1)
            {
                TicketPrice ticketPrice = ticketPrices.Find(emp => emp.id == cbId.Text);
                if (ticketPrice != null)
                {
                    setData(ticketPrice);
                }
            }
        }

        private void dgvManufacturer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (action == EDIT && dgvManufacturer.SelectedRows.Count > 0)
            {
                TicketPrice ticketPrice = ticketPrices.Find(emp => emp.id == dgvManufacturer.Rows[dgvManufacturer.SelectedRows[0].Index].Cells[0].Value.ToString());
                if (ticketPrice != null)
                {
                    setData(ticketPrice);
                }
            }
        }
    }
}
