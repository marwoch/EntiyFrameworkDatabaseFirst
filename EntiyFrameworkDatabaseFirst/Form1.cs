using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EntiyFrameworkDatabaseFirst
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopulateVendorComboBox();
        }

        private void PopulateVendorComboBox()
        {
            //create instance of DBContext
            APEntities db = new APEntities();

            //object notation query, easy to read
            List<Vendor> vendors =
                db
                .Vendors
                .Where(v => v.VendorState == "CA")
                .OrderBy(v => v.VendorName)
                .ToList();

            ////query syntax
            //List<Vendor> vendors =
            //    (from v in db.Vendors
            //     where v.VendorState == "CA"
            //     orderby v.VendorName ascending
            //     select v).ToList();


            foreach (Vendor v in vendors)
            {
                cboVendors.Items.Add(v.VendorName);

                //foreach (Invoice inv in v.Invoices)
                //{
                //    cboVendors.Items.Add(inv.InvoiceNumber);
                //}
            }
        }
    }
}
