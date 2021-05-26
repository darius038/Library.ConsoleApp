using Library.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces
{
    public interface IFileHandler
    {
        IEnumerable<Book> GetData();
        void SaveData(IEnumerable books);
    }
}
