using System;

namespace QuanLyRapChieuPhimCGV.src.models
{
    public class Bill
    {
        public string id;
        public DateTime date;
        public Employee employee;
        public Customer customer;
        public decimal totalPrice;
        public int point;

        public Bill() { }
    }
}
