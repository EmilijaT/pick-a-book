using BookStore.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Domain.DTO
{
    public class BookDto
    {
        public List<Book> Books { get; set; }
        public DateTime Date { get; set; }

        public string SearchName { get; set; }
    }
}
