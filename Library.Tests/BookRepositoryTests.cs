using FluentAssertions;
using Library.Application.Interfaces;
using Library.Application.Repositories;
using Library.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Library.Tests
{
    public class BookRepositoryTests
    {
        [Fact]
        public void GetAll_ReturnsBooksList()
        {
            //Arrange

            List<Book> bookList = new()
            {
                new Book
                {
                    Name = "BookName1",
                    Author = "Author1",
                    Category = "Category1",
                    Language = "Language1",
                    PublicationDate = "PublicationDate1",
                    ISBN = "100-100"
                },
                new Book
                {
                    Name = "BookName2",
                    Author = "Author2",
                    Category = "Category2",
                    Language = "Language2",
                    PublicationDate = "PublicationDate2",
                    ISBN = "200-200"
                }
            };

            var mockFileHandler = new Mock<IFileHandler>();

            mockFileHandler.Setup(g => g.GetData()).Returns(bookList);

            var repository = new BookRepository(mockFileHandler.Object);

            // Act

            var books = repository.GetAllBooks();

            //Assert

            books.Should().NotBeNull();
            books.Count.Should().Be(2);
        }

        [Theory]
        [InlineData(1, "Author1")]
        [InlineData(2, "Category1")]
        [InlineData(3, "Language1")]
        [InlineData(4, "100-100")]
        [InlineData(5, "BookName1")]
        [InlineData(6, "")]
        public void GetFilteredBooks_ReturnsFilteredBooksList(int filter, string searchString)
        {
            //Arrange

            var book1 = new Book()
            {
                Name = "BookName1",
                Author = "Author1",
                Category = "Category1",
                Language = "Language1",
                PublicationDate = "PublicationDate1",
                ISBN = "100-100"
            };
            var book2 = new Book()
            {
                Name = "BookName2",
                Author = "Author2",
                Category = "Category2",
                Language = "Language2",
                PublicationDate = "PublicationDate2",
                ISBN = "200-200",
                TakenBy = "Book2 User",
                TakenDate = DateTime.Now
            };

            List<Book> bookList = new() { book1, book2};

            var mockFileHandler = new Mock<IFileHandler>();

            mockFileHandler.Setup(g => g.GetData()).Returns(bookList);

            var repository = new BookRepository(mockFileHandler.Object);

            // Act

            var books = repository.GetFilteredBooks(filter, searchString);

            //Assert

            books.Should().NotBeNull();
            books.Count.Should().Be(1);
            books.Any(x => x.ISBN == book1.ISBN).Should().BeTrue();
        }

        [Theory]
        [InlineData("Book was created successfully")]
        public void CreateBook_ReturnsMessage(string returnMessage)
        {
            //Arrange

            var book1 = new Book()
            {
                Name = "BookName1",
                Author = "Author1",
                Category = "Category1",
                Language = "Language1",
                PublicationDate = "PublicationDate1",
                ISBN = "100-100"
            };           

            var mockFileHandler = new Mock<IFileHandler>();            

            var repository = new BookRepository(mockFileHandler.Object);

            // Act

            var result = repository.CreateBook(book1);

            var exception = Record.Exception(() => repository.CreateBook(book1));

            //Assert

            result.Should().Be(returnMessage);
            exception.Should().BeNull();
        }

        [Theory]
        [InlineData("Book was deleted successfully")]
        public void DeleteBook_ReturnsMessage(string returnMessage)
        {
            //Arrange

            var book1 = new Book()
            {
                Name = "BookName1",
                Author = "Author1",
                Category = "Category1",
                Language = "Language1",
                PublicationDate = "PublicationDate1",
                ISBN = "100-100"
            };

            List<Book> bookList = new() { book1 };

            var mockFileHandler = new Mock<IFileHandler>();

            mockFileHandler.Setup(g => g.GetData()).Returns(bookList);

            var repository = new BookRepository(mockFileHandler.Object);

            // Act

            var result = repository.DeleteBook(book1.ISBN);

            var exception = Record.Exception(() => repository.DeleteBook(book1.ISBN));

            //Assert

            result.Should().Be(returnMessage);
            exception.Should().BeNull();
        }

        [Theory]
        [InlineData("Have a nice reading User")]
        public void TakeBook_ReturnsMessage(string returnMessage)
        {
            //Arrange

            var book1 = new Book()
            {
                Name = "BookName1",
                Author = "Author1",
                Category = "Category1",
                Language = "Language1",
                PublicationDate = "PublicationDate1",
                ISBN = "100-100"
            };

            List<Book> bookList = new() { book1 };

            var mockFileHandler = new Mock<IFileHandler>();

            mockFileHandler.Setup(g => g.GetData()).Returns(bookList);

            var repository = new BookRepository(mockFileHandler.Object);

            // Act

            var result = repository.TakeBook(book1.ISBN, "User");

            var exception = Record.Exception(() => repository.TakeBook(book1.ISBN, "User"));

            //Assert

            result.Should().Be(returnMessage);

            book1.TakenBy.Should().Be("User");

            exception.Should().BeNull();
        }

        [Theory]
        [InlineData("Book was returned successfully")]
        public void ReturnBook_ReturnsMessage(string returnMessage)
        {
            //Arrange

            var book1 = new Book()
            {
                Name = "BookName2",
                Author = "Author2",
                Category = "Category2",
                Language = "Language2",
                PublicationDate = "PublicationDate2",
                ISBN = "200-200",
                TakenBy = "User",
                TakenDate = DateTime.Now
            };

            List<Book> bookList = new() { book1 };

            var mockFileHandler = new Mock<IFileHandler>();

            mockFileHandler.Setup(g => g.GetData()).Returns(bookList);

            var repository = new BookRepository(mockFileHandler.Object);

            // Act

            var result = repository.ReturnBook(book1.ISBN, book1.TakenBy);

            //Assert

            result.Should().Be(returnMessage);

            book1.TakenBy.Should().BeNullOrEmpty();

            book1.TakenDate.Should().BeNull();
        }
    }
}