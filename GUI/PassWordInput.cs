using DAL;
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
    public partial class fPassWordInput : Form
    {
        private string username;
        private string displayname;

        private Action check;
        private Action rename;

        public Action Check { get => check; set => check = value; }
        public Action Rename { get => rename; set => rename = value; }

        public fPassWordInput(string username, string displayname)
        {
            InitializeComponent();
            this.username = username;
            this.displayname = displayname;
        }

        private void chkSeenpass_CheckedChanged(object sender, EventArgs e)
        {
            if(chkSeenpass.Checked) txtPass.UseSystemPasswordChar = false;
            else txtPass.UseSystemPasswordChar = false;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (AccountDAL.Instance.Login(username, txtPass.Text))
            {
                AccountDAL.Instance.UpdateAccountInfor(username, displayname);
                MessageBox.Show("Cập nhật thông tin thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                check.Invoke();
                //Thực thi các delegate khi người dùng nhập đúng mật khẩu
                rename.Invoke();
            }
            else MessageBox.Show("Sai mật khẩu! Vui lòng thử lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
