using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhitehamBooks
{
    public partial class frmBooks : Form
    {

        List<Book> listOfBooks = DataAccess.getAllBooks();
        int recordNumber = 0;
        public frmBooks()
        {
            InitializeComponent();
            this.Text = $"Books Carl Wainwright {DateTime.Now.ToLongDateString()}";
            ShowRecord();
        }

        private void ShowRecord()
        { //Method to show data according to list number called 
            if (listOfBooks.Count > 0)
            {
                txtISBN.Text = listOfBooks[recordNumber].IsbnNo;
                txtTitle.Text = listOfBooks[recordNumber].Title;
                txtAuthor.Text = listOfBooks[recordNumber].Author;
                txtPublisher.Text = listOfBooks[recordNumber].Publisher;
                txtDatePublished.Text = listOfBooks[recordNumber].DatePublished.ToString("dd/MM/yyyy");
                chkAvailable.Checked = listOfBooks[recordNumber].Available;
                txtPrice.Text = listOfBooks[recordNumber].Price.ToString();
                txtRecordCount.Text = (recordNumber + 1) + " of " + listOfBooks.Count;
            }
            else
            {   //If not cars exist show empty fields
                foreach (Control c in this.Controls)
                {
                    TextBox box = c as TextBox;
                    if (box != null)
                    {
                        box.Text = string.Empty;
                    }
                }
                chkAvailable.Checked = false;
                txtRecordCount.Text = "No Records";
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnCancelAll_Click(object sender, EventArgs e)
        {

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }

        private void btnFirst_Click(object sender, EventArgs e)
        {

        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {

        }

        private void btnNext_Click(object sender, EventArgs e)
        {

        }

        private void btnLast_Click(object sender, EventArgs e)
        {

        }
    }
}
