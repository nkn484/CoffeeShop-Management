using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DAL
{
    public class TableDAL
    {
        private static TableDAL instance;
        /// <summary>
        /// Design pattern singleton
        /// </summary>
        public static TableDAL Instance { get => instance == null ? instance = new TableDAL() : instance; private set => instance = value; }

        public static int TableWidth = 105;
        public static int TableHeigh = 105;


        public void SwapTable(int idl, int idr)
        {
            DataProvider.Instance.ExecuteNonQuery("EXECUTE SP_SwapTable @IDL, @IDR", new object[] { idl, idr });
        }

        public List<Table> LoadTable()
        {
            List<Table> list = new List<Table>();
            DataTable data = DataProvider.Instance.ExecuteQuery("EXECUTE SP_TableLoad");
            foreach (DataRow row in data.Rows)
            {
                Table table = new Table(row);
                list.Add(table);
            }
            return list;
        }
        

        public DataTable TableList()
        {
            try
            {
                return DataProvider.Instance.ExecuteQuery("EXECUTE SP_TableLoad");
            }
            catch {  return null; }
        }


        public DataTable TableListSearching(string keyword)
        {
            try
            {
                return DataProvider.Instance.ExecuteQuery($@"SELECT ID, TableNumber AS [Tên bàn], Status as [Trạng thái] FROM TABLES WHERE TableNumber COLLATE SQL_Latin1_General_CP1_CI_AI LIKE N'%{keyword}%'");
            }catch { return null; }
        }


        /// <summary>
        /// Cập nhật lại danh sách bàn sau các thao tác (Thêm, xóa, sửa, đổi) bàn
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        public void UpdateStatusTable(int id, string status)
        {
            DataProvider.Instance.ExecuteNonQuery(@"UPDATE TABLES SET STATUS = @STATUS WHERE ID = @ID ", new object[] { status, id });
        }


        public void MergeTable(int idl, int idr)
        {
            DataProvider.Instance.ExecuteNonQuery("EXECUTE SP_MergeTable @IDL, @IDR", new object[] {idl, idr});
        }


        /// <summary>
        /// Lấy ra mã bàn thông qua mã đồ uống
        /// </summary>
        /// <param name="DrinkID"></param>
        /// <returns></returns>
        public int GetTableIDByDrinkID(int DrinkID)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SP_GetTableIDByDrinkID @ID", new object[] { DrinkID });
            foreach (DataRow row in data.Rows)
            {
                return (int)row[0];
            }
            return -1;
        }


        public int AddTable(string tableName)
        {
            return DataProvider.Instance.ExecuteNonQuery(@"INSERT INTO TABLES VALUES ( @TABLENAME, N'Trống')", new object[]{tableName});
        }


        public int DeleteTable(int IDtable, string TableName)
        {
            return DataProvider.Instance.ExecuteNonQuery("EXECUTE SP_DeleteTable @ID, @TABLENAME", new object[] {IDtable, TableName});
        }


        public int EditTable(int IDtable, string tableName, string status)
        {
            return DataProvider.Instance.ExecuteNonQuery("UPDATE TABLES SET TableNumber = @TABLENAME, Status = @STATUS WHERE ID = @ID", new object[] { tableName, status, IDtable });
        }
    }
}
