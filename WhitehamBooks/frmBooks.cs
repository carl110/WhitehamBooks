using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhitehamBooks
{
    public partial class frmBooks : Form
    {
        List<Book> listOfBooks = DataAccess.getAllBooks();
        static List<Book> originalList = DataAccess.getAllBooks();
        int recordNumber = 0;
        private PrintDocument myPrintDocument = new PrintDocument();
        private Font printFont;
        private int intPageCount;
        public frmBooks()
        {
            InitializeComponent();
            myPrintDocument.BeginPrint += myPrintDocument_BeginPrint;
            myPrintDocument.PrintPage += myPrintDocument_PrintPage;
            printFont = new Font("Ariel", 10.0f);
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
            {   //If no books exist show empty fields
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
        private bool isbnCheck()
        {
            if (txtISBN.TextLength < 10)
            {
                MessageBox.Show("ISBN Must be 10 characters", "ISBN Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!mod11ISBN(txtISBN.Text))
            {
                MessageBox.Show("ISBN Number is not a valid modulus 11 number", "ISBN Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private bool mod11ISBN(string number)
        {
            int multiplier = 10;
            int calc = 0;
            for (int i = 0; i < number.Length; i++)
            {
                if (number[i].Equals('x') || number[i].Equals('X'))
                {
                    calc += (10 * multiplier);
                }
                else
                {
                    calc += ((int)char.GetNumericValue(number[i]) * multiplier);
                }
                multiplier--;
            }
            if (calc % 11 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool invalidFields()
        {
            DateTime value;
            //check is an txtfields are empty
            if (txtISBN.TextLength == 0 || txtTitle.TextLength == 0 || txtAuthor.TextLength == 0 || txtPublisher.TextLength == 0 || txtDatePublished.TextLength == 0 || txtPrice.TextLength == 0)
            {
                return true;
            }//Check if date field holds a date
            else if (DateTime.TryParse(txtDatePublished.Text, out value) == false)
            {
                return true;
            }
            else return false;
        }
        private void myPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            float linesPerPage = 0;
            float yPos = 0;
            float leftMargin = e.MarginBounds.Left;
            float topMargin = e.MarginBounds.Top;
            string employeeRecord = null;
            int currentPage = intPageCount + 1;
            Font printFont = new Font("Courier New", 14, FontStyle.Bold);
            // Calculate the number of lines per page.
            linesPerPage = e.MarginBounds.Height / printFont.GetHeight(e.Graphics);
            StringFormat centeredText = new StringFormat();
            centeredText.Alignment = StringAlignment.Center;
            // Title & Page Number
            string title = "WHITEHAM BOOKS";
            e.Graphics.DrawString(title, printFont, Brushes.Black,
            e.PageSettings.PaperSize.Width / 2, yPos, centeredText);
            e.Graphics.DrawString($"Page {currentPage.ToString().PadLeft(2, '1')}", printFont, Brushes.Black, e.MarginBounds.Right, yPos);
            yPos += Convert.ToInt32(printFont.GetHeight());
            linesPerPage -= 1;
            // Today's Date
            printFont = new Font("Courier New", 10, FontStyle.Bold);
            string date = $"Date {DateTime.Today:d}";
            e.Graphics.DrawString(date, printFont, Brushes.Black, e.PageSettings.PaperSize.Width / 2, yPos, centeredText);
            yPos += Convert.ToInt32(printFont.GetHeight()) * 2;
            linesPerPage -= 1;
            // Column Headings
            string headings = string.Format(
                "{0} {1} {2} {3}",
                "ISBN No".PadRight(15),
                "Title".PadRight(35),
                "Available".PadRight(15),
                "Price".PadRight(10)
            );
            e.Graphics.DrawString(headings, printFont, Brushes.Black, 10, yPos);
            yPos += Convert.ToInt32(printFont.GetHeight());
            linesPerPage -= 1;
            //for each record in list
            for (int i = 0; recordNumber < listOfBooks.Count && i < linesPerPage; recordNumber++, i++)
            {
                yPos = topMargin + (recordNumber * printFont.GetHeight());
                //combine list deatils into 1 line
                employeeRecord = string.Format(
                       "{0} {1} {2} {3}",
                       listOfBooks[recordNumber].IsbnNo.ToString().PadRight(15),
                       listOfBooks[recordNumber].Title.ToString().PadRight(35),
                       (listOfBooks[recordNumber].Available ? "Yes" : "No").PadRight(15),
                       String.Format(new CultureInfo("en-GB"), "{0:C}", listOfBooks[recordNumber].Price, 2).PadRight(10)
                       );
                e.Graphics.DrawString(employeeRecord, printFont, Brushes.Black, 10, yPos, new StringFormat());
            }
            //If more lines exist, print another page.
            if (listOfBooks.Count > recordNumber)
            {
                e.HasMorePages = true;
                currentPage++;
            }
            else
                e.HasMorePages = false;
        }
        private void myPrintDocument_BeginPrint(object sender, PrintEventArgs e)
        {
            recordNumber = 0;
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (isbnCheck())
            {
                bool exists = false;
                for (int i = 0; i < listOfBooks.Count; i++)
                {   //loop list of cars to see if ISBN exists
                    if (listOfBooks[i].IsbnNo.Equals(txtISBN.Text))
                    {
                        exists = true;
                        i = listOfBooks.Count;
                    }
                }
                if (exists == true)
                {   //allow update if isbn exists
                    if (invalidFields() == false)
                    {       //update sql table and show confiration message
                        DataAccess.UpdateBook(txtISBN.Text, txtTitle.Text, txtAuthor.Text, txtPublisher.Text, DateTime.ParseExact(txtDatePublished.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture), chkAvailable.Checked, Convert.ToDouble(txtPrice.Text));
                        MessageBox.Show("Your update has been processed", "Book List Updated", MessageBoxButtons.OK);
                        listOfBooks = DataAccess.getAllBooks();
                    }
                    else
                    {   //advise invalid field data
                        MessageBox.Show("One of your fields are invalid", "Invalid Field(s)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {   //message to advise ISBN doesnt exist
                    MessageBox.Show("This ISBN does not exist, you may Add this book as a new record", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (isbnCheck())
            {
                bool duplicate = false;
                for (int i = 0; i < listOfBooks.Count - 1; i++)
                {   //loop list to see if isbn exists
                    if (listOfBooks[i].IsbnNo.Equals(txtISBN.Text))
                    {
                        duplicate = true;
                        i = listOfBooks.Count;
                    }
                }
                if (duplicate == true)
                {   //if exists then advise they can update
                    MessageBox.Show("This ISBN already exists on the database, you may update but not add as new", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {   //check txt fields
                    if (invalidFields() == false)
                    {   //Add book to table and update ist
                        DataAccess.AddBook(txtISBN.Text, txtTitle.Text, txtAuthor.Text, txtPublisher.Text, DateTime.ParseExact(txtDatePublished.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture), chkAvailable.Checked, Convert.ToDouble(txtPrice.Text));
                        listOfBooks = DataAccess.getAllBooks();
                        recordNumber = listOfBooks.Count - 1;
                        MessageBox.Show("Your book has been added to the database", "New Book Added", MessageBoxButtons.OK);
                        ShowRecord();
                    }
                    else
                    {
                        MessageBox.Show("One of your fields are invalid", "Invalid Field(s)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool exists = false;
            for (int i = 0; i < listOfBooks.Count; i++)
            {   //loop list of books to see if ISBN exists
                if (listOfBooks[i].IsbnNo.Equals(txtISBN.Text))
                {
                    exists = true;
                    i = listOfBooks.Count;
                }
            }
            if (exists == true)
            {   //Delete current item for sql table
                DataAccess.DeleteBook(txtISBN.Text);
                if (recordNumber != 0)
                {
                    recordNumber -= 1;
                }
                listOfBooks = DataAccess.getAllBooks();
                ShowRecord();
            }
            else
            {
                MessageBox.Show("This ISBN Number does not exist, therefore it cannot be deleted.", "Invalid ISBN Number", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ShowRecord();
        }
        private void btnCancelAll_Click(object sender, EventArgs e)
        {
            listOfBooks = originalList;
            ShowRecord();
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                myPrintDocument.Print();
                intPageCount++;
            }
        }
        private void btnClose_Click(object sender, EventArgs e) => Application.Exit();
        private void btnFirst_Click(object sender, EventArgs e)
        {   //goto record 0 on list
            recordNumber = 0;
            ShowRecord();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {   //check if already on first item, if so goto last
            if (recordNumber == 0)
            {
                recordNumber = listOfBooks.Count - 1;
            }
            else
            {
                recordNumber -= 1;
            }
            ShowRecord();
        }
        private void btnNext_Click(object sender, EventArgs e)
        {   //check if on last item, if so goto first
            if (recordNumber == listOfBooks.Count - 1)
            {
                recordNumber = 0;
            }
            else
            {
                recordNumber += 1;
            }
            ShowRecord();
        }
        private void btnLast_Click(object sender, EventArgs e)
        {   //goto last item in list
            recordNumber = listOfBooks.Count - 1;
            ShowRecord();
        }
    }
}
