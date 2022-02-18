using BookStore.Domain.DomainModels;
using BookStore.Domain.DTO;
using BookStore.Domain.Identity;
using BookStore.Repository.Interface;
using BookStore.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Services.Implementation
{
    public class BookService : IBookService
    {
        private readonly IRepository<Book> _BookRepository;
        private readonly IRepository<BookInShoppingCart> _bookInShoppingCartRepository;
        private readonly IUserRepository _userRepository;
        public BookService(IRepository<Book> BookRepository, IRepository<BookInShoppingCart> bookInShoppingCartRepository, IUserRepository userRepository)
        {
            _BookRepository = BookRepository;
            _userRepository = userRepository;
            _bookInShoppingCartRepository = bookInShoppingCartRepository;
        }

        public void CreateNewBook(Book t)
        {
            this._BookRepository.Insert(t);
        }

        public void DeleteBook(Guid id)
        {
            var Book = this.GetDetailsForBook(id);
            this._BookRepository.Delete(Book);
        }

        public List<Book> GetAllBooks()
        {
            return this._BookRepository.GetAll().ToList();
        }

        public Book GetDetailsForBook(Guid? id)
        {
            return this._BookRepository.Get(id);
        }

        public void UpdateExistingBook(Book t)
        {
            this._BookRepository.Update(t);
        }
        public bool AddToShoppingCart(AddToShoppingCartDto item, string userID)
        {

            var user = this._userRepository.Get(userID);

            var userShoppingCard = user.ShoppingCart;

            if (item.BookId != null && userShoppingCard != null)
            {
                var book = this.GetDetailsForBook(item.BookId);

                if (book != null)
                {
                    BookInShoppingCart itemToAdd = new BookInShoppingCart
                    {
                        Id = Guid.NewGuid(),
                        Book = book,
                        BookId = book.Id,
                        ShoppingCart = userShoppingCard,
                        ShoppingCartId = userShoppingCard.Id,
                        Quantity = item.Quantity
                    };

                    this._bookInShoppingCartRepository.Insert(itemToAdd);
                    return true;
                }
                return false;
            }
            return false;
        }
        public AddToShoppingCartDto GetShoppingCartInfo(Guid? id)
        {
            var book = this.GetDetailsForBook(id);
            AddToShoppingCartDto model = new AddToShoppingCartDto
            {
                Book = book,
                BookId = book.Id,
                Quantity = 1
            };

            return model;
        }
    }
}
