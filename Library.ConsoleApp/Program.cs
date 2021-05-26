using Library.Application.Interfaces;
using Library.Application.Repositories;
using Library.Application.Services;
using System;

namespace Library.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            JsonFileHandler fileHandler = new();

            BookRepository repository = new (fileHandler);

            LibraryApp libraryApp = new(repository);

            libraryApp.Start();

        }
    }
}
