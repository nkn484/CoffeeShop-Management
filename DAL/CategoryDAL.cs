using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CategoryDAL
    {
        private static CategoryDAL instance;

        public static CategoryDAL Instance { get => instance == null ? instance = new CategoryDAL() : instance; private set => instance = value; }

        public List<Category> LoadCategory()
        {
            List<Category> list = new List<Category>();
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from DRINK_CATEGORY");
            foreach (DataRow row in data.Rows)
            {
                list.Add(new Category(row));
            }
            return list;
        }

        public DataTable CategoryList()
        {
            try
            {
                return DataProvider.Instance.ExecuteQuery("SELECT ID, NAME AS [Tên danh mục] FROM DRINK_CATEGORY");
            }
            catch {  return null; }
        }


        public DataTable CategoryListSearching(string keyword)
        {
            try
            {
                return DataProvider.Instance.ExecuteQuery($"SELECT ID, NAME AS [Tên danh mục] FROM DRINK_CATEGORY where Name COLLATE SQL_Latin1_General_CP1_CI_AI LIKE N'%{keyword}%'");
            }
            catch { return null; }
        }



        public int DeleteCategory(int id, string name)
        {
           return DataProvider.Instance.ExecuteNonQuery(@"EXECUTE SP_DeleteCategory @ID, @NAME", new object[] { id, name });
        }

        public int AddCategory(string name)
        {
            return DataProvider.Instance.ExecuteNonQuery(@"INSERT INTO DRINK_CATEGORY VALUES ( @NAME )", new object[] { name });
        }

        public int EditCategory(int id, string name)
        {
            return DataProvider.Instance.ExecuteNonQuery("UPDATE DRINK_CATEGORY SET Name = @NAME WHERE ID = @ID", new object[] {name ,id });
        }
    }
}
