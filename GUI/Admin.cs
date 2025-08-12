using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using DTO;

namespace App
{
    public partial class fAdmin : Form
    {
        //Tạo liên kết Lưu lại Binding để khi làm mới lại dữ liệu sẽ không bị mất Binding
        #region Source

        BindingSource DrinkSource = new BindingSource();

        BindingSource TableSource = new BindingSource();

        BindingSource AccountSource = new BindingSource();

        BindingSource CategorySource = new BindingSource();

        #endregion


        #region Delegate

        private Action<int> tableRefresh;
        private Action<int> drinkListRefresh;

        private Action reLoadTable;

        private Action reLoadCategory;


        public Action<int> TableRefresh { get => tableRefresh; set => tableRefresh = value; }
        public Action<int> DrinkListRefresh { get => drinkListRefresh; set => drinkListRefresh = value; }
        public Action ReLoadTable { get => reLoadTable; set => reLoadTable = value; }
        public Action ReLoadCategory { get => reLoadCategory; set => reLoadCategory = value; }

        #endregion


        //Check xem tài khoản có đang đăng nhập không ? 
        public Account acc;

        public fAdmin()
        {
            InitializeComponent();
            Loading();

            AddBindingDrink();
            AddBindingTable();
            AddBindingAccount();
            AddBindingCategory();
        }


        #region Method
        void Loading()
        {
            LoadDate();
            LoadDrinks();
            LoadCategoryToCombobox();
            LoadTables();
            LoadAccount();
            LoadCategory();

            dtgvDrinks.DataSource = DrinkSource;
            dtgvTables.DataSource = TableSource;
            dtgvAccount.DataSource = AccountSource;
            dtgvCategory.DataSource = CategorySource;
        }



        #region Tab Doanh thu

        //Thiết lập thời gian mặc định cho datetimePicker, phạm vi nhập xuất hóa đơn trong 1 tháng
        void LoadDate()
        {
            DateTime now = DateTime.Now;
            dtpBeginDate.Value = new DateTime(now.Year, now.Month, 1);
            dtpEndDate.Value = dtpBeginDate.Value.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
        }


        #endregion



        #region Tab Đồ uống

        void LoadDrinks()
        {
            DrinkSource.DataSource = DrinkDAL.Instance.DrinkList();
        }


        void LoadCategoryToCombobox()
        {
           cbCategory.DataSource = CategoryDAL.Instance.LoadCategory();
           cbCategory.DisplayMember = "Name";
        }

        void AddBindingDrink()
        {
            txtIDrink.DataBindings.Add(new Binding("Text", dtgvDrinks.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txtNamedrink.DataBindings.Add(new Binding("Text", dtgvDrinks.DataSource, "Tên đồ", true, DataSourceUpdateMode.Never));
            cbCategory.DataBindings.Add(new Binding("Text", dtgvDrinks.DataSource, "Danh mục", true, DataSourceUpdateMode.Never));
            numPrice.DataBindings.Add(new Binding("Value", dtgvDrinks.DataSource, "Giá tiền", true, DataSourceUpdateMode.Never));
        }

        #endregion



        #region Tab Bàn

        void LoadTables()
        {
            TableSource.DataSource = TableDAL.Instance.TableList();
        }


        void AddBindingTable()
        {
            txtIDtable.DataBindings.Add(new Binding("Text", dtgvTables.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txtNametable.DataBindings.Add(new Binding("Text", dtgvTables.DataSource, "Tên bàn", true, DataSourceUpdateMode.Never));
            txtStatus.DataBindings.Add(new Binding("Text", dtgvTables.DataSource, "Trạng thái", true, DataSourceUpdateMode.Never));
        }

        #endregion



        #region Tab Tài khoản
        void LoadAccount()
        {
            AccountSource.DataSource = AccountDAL.Instance.AccountList();
        }

        void AddBindingAccount()
        {
            txtDisplayname.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "Tên hiển thị", true, DataSourceUpdateMode.Never));
            txtNameacc.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "Tên người dùng", true, DataSourceUpdateMode.Never));
            cbAccountType.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "Loại tài khoản", true, DataSourceUpdateMode.Never));

        }

        #endregion



        #region Tab Danh mục

        void LoadCategory()
        {
           CategorySource.DataSource = CategoryDAL.Instance.CategoryList();
        }


        void AddBindingCategory()
        {
            txtIDcategory.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txtNamecategory.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "Tên danh mục", true, DataSourceUpdateMode.Never));
        }

        #endregion


        #endregion




        #region Event


        #region Tab Doanh thu

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BillDAL.Instance.GetBills(dtpBeginDate.Value, dtpEndDate.Value, 1);
            txtPage.Text = "1";
            CultureInfo cul = new CultureInfo("vi-VN");
            lblTotalrevenue.Text = BillDAL.Instance.TotalRevenue(dtpBeginDate.Value, dtpEndDate.Value).ToString("C0",cul);
        }


        private void btnFirst_Click(object sender, EventArgs e)
        {
            txtPage.Text = "1";
        }


        private void btnPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                int page = int.Parse(txtPage.Text);
                if (page > 1) --page;

                txtPage.Text = page.ToString();
            }
            catch
            {
                MessageBox.Show("Không có hóa đơn\nBấm [Tìm kiếm] để xem danh sách hóa đơn", "Quản lý", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }


        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                int page = int.Parse(txtPage.Text);
                int rows = BillDAL.Instance.GetAllRows(dtpBeginDate.Value, dtpEndDate.Value);

                if (page <= rows / 10) ++page;

                txtPage.Text = page.ToString();
            }catch
            {
                MessageBox.Show("Không có hóa đơn\nBấm [Tìm kiếm] để xem danh sách hóa đơn", "Quản lý", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
            
        }


        private void btnLast_Click(object sender, EventArgs e)
        {
            int rows = BillDAL.Instance.GetAllRows(dtpBeginDate.Value, dtpEndDate.Value);
            int numpage = rows / 10;

            if(numpage % 10 != 0) ++numpage;
            if (numpage <= 0) numpage = 1;
            txtPage.Text = numpage.ToString();
        }


        private void txtPage_TextChanged(object sender, EventArgs e)
        {
            int page = int.Parse(txtPage.Text);
            dtgvBill.DataSource = BillDAL.Instance.GetBills(dtpBeginDate.Value, dtpEndDate.Value, page);
        }


        #endregion



        #region Tab đồ uống

        private void btnViewdrink_Click(object sender, EventArgs e)
        {
            LoadDrinks();
        }


        //Xóa đồ uống
        private void btnDeletedrink_Click(object sender, EventArgs e)
        {

            int drinkID = int.Parse(txtIDrink.Text);
            string DrinkName = txtNamedrink.Text;

            //Cần lấy ra ID của bàn trước khi xóa tránh việc không tìm thấy ID bàn do đã bị xóa
            int tableID = TableDAL.Instance.GetTableIDByDrinkID(drinkID);

            if (DrinkDAL.Instance.DeleteDrink(drinkID, DrinkName) > 0)
            {
                //Thực thi Delegate làm mới danh sách bàn và hóa đơn sau khi xóa đồ uống
                tableRefresh.Invoke(tableID);

                MessageBox.Show("Xóa đồ uống thành công", "Quản lý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDrinks();
            }
            else
            {
                MessageBox.Show("Không thể xóa đồ\nVui lòng kiểm tra thông tin đồ uống trước khi xóa", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        //Thêm đồ uống
        private void btnAdddrink_Click(object sender, EventArgs e)
        {
            string name = txtNamedrink.Text;
            int categoryID = (cbCategory.SelectedItem as Category).Id;
            double price = (double)numPrice.Value;

            if (name != "" && DrinkDAL.Instance.AddDrink(name, categoryID, price) > 0)
            {
                MessageBox.Show("Thêm đồ uống thành công", "Quản lý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDrinks();
            }
            else
            {
                MessageBox.Show("Không thể thêm đồ uống", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        //Sửa đồ
        private void btnEditdrink_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtIDrink.Text);
            string name = txtNamedrink.Text;
            int categoryID = (cbCategory.SelectedItem as Category).Id;
            double price = (double)numPrice.Value;

            if (name != "" && DrinkDAL.Instance.EditDrink(id, name, categoryID, price) > 0)
            {
                //Thực thi delegate làm mới danh sách đồ uống khi sửa lại thông tin 
                drinkListRefresh.Invoke(categoryID);

                MessageBox.Show("Sửa thông tin đồ uống thành công", "Quản lý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDrinks();
            }
            else
            {
                MessageBox.Show("Không thể sửa thông tin đồ uống", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        //Tìm kiếm gần đúng đồ uống
        private void btnSearchdrink_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            string keyword = txtSearchdrink.Text;
            if(keyword.Contains(';') || keyword.Contains("'") || keyword.Contains("--"))
            {
                errorProvider1.SetError(txtSearchdrink, "Từ khóa không hợp lệ");
                return;
            }

            DrinkSource.DataSource = DrinkDAL.Instance.DrinkListSearching(keyword);
        }


        #endregion



        #region Tab Bàn

//
        private void btnViewtable_Click(object sender, EventArgs e)
        {
            LoadTables();
        }



//
        private void btnAddtable_Click(object sender, EventArgs e)
        {
            string name = txtNametable.Text;

            if (name != "" && TableDAL.Instance.AddTable(name) > 0)
            {
                reLoadTable.Invoke();
                MessageBox.Show("Thêm bàn thành công", "Quản lý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTables();
            }
            else
            {
                MessageBox.Show("Không thể thêm bàn", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



//
        private void btnDeletetable_Click(object sender, EventArgs e)
        {
            int idtable = int.Parse(txtIDtable.Text);
            string TableName = txtNametable.Text;

            if(MessageBox.Show("Xóa bàn có thể dẫn đến việc mất hóa đơn nếu bàn đó có hóa đơn đã thanh toán hoặc lỗi không mong muốn\nBạn có chắc vẫn muốn xóa ?", "Cảnh báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (TableDAL.Instance.DeleteTable(idtable, TableName) > 0)
                {
                    reLoadTable.Invoke();
                    MessageBox.Show("Xóa bàn thành công", "Quản lý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadTables();
                }
                else
                {
                    MessageBox.Show("Không thể xóa bàn\nVui lòng kiểm tra kỹ thông tin bàn trước khi xóa", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



//
        private void btnEdittable_Click(object sender, EventArgs e)
        {
            int idtable = int.Parse(txtIDtable.Text);
            string name = txtNametable.Text;
            string status = txtStatus.Text;

            if (name != "" && TableDAL.Instance.EditTable(idtable, name, status) > 0)
            {
                reLoadTable.Invoke();
                MessageBox.Show("Chỉnh sửa bàn thành công", "Quản lý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTables();
            }
            else
            {
                MessageBox.Show("Không thể chỉnh sửa bàn", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


//
        private void btnSearchtable_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            string keyword = txtSearchtable.Text;
            if (keyword.Contains(';') || keyword.Contains("'") || keyword.Contains("--"))
            {
                errorProvider1.SetError(txtSearchtable, "Từ khóa không hợp lệ");
                return;
            }

           TableSource.DataSource = TableDAL.Instance.TableListSearching(keyword);
        }


        #endregion



        #region Tab Tài khoản

        private void btnViewacc_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }


        private void btnAddacc_Click(object sender, EventArgs e)
        {
            string username = txtNameacc.Text;
            string displayname = txtDisplayname.Text;
            string accounttype = cbAccountType.Text;
            if(AccountDAL.Instance.AddAccount(username, displayname, accounttype) > 0)
            {
                MessageBox.Show("Thêm tài khoản thành công", "Quản lý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadAccount();
            }
            else
            {
                MessageBox.Show("Không thể thêm tài khoản\nTên tài khoản đã được sử dụng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnDeleteacc_Click(object sender, EventArgs e)
        {
            string username = txtNameacc.Text;
            string displayname = txtDisplayname.Text;

            if(username == acc.Username)
            {
                MessageBox.Show("Bạn không thể xóa tài khoản của chính mình", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (AccountDAL.Instance.DeleteAccount(username, displayname) > 0)
            {
                MessageBox.Show("Xóa tài khoản thành công", "Quản lý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadAccount();
            }
            else
            {
                MessageBox.Show("Không thể xóa tài khoản", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnEditacc_Click(object sender, EventArgs e)
        {
            string username = txtNameacc.Text;
            string displayname = txtDisplayname.Text;
            string accounttype = cbAccountType.Text;
            if (AccountDAL.Instance.EditAccount(username, displayname, accounttype) > 0)
            {
                MessageBox.Show("Cập nhật tài khoản thành công\nVui lòng đăng nhập lại để hệ thống tải dữ liệu", "Quản lý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadAccount();
            }
            else
            {
                MessageBox.Show("Không thể sửa tên tài khoản", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnResetaccount_Click(object sender, EventArgs e)
        {
            string username = txtNameacc.Text;
            string accounttype = cbAccountType.Text;

            if(MessageBox.Show($"Xác nhận thiết lập lại mật khẩu cho tài khoản {username}", "Quản lý", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (AccountDAL.Instance.ResetPassword(username, accounttype) > 0)
                {
                    MessageBox.Show($"Đặt lại mật khẩu cho tài khoản {username} thành công\nMật khẩu mới là: {accounttype}123", "Quản lý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không thể đặt lại mật khẩu", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion



        #region Tab Danh mục

        private void btnViewcategory_Click(object sender, EventArgs e)
        {
            LoadCategory();
        }


        private void btnAddcategory_Click(object sender, EventArgs e)
        {
            string name = txtNamecategory.Text;

            if(name != "" && CategoryDAL.Instance.AddCategory(name) > 0)
            {
                reLoadCategory.Invoke();
                MessageBox.Show("Thêm danh mục thành công", "Quản lý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCategory();
                LoadDrinks(); 
                LoadCategoryToCombobox();
            }
            else
            {
                MessageBox.Show("Không thể thêm danh mục\nVui lòng kiểm tra thông tin danh mục trước khi thêm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnDeletecategory_Click(object sender, EventArgs e)
        {
            int categoryID = int.Parse(txtIDcategory.Text);
            string Categoryname = txtNamecategory.Text;

            if(MessageBox.Show($"Bạn có chắc muốn xóa danh mục {Categoryname} ?\nTất cả đồ uống thuộc danh mục trên sẽ bị xóa", "Cảnh báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (CategoryDAL.Instance.DeleteCategory(categoryID, Categoryname) > 0)
                {
                    //Thực thi Delegate làm mới danh sách bàn và danh mục sau khi xóa 
                    reLoadCategory.Invoke();
                    MessageBox.Show("Xóa danh mục thành công", "Quản lý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCategory();
                    LoadDrinks();
                    LoadCategoryToCombobox();
                }
                else
                {
                    MessageBox.Show("Không thể xóa danh mục\nVui lòng kiểm tra thông tin danh mục trước khi xóa", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnEditcategory_Click(object sender, EventArgs e)
        {
            int idcategory = int.Parse(txtIDcategory.Text);
            string namecategory = txtNamecategory.Text;
            if (namecategory != "" && CategoryDAL.Instance.EditCategory(idcategory, namecategory) > 0)
            {
                reLoadCategory.Invoke();
                MessageBox.Show("Chỉnh sửa danh mục thành công", "Quản lý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCategory();
                LoadDrinks();
                LoadCategoryToCombobox();
            }
            else
            {
                MessageBox.Show("Không thể chỉnh sửa danh mục\nVui lòng kiểm tra thông tin danh mục trước khi sửa", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnSearchcategory_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            string keyword = txtSearchcategory.Text;
            if (keyword.Contains(';') || keyword.Contains("'") || keyword.Contains("--"))
            {
                errorProvider1.SetError(txtSearchcategory, "Từ khóa không hợp lệ");
                return;
            }

            CategorySource.DataSource = CategoryDAL.Instance.CategoryListSearching(keyword);
        }




        #endregion

        #endregion

 
    }
}
