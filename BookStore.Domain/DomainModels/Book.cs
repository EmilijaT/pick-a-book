using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStore.Domain.DomainModels
{
    public enum Genre
    {
        Action,
        Classics,
        Drama,
        Thriller,
        Mistery,
        Fantasy,
        Comic,
        History,
        Horror,
        Romance,
        Psychology
    }
    public class Book : BaseEntity
    {
        [Required]
        public string BookName { get; set; }
        [Required]
        public Genre BookGenre { get; set; }
        [Required]
        public string BookYear { get; set; }
        [Required]
        public string BookDescription { get; set; }
        [Required]
        public string BookImage { get; set; }
        [Required]
        public int BookPrice { get; set; }
        [Required]
       public DateTime StartDate { get; set; }
      [Required]
       public DateTime EndDate { get; set; }
        public virtual ICollection<BookInShoppingCart> BooksInShoppingCart { get; set; }
        public virtual ICollection<BookInOrder> BookInOrder { get; set; }
    }
}
