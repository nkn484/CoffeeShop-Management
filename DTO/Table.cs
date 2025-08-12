using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Table
    {

        private int iD;
        private string tableName;
        private string status;

        public Table() { }
        public Table(int iD, string tableName, string status)
        {
            this.iD = iD;
            this.tableName = tableName;
            this.status = status;
        }
        public Table(DataRow rows) 
        {
            iD = (int)rows["ID"];
            tableName = (string)rows["Tên bàn"];
            status = (string)rows["Trạng thái"];
        }


        public int ID { get => iD; set => iD = value; }
        public string TableName { get => tableName; set => tableName = value; }
        public string Status { get => status; set => status = value; }
    }
}
