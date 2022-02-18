using BookStore.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        public string UserId { get; set; }
        public BookStoreApplicationUser User { get; set; }
        public virtual ICollection<BookInShoppingCart> BooksInShoppingCart { get; set; }
    }
}
