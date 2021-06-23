using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhitehamBooks
{
    class DataAccess
    {
        private static OleDbConnection createConnection(string database = "Microsoft.ACE.OLEDB.12.0", string dataSource = "BookSeller.accdb")
        {
            OleDbConnectionStringBuilder connBuilder = new OleDbConnectionStringBuilder();
            connBuilder.Add("Provider", database);
            connBuilder.Add("Data Source", dataSource);
            OleDbConnection conn = new OleDbConnection(connBuilder.ConnectionString);
            return conn;
        }
        public static List<Book> getAllBooks()
        {
            List<Book> bookList = new List<Book>();
            try
            {
                OleDbConnection conn = createConnection();
                OleDbCommand cmd = conn.CreateCommand();
                conn.Open();
                //OleDb command
                OleDbCommand command = new OleDbCommand("select * from Book", conn);
                //Used to store the result from an OleDb statement
                OleDbDataReader dataReader = null;
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    Book tempBook = new Book();
                    tempBook.IsbnNo = dataReader[0].ToString();
                    tempBook.Title = dataReader[1].ToString();
                    tempBook.Author = dataReader[2].ToString();
                    tempBook.Publisher = dataReader[3].ToString();
                    tempBook.DatePublished = Convert.ToDateTime(dataReader[4]);
                    tempBook.Available = Convert.ToBoolean(dataReader[5]);
                    tempBook.Price = Convert.ToDouble(dataReader[6]);
                    bookList.Add(tempBook);
                }
                dataReader.Close();
                command.Dispose();
                conn.Close();
            }
            catch (Exception e)
            {
                throw (new Exception("Error***" + e.Message));
            }
            return bookList;
        }
        public static void DeleteBook(string isbn)
        {
            try
            {
                OleDbConnection conn = createConnection();
                OleDbDataAdapter da = new OleDbDataAdapter();
                da.DeleteCommand = new OleDbCommand("DELETE FROM Book WHERE `ISBN No` ='" + isbn + "'", conn);
                conn.Open();
                da.DeleteCommand.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void UpdateBook(string isbn, string title, string author, string publisher, DateTime datePublished, bool available, double price)
        {
            try
            {
                OleDbConnection conn = createConnection();
                conn.Open();
                OleDbCommand cmd = new OleDbCommand("UPDATE Book SET Title=@2, Author=@3, Publisher=@4, Date Published=@5, Available=@6, Price=@7" +
                        " WHERE `ISBN No`=@1", conn);
                cmd.Parameters.Add(new OleDbParameter("@1", OleDbType.VarChar) { Value = isbn });
                cmd.Parameters.Add(new OleDbParameter("@2", OleDbType.VarChar) { Value = title });
                cmd.Parameters.Add(new OleDbParameter("@3", OleDbType.VarChar) { Value = author });
                cmd.Parameters.Add(new OleDbParameter("@4", OleDbType.VarChar) { Value = publisher });
                cmd.Parameters.Add(new OleDbParameter("@5", OleDbType.Date) { Value = datePublished });
                cmd.Parameters.Add(new OleDbParameter("@6", OleDbType.Boolean) { Value = available });
                cmd.Parameters.Add(new OleDbParameter("@7", OleDbType.Currency) { Value = price });
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR**** " + e.Message);
            }
        }
        public static void AddBook(string isbn, string title, string author, string publisher, DateTime datePublished, bool available, double price)
        {
            try
            {
                OleDbConnection conn = createConnection();
                {
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand("INSERT INTO Book VALUES" +
                        "(@1, @2, @3, @4, @5, @6, @7)", conn);
                    cmd.Parameters.Add(new OleDbParameter("@1", OleDbType.VarChar) { Value = isbn });
                    cmd.Parameters.Add(new OleDbParameter("@2", OleDbType.VarChar) { Value = title });
                    cmd.Parameters.Add(new OleDbParameter("@3", OleDbType.VarChar) { Value = author });
                    cmd.Parameters.Add(new OleDbParameter("@4", OleDbType.VarChar) { Value = publisher });
                    cmd.Parameters.Add(new OleDbParameter("@5", OleDbType.Date) { Value = datePublished });
                    cmd.Parameters.Add(new OleDbParameter("@6", OleDbType.Boolean) { Value = available });
                    cmd.Parameters.Add(new OleDbParameter("@7", OleDbType.Currency) { Value = price });
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
