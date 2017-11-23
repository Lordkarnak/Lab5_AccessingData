using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyDataApp
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            String strSQL = "select * from products";

            try
            {
                using (OleDbConnection cnn = new OleDbConnection(Properties.Settings.Default.NorthwindConnectionString))
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
                                comboBox1.Items.Add(string.Format("{0}", dr["ProductID"]));
                                comboBox2.Items.Add(string.Format("{0}", dr["ProductName"]));
                                comboBox3.Items.Add(string.Format("{0}", dr["SupplierID"]));
                            }
                            comboBox1.SelectedItem = comboBox1.Items[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
