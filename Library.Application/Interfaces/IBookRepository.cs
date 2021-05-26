using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces
{
    public interface IBookRepository
    {
         string CreateBook(Book book);

         string DeleteBook(string isbn);

         string TakeBook(string isbn, string user);

         string ReturnBook(string isbn, string user);

        List<Book> GetAllBooks();

        List<Book> GetFilteredBooks(int filter, string searchString);
    }
}
