using QuanLyRapChieuPhimCGV.src.views.forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.DAO;
using System.IO;

namespace QuanLyRapChieuPhimCGV.src.views.usercontrols
{
    public partial class ucMovie : UserControl
    {
        private const string ADD = "Đang thêm";
        private const string EDIT = "Đang sửa";
        private string action = "";
        private ucAdministration admin;
        private Employee employee;
        private DAO_FormatMovie dao_fm = new DAO_FormatMovie();
        //private List<Distributor> distributors = new List<Distributor>();
        private DAO_Movie dao_m = new DAO_Movie();
        private List<Movie> movies = new List<Movie>();
        private string pathImage = "";
        public ucMovie(ucAdministration uc, Employee e)
        {
            admin = uc;
            employee = e;
            InitializeComponent();
            Dock = DockStyle.Fill;

            //distributors = dao_d.getAll();

            //distributors.ForEach(distributor =>
            //{
            //    cbDistributor.Items.Add(distributor.name);
            //});

            movies = dao_m.getAll();
            cbId.Items.Clear();
            movies.ForEach(movie =>
            {
                cbId.Items.Add(movie.id);
                addToDGV(movie);
            });

            setEnabled(false);
            cbId.Enabled = false;
        }

        public void addToDGV(Movie movie)
        {
            dgvMovie.Rows.Add(new object[]
            {
                movie.id,
                movie.name,
                movie.producer,
                dao_fm.getCategories(movie),
                movie.dateStart.ToString("dd-MM-yyyy"),
                movie.length,
                movie.language,
                movie.ageLimit
                //movie.distributor.name
            });
        }

        public void setEnabled(bool status)
        {
            //cbDistributor.Enabled = status;
            txtName.Enabled = dateStart.Enabled = txtDescription.Enabled = btnSelectImage.Enabled = numAge.Enabled = numLength.Enabled = txtProducer.Enabled = txtLanguage.Enabled = status;
        }

        public void resetTextBox()
        {
            txtDescription.Text = txtLanguage.Text = txtName.Text = txtProducer.Text = "";
            numLength.Value = numAge.Value = 0;
            pictureMovie.Image = null;
            pathImage = "";

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            resetTextBox();
            if(action != ADD)
            {
                action = ADD;
                cbId.Text = dao_m.generateId();
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
            resetTextBox();
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
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            resetTextBox();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = dgvMovie.SelectedRows.Count - 1; i >= 0; i--)
            {
                int index = dgvMovie.SelectedRows[i].Index;//Lấy vị trí hàng đang chọn thứ i
                if (index != -1)
                {
                    DialogResult answer = MessageBox.Show($"Bạn có chắc chắn xoá <{dgvMovie.Rows[index].Cells[1].Value}> ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer == DialogResult.Yes)
                    {
                        dao_m.deleteById(dgvMovie.Rows[index].Cells[0].Value.ToString());
                        int index_movie = movies.FindIndex(m => m.id == dgvMovie.Rows[index].Cells[0].Value.ToString());
                        if (index_movie != -1)
                        {
                            movies.RemoveAt(index_movie);
                            if (cbId.Items.Contains(dgvMovie.Rows[index].Cells[0].Value.ToString()))
                            {
                                cbId.Items.Remove(dgvMovie.Rows[index].Cells[0].Value.ToString());
                            }
                            dgvMovie.Rows.RemoveAt(index);
                        }
                    }
                }
            }
        }

        public string validate()
        {
            string error = "";

            //if (cbDistributor.Text == "")
            //{
            //    error += "Chưa chọn nhà phát hành\n";
            //}
            if (cbId.Text == "")
            {
                error += "Chưa chọn mã phim\n";
            }
            if (txtName.Text == "")
            {
                error += "Tên không được để trống\n";
            }
            if (numLength.Value == 0)
            {
                error += "Thời lượng phim phải lớn hơn 0\n";
            }
            if(txtProducer.Text == "")
            {
                error += "Đạo diễn không được để trống\n";
            }
            if(txtLanguage.Text == "")
            {
                error += "Ngôn ngữ không được để trống\n";
            }
            if(pathImage == "")
            {
                error += "Chưa chọn hình ảnh\n";
            }

            return error;
        }

        public Movie getData()
        {
            string error = validate();

            if (error == "")
            {
                Movie movie = new Movie()
                {
                    id = cbId.Text,
                    name = txtName.Text,
                    image = pathImage,
                    producer = txtProducer.Text,
                    dateStart = dateStart.Value,
                    language = txtLanguage.Text,
                    description = txtDescription.Text,
                    //distributor = distributors.Find(distributor=>distributor.name == cbDistributor.Text),
                    ageLimit = Convert.ToInt32(numAge.Value),
                    length = Convert.ToInt32(numLength.Value)
                };
                return movie;
            }
            else
            {
                MessageBox.Show(error, "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        public void addMovie(Movie movie)
        {
            dao_m.insertOne(movie);
            movies.Add(movie);
            cbId.Items.Add(movie.id);
            addToDGV(movie);
            cbId.Text = dao_m.generateId();
        }
        public void updateDGV(Movie movie)
        {
            for (int i = 0; i < dgvMovie.RowCount; i++)
            {
                if (dgvMovie.Rows[i].Cells[0].Value.ToString() == movie.id)
                {
                    dgvMovie.Rows[i].Cells[1].Value = movie.name;
                    dgvMovie.Rows[i].Cells[2].Value = movie.producer;
                    dgvMovie.Rows[i].Cells[3].Value = dao_fm.getCategories(movie);
                    dgvMovie.Rows[i].Cells[4].Value = movie.dateStart.ToString("dd-MM-yyyy");
                    dgvMovie.Rows[i].Cells[5].Value = movie.length;
                    dgvMovie.Rows[i].Cells[6].Value = movie.language;
                    dgvMovie.Rows[i].Cells[7].Value = movie.ageLimit;
                    //dgvMovie.Rows[i].Cells[8].Value = movie.distributor.name;
                    break;
                }
            }
        }
        public void editMovie(Movie movie)
        {
            dao_m.updateOne(movie);
            int index = movies.FindIndex(c => c.id == movie.id);
            if (index != -1)
            {
                movies[index] = movie;
            }
            updateDGV(movie);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Movie movie = getData();
            if (movie != null)
            {
                if (action == ADD)
                {
                    addMovie(movie);
                }
                if (action == EDIT)
                {
                    editMovie(movie);
                }
                resetTextBox();
            }
        }
        public void setData(Movie movie)
        {
            //cbDistributor.Text = movie.distributor.name;
            txtName.Text = movie.name;
            txtLanguage.Text = movie.language;
            txtProducer.Text = movie.producer;
            txtDescription.Text = movie.description;
            dateStart.Value = movie.dateStart;
            numAge.Value = movie.ageLimit;
            numLength.Value = movie.length;
            pictureMovie.Image = Image.FromFile(movie.image);
            pathImage = movie.image;
        }
        private void cbId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbId.SelectedIndex != -1)
            {
                Movie movie = movies.Find(c => c.id == cbId.Text);
                if (movie != null)
                {
                    setData(movie);
                }
            }
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string path = $@"..\..\public\img\{Path.GetFileName(openFileDialog.FileName)}";
                    if (!File.Exists(Path.GetFullPath(path)))
                    {
                        File.Copy(openFileDialog.FileName, Path.GetFullPath(path));
                    }
                    pictureMovie.Image = Image.FromFile(path);
                    pathImage = path;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    MessageBox.Show("File không hợp lệ");
                }
            }
        }

        private void dgvMovie_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(action == EDIT)
            {
                int index = e.RowIndex;
                if (index != -1)
                {
                    Movie movie = movies.Find(mov => mov.id == dgvMovie.Rows[index].Cells[0].Value.ToString());
                    if (movie != null)
                    {
                        setData(movie);
                        cbId.Text = movie.id;
                    }
                }
            }
        }
    }
}
