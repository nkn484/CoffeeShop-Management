using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App
{
    public partial class fLogin : Form
    {
        Account acc;
        public fLogin()
        {
            InitializeComponent();
        }

        #region Method

        void ResetDataLogin()
        {
            txtUsername.ResetText();
            txtPassword.ResetText();
        }

        #endregion


        #region Event

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (AccountDAL.Instance.Login(txtUsername.Text, txtPassword.Text))
            {
                acc = AccountDAL.Instance.LoadAccount(txtUsername.Text, txtPassword.Text);
                fManager manager = new fManager(acc);
                btnLogin.Visible = false;
                picLoad.Visible = true;

                await Task.Run(() =>
                {
                    return Task.Delay(4000);
                });

                this.Hide();
                manager.Reset = ResetDataLogin;
                notiLoginsucess.ShowBalloonTip(5500);
                manager.ShowDialog();
                this.Show();
                btnLogin.Visible = true;
                picLoad.Visible = false;
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác!\nVui lòng thử lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkViewPass_CheckedChanged(object sender, EventArgs e)
        {
            if(chkViewPass.Checked) txtPassword.UseSystemPasswordChar = false;
            else txtPassword.UseSystemPasswordChar = true;
        }
        #endregion

    }
}
