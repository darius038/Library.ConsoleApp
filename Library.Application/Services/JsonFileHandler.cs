using Library.Application.Interfaces;
using Library.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Services
{
    public class JsonFileHandler : IFileHandler
    {
        private readonly string _fileName = "library.json";
        public IEnumerable<Book> GetData()
        {
            if (!File.Exists(_fileName))
            {
                return new List<Book>();
            }

            var jsonData = File.ReadAllText(_fileName);

            var booksList = JsonConvert.DeserializeObject<List<Book>>(jsonData);

            return booksList;
        }

        public void SaveData(IEnumerable books)
        {
            string jsonData = JsonConvert.SerializeObject(books);

            File.WriteAllText(_fileName, jsonData);
        }
    }
}
