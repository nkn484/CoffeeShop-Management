using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class BillInforDAL
    {
        private static BillInforDAL instance;

        public static BillInforDAL Instance { get => instance == null ? instance = new BillInforDAL() : instance; set => instance = value; }


        /// <summary>
        /// Thêm thông tin bill nếu như thêm mới 1 bill. Cập nhật thông tin bill nếu như bill đã tồn tại
        /// </summary>
        /// <param name="billid"></param>
        /// <param name="drinkid"></param>
        /// <param name="count"></param>
        public void AddOrUpdateBillInfor(int billid, int drinkid, int count)
        {
            DataProvider.Instance.ExecuteNonQuery("SP_AddOrUpdateBillInfor @IDBILL, @IDDRINK, @COUNT", new object[] {billid, drinkid, count});
        }

    }
}
