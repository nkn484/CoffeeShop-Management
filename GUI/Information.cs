using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App
{
    public partial class fInformation : Form
    {
        private Account acc;

        private event EventHandler<AccountEvent> updatedt;

        public event EventHandler<AccountEvent> Updatedt
        {
            add { updatedt += value; }
            remove { updatedt -= value; }
        }
        public fInformation(Account acc)
        {
            InitializeComponent();
            this.acc = acc;
            LoadData();
        }


        #region Method


        //Delegate kiểm tra nếu người dùng cập nhật thành công sẽ đóng form
        void FClosing()
        {
            Close();
        }


        void LoadData()
        {
            txtAccountType.Text = acc.AccountType;
            txtUsername.Text = acc.Username;
            txtDisplayname.Text = acc.Displayname;
        }


        /// <summary>
        /// Delegate thực hiện truyền tham số cho event của information truyền dữ liệu qua form manager
        /// </summary>
        void UpdateDisplayName()
        {
            updatedt(this, new AccountEvent(AccountDAL.Instance.LoadAccount(txtUsername.Text, acc.Password)));
        }


        #endregion


        private void btnUpdateInfor_Click(object sender, EventArgs e)
        {
             fPassWordInput input = new fPassWordInput(txtUsername.Text, txtDisplayname.Text);

//Gọi delegate thông qua đối tượng (Form con) được khởi tạo ,Gán các method cho delegate để thực thi
             input.Rename = UpdateDisplayName;
             input.Check = FClosing;

             input.ShowDialog();
        }
    }


    /// <summary>
    /// Lớp AccountEvent lưu dữ liệu tài khoản sau khi người dùng cập nhật thông tin
    /// </summary>
    public class AccountEvent : EventArgs
    {
        private Account acc;
        public AccountEvent(Account acc)
        {
            this.Acc = acc;
        }

        public Account Acc { get => acc; set => acc = value; }
    }
}
