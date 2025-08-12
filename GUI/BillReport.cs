using DAL;
using DTO;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class fBillReport : Form
    {
        Table table;
        string totalprice;

        //Delegate lấy dữ liệu từ form chính về form con
        Func<int, string> priceFormatter;
        public Func<int, string> PriceFormatter { get => priceFormatter; set => priceFormatter = value; }


        public fBillReport(Table table, string totalprice)
        {
            InitializeComponent();
            this.table = table;
            this.totalprice = totalprice;
        }

        private void BillReport_Load(object sender, EventArgs e)
        {
            //Đổ dữ liệu từ dataset vào Report
            ReportDataSource report = new ReportDataSource("ExtractBill", BillDetailDAL.Instance.BillDetalList(table.ID));
            this.reportViewer1.LocalReport.DataSources.Clear();

            this.reportViewer1.LocalReport.DataSources.Add(report);

            #region Parameter
            ReportParameter para1 = new ReportParameter("PriceParam", totalprice);
            ReportParameter para2 = new ReportParameter("TableName", table.TableName);
            ReportParameter para3 = new ReportParameter("CurTotal", priceFormatter.Invoke(table.ID));
            ReportParameter para4 = new ReportParameter("TimeIn", BillDetailDAL.Instance.GetBillDate(table.ID).ToLongTimeString());
            ReportParameter para5 = new ReportParameter("DateIn", BillDetailDAL.Instance.GetBillDate(table.ID).ToLongDateString());
            #endregion

            this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] {para1, para2, para3, para4, para5 });

            this.reportViewer1.LocalReport.Refresh();
            this.reportViewer1.RefreshReport();
        }
    }
}
