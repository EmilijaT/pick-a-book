using BookStore.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Domain.DTO
{
    public class AddToShoppingCartDto
    {
        public Book Book { get; set; }
        public Guid BookId { get; set; }
        public int Quantity { get; set; }
    }
}
