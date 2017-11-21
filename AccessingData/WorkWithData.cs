using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.OleDb;
using System.IO;

namespace AccessingData
{
    public partial class WorkWithData : Form
    {
        public WorkWithData()
        {
            InitializeComponent();

            string strSQL = "select * from Employees";
            using(OleDbDataAdapter adapter = new OleDbDataAdapter(strSQL, Properties.Settings.Default.OleDbConnectionString))
            {
                DataSet ds = new DataSet();
                adapter.Fill(ds, "EmployeesInfo");

                comboBox1.DataSource = ds.Tables["EmployeesInfo"];
                comboBox1.DisplayMember = "EmployeeID";
                //comboBox1.ValueMember = "FirstName";
                
            }
                
        }

        private void PrepareDemo(bool ShowGrid)
        {
            PrepareDemo(ShowGrid, pgeListBox);
        }

        private void PrepareDemo(bool ShowGrid, TabPage SelectedPage)
        {
            if (demoList.DataSource == null)
            {
                demoList.Items.Clear();
            }
            else
            {
                demoList.DataSource = null;
                demoList.DisplayMember = "";
            }

            demoGrid.Visible = ShowGrid;
            tabDemo.SelectedTab = SelectedPage;
        }


        private void sqlDataReaderButton_Click(System.Object sender, System.EventArgs e)
        {
            PrepareDemo(false);
            DataReaderFromOleDB();
        }
        private void DataReaderFromOleDB()
        {
            string strSQL = "SELECT * FROM Customers";

            try
            {

                using (OleDbConnection cnn = new OleDbConnection(Properties.Settings.Default.OleDbConnectionString))
                {
                    using (OleDbCommand cmd = new OleDbCommand(strSQL, cnn))
                    {
                        cnn.Open();

                        using (OleDbDataReader dr = cmd.ExecuteReader())
                        {
                            // Loop through all the rows, retrieving the 
                            // columns you need. Also look into the GetString
                            // method (and other Get... methods) for a faster 
                            // way to retrieve individual columns.
                           
                            while (dr.Read())
                            {
                                demoList.Items.Add(string.Format("{0} {1}: {2}", dr["CompanyName"], dr["ContactName"], dr["ContactTitle"]));
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void OleDbDataSetButton_Click(System.Object sender, System.EventArgs e)
        {
            PrepareDemo(true);
            DataSetFromOleDb();
        }
        private void DataSetFromOleDb()
        {
            string strSQL = "SELECT * FROM Products WHERE CategoryID=1";

            try
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(strSQL, Properties.Settings.Default.OleDbConnectionString))
                {

                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "ProductInfo");


                    demoList.DataSource = ds.Tables["ProductInfo"];
                    demoList.DisplayMember = "ProductID";

                    // Εναλλάκτικά θα μπορούσαμε να γεμίσουμε το Listbox
                    // παίρνοντας μία μία τις γραμμές του πίνακα και προσθέτοντάς τες

                    //For Each dr As DataRow In _
                    // ds.Tables("ProductInfo").Rows
                    //    demoList.Items.Add(dr("ProductName").ToString)
                    //Next dr

                    // Want to bind a grid? It's this easy:
                    demoGrid.DataSource = ds.Tables["ProductInfo"];
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void sqlDataSetButton_Click(System.Object sender, System.EventArgs e)
        {
            PrepareDemo(true);
            DataTableFromSQL();
        }
        private void DataTableFromSQL()
        {
            string strSQL = "SELECT * FROM Products, Employees WHERE CategoryID = 1";

            try
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(strSQL, Properties.Settings.Default.OleDbConnectionString))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);


                    foreach (DataColumn dc in dt.Columns)
                    {
                        demoList.Items.Add(string.Format("{0} ({1}) {2}", dc.ColumnName, dc.DataType, dc.AllowDBNull));
                    }

                    string filter = "City = 'London'";
                    DataRow[] employeesFromLondon = dt.Select(filter);
                    demoGrid.DataSource = dt.Select(filter);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void dataTableButton_Click(System.Object sender, System.EventArgs e)
        {
            PrepareDemo(true, pgeGrid);
            CreateDataTable();
        }
        private void CreateDataTable()
        {
            // Create a DataTable filled with information
            // about files in the current folder.
            // 
            // Note the use of the FileInfo and 
            // DirectoryInfo objects, provided by the 
            // .NET framework, in the System.IO namespace.

            DataTable dt = new DataTable();
            dt.Columns.Add("Filename", typeof(System.String));
            dt.Columns.Add("ReadOnly", typeof(System.Boolean));
            //dt.Columns.Add("Size", typeof(System.Int64));

            DataRow dr = default(DataRow);
            DirectoryInfo dir = new DirectoryInfo("C:\\");
            foreach (FileInfo fi in dir.GetFiles())
            {
                dr = dt.NewRow();
                dr[0] = fi.Name;
                dr[1] = fi.IsReadOnly;
                dt.Rows.Add(dr);
            }

            // Bind the DataGridView to this DataTable.
            demoGrid.DataSource = dt;
        }

        private void WorkWithData_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'employeeDataset.Employees' table. You can move, or remove it, as needed.
            this.employeesTableAdapter.Fill(this.employeeDataset.Employees);

        }
    }
}
