using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using System.Data.OleDb;

namespace FilteringDetails
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OleDbConnection connection = new OleDbConnection(
              "Provider=Microsoft.Jet.OLEDB.4.0;Data Source = .\\nwind.mdb");
            OleDbDataAdapter AdapterCategories = new OleDbDataAdapter(
              "SELECT CustomerID, CompanyName, ContactName FROM Customers", connection);
            OleDbDataAdapter AdapterProducts = new OleDbDataAdapter(
              "SELECT OrderID, CustomerID, EmployeeID, OrderDate FROM Orders", connection);

            DataSet dataSet11 = new DataSet();
            AdapterCategories.Fill(dataSet11, "Customers");
            AdapterProducts.Fill(dataSet11, "Orders");

            DataColumn keyColumn = dataSet11.Tables["Customers"].Columns["CustomerID"];
            DataColumn foreignKeyColumn = dataSet11.Tables["Orders"].Columns["CustomerID"];
            dataSet11.Relations.Add("CustomersOrders", keyColumn, foreignKeyColumn);

            gridControl1.DataSource = dataSet11.Tables["Customers"];
            gridControl1.ForceInitialize();

            GridView gridView2 = new GridView(gridControl1);
            gridControl1.LevelTree.Nodes.Add("CustomersOrders", gridView2);

            gridView1.Columns["CustomerID"].VisibleIndex = -1;
            gridView2.PopulateColumns(dataSet11.Tables["Orders"]);
            gridView2.Columns["CustomerID"].VisibleIndex = -1;
            gridView2.SynchronizeClones = false;
        }

        private void gridView1_MasterRowExpanded(object sender, CustomMasterRowEventArgs e)
        {
            GridView detailView = (sender as GridView).GetDetailView(e.RowHandle, e.RelationIndex) as GridView;
            detailView.ClearColumnsFilter();
            detailView.Columns["EmployeeID"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo(3, "3");
        }
    }
}