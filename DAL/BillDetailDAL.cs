using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class BillDetailDAL
    {
        private static BillDetailDAL instance;
        /// <summary>
        /// Design pattern singleton
        /// </summary>
        public static BillDetailDAL Instance { get => instance == null ? instance = new BillDetailDAL() : instance; private set => instance = value; }

        
        /// <summary>
        /// Lấy ra thông tin hóa đơn chi tiết dựa trên danh sách bàn, đồ uống, hóa đơn và thông tin hóa đơn đó
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Hóa đơn chưa thanh toán của mỗi bàn</returns>
        public List<BillDetail> LoadBill(int id)
        {
            List<BillDetail> list = new List<BillDetail>();
            DataTable data = DataProvider.Instance.ExecuteQuery("select BillID, DRINKS.Name, BILL_INFOMATION.Count, Price, BILL_INFOMATION.Count * Price AS SUM from DRINKS join BILL_INFOMATION ON DRINKS.ID = BILL_INFOMATION.DrinkID JOIN BILL ON BILL.ID = BILL_INFOMATION.BillID WHERE BILL.TableID = @ID AND BILL.Status = 0", new object[] { id });
            foreach (DataRow row in data.Rows)
            {
                BillDetail bill = new BillDetail(row);
                list.Add(bill);
            }
            return list;
        }


        public DataTable BillDetalList(int idtable)
        {
            return DataProvider.Instance.ExecuteQuery("EXEC SP_ExtractBill @ID", new object[] {idtable});
        }

        public DateTime GetBillDate(int idtable) 
        {
            return (DateTime)DataProvider.Instance.ExecuteScalar("EXEC SP_GetDatetimeBill @IDTABLE", new object[] { idtable });
        }
    }
}
