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
    public partial class frmUpdateVendor : Form
    {
        public frmUpdateVendor(Vendor v)
        {
            InitializeComponent();

            //populate form with vendor data
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //grab all info off form
            // and put in vendor object
            Vendor v = new Vendor()
            {
                VendorID = 1,
                VendorName = "Joe's school supplies",
                VendorAddress1 = "123 fake street",
                VendorCity = "Lakewood",
                VendorState = "WA",
                VendorZipCode = "98499",
                DefaultAccountNo = 100,
                DefaultTermsID = 1,


            };

            //do validation

            var db = new APEntities();
            db.Entry(v).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
    }
}
