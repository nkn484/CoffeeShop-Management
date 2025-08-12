using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AccountDAL
    {
        private static AccountDAL instance;

        /// <summary>
        /// Design pattern singleton
        /// </summary>
        public static AccountDAL Instance { get => instance == null ? instance = new AccountDAL() : instance; private set => instance = value; }

        public bool Login(string username, string password)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery(@"EXECUTE SP_UserLogin @Username, @Password", new object[] { username, password });
            return data.Rows.Count > 0;
        }


        public Account LoadAccount(string username, string password)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery(@"EXECUTE SP_UserLogin @Username, @Password", new object[] { username, password });
            foreach (DataRow row in data.Rows)
            {
                return new Account(row);
            }
            return null;
        }


        public void UpdateAccountInfor(string username, string newname)
        {
            DataProvider.Instance.ExecuteNonQuery("update ACCOUNT set DisplayName = @NEWNAME where Username = @USERNAME", new object[] { newname, username });
        }


        public void ChangePassword(string username, string password)
        {
            DataProvider.Instance.ExecuteNonQuery("UPDATE ACCOUNT SET Password = @PASSWORD where Username = @USERNAME", new object[] { password, username });
        }


        public DataTable AccountList()
        {
            try
            {
                return DataProvider.Instance.ExecuteQuery("SELECT USERNAME AS [Tên người dùng], DisplayName as [Tên hiển thị], AccountType as [Loại tài khoản] FROM ACCOUNT");
            }
            catch {  return null; }
        }

        public int AddAccount(string username, string displayname, string accounttype)
        {
            return DataProvider.Instance.ExecuteNonQuery("EXECUTE SP_AddAccount @USERNAME, @DISPLAYNAME, @ACCOUNTTYPE", new object[] {username, displayname, accounttype});
        }

        public int DeleteAccount(string username, string displayname)
        {
            return DataProvider.Instance.ExecuteNonQuery("DELETE ACCOUNT WHERE Username = @USERNAME AND Displayname = @DISPLAYNAME", new object[] {username, displayname });
        }

        public int EditAccount(string  username, string displayname, string accounttype)
        {
            return DataProvider.Instance.ExecuteNonQuery("UPDATE ACCOUNT SET Displayname = @DISPLAYNAME, AccountType = @ACCOUNTTYPE WHERE Username = @USERNAME1", new object[] { displayname, accounttype, username });
        }

        public int ResetPassword(string username, string accounttype)
        {
            return DataProvider.Instance.ExecuteNonQuery("UPDATE ACCOUNT SET Password = @ACCOUNTTYPE + '123' WHERE Username = @USERNAME", new object[] { accounttype, username });

        }
    }
}
