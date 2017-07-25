using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;

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

            //query syntax
            List<Vendor> vendors2 =
                (from v in db.Vendors
                 where v.VendorState == "CA"
                 orderby v.VendorName ascending
                 select v).ToList();

            //bind vendor list to combo box
            cboVendors.DataSource = vendors; // databinding all vendors store in it
            cboVendors.DisplayMember = nameof(Vendor.VendorName); // displays the vendor name strong typing & refactoring support;

            //foreach (Vendor v in vendors)
            //{
            //    cboVendors.Items.Add(v);

            //    //foreach (Invoice inv in v.Invoices)
            //    //{
            //    //    cboVendors.Items.Add(inv.InvoiceNumber);
            //    //}
            //}
        }

        /// <summary>
        /// deletes vendor from the db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            //entity framework delete query
            APEntities db = new APEntities();

            //find the id of the selected vendor
            int v = GetSelectedVendor().VendorID;

            //get that vendor from the db context
            Vendor found = db.Vendors.Find(v);

            //remove the vendor that was found
            db.Vendors.Remove(found);
            db.SaveChanges(); // can wait on 'til ready to commit changes

            //refresh vendor list (works for now)
            PopulateVendorComboBox();
        }

        /// <summary>
        /// alternate delete method
        /// </summary>
        /// <param name="v"></param>
        //private void DeleteVendor(Vendor v)
        //{
        //    var db = new APEntities();
        //    //db.Vendors.Attach(v);
        //    db.Entry(v).State = System.Data.Entity.EntityState.Deleted;
        //    db.SaveChanges();
        //}

        private Vendor GetSelectedVendor()
        {
            Vendor v = cboVendors.SelectedItem as Vendor;
            return v; // or as return cboVendors.SelectedItem as Vendor
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            Vendor v = new Vendor()
            {
                VendorName = "Joe's school supplies",
                VendorAddress1 = "123 fake street",
                VendorCity = "Lakewood",
                VendorState = "CA",
                VendorZipCode = "90201",
                DefaultAccountNo = 100,
                DefaultTermsID = 1,
            };

            var db = new APEntities();
            db.Vendors.Add(v);
            db.SaveChanges();

            PopulateVendorComboBox();
        }

        private void btnTesting_Click(object sender, EventArgs e)
        {
            APEntities db = new APEntities();

            //inner join example
            List<Vendor> vendorWithInvoices =
                (from v in db.Vendors
                 join i in db.Invoices on
                     v.VendorID equals i.VendorID
                 select v).ToList();

            //get first ten vendors sorted by vendor name
            List<Vendor> page1 = (from v in db.Vendors
                                  orderby v.VendorName ascending
                                  select v)
                                  .Take(10)
                                  .ToList();

            //get second set of 10 vendors sorted by vendor name
            List<Vendor> page2 = (from v in db.Vendors
                                  orderby v.VendorName ascending
                                  select v).Skip(10).Take(10).ToList();

            List<Vendor> page2Alt =
                db.Vendors
                    .OrderBy(v => v.VendorName)
                    .Skip(10)
                    .Take(10)
                    .ToList();

            var query =
                from i in db.Invoices
                where i.TermsID == 5
                select i;

            //query results of another query
            var query2 =
                from inv in query
                where inv.InvoiceTotal > 50
                select inv;

            //anonoumus object creating object on the fly
            var vendorDisplay =
                (from v in db.Vendors
                 select new
                 {
                     v.VendorName,
                     v.VendorAddress1
                 }).ToList();

            foreach (var v in vendorDisplay)
            {
                Console.Write(v.VendorName);
            }

            //get exactly one invoice back( if none found throws exception
            Invoice myInvoice =
                (from i in db.Invoices
                 where i.InvoiceID == 1
                 select i).Single();
        }
    }
}
