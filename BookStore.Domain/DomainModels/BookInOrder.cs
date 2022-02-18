using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Domain.DomainModels
{
    public class BookInOrder : BaseEntity
    {
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public Guid OrderId { get; set; }
        public Order UserOrder { get; set; }
        public int Quantity { get; set; }
    }
}
