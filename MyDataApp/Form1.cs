using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyDataApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 newMDIChild = new Form2();
            newMDIChild.MdiParent = this;
            newMDIChild.Show();

        }
    }
}
