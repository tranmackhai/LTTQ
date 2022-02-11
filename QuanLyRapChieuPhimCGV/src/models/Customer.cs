using System;

namespace QuanLyRapChieuPhimCGV.src.models
{
    public class Customer
    {
        public string id;
        public string phone;
        public string email;
        public string gender;
        public DateTime dayOfBirth;
        public Card card;
        public float totalPoint;

        public Customer() { }
    }
}
