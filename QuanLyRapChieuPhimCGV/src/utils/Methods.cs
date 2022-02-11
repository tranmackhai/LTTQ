using QuanLyRapChieuPhimCGV.src.DAO;
using QuanLyRapChieuPhimCGV.src.models;
using System;

namespace QuanLyRapChieuPhimCGV.src.utils
{
    public class Methods
    {
        public DAO_Customer dao_cus = new DAO_Customer();
        public string addZero(int num, int value)
        {
            string result = "";
            for (int i = value.ToString().Length; i < num; i++)
            {
                result += "0";
            }
            return result + value;
        }
        public Customer upgradeCard(Customer customer)
        {
            if(customer != null)
            {
                DateTime today = DateTime.Today;
                int age = today.Year - customer.dayOfBirth.Year;
                if (customer.dayOfBirth > today.AddYears(-age))
                    age--;
                if(age > 22 && customer.id == "210001")
                {
                    customer.card.id = "210002";
                    dao_cus.updateOne(customer);
                }
                decimal expense = dao_cus.getExpenseOfCustomer(customer, 15);
                if (expense >= 2500000 && expense < 5000000)
                {
                    customer.card.id = "210003";
                    dao_cus.updateOne(customer);
                }
                else if (expense > 5000000)
                {
                    customer.card.id = "210004";
                    dao_cus.updateOne(customer);
                }
            }
            return customer;
        }
    }
}
