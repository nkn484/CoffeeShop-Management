using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class BillDetail
    {
        private int id;
        private string name;
        private int count;
        private double price;
        private double total;
        public BillDetail(int id, string name, int count, double price, double total)
        {
            this.id = id;
            this.name = name;
            this.count = count;
            this.price = price;
            this.total = total;
        }
        public BillDetail(DataRow row)
        {
            this.id = (int)row["BillID"];
            this.name = row["Name"].ToString();
            this.count = (int)row["Count"];
            this.price = (double)row["Price"];
            this.total = (double)row["SUM"];
        }
        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int Count { get => count; set => count = value; }
        public double Price { get => price; set => price = value; }
        public double Total { get => total; set => total = value; }
    }
}
