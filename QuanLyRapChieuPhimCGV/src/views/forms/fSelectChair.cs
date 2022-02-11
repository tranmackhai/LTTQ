using QuanLyRapChieuPhimCGV.src.DAO;
using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.views.usercontrols;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyRapChieuPhimCGV.src.views.forms
{
    public partial class fSelectChair : Form
    {
        private ucBookTicket preComponent;
        private Schedule schedule;
        private DAO_Chair dao_ch = new DAO_Chair();
        private List<Chair> chairs = new List<Chair>();
        private List<ChairType> chairTypes = new List<ChairType>();
        private DAO_ChairType dao_ct = new DAO_ChairType();
        private Button curBtnChair;
        private Customer curCustomer;
        public fSelectChair(ucBookTicket u, Schedule sch, Customer c)
        {
            preComponent = u;
            curCustomer = c;
            schedule = sch;
            InitializeComponent();
            chairs = dao_ch.getAllByRoom(schedule.room);
            chairTypes = dao_ct.getAll();
            init(schedule.room);
        }
        public fSelectChair(Room room)
        {
            InitializeComponent();
            chairs = dao_ch.getAllByRoom(room);
            chairTypes = dao_ct.getAll();
            Controls.Remove(btnSelect);
            init(room);
        }

        public void init(Room room)
        {
            int tongSoHang = room.totalRows;
            int tongSoCot = room.totalColumns;
            int btnSize = 50;
            Size = new Size(tongSoCot * (btnSize + 6) + 6, (tongSoHang+1) * (btnSize + 6) + 30 + 90);
            Label lblScreen = new Label();
            lblScreen.Location = new Point(6, 0);
            lblScreen.Size = new Size((tongSoCot - 1) * (btnSize + 6), 60);
            lblScreen.Text = "MÀN HÌNH";
            lblScreen.AutoSize = false;
            lblScreen.Font = new Font("Arial", 18, FontStyle.Bold);
            lblScreen.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(lblScreen);
            panel1.Location = new Point(6, 60);
            panel1.Size = new Size((tongSoCot - 1) * (btnSize + 6), (tongSoHang - 1) * (btnSize + 6) + 12);
            Button[,] btnChairs = new Button[tongSoHang, tongSoCot];
            for (int j = 0; j < tongSoHang; j++)
            {
                int z = 1;
                dao_ch.getAllInRow(tongSoHang - j, room).ForEach(chair =>
                {
                    btnChairs[tongSoHang - j - 1, tongSoCot - chair.column] = new Button();
                    btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].Name = chair.id;
                    btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].Text = "" + (char)((chair.row - 1) + 65) + z++;
                    btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].Size = new Size(btnSize, btnSize);
                    btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].Location = new Point((tongSoCot - chair.column) * btnSize + 6, (chair.row - 1) * btnSize + 6);
                    ChairType chairType = chairTypes.Find(type => type.id == chair.type.id);
                    if (chairType != null)
                    {
                        string name = chairType.name.ToLower();
                        if (name.IndexOf("thường") != -1)//Ghế thường
                        {
                            btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].BackColor = Color.FromArgb(242, 231, 215);
                        }
                        else if (name.IndexOf("vip") != -1)//Ghế vip
                        {
                            btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].BackColor = Color.FromArgb(218, 70, 88);
                        }
                        else if (name.IndexOf("khuyết tật") != -1)//Ghế cho người khuyết tật
                        {
                            btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].BackColor = Color.Green;
                            btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].Text = "W";
                        }
                        else if (name.IndexOf("sweet") != -1)//SweetBox
                        {
                            btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].BackColor = Color.FromArgb(250, 0, 185);
                        }
                        else
                        {
                            btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].BackColor = Color.FromArgb(15, 20, 25);
                        }
                    }
                    if (dao_ch.isBooked(schedule, chair) == true)
                    {
                        btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].BackColor = Color.FromArgb(111, 11, 1);
                        btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].Text = "";
                        btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].Image = Image.FromFile(@"..\..\public\img\cancel.png");
                        btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].Enabled = false;
                    }
                    else
                    {
                        btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].Cursor = Cursors.Hand;
                    }
                    btnChairs[tongSoHang - j - 1, tongSoCot - chair.column].Click += new EventHandler(clickChair);
                    panel1.Controls.Add(btnChairs[tongSoHang - j - 1, tongSoCot - chair.column]);
                });
                //Chú thích
                int lblSize = 120;
                int i = 0;
                dao_ct.getAllInRoom(room).ForEach(ct =>
                {
                    renderChairComment(btnSize, lblSize, ct.name, tongSoHang, i++);
                });
                //Đã chọn
                Button btnComment5 = new Button();
                btnComment5.Location = new Point(i * 130 + i * btnSize + 6, 32 + tongSoHang * btnSize + 6 + 60);
                btnComment5.Size = new Size(btnSize, btnSize);
                btnComment5.BackColor = Color.FromArgb(255, 0, 185);
                btnComment5.BackColor = Color.FromArgb(111, 11, 1);
                btnComment5.Image = Image.FromFile(@"..\..\public\img\cancel.png");
                btnComment5.Enabled = false;
                Controls.Add(btnComment5);
                Label lblComment5 = new Label();
                lblComment5.Location = new Point(i * 130 + i * btnSize + 6 + btnSize + 6, 32 + tongSoHang * btnSize + 6 + 60);
                lblComment5.Size = new Size(lblSize, btnSize);
                lblComment5.Text = "Đã chọn";
                lblComment5.AutoSize = false;
                lblComment5.TextAlign = ContentAlignment.MiddleLeft;
                Controls.Add(lblComment5);
            }

            // 1 ô btnSize
            // margin 6
            //width = tongcot * (btnSize + 6) + 20
            //height = tonghang * (btnSize +6)
        }

        public void renderChairComment(int btnSize, int lblSize, string chairName, int tongSoHang, int index)
        {
            Panel pnlComment1 = new Panel();
            pnlComment1.Location = new Point(index * 130 + index * btnSize + 6, 32 + tongSoHang * btnSize + 6 + 60);
            pnlComment1.Size = new Size(btnSize, btnSize);
            Label lblComment1 = new Label();
            lblComment1.Location = new Point(index * 130 + index * btnSize + 6 + btnSize + 6, 32 + tongSoHang * btnSize + 6 + 60);
            lblComment1.Size = new Size(lblSize, btnSize);
            lblComment1.TextAlign = ContentAlignment.MiddleLeft;
            lblComment1.AutoSize = false;
            if (chairName.ToLower().IndexOf("thường") != -1)
            {
                pnlComment1.BackColor = Color.FromArgb(242, 231, 215); 
                lblComment1.Text = chairName;
            }
            if (chairName.ToLower().IndexOf("vip") != -1)
            {
                pnlComment1.BackColor = Color.FromArgb(218, 70, 88);
                lblComment1.Text = chairName;
            }
            if (chairName.ToLower().IndexOf("khuyết") != -1)
            {
                pnlComment1.BackColor = Color.Green;
                lblComment1.Text = chairName;
            }
            if (chairName.ToLower().IndexOf("sweet") != -1)
            {
                pnlComment1.BackColor = Color.FromArgb(250, 0, 185);
                lblComment1.Text = "Ghế Cặp Đôi";
            }
            Controls.Add(pnlComment1);
            Controls.Add(lblComment1);
        }

        private void fSelectChair_Load(object sender, EventArgs e)
        {
            
        }

        private void clickChair(object sender, EventArgs e)
        {
            curBtnChair = sender as Button;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (schedule != null && curBtnChair != null)
            {
                Chair chair = dao_ch.getById(curBtnChair.Name);
                if (curBtnChair != null)
                {
                    //Ko có thẻ vip nhưng chọn ghế vip
                    if(curCustomer!=null && curCustomer.card.name.ToLower().IndexOf("vip") == -1 && chair.type.name.ToLower().IndexOf("vip") != -1)
                    {
                        MessageBox.Show("Ghế này dành cho khách có thẻ VIP", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        preComponent.getChair(dao_ch.getById(curBtnChair.Name), curBtnChair.Text);
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("Chưa chọn ghế", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
