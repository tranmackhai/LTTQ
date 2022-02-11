using QuanLyRapChieuPhimCGV.src.views.forms;
using System;
using System.Collections.Generic;
using QuanLyRapChieuPhimCGV.src.models;
using System.Windows.Forms;
using QuanLyRapChieuPhimCGV.src.DAO;

namespace QuanLyRapChieuPhimCGV.src.views.usercontrols
{
    public partial class ucBill : UserControl
    {
        private ucAdministration admin;
        private Employee employee; 
        private const string ADD = "Đang thêm";
        private const string EDIT = "Đang sửa";
        private string action = "";
        private DAO_Bill dao_b = new DAO_Bill();
        private DAO_Customer dao_cus = new DAO_Customer();
        private DAO_BillDetail dao_bd = new DAO_BillDetail();
        private List<Bill> bills = new List<Bill>();
        private List<BillDetail> billDetails = new List<BillDetail>();
        private decimal totalPrice = 0;
        private Customer curCustomer;
        public ucBill(ucAdministration uc, Employee e)
        {
            admin = uc;
            employee = e;
            InitializeComponent();
            Dock = DockStyle.Fill;
        }
        public ucBill(Employee e)
        {
            employee = e;
            InitializeComponent();
            Dock = DockStyle.Fill;
            flowLayoutPanel1.Controls.Remove(btnEdit);
            flowLayoutPanel1.Controls.Remove(btnDelete);
        }

        public void getBillDetails(List<BillDetail> bds)
        {
            billDetails = bds;
            billDetails.ForEach(billDetail =>
            {
                totalPrice += (billDetail.food.price * billDetail.quantity);
            });
            txtPrice.Text = $"{totalPrice.ToString("#,##")}đ";
        }

        private void btnSelectFood_Click(object sender, EventArgs e)
        {
            new fSelectFood(this, billDetails).Visible = true;
        }
        public void getCustomer(Customer cus)
        {
            dao_cus.getExpenseOfCustomer(cus, 15);
            curCustomer = dao_cus.getById(cus.id);
            txtCustomer.Text = curCustomer.id;
            txtCustomer.Text = curCustomer.id;
            numPoint.Maximum = (decimal)curCustomer.totalPoint;
            label9.Text = $"Điểm ({curCustomer.totalPoint})";
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (action != ADD)
            {
                action = ADD;
                cbId.Text = dao_b.generateId();
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
            btnSelectFood.Enabled = rbtnNo.Enabled = rbtnYes.Enabled = numPoint.Enabled = status;
        }
        private void ucBill_Load(object sender, EventArgs e)
        {
            bills = dao_b.getAll();

            bills.ForEach(bill =>
            {
                cbId.Items.Add(bill.id);
                addToDGV(bill);
            });

            setEnabled(false);
            cbId.Enabled = false;
        }
        public void addToDGV(Bill bill)
        {
            dgvBill.Rows.Add(new object[]
            {
                bill.id, bill.date.ToString("dd-MM-yyyy"), bill.employee.name, bill.customer == null?"":bill.customer.id, bill.totalPrice.ToString("#,##")
            });
        }

        private void rbtnNo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnNo.Checked == true)
            {
                btnSelectCustomer.Enabled = false;
            }
            else
            {
                btnSelectCustomer.Enabled = true;
            }
        }

        private void rbtnYes_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnYes.Checked == false)
            {
                btnSelectCustomer.Enabled = false;
            }
            else
            {
                btnSelectCustomer.Enabled = true;
            }
        }
        public string validate()
        {
            string error = "";
            if(cbId.Text == "")
            {
                error += "Chưa chọn số hoá đơn\n";
            }
            if(billDetails.Count == 0)
            {
                error += "Chưa chọn đồ ăn\n";
            }
            if(numPoint.Value >0 && numPoint.Value < 20)
            {
                error += "Điểm tối thiểu là 20\n";
            }
            return error;
        }
        public Bill getData()
        {
            string error = validate();
            if(error == "")
            {
                return new Bill()
                {
                    id = cbId.Text,
                    date = DateTime.Now,
                    employee = employee,
                    customer = curCustomer,
                    point = (int)numPoint.Value,
                    totalPrice = totalPrice - numPoint.Value * 1000
                };
            }
            else
            {
                MessageBox.Show(error, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        public void addBill(Bill bill)
        {
            dao_b.insertOne(bill);
            if (curCustomer != null)
            {
                if (numPoint.Value > 0)//Trả bằng điểm
                {
                    if (numPoint.Value * 1000 > bill.totalPrice)
                    {
                        numPoint.Maximum = bill.totalPrice / 1000;
                        numPoint.Value = bill.totalPrice / 1000;
                    }
                    curCustomer.totalPoint -= (float)numPoint.Value;
                    if (curCustomer.totalPoint < 0)
                    {
                        curCustomer.totalPoint = 0;
                    }
                }
                //Tính điểm
                float point = getPoint(bill, curCustomer);
                //Cập nhật điểm
                curCustomer.totalPoint += point;
                dao_cus.updateOne(curCustomer);
                bills.Add(bill);
            }
            cbId.Items.Add(bill.id);
            addToDGV(bill);
            MessageBox.Show("Lưu thành công", "Thành công");
            cbId.Text = dao_b.generateId();
        }
        public float getPoint(Bill bill, Customer customer)
        {
            float point = (customer.card.percentForFood * (float)bill.totalPrice) / 100000;
            if (point % Math.Floor(point) >= 0.5)
            {
                point = (float)Math.Floor(point);
                point++;
            }
            else
            {
                point = (float)Math.Floor(point);
            }
            return point;
        }
        public void editBill(Bill bill)
        {
            dao_b.updateOne(bill);
            int index = bills.FindIndex(b => b.id == bill.id);
            if (index != -1)
            {
                if(curCustomer != null)
                {
                    if (numPoint.Value > 0)//Trả bằng điểm
                    {
                        curCustomer.totalPoint -= (float)numPoint.Value;
                        if (curCustomer.totalPoint < 0)
                        {
                            curCustomer.totalPoint = 0;
                        }
                        //Tính điểm
                        float point = getPoint(bill, curCustomer);
                        //Cập nhật điểm
                        curCustomer.totalPoint += point;
                        dao_cus.updateOne(curCustomer);
                    }
                }
                else
                {
                    //Tính điểm
                    float point = getPoint(bill, bills[index].customer);
                    //Cập nhật điểm
                    bills[index].customer.totalPoint += point;
                    dao_cus.updateOne(bills[index].customer);
                }
                bills[index] = bill;
                editDGV(bill);
            }
        }
        public void editDGV(Bill bill)
        {
            for (int i = 0; i < dgvBill.RowCount; i++)
            {
                if (dgvBill.Rows[i].Cells[0].Value.ToString() == bill.id)
                {
                    dgvBill.Rows[i].Cells[1].Value = bill.date.ToString("dd/MM/yyyy");
                    dgvBill.Rows[i].Cells[2].Value = bill.employee.name;
                    dgvBill.Rows[i].Cells[3].Value = bill.customer == null ? "" : bill.customer.id;
                    dgvBill.Rows[i].Cells[4].Value = bill.totalPrice.ToString("#,##");
                }
            }
        }

        private void btnSelectCustomer_Click(object sender, EventArgs e)
        {
            new fSelectCustomer(this).Visible = true;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            reset();
            if(action != EDIT)
            {
                action = EDIT;
            }
            else
            {
                action = "";
            }
            setEnabled(action == EDIT);
            cbId.Text = "";
            cbId.Enabled = action == EDIT;
        }
        public void reset()
        {
            curCustomer = null;
            txtCustomer.Text = "";
            billDetails.Clear();
            txtPrice.Text = "";
            totalPrice = 0;
            numPoint.Value = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = dgvBill.SelectedRows.Count - 1; i >= 0; i--)
            {
                int index = dgvBill.SelectedRows[i].Index;
                if (index != -1)
                {
                    DialogResult answer = MessageBox.Show($"Bạn có chắc chắn xoá <{dgvBill.Rows[index].Cells[0].Value}> ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer == DialogResult.Yes)
                    {
                        dao_b.deleteById(dgvBill.Rows[index].Cells[0].Value.ToString());
                        int index_bill = bills.FindIndex(b => b.id == dgvBill.Rows[index].Cells[0].Value.ToString());
                        if (index_bill != -1)
                        {
                            if (bills[index_bill].customer != null)
                            {
                                float point = getPoint(bills[index_bill], bills[index_bill].customer);
                                bills[index_bill].customer.totalPoint -= point;
                                dao_cus.updateOne(bills[index_bill].customer);
                            }
                            bills.RemoveAt(index_bill);
                            cbId.Items.RemoveAt(index_bill);
                            dgvBill.Rows.RemoveAt(index);
                        }
                    }
                }
            }
        }
        public void setData(Bill bill)
        {
            curCustomer = bill.customer;
            if (curCustomer == null)
            {
                rbtnNo.Checked = true;
                txtCustomer.Text = "";
                numPoint.Maximum = 0;
            }
            else
            {
                rbtnYes.Checked = true;
                txtCustomer.Text = curCustomer.id;
                numPoint.Maximum = (decimal)bill.customer.totalPoint;
            }
            billDetails = dao_bd.getAll(bill);
            dateTime.Value = bill.date;
            txtPrice.Text = bill.totalPrice.ToString("#,##");
        }
        private void dgvBill_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (action == EDIT)
            {
                int index = e.RowIndex;
                if (index != -1)
                {
                    Bill bill = bills.Find(tik => tik.id == dgvBill.Rows[index].Cells[0].Value.ToString());
                    if (bill != null)
                    {
                        cbId.Text = dgvBill.Rows[index].Cells[0].Value.ToString();
                        setData(bill);
                    }
                }
            }
        }

        private void cbId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbId.SelectedIndex != -1)
            {
                Bill bill = bills.Find(tik => tik.id == cbId.Text);
                if (bill != null)
                {
                    setData(bill);
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvBill.SelectedRows.Count; i++)
            {
                int index = dgvBill.SelectedRows[i].Index;
                if (index != -1)
                {
                    Bill bill = bills.Find(tik => tik.id == dgvBill.Rows[index].Cells[0].Value.ToString());
                    if (bill != null)
                    {
                        new fReport(bill).Visible = true;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bill bill = getData();
            if (bill != null)
            {
                if (action == ADD)
                {
                    addBill(bill);
                    billDetails.ForEach(billDetail =>
                    {
                        dao_bd.insertOne(bill.id, billDetail);
                    });
                }
                if (action == EDIT)
                {
                    editBill(bill);
                    dao_bd.deleteAll(bill.id);
                    billDetails.ForEach(billDetail =>
                    {
                        dao_bd.insertOne(bill.id, billDetail);
                    });
                }
            }
            totalPrice = 0;
        }
    }
}
