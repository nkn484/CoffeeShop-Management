using DAL;
using DTO;
using GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace App
{
    public partial class fManager : Form
    {

        #region Delegate

        private Action reset;
        private Account acc;
        public Action Reset { get => reset; set => reset = value; }

        #endregion


        bool moving;
        int x, y;

        public fManager(Account acc)
        {
            InitializeComponent();
            LoadTable();
            LoadCategory();
            LoadcbTables();
            this.acc = acc;
            DecentralizationAndLoadName();
        }
        


        #region Method


        /// <summary>
        /// Làm mới dữ liệu khi xóa đồ uống trong mục Quản lý
        /// </summary>
        /// <param name="TableID"></param>
        void RefreshTable(int TableID)
        {
            LoadBill(TableID);
            if(BillDAL.Instance.GetBillID(TableID) == -1)
            {
                TableDAL.Instance.UpdateStatusTable(TableID, "Trống");
            }
            LoadTable();
            LoadcbTables();
        }


        void DecentralizationAndLoadName()
        {
            AdminToolStripMenuItem.Enabled = acc.AccountType == "ADMIN" ? true : false;
            InforToolStripMenuItem.Text += ("  " + $"({acc.Displayname})");
        }


        void LoadcbTables()
        {
            List<Table> table = TableDAL.Instance.LoadTable();
            cbTables.DataSource = table;
            cbTables.DisplayMember = "TableName";
        }


        void LoadTable()
        {
            flpTables.Controls.Clear();
            List<Table> Tables = TableDAL.Instance.LoadTable();
            foreach (Table table in Tables)
            {
                Button btn = new Button()
                {
                    Text = $"{table.TableName}\nTrạng thái: {table.Status}",
                    Size = new Size(TableDAL.TableWidth, TableDAL.TableHeigh),
                    Font = new Font("Microsoft Sans Serif", 10 ,FontStyle.Regular)
                };
                btn.Cursor = Cursors.Hand;
                btn.Tag = table;
                btn.Click += Btn_Click;
                var path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddArc(0, 0, 20, 20, 180, 90);
                path.AddArc(btn.Width - 20, 0, 20, 20, 270, 90);
                path.AddArc(btn.Width - 20, btn.Height - 20, 20, 20, 0, 90);
                path.AddArc(0, btn.Height - 20, 20, 20, 90, 90);
                path.CloseAllFigures();
                btn.Region = new Region(path);
                switch (table.Status)
                {
                    case "Có khách":
                        btn.BackColor = Color.Green;
                        break;
                    case "Khách đặt trước":
                        btn.BackColor = Color.Yellow;
                        break;
                    default:
                        btn.BackColor = Color.White;
                        break;
                }
                flpTables.Controls.Add(btn);
            }
        }


        void LoadCategory()
        {
            cbCategory.DataSource = CategoryDAL.Instance.LoadCategory();
            cbCategory.DisplayMember = "Name";
        }



        /// <summary>
        /// Tải danh sách đồ uống dựa theo mã danh mục lên combobox
        /// </summary>
        /// <param name="id"></param>
        void LoadDrink(int id)
        {
            cbDrinkName.DataSource = DrinkDAL.Instance.LoadDrink(id);
            cbDrinkName.DisplayMember = "Name";
        }



        /// <summary>
        /// Tải lên danh sách hóa đơn của khách hàng mỗi khi nhấn vào từng bàn
        /// </summary>
        /// <param name="id"></param> id của bàn chứa hóa đơn đó
        void LoadBill(int id)
        {
            lvBills.Items.Clear();
            numDiscount.Value = 0;
            List<BillDetail> Bills = BillDetailDAL.Instance.LoadBill(id);

            //Chỉnh thành đơn vị tiền tệ của Việt Nam
            CultureInfo cul = new CultureInfo("vi-VN");
            double sum = 0;
            foreach (BillDetail bill in Bills)
            {
                sum += bill.Total;
                ListViewItem item = new ListViewItem(bill.Id.ToString());
                item.SubItems.Add(bill.Name);
                item.SubItems.Add(bill.Count.ToString());
                item.SubItems.Add(bill.Price.ToString("n0", cul));
                item.SubItems.Add(bill.Total.ToString("n0", cul));
                lvBills.Items.Add(item);
            }
            txtPriceSum.Text = sum.ToString("c0", cul);
        }


        string GetTotalBill(int id)
        {
            List<BillDetail> Bills = BillDetailDAL.Instance.LoadBill(id);

            //Chỉnh thành đơn vị tiền tệ của Việt Nam
            CultureInfo cul = new CultureInfo("vi-VN");
            double sum = 0;
            foreach (BillDetail bill in Bills)
            {
                sum += bill.Total;
            }
            return sum.ToString("C0", cul);
        }


        double CurrentPrice()
        {
            double sum = 0;
            foreach (ListViewItem item in lvBills.Items)
            {
                string tmp = item.SubItems[4].Text;
                tmp = tmp.Replace(".", "");
                sum += double.Parse(tmp);
            }
            return sum;
        }


        #endregion




        #region Event
        private void tàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fInformation inf = new fInformation(acc);

            //Gọi event thông qua đối tượng được khởi tạo trong form chính, gán các method cho event để thực thi
            inf.Updatedt += Inf_Updatedt;

            inf.ShowDialog();
        }


        /// <summary>
        /// Event thực hiện chức năng cập nhật thông tin người dùng khi có thay đổi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Inf_Updatedt(object sender, AccountEvent e)
        {
            acc = e.Acc;
            InforToolStripMenuItem.Text = "Thông tin  " + $"({acc.Displayname})";
        }


        private void quảnLýToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin ad = new fAdmin();
            ad.acc = acc;

            //Gán phương thức cho Delegate để thực thi 
            ad.TableRefresh = RefreshTable;
            ad.DrinkListRefresh = LoadDrink;
            ad.ReLoadTable = LoadTable;
            ad.ReLoadTable += LoadcbTables;
            ad.ReLoadCategory = LoadCategory;
            ad.ReLoadCategory += LoadTable;

            ad.ShowDialog();
        }

        private void đổiMậtKhẩuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fChangePass newpass = new fChangePass(acc);
            newpass.Restart = Close;
            newpass.ShowDialog();
        }


        private void LogOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Bạn có chắc muốn đăng xuất tài khoản", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                reset.Invoke();
                this.Close();
            }
        }

        private void toolStripPrint_Click(object sender, EventArgs e)
        {
            Table table = lvBills.Tag as Table;
            if (table == null)
            {
                MessageBox.Show("Bạn chưa chọn bàn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(BillDAL.Instance.GetBillID(table.ID) == -1)
            {
                MessageBox.Show("Không tìm thấy hóa đơn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }

            fBillReport report = new fBillReport(table, txtPriceSum.Text);
            report.PriceFormatter = GetTotalBill;
            report.ShowDialog();
        }


        private void Btn_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            int id = (b.Tag as Table).ID;
            //Lưu dữ liệu của đối tượng Table được lưu từ thuộc tính Tag của mỗi Button b
            lvBills.Tag = b.Tag;
            LoadBill(id);
        }

        private void numDiscount_ValueChanged(object sender, EventArgs e)
        {

            double Pricesum = CurrentPrice();
            Pricesum -= Pricesum * ((double)numDiscount.Value/100);
            CultureInfo cul = new CultureInfo("vi-VN");
            txtPriceSum.Text = Pricesum.ToString("c0", cul);
        }



        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            Category cate = cbCategory.SelectedValue as Category;
            int id = cate.Id;
            LoadDrink(id);
        }


        //Thêm đồ
        //Quy ước: đối với mỗi bàn chỉ được tồn tại nhiều nhất 1 hóa đơn chưa thanh toán
        private void btnAddDrink_Click(object sender, EventArgs e)
        {
            if (numCount.Value == 0)
            {
                MessageBox.Show("Vui lòng nhập số lượng đồ cần thêm/bớt!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Table table = lvBills.Tag as Table;
            if (table == null)
            {
                MessageBox.Show("Bạn chưa chọn bàn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //nếu bàn đó không có bill nào chưa thanh toán sẽ trả về id bill = -1
            int idBill = BillDAL.Instance.GetBillID(table.ID);
            int idDrink = (cbDrinkName.SelectedValue as Drink).Id;
            if (idBill == -1)
            {
                BillDAL.Instance.AddBill(table.ID, (int)numCount.Value);
                BillInforDAL.Instance.AddOrUpdateBillInfor(BillDAL.Instance.CreateNewIDBill(), idDrink, (int)numCount.Value);

                if (numCount.Value > 0)
                    TableDAL.Instance.UpdateStatusTable(table.ID, "Có khách");
            }
            else
            {
                BillInforDAL.Instance.AddOrUpdateBillInfor(idBill, idDrink, (int)numCount.Value);

                int Cur_idBill = BillDAL.Instance.GetBillID(table.ID);

                if (Cur_idBill == -1) TableDAL.Instance.UpdateStatusTable(table.ID, "Trống");

            }
            LoadBill(table.ID);
            LoadTable();
        }



        //Thanh toán! Cập nhật trạng thái của bill về 1: "Đã thanh toán"
        private void btnPay_Click(object sender, EventArgs e)
        {
            Table table = lvBills.Tag as Table;
            if (table == null)
            {
                MessageBox.Show("Bạn chưa chọn bàn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int idbill = BillDAL.Instance.GetBillID(table.ID);
            if (idbill != -1)
            {
                DialogResult r = MessageBox.Show($"Xác nhận thanh toán cho {table.TableName}", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (r == DialogResult.OK)
                {
                    MessageBox.Show("Thanh toán thành công!\nCảm ơn bạn đã sử dụng dịch vụ (≧∇≦)ﾉ", "Thank You", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    string tmp = txtPriceSum.Text.Replace("₫", "");
                    tmp = tmp.Replace(".", "");

                    BillDAL.Instance.UpdateBill(idbill, table.ID, (int)numDiscount.Value, double.Parse(tmp));
                    LoadBill(table.ID);
                    TableDAL.Instance.UpdateStatusTable(table.ID, "Trống");
                    LoadTable();
                }
            }
        }


        //Chuyển bàn
        private void btnSwitch_Click(object sender, EventArgs e)
        {
            Table tableL = lvBills.Tag as Table;
            Table tableR = cbTables.SelectedItem as Table;
            if (tableL == null)
            {
                MessageBox.Show("Bạn chưa chọn bàn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int l = tableL.ID;
            int r = tableR.ID;

            if (BillDAL.Instance.GetBillID(l) == -1 && BillDAL.Instance.GetBillID(r) == -1)
            {
                MessageBox.Show("Không thể thực hiện chuyển bàn khi không có dữ liệu hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc muốn chuyển từ {tableL.TableName} sang {tableR.TableName}", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                TableDAL.Instance.SwapTable(l, r);
                LoadTable();
                LoadBill(l);
            }
        }


        //Gộp bàn
        private void btnMerge_Click(object sender, EventArgs e)
        {
            Table tableL = lvBills.Tag as Table;
            Table tableR = cbTables.SelectedItem as Table;
            if (tableL == null)
            {
                MessageBox.Show("Bạn chưa chọn bàn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int l = tableL.ID;
            int r = tableR.ID;

            if (BillDAL.Instance.GetBillID(l) == -1 || BillDAL.Instance.GetBillID(r) == -1)
            {
                MessageBox.Show("Không thể gộp bàn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (MessageBox.Show($"Bạn có chắc muốn gộp {tableL.TableName} vào {tableR.TableName}", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                TableDAL.Instance.MergeTable(l, r);
                LoadTable();
                LoadBill(l);
            }
        }


        private void btnTableregister_Click(object sender, EventArgs e)
        {
            Table table = lvBills.Tag as Table;
            if (table == null)
            {
                MessageBox.Show("Bạn chưa chọn bàn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            TableDAL.Instance.UpdateStatusTable(table.ID, "Khách đặt trước");
            LoadTable();
        }


        private void thêmbớtĐồToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnAddDrink.PerformClick();
        }


        private void thanhToánToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnPay.PerformClick();
        }
        

        private void chuyểnBànToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnSwitch.PerformClick();
        }


        private void fManager_MouseDown(object sender, MouseEventArgs e)
        {
            moving = true;
            //Lưu tọa độ hiện tại của con trỏ
            x = e.X;
            y = e.Y;
        }

        private void fManager_MouseMove(object sender, MouseEventArgs e)
        {
            if (moving)
            {
                //Tham số truyền vào: Khoảng cách từ vị trí trước đó của con trỏ đến vị trí hiện tại
                SetDesktopLocation(Cursor.Position.X - x, Cursor.Position.Y - y);
            }
        }

        private void fManager_DoubleClick(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void fManager_MouseUp(object sender, MouseEventArgs e)
        {
            moving = false;
        }


        #endregion


    }
}
