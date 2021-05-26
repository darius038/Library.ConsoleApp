using Library.Application.Interfaces;
using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ConsoleApp
{
    public class LibraryApp
    {

        private readonly IBookRepository _repository;

        public LibraryApp(IBookRepository repository)
        {
            _repository = repository;
        }        
                
        public void Start()
        {
            int  menuOption;
            do
            {
                menuOption = AppMenu();

                switch (menuOption)
                {
                    case 1:
                        AddNewBook();
                        break;
                    case 2:
                        TakeBook();
                        break;
                    case 3:
                        ReturnBook();
                        break;
                    case 4:
                        DeleteBook();
                        break;
                    case 5:
                        ListBooks();
                        break;
                    case 6:
                        break;
                }
            } while (menuOption != 6);
        }

        private void TakeBook()
        {
            Console.WriteLine("Enter Your name");

            var user = Console.ReadLine();

            Console.WriteLine("Enter ISBN of Book to take it");

            var isbn = Console.ReadLine();

            Console.WriteLine(_repository.TakeBook(isbn, user));
        }

        private void ReturnBook()
        {
            Console.WriteLine("Enter Your name");

            var user = Console.ReadLine();

            Console.WriteLine("Enter ISBN of Book to return it");

            var isbn = Console.ReadLine();

            Console.WriteLine(_repository.ReturnBook(isbn, user));
        }

        private int AppMenu()
        {
            string input = "";

            int menuOption;

            do { 
            Console.WriteLine("----- Library App Menu -----");
            Console.WriteLine("1 - Add new book");
            Console.WriteLine("2 - Take book from library");
            Console.WriteLine("3 - Return book");
            Console.WriteLine("4 - Delete book from library");
            Console.WriteLine("5 - List all books. Filter parameters available");
            Console.WriteLine("6 - Exit");
            Console.WriteLine("----------------------------");

            input = Console.ReadLine();

            } while (!int.TryParse(input, out menuOption));

            return menuOption;
        }

        private void AddNewBook()
        {
            Console.WriteLine("Please enter new book data");

            Console.WriteLine("Book name:");
            var bookName = Console.ReadLine();
                        
            Console.WriteLine("Book author:");
            var bookAuthor = Console.ReadLine();

            Console.WriteLine("Category:");
            var bookCategory = Console.ReadLine();

            Console.WriteLine("Language:");
            var bookLanguage = Console.ReadLine();

            Console.WriteLine("Publication date:");
            var bookDate = Console.ReadLine();

            Console.WriteLine("ISBN:");
            var bookISBN = Console.ReadLine();

            var newBook = new Book()
            {
                Author = bookAuthor,
                Category = bookCategory,
                Language = bookLanguage,
                ISBN = bookISBN,
                Name = bookName,
                PublicationDate = bookDate
            };

            Console.WriteLine(_repository.CreateBook(newBook));
        }

        private void DeleteBook()
        {
            Console.WriteLine("Enter ISBN of Book to delete it");

            var isbn = Console.ReadLine();

            Console.WriteLine(_repository.DeleteBook(isbn));
        }

        private void ListBooks()
        {
            Console.WriteLine("Books list:");

            var books = _repository.GetAllBooks();

            foreach (var item in books)
            {
                Console.WriteLine($"<> {item.Author} <> {item.Name} <>  {item.Language} <>  {item.Category} <>  {item.ISBN} <> ");
            }

            string input;
            int filterOption;

            do            
            {
                Console.WriteLine("----- Filter books list -----");
                Console.WriteLine("1 - Filter by author");
                Console.WriteLine("2 - Filter by category");
                Console.WriteLine("3 - Filter by language");
                Console.WriteLine("4 - Filter by ISBN");
                Console.WriteLine("5 - Filter by name");
                Console.WriteLine("6 - Filter available books");
                Console.WriteLine("7 - Exit to main menu");
                Console.WriteLine("----------------------------");

                input = Console.ReadLine();

            } while (!int.TryParse(input, out filterOption));

            if (filterOption > 0 && filterOption < 6)
            {
                Console.WriteLine("Enter text for selected filter option");

                var searchString = Console.ReadLine();

                FilterBooks(filterOption, searchString);
            }

            if (filterOption == 6)
            {
                FilterBooks(filterOption, string.Empty);
            }
        }

        private void FilterBooks(int filterOption, string searchString)
        {
            List<Book> filteredList = _repository.GetFilteredBooks(filterOption, searchString);

            if (filteredList.Count > 0)
            {
                foreach (var item in filteredList)
                {
                    Console.WriteLine($"<> {item.Author} <> {item.Name} <>  {item.Language} <>  {item.Category} <>  {item.ISBN} <> ");
                }

            } else Console.WriteLine("No books was found");
        }
    }
}
