using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class BillDAL
    {
        private static BillDAL instance;

        public static BillDAL Instance { get => instance == null ? instance = new BillDAL() : instance; set => instance = value; }


        /// <summary>
        /// Lấy ra ID hóa đơn chưa thanh toán của bàn hiện tại 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>BillID nếu tìm thấy, ngược lại trả về -1</returns>
        public int GetBillID(int id)
        {
          DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM BILL WHERE TABLEID = @ID AND STATUS = 0", new object[] { id });
            try
            {
              return (int)data.Rows[0]["ID"];
            }
            catch { return -1; }
        }


        public int CreateNewIDBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("SELECT MAX(ID) FROM BILL");
            }catch { return 1; }
        }


        public void AddBill(int id, int cnt)
        {
            DataProvider.Instance.ExecuteNonQuery("EXECUTE SP_AddBill @ID, @CNT", new object[] { id, cnt });
        }




        /// <summary>
        /// Cập nhật trạng thái hóa đơn
        /// </summary>
        /// <param name="id"></param>
        /// <param name="discount"></param>
        /// <param name="totalprice"></param>
        public void UpdateBill(int id, int idtable ,int discount, double totalprice)
        {
            DataProvider.Instance.ExecuteNonQuery("UPDATE BILL SET TimeCheckOut = GETDATE(), Status = 1, Discount = @DISCOUNT, TotalPrice = @TOTALPRICE WHERE TableID = @IDTABLE AND ID = @ID", new object[] { discount, totalprice, idtable,id });
        }




        /// <summary>
        /// Thống kê hóa đơn
        /// </summary>
        /// <param name="In"></param>
        /// <param name="Out"></param>
        /// <returns>Bảng hóa đơn</returns>
        public DataTable GetBills(DateTime In, DateTime Out, int page)
        {
           return DataProvider.Instance.ExecuteQuery("EXECUTE SP_SearchBill @IN, @OUT, @PAGE", new object[] {In, Out, page });
        }




        /// <summary>
        /// Tính tổng doanh thu
        /// </summary>
        /// <param name="In"></param>
        /// <param name="Out"></param>
        /// <returns>Tổng doanh thu trong khoảng In -> Out</returns>
        public double TotalRevenue(DateTime In, DateTime Out)
        {
            try
            {
                return (double)DataProvider.Instance.ExecuteScalar(@"SELECT SUM(TotalPrice) FROM BILL WHERE BILL.Status = 1 AND TimeCheckOut >= @In AND TimeCheckOut <= @Out", new object[] { In, Out });
            }
            catch { return 0; }
        }

        public int GetAllRows(DateTime In, DateTime Out)
        {
            return (int)DataProvider.Instance.ExecuteScalar(@"SELECT COUNT(*) FROM BILL WHERE BILL.Status = 1 AND TimeCheckOut >= @In AND TimeCheckOut <= @Out", new object[] { In, Out });

        }

    }
}
