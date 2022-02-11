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
    public partial class ucCategoryMovie : UserControl
    {
        private const string ADD = "Đang thêm";
        private const string EDIT = "Đang sửa";
        private string action = "";
        private DAO_CategoryMovie dao_cm = new DAO_CategoryMovie();
        private List<CategoryMovie> categoryMovies = new List<CategoryMovie>();
        private DAO_Movie dao_m = new DAO_Movie();
        private DAO_FormatMovie dao_fm = new DAO_FormatMovie();
        private List<Movie> movies = new List<Movie>();
        private ucAdministration admin;
        private Employee employee;
        public ucCategoryMovie(ucAdministration uc, Employee e)
        {
            admin = uc;
            employee = e;
            InitializeComponent();
            Dock = DockStyle.Fill;

            categoryMovies = dao_cm.getAll();
            listCategories.Items.Clear();
            categoryMovies.ForEach(categoryMovie =>
            {
                cbId.Items.Add(categoryMovie.id);
                addToDGV(categoryMovie);
                listCategories.Items.Add(categoryMovie.name);
            });

            movies = dao_m.getAll();

            movies.ForEach(movie =>
            {
                dgvMovie.Rows.Add(new object[]
                {
                    movie.name
                });
            });

            setEnabled(false);
            cbId.Enabled = false;
        }

        public void addToDGV(CategoryMovie categoryMovie)
        {
            dgvCategory.Rows.Add(new object[]
            {
                categoryMovie.id, categoryMovie.name
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            resetTextBox();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            resetTextBox();
            if (action != ADD)
            {
                action = ADD;
                cbId.Text = dao_cm.generateId();//Tạo id mới
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
                error += "Chưa chọn mã loại";
            }
            if (txtName.Text == "")
            {
                error += "Tên thể loại không được để trống\n";
            }
            return error;
        }
        public CategoryMovie getData()//Lấy thông tin nhân viên từ giao diện
        {
            string error = validate();
            if (error == "")
            {
                CategoryMovie categoryMovie = new CategoryMovie();
                categoryMovie.id = cbId.Text;
                categoryMovie.name = txtName.Text;
                return categoryMovie;
            }
            else
            {
                MessageBox.Show(error, "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
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
        public void updateDataGridView(CategoryMovie categoryMovie)//Cập nhật 1 hàng datagridview
        {
            for (int i = 0; i < dgvCategory.RowCount; i++)
            {
                if (dgvCategory.Rows[i].Cells[0].Value.ToString() == categoryMovie.id)
                {
                    dgvCategory.Rows[i].Cells[1].Value = categoryMovie.name;
                }
            }
        }

        //Cập nhật nhân viên trong CSDL
        //Tìm vị trí nhân viên vừa cập nhật
        //Sửa datagridview
        public void editCategoryMovie(CategoryMovie categoryMovie)
        {
            dao_cm.updateOne(categoryMovie);
            int index = categoryMovies.FindIndex(gf => gf.id == categoryMovie.id);
            if (index != -1)
            {
                categoryMovies[index] = categoryMovie;
                updateDataGridView(categoryMovie);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = dgvCategory.SelectedRows.Count - 1; i >= 0; i--)
            {
                int index = dgvCategory.SelectedRows[i].Index;
                if (index != -1)
                {
                    DialogResult answer = MessageBox.Show($"Bạn có chắc chắn xoá <{dgvCategory.Rows[index].Cells[1].Value}> ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer == DialogResult.Yes)
                    {
                        dao_cm.deleteById(dgvCategory.Rows[index].Cells[0].Value.ToString());
                        int index_categoryMovie = categoryMovies.FindIndex(gf => gf.id == dgvCategory.Rows[index].Cells[0].Value.ToString());
                        if (index_categoryMovie != -1)
                        {
                            categoryMovies.RemoveAt(index_categoryMovie);
                            cbId.Items.RemoveAt(index_categoryMovie);
                            listCategories.Items.Remove(dgvCategory.Rows[index].Cells[1].Value.ToString());
                            dgvCategory.Rows.RemoveAt(index);
                        }
                    }
                }
            }
        }

        public void addCategoryMovie (CategoryMovie categoryMovie)
        {
            dao_cm.insertOne(categoryMovie);
            categoryMovies.Add(categoryMovie);
            cbId.Items.Add(categoryMovie.id);
            cbId.Text = dao_cm.generateId();
            listCategories.Items.Add(categoryMovie.name);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            CategoryMovie categoryMovie = getData();
            if (categoryMovie != null)
            {
                if (action == ADD)
                {
                    addCategoryMovie(categoryMovie);
                    addToDGV(categoryMovie);
                }
                if (action == EDIT)
                {
                    editCategoryMovie(categoryMovie);
                }
                resetTextBox();
            }
        }

        private void ucCategoryMovie_Load(object sender, EventArgs e)
        {

        }
        public void setData(CategoryMovie categoryMovie)
        {
            txtName.Text = categoryMovie.name;
        }

        private void cbId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbId.SelectedIndex != -1)
            {
                CategoryMovie categoryMovie = categoryMovies.Find(gf => gf.id == cbId.Text);
                if (categoryMovie != null)
                {
                    setData(categoryMovie);
                }
            }
        }

        private void listCategories_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if(dgvMovie.SelectedRows.Count > 0)
            {
                Movie movie = movies.Find(m => m.name == dgvMovie.Rows[dgvMovie.SelectedRows[0].Index].Cells[0].Value.ToString());
                CategoryMovie categoryMovie = categoryMovies.Find(c => c.name == listCategories.Items[e.Index].ToString());
                if (movie != null && categoryMovie!= null)
                {
                    dao_fm.deleteOne(movie.id, categoryMovie.id);
                    if (e.NewValue == CheckState.Checked)
                    {
                        dao_fm.insertOne(new FormatMovie()
                        {
                            movie = movie,
                            category = categoryMovie
                        });
                    }
                }
            }
        }

        private void dgvMovie_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                Movie movie = movies.Find(m => m.name == dgvMovie.Rows[e.RowIndex].Cells[0].Value.ToString());
                if (movie != null)
                {
                    List<FormatMovie> formatMovies = dao_fm.getAllByMovie(movie.id);
                    for (int i = 0; i < listCategories.Items.Count; i++)
                    {
                        CategoryMovie categoryMovie = categoryMovies.Find(c => c.name == listCategories.Items[i].ToString());
                        if (categoryMovie != null)
                        {
                            if (formatMovies.FindIndex(f => f.category.name == categoryMovie.name) != -1)
                            {
                                listCategories.SetItemCheckState(i, CheckState.Checked);
                            }
                            else
                            {
                                listCategories.SetItemCheckState(i, CheckState.Unchecked);
                            }
                        }
                    }
                }
            }
        }
    }
}
