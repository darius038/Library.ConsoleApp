using Library.Application.Interfaces;
using Library.Application.Services;
using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IFileHandler _dataFile;

        private List<Book> _context = new();

        private List<Book> Context
        {
            get
            {
                _context = _dataFile.GetData().ToList();

                return _context;
            }
            set
            {
                _context = value;
            }
        }

        public BookRepository(IFileHandler dataFile)
        {
            _dataFile = dataFile;
        }

        public string CreateBook(Book book)
        {
            if (BookExist(book.ISBN))
            {
                return $"Book with ISBN - {book.ISBN} already exist";
            }

            if (BookValidation(book))
            {
                return "All book properties are required!";
            }

            Context.Add(book);

            SaveChanges();

            return "Book was created successfully";
        }

        public string DeleteBook(string isbn)
        {
            if (!BookExist(isbn))
            {
                return $"Book with ISBN - {isbn} not found";
            }

            Context = Context.Where(x => x.ISBN != isbn).ToList();

            SaveChanges();

            return "Book was deleted successfully";
        }

        public List<Book> GetAllBooks()
        {
            return Context;
        }

        public List<Book> GetFilteredBooks(int filter, string searchString)
        {
            List<Book> filteredList = new();

            switch (filter)
            {
                case 1:
                    filteredList = Context.Where(x => x.Author == searchString).ToList();
                    break;
                case 2:
                    filteredList = Context.Where(x => x.Category == searchString).ToList();
                    break;
                case 3:
                    filteredList = Context.Where(x => x.Language == searchString).ToList();
                    break;
                case 4:
                    filteredList = Context.Where(x => x.ISBN == searchString).ToList();
                    break;
                case 5:
                    filteredList = Context.Where(x => x.Name == searchString).ToList();
                    break;
                case 6:
                    filteredList = Context.Where(x => string.IsNullOrEmpty(x.TakenBy)).ToList();
                    break;
            }
            return filteredList;
        }

        public string ReturnBook(string isbn, string user)
        {

            if (Context.Any(x => x.ISBN == isbn && x.TakenBy == user))
            {

                var book = Context.FirstOrDefault(x => x.ISBN == isbn);

                var takenDate = book.TakenDate.ToString();

                book.TakenBy = null;
                book.TakenDate = null;

                UpdateBook(book);

                SaveChanges();

                if ((DateTime.Now - DateTime.Parse(takenDate)).Days > 60)
                {
                    Console.Beep();
                    return $"We hope you brought pizza {user}, because You are late !!!";
                }

                return "Book was returned successfully";
            }

            return "Book not found";
        }

        public string TakeBook(string isbn, string user)
        {
            if (!BookExist(isbn))
            {
                return $"Book with ISBN - {isbn} not found";
            }

            if (Context.Count(x => x.TakenBy == user) == 3)
            {
                return "Your allreade took 3 books ;)";
            }

            var book = Context.FirstOrDefault(x => x.ISBN == isbn);

            book.TakenBy = user;
            book.TakenDate = DateTime.Now;

            UpdateBook(book);

            return $"Have a nice reading {user}";
        }

        private void SaveChanges()
        {
            _dataFile.SaveData(_context);
        }

        private bool BookExist(string isbn)
        {
            if (Context.Any(x => x.ISBN == isbn))
            {
                return true;
            }
            return false;
        }

        private void UpdateBook(Book book)
        {
            _context = _context.Where(x => x.ISBN != book.ISBN).ToList();

            _context.Add(book);

            SaveChanges();
        }

        private bool BookValidation(Book book)
        {
            var emptyProperties = book.GetType()
                .GetProperties()
                .Where(pi => pi.PropertyType == typeof(string))
                .Select(pi => (string)pi.GetValue(book))
                .Count(value => string.IsNullOrEmpty(value));

            if (emptyProperties > 2)
            {
                return true;
            }

            return false;
        }
    }
}
