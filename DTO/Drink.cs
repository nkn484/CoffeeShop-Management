using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Drink
    {
        public Drink() { }

        private int id;
        private string name;
        private int categoryID;
        private double price;

        public Drink(int id, string name, int categoryID, double price)
        {
            this.id = id;
            this.name = name;
            this.categoryID = categoryID;
            this.price = price;
        }

        public Drink(DataRow row)
        {
            this.id = (int)row["ID"];
            this.name = row["Name"].ToString();
            this.categoryID = (int)row["DrinkCategoryID"];
            this.price = (double)row["Price"];
        }
        public string Name { get => name; set => name = value; }
        public int CategoryID { get => categoryID; set => categoryID = value; }
        public double Price { get => price; set => price = value; }
        public int Id { get => id; set => id = value; }
    }
}
