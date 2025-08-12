using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Bill
    {
        public Bill() { }
        private int iD;
        private DateTime? timeCheckIn;
        private DateTime? timeCheckOut;
        private bool status;
        private int discount;
        private double totalprice;

        public Bill(int iD, DateTime? timeCheckIn, DateTime? timeCheckOut, bool status, int discount, double totalprice)
        {
            this.iD = iD;
            this.timeCheckIn = timeCheckIn;
            this.timeCheckOut = timeCheckOut;
            this.status = status;
            this.discount = discount;
            this.totalprice = totalprice;
        }

        public Bill(DataRow row)
        {
            iD = (int)row["ID"];
            timeCheckIn = (DateTime?)row["TimeCheckIn"];
            timeCheckOut = timeCheckOut.ToString() == "" ? null : (DateTime?)row["TimeCheckOut"];
            status = (bool)row["Status"];
            discount = (int)row["Discount"];
            totalprice = (double)row["TotalPrice"];
        }

        public int ID { get => iD; set => iD = value; }
        public DateTime? TimeCheckIn { get => timeCheckIn; set => timeCheckIn = value; }
        public DateTime? TimeCheckOut { get => timeCheckOut; set => timeCheckOut = value; }
        public bool Status { get => status; set => status = value; }
        public int Discount { get => discount; set => discount = value; }
        public double Totalprice { get => totalprice; set => totalprice = value; }
    }
}
