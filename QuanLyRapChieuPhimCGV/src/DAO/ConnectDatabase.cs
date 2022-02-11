using System.Data.SqlClient;

namespace QuanLyRapChieuPhimCGV.src.DAO
{
    public class ConnectDatabase
    {
        public SqlConnection cnn = new SqlConnection(@"Data Source=CHIBISUKE\SQLEXPRESS;Initial Catalog=QLRapChieuPhimCGV;User ID=sa;Password=123");
    }
}
