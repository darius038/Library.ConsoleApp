using System;

namespace Library.Domain.Entities
{
    public class Book
    {        
        public string Name { get; set; }

        public string Author { get; set; }

        public string Category { get; set; }

        public string Language { get; set; }

        public string PublicationDate { get; set; }

        public string ISBN { get; set; }

        public string TakenBy { get; set; }

        public DateTime? TakenDate { get; set; }
    }
}
