using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyRapChieuPhimCGV.src.DAO
{
    public class DAO_Employee
    {
        private SqlConnection cnn;
        private SqlCommand scm;
        private SqlDataReader reader;
        public DAO_Employee()
        {
            cnn = new ConnectDatabase().cnn;
        }

        public List<Employee> getAll()//Lấy tất cả danh sách nhân viên
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                cnn.Open();
                string query = "select manv, tennv, cccd, gioitinh, sdt, vitri, luong, matkhau, quyen from nhanvien";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while(reader.Read()){
                    Employee employee = new Employee();
                    employee.id = reader.GetString(0);
                    employee.name = reader.GetString(1);
                    employee.person_id = reader.GetString(2);
                    employee.gender = (reader.GetBoolean(3) == true)?"Nam":"Nữ";
                    employee.phone = reader.GetString(4);
                    employee.position = reader.GetString(5);
                    employee.salary = reader.GetDecimal(6);
                    employee.password = reader.GetString(7);
                    employee.permission = reader.GetInt32(8);
                    employees.Add(employee);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
            return employees;
        }

        public Employee getById(string employeeId)//Lấy nhân viên theo mã nhân viên
        {
            Employee employee = null;
            try
            {
                cnn.Open();
                string query = $"select manv, tennv, cccd, gioitinh, sdt, vitri, luong, matkhau, quyen from nhanvien where manv = '{employeeId}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    employee = new Employee();
                    employee.id = reader.GetString(0);
                    employee.name = reader.GetString(1);
                    employee.person_id = reader.GetString(2);
                    employee.gender = (reader.GetBoolean(3) == true) ? "Nam" : "Nữ";
                    employee.phone = reader.GetString(4);
                    employee.position = reader.GetString(5);
                    employee.salary = reader.GetDecimal(6);
                    employee.password = reader.GetString(7);
                    employee.permission = reader.GetInt32(8);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
            return employee;
        }

        public Employee getByPhone(string phone)//Lấy nhân viên theo mã nhân viên
        {
            Employee employee = null;
            try
            {
                cnn.Open();
                string query = $"select manv, tennv, cccd, gioitinh, sdt, vitri, luong, matkhau, quyen from nhanvien where sdt = '{phone}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    employee = new Employee();
                    employee.id = reader.GetString(0);
                    employee.name = reader.GetString(1);
                    employee.person_id = reader.GetString(2);
                    employee.gender = (reader.GetBoolean(3) == true) ? "Nam" : "Nữ";
                    employee.phone = reader.GetString(4);
                    employee.position = reader.GetString(5);
                    employee.salary = reader.GetDecimal(6);
                    employee.password = reader.GetString(7);
                    employee.permission = reader.GetInt32(8);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
            return employee;
        }

        public void insertOne(Employee employee)//Thêm nhân viên
        {
            try
            {
                cnn.Open();
                string query = $@"insert into nhanvien(manv, tennv, cccd, gioitinh, sdt, vitri, luong, matkhau, quyen)
                           values ('{employee.id}',N'{employee.name}','{employee.person_id}',{(employee.gender == "Nam"?1:0)},'{employee.phone}',N'{employee.position}',{employee.salary},'{employee.password}',{employee.permission})";
                scm = new SqlCommand(query, cnn);
                scm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
        }

        public void updateOne(Employee employee)//Cập nhật nhân viên
        {
            try
            {
                cnn.Open();
                string query = $@"update nhanvien set tennv = N'{employee.name}', cccd = '{employee.person_id}', 
                    gioitinh = {(employee.gender == "Nam" ? 1 : 0)}, sdt = '{employee.phone}', vitri = N'{employee.position}', luong = {employee.salary}, 
                    matkhau = '{employee.password}', quyen = {employee.permission} where manv = '{employee.id}'";
                scm = new SqlCommand(query, cnn);
                scm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
        }

        public void deleteById(string employeeId)//Xoá nhân viên theo mã nhân viên
        {
            try
            {
                cnn.Open();
                string query = $@"delete from nhanvien where manv = '{employeeId}'";
                scm = new SqlCommand(query, cnn);
                scm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
        }

        public string login(string username, string password)//Đăng nhập
        {
            Employee employee = getById(username);
            string error = "";
            if(employee == null)
            {
                error = "Mã nhân viên không chính xác";
            }
            else
            {
                if(employee.password != password)
                {
                    error = "Mật khẩu không chính xác";
                }
            }
            return error;
        }

        public string changePassword(Employee employee, string oldPassword, string newPassword, string confirmNewPassword)
        {
            string error = "";
            if (employee.password != oldPassword)
            {
                error = "Mật khẩu cũ không chính xác";
            }
            else
            {
                if(newPassword != confirmNewPassword)
                {
                    error = "Xác nhận lại mật khẩu mới không chính xác";
                }
                else
                {
                    employee.password = newPassword;
                    updateOne(employee);
                }
            }
            return error;
        }

        public string generateId()//Tạo mã nhân viên
        {
            Methods methods = new Methods();
            string result = "";

            string str_now = DateTime.Now.Year.ToString().Substring(2, 2);//2 chữ số đầu tiên là năm hiện tại
            int i = 1;

            string str = str_now + methods.addZero(4, i);
            while (true)
            {
                if (getById(str) == null)
                {
                    result += str;
                    break;
                }
                else
                {
                    str = str_now + methods.addZero(4, ++i);
                }
            }

            return result;
        }
    }
}
