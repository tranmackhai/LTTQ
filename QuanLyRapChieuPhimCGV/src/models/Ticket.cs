using System;

namespace QuanLyRapChieuPhimCGV.src.models
{
    public class Ticket
    {
        public string id;
        public Schedule schedule;
        public Customer customer;
        public Employee employee;
        public Chair chair;
        public TicketPrice price;
        public decimal totalPrice;
        public DateTime date;
        public int point;//Trả bằng điểm
        public Ticket() { }
    }
}
