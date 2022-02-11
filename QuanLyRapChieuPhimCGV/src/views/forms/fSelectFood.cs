using QuanLyRapChieuPhimCGV.src.DAO;
using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.views.usercontrols;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuanLyRapChieuPhimCGV.src.views.forms
{
    public partial class fSelectFood : Form
    {
        private ucBill preComponent;
        private List<GroupFood> groupFoods = new List<GroupFood>();
        private List<Food> foods = new List<Food>();
        private DAO_GroupFood dao_g = new DAO_GroupFood();
        private DAO_Food dao_f = new DAO_Food();
        private List<BillDetail> billDetails = new List<BillDetail>();
        public fSelectFood(ucBill u, List<BillDetail> bds)
        {
            preComponent = u;
            billDetails = bds;
            InitializeComponent();
            groupFoods = dao_g.getAll();
            groupFoods.ForEach(group =>
            {
                cbGroup.Items.Add(group.name);
            });
            cbGroup.SelectedIndex = 0;

            billDetails.ForEach(billDetail =>
            {
                listFood.Items.Add($"{billDetail.food.name} (SL: {billDetail.quantity})");
            });
        }

        private void fSelectFood_Load(object sender, EventArgs e)
        {

        }

        private void cbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbGroup.SelectedIndex != -1)
            {
                GroupFood groupFood = groupFoods.Find(g => g.name == cbGroup.Text);
                if(groupFood != null)
                {
                    cbFood.Items.Clear();
                    foods = dao_f.getAllByGroup(groupFood);
                    foods.ForEach(food =>
                    {
                        cbFood.Items.Add(food.name);
                    });
                    if (cbFood.Items.Count > 0)
                    {
                        cbFood.SelectedIndex = 0;
                    }
                }
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            string error = "";
            Food food = foods.Find(f => f.name == cbFood.Text);
                int quantity = (int)numQuantity.Value;
            if(food == null)
            {
                error += "Chưa chọn đồ ăn\n";
            }
            if(quantity  == 0)
            {
                error += "Chưa chọn số lượng\n";
            }

            if(error == "")
            {
                BillDetail billDetail = new BillDetail()
                {
                    food = food,
                    quantity = quantity
                };
                int index = billDetails.FindIndex(bd => bd.food.id == billDetail.food.id);
                if(index == -1)
                {
                    billDetails.Add(billDetail);
                    addToListBox(billDetail);
                }
                else
                {
                    billDetails[index].quantity += billDetail.quantity;
                    updateToListBox(billDetails[index]);
                }
            }
            else
            {
                MessageBox.Show(error, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        public void addToListBox(BillDetail billDetail)
        {
            listFood.Items.Add($"{billDetail.food.name} (SL: {billDetail.quantity})");
        }
        public void updateToListBox(BillDetail billDetail)
        {
            for(int i = 0; i < listFood.Items.Count; i++)
            {
                if (listFood.Items[i].ToString().Contains(billDetail.food.name))
                {
                    listFood.Items[i] = $"{billDetail.food.name} (SL: {billDetail.quantity})";
                    break;
                }
            }
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            preComponent.getBillDetails(billDetails);
            Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = listFood.SelectedIndices.Count - 1; i >= 0; i--)
            {
                int index = listFood.SelectedIndices[i];
                if (index != -1)
                {
                    billDetails.RemoveAt(billDetails.FindIndex(f=>listFood.Items[index].ToString().Contains(f.food.name)));
                    listFood.Items.RemoveAt(index);
                }
            }
        }
    }
}
