using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DrinkDAL
    {
        private static DrinkDAL instance;

        public static DrinkDAL Instance { get => instance == null ? instance = new DrinkDAL() : instance; private set => instance = value; }

        /// <summary>
        /// Tải lên danh sách đồ uống theo danh mục
        /// </summary>
        /// <param name="id"></param>
        /// <returns>danh sách đồ uống theo mã của từng danh mục</returns>
        public List<Drink> LoadDrink(int id)
        {
            List<Drink> list = new List<Drink>();
            DataTable data = DataProvider.Instance.ExecuteQuery("select DRINKS.ID, DRINKS.Name, DRINKS.DrinkCategoryID ,DRINKS.Price from DRINKS join DRINK_CATEGORY on DRINK_CATEGORY.ID = DRINKS.DrinkCategoryID where DRINK_CATEGORY.ID = @ID", new object[] { id });
            foreach (DataRow row in data.Rows)
            {
                list.Add(new Drink(row));
            }
            return list;
        }



        /// <summary>
        /// Tìm kiếm đồ uống gần đúng
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns>Danh sách các đồ uống được tìm thấy qua từ khóa</returns>
        public DataTable DrinkListSearching(string keyword)
        {
            try
            {
                return DataProvider.Instance.ExecuteQuery($@"SELECT DRINKS.ID, DRINKS.Name AS [Tên đồ], DRINK_CATEGORY.Name AS [Danh mục] ,Price as [Giá tiền] FROM DRINKS JOIN DRINK_CATEGORY ON DRINKS.DrinkCategoryID = DRINK_CATEGORY.ID WHERE DRINKS.Name COLLATE SQL_Latin1_General_CP1_CI_AI LIKE N'%{keyword}%'");
            }
            catch { return null; }
        }



        public DataTable DrinkList()
        {
            try
            {
                return DataProvider.Instance.ExecuteQuery(@"SELECT DRINKS.ID, DRINKS.Name AS [Tên đồ], DRINK_CATEGORY.Name AS [Danh mục] ,Price as [Giá tiền] FROM DRINKS JOIN DRINK_CATEGORY ON DRINKS.DrinkCategoryID = DRINK_CATEGORY.ID");
            }
            catch { return null; }
        }


        /// <summary>
        /// Xóa đồ uống thông qua mã đồ uống
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Số dòng đã xóa</returns>
        public int DeleteDrink(int id, string name)
        {
            return DataProvider.Instance.ExecuteNonQuery("EXECUTE SP_DeleteDrink @ID, @NAME", new object[] { id, name });
        }


        /// <summary>
        /// Thêm đồ uống
        /// </summary>
        /// <param name="name"></param>
        /// <param name="categoryID"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public int AddDrink(string name, int categoryID, double price )
        {
            return DataProvider.Instance.ExecuteNonQuery(@"INSERT INTO DRINKS
                                                           VALUES ( @NAME, @CATEGORYID, @PRICE )", new object[] { name, categoryID, price });
        }


        /// <summary>
        /// Chỉnh sửa thông tin đồ uống
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="categoryID"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public int EditDrink(int id, string name, int categoryID, double price)
        {
            return DataProvider.Instance.ExecuteNonQuery(@"UPDATE DRINKS SET Name = @NAME, DrinkCategoryID = @CATEGORYID, Price = @PRICE WHERE ID = @ID", new object[] { name, categoryID, price, id });
        }

    }
}
