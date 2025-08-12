using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class BillInfor
    {
        public BillInfor() { }

        private int billID;
        private int drinkID;
        private int count;

        public BillInfor(int billID, int drinkID, int count)
        {
           this.billID = billID;
           this.drinkID = drinkID;
           this.count = count;
        }

        public BillInfor(DataRow row)
        {
            billID = (int)row["BillID"];
            drinkID = (int)row["DrinkID"];
            count = (int)row["count"];
        }
        public int BillID { get => billID; set => billID = value; }
        public int DrinkID { get => drinkID; set => drinkID = value; }
        public int Count { get => count; set => count = value; }
    }
}
