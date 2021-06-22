using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhitehamBooks
{
    class Book
    {
        string isbnNo;
        string title;
        string author;
        string publisher;
        DateTime datePublished;
        bool available;
        double price;

        public string IsbnNo { get => isbnNo; set => isbnNo = value; }
        public string Title { get => title; set => title = value; }
        public string Author { get => author; set => author = value; }
        public string Publisher { get => publisher; set => publisher = value; }
        public DateTime DatePublished { get => datePublished; set => datePublished = value; }
        public bool Available { get => available; set => available = value; }
        public double Price { get => price; set => price = value; }
    }
}
