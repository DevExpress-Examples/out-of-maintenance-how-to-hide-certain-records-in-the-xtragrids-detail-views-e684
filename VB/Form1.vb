Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraGrid.Views.Grid
Imports System.Data.OleDb

Namespace FilteringDetails
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			Dim connection As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source = .\nwind.mdb")
			Dim AdapterCategories As New OleDbDataAdapter("SELECT CustomerID, CompanyName, ContactName FROM Customers", connection)
			Dim AdapterProducts As New OleDbDataAdapter("SELECT OrderID, CustomerID, EmployeeID, OrderDate FROM Orders", connection)

			Dim dataSet11 As New DataSet()
			AdapterCategories.Fill(dataSet11, "Customers")
			AdapterProducts.Fill(dataSet11, "Orders")

			Dim keyColumn As DataColumn = dataSet11.Tables("Customers").Columns("CustomerID")
			Dim foreignKeyColumn As DataColumn = dataSet11.Tables("Orders").Columns("CustomerID")
			dataSet11.Relations.Add("CustomersOrders", keyColumn, foreignKeyColumn)

			gridControl1.DataSource = dataSet11.Tables("Customers")
			gridControl1.ForceInitialize()

			Dim gridView2 As New GridView(gridControl1)
			gridControl1.LevelTree.Nodes.Add("CustomersOrders", gridView2)

			gridView1.Columns("CustomerID").VisibleIndex = -1
			gridView2.PopulateColumns(dataSet11.Tables("Orders"))
            gridView2.Columns("CustomerID").VisibleIndex = -1
            gridView2.SynchronizeClones = False
		End Sub

		Private Sub gridView1_MasterRowExpanded(ByVal sender As Object, ByVal e As CustomMasterRowEventArgs) Handles gridView1.MasterRowExpanded
			Dim detailView As GridView = TryCast((TryCast(sender, GridView)).GetDetailView(e.RowHandle, e.RelationIndex), GridView)
			detailView.ClearColumnsFilter()
			detailView.Columns("EmployeeID").FilterInfo = New DevExpress.XtraGrid.Columns.ColumnFilterInfo(3, "3")
		End Sub
	End Class
End Namespace