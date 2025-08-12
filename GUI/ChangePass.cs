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
    public partial class fChangePass : Form
    {
        private Account acc;

        private Action restart;
        public fChangePass(Account acc)
        {
            InitializeComponent();
            this.acc = acc;
        }

        public Action Restart { get => restart; set => restart = value; }



        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (AccountDAL.Instance.Login(acc.Username, txtOldpass.Text))
            {
                if (txtNewpass.Text != txtConfirmpass.Text)
                {
                    errorProvider1.Clear();
                    errorProvider1.SetError(txtConfirmpass, "Dữ liệu mật khẩu không trùng khớp");
                }
                else if(txtNewpass.Text == "")
                {
                    errorProvider1.Clear();
                    errorProvider1.SetError(txtNewpass, "Mật khẩu không được bỏ trống");
                }
                else
                {
                    AccountDAL.Instance.ChangePassword(acc.Username, txtNewpass.Text);
                    errorProvider1.Clear();
                    MessageBox.Show("Đổi mật khẩu thành công\nVui lòng đăng nhập lại ứng dụng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                    restart.Invoke();
                }
            }
            else
                MessageBox.Show("Sai mật khẩu! Vui lòng thử lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
