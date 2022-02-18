using BookStore.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Domain.DTO
{
    public class ShoppingCartDto
    {
        public List<BookInShoppingCart> Books { get; set; }
        public double TotalPrice { get; set; }
    }
}
