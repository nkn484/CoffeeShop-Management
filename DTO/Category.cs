using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Category
    {
        public Category() { }
        private int id;
        private string name;

        public Category(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public Category(DataRow row)
        {
            id = (int)row["ID"];
            name = row["Name"].ToString();
        }
        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
    }
}
