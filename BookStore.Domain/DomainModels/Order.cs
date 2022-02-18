using BookStore.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Domain.DomainModels
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public BookStoreApplicationUser User { get; set; }
        public IEnumerable<BookInOrder> BooksInOrder { get; set; }
    }
}
