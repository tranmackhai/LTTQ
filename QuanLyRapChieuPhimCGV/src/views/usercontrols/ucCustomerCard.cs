using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyRapChieuPhimCGV.src.DAO;
using QuanLyRapChieuPhimCGV.src.models;

namespace QuanLyRapChieuPhimCGV.src.views.usercontrols
{
    public partial class ucCustomerCard : UserControl
    {
        private ucAdministration admin;
        private Employee employee;
        private const string ADD = "Đang thêm";
        private const string EDIT = "Đang sửa";
        private string action = "";
        private DAO_Card dao_c = new DAO_Card();
        private List<Card> cards = new List<Card>();
        public ucCustomerCard(ucAdministration uc, Employee e)
        {
            admin = uc;
            employee = e;
            InitializeComponent();
            Dock = DockStyle.Fill;

            cards = dao_c.getAll();

            cards.ForEach(card =>
            {
                addToDataGridView(card);
                cbId.Items.Add(card.id);
            });

            setEnabled(false);
            cbId.Enabled = false;
        }

        public void addToDataGridView(Card card)//Thêm 1 hàng vào datagridview
        {
            dgvCard.Rows.Add(new object[]
            {
                card.id,
                card.name,
                card.percentForTicket,
                card.percentForFood
            });
        }

        public void setEnabled(bool status)
        {
            txtName.Enabled = status;
            txtPercentTicket.Enabled = status;
            txtPercentFood.Enabled = status;
        }

        public void resetTextBox()
        {
            txtName.Text = txtPercentTicket.Text = txtPercentFood.Text = "";
        }

        public string validate()
        {
            string error = "";

            if (cbId.Text == "")
            {
                error += "Chưa chọn mã thẻ\n";
            }
            if (txtName.Text == "")
            {
                error += "Tên thẻ không được để trống\n";
            }
            try
            {
                float percentTicket = float.Parse(txtPercentTicket.Text);
                if (txtPercentTicket.Text == "")
                {
                    error += "Chưa nhập % điểm theo giá vé\n";
                }
            }
            catch(Exception ex)
            {
                error += "% điểm theo giá vé không hợp lệ\n";
                Console.WriteLine(ex);
            }
            try
            {
                float percentFood = float.Parse(txtPercentFood.Text);
                if (txtPercentFood.Text == "")
                {
                    error += "Chưa nhập % điểm theo giá hoá đơn\n";
                }
            }
            catch (Exception ex)
            {
                error += "% điểm theo giá hoá đơn không hợp lệ\n";
                Console.WriteLine(ex);
            }

            return error;
        }

        public Card getData()//Lấy thông tin nhân viên từ giao diện
        {
            string error = validate();
            if (error == "")
            {
                Card card = new Card();
                card.id = cbId.Text;
                card.name = txtName.Text;
                card.percentForTicket = float.Parse(txtPercentTicket.Text);
                card.percentForFood = float.Parse(txtPercentFood.Text);
                return card;
            }
            else
            {
                MessageBox.Show(error, "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        public void addEmployee(Card card)
        {
            dao_c.insertOne(card);
            cards.Add(card);
            cbId.Items.Add(card.id);
            cbId.Text = dao_c.generateId();
        }
        public void setData(Card card)
        {
            txtName.Text = card.name;
            txtPercentTicket.Text = "" + card.percentForTicket;
            txtPercentFood.Text = "" + card.percentForFood;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            resetTextBox();
            if (action != ADD)
            {
                action = ADD;
                cbId.Text = dao_c.generateId();//Tạo id mới
            }
            else
            {
                action = "";
                cbId.Text = "";
            }
            cbId.Enabled = false;
            setEnabled(action == ADD);
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
        } //Cập nhật nhân viên trong CSDL
        //Tìm vị trí nhân viên vừa cập nhật
        //Sửa datagridview
        public void editEmployee(Card card)
        {
            dao_c.updateOne(card);
            int index = cards.FindIndex(emp => emp.id == card.id);
            if (index != -1)
            {
                cards[index] = card;
                updateDataGridView(card);
            }
        }
        public void updateDataGridView(Card card)//Cập nhật 1 hàng datagridview
        {
            for (int i = 0; i < dgvCard.RowCount; i++)
            {
                if (dgvCard.Rows[i].Cells[0].Value.ToString() == card.id)
                {
                    dgvCard.Rows[i].Cells[1].Value = card.name;
                    dgvCard.Rows[i].Cells[2].Value = card.percentForTicket;
                    dgvCard.Rows[i].Cells[3].Value = card.percentForFood;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = dgvCard.SelectedRows.Count - 1; i >= 0; i--)
            {
                int index = dgvCard.SelectedRows[i].Index;
                if (index != -1)
                {
                    DialogResult answer = MessageBox.Show($"Bạn có chắc chắn xoá <{dgvCard.Rows[index].Cells[1].Value}> ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer == DialogResult.Yes)
                    {
                        dao_c.deleteById(dgvCard.Rows[index].Cells[0].Value.ToString());
                        int index_card = cards.FindIndex(emp => emp.id == dgvCard.Rows[index].Cells[0].Value.ToString());
                        if (index_card != -1)
                        {
                            cards.RemoveAt(index_card);
                            cbId.Items.RemoveAt(index_card);
                            dgvCard.Rows.RemoveAt(index);
                        }
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            resetTextBox();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Card card = getData();
            if (card != null)
            {
                if (action == ADD)
                {
                    addEmployee(card);
                    addToDataGridView(card);
                }
                if (action == EDIT)
                {
                    editEmployee(card);
                }
                resetTextBox();
            }
        }

        private void cbId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbId.SelectedIndex != -1)
            {
                Card card = cards.Find(emp => emp.id == cbId.Text);
                if (card != null)
                {
                    setData(card);
                }
            }
        }

        private void dgvCard_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (action == EDIT)
            {
                int index = e.RowIndex;
                if (index != -1)
                {
                    Card card = cards.Find(emp => emp.id == dgvCard.Rows[index].Cells[0].Value.ToString());
                    if (card != null)
                    {
                        cbId.Text = dgvCard.Rows[index].Cells[0].Value.ToString();
                        setData(card);
                    }
                }
            }
        }
    }
}
