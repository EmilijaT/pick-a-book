using BookStore.Domain.DomainModels;
using BookStore.Domain.DTO;
using BookStore.Repository.Interface;
using BookStore.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Services.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepositorty;
        private readonly IRepository<Order> _orderRepositorty;
        private readonly IRepository<BookInOrder> _BookInOrderRepositorty;
        private readonly IRepository<EmailMessage> _emailMessageRepository;
        private readonly IUserRepository _userRepository;

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository,
                                    IRepository<BookInOrder> BookInOrderRepositorty,
                                    IRepository<Order> orderRepositorty,
                                    IUserRepository userRepository,
                                    IRepository<EmailMessage> emailMessageRepository)
        {
            _shoppingCartRepositorty = shoppingCartRepository;
            _userRepository = userRepository;
            _orderRepositorty = orderRepositorty;
            _BookInOrderRepositorty = BookInOrderRepositorty;
            _emailMessageRepository = emailMessageRepository;
        }

        public bool deleteBookFromShoppingCart(string userId, Guid id)
        {
            if (!string.IsNullOrEmpty(userId) && id != null)
            {
                var loggedInUser = this._userRepository.Get(userId);
                var userShoppingCart = loggedInUser.ShoppingCart;
                var itemToDelete = userShoppingCart.BooksInShoppingCart.Where(z => z.BookId.Equals(id)).FirstOrDefault();

                userShoppingCart.BooksInShoppingCart.Remove(itemToDelete);

                this._shoppingCartRepositorty.Update(userShoppingCart);

                return true;
            }

            return false;
        }

        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            var loggedInUser = this._userRepository.Get(userId);

            var userShoppingCart = loggedInUser.ShoppingCart;

            var books = userShoppingCart.BooksInShoppingCart.ToList();

            var allBookPrice = books.Select(z => new
            {
                BookPrice = z.Book.BookPrice,
                Quanitity = z.Quantity
            }).ToList();

            var totalPrice = 0;


            foreach (var item in allBookPrice)
            {
                totalPrice += item.Quanitity * item.BookPrice;
            }


            ShoppingCartDto scDto = new ShoppingCartDto
            {
                Books = books,
                TotalPrice = totalPrice
            };


            return scDto;

        }

        public bool orderNow(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                //Select * from Users Where Id LIKE userId

                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.ShoppingCart;

                EmailMessage mail = new EmailMessage
                {
                    MailTo = loggedInUser.Email,
                    Subject = "Successfully created order",
                    Status = false
                };

                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    User = loggedInUser,
                    UserId = userId
                };

                this._orderRepositorty.Insert(order);

                List<BookInOrder> BookInOrder = new List<BookInOrder>();

                var result = userShoppingCart.BooksInShoppingCart.Select(z => new BookInOrder
                {
                    Id = Guid.NewGuid(),
                    BookId = z.Book.Id,
                    Book = z.Book,
                    OrderId = order.Id,
                    UserOrder = order,
                    Quantity = z.Quantity
                }).ToList();

                StringBuilder sb = new StringBuilder();

                var totalPrice = 0;

                sb.AppendLine("Your order is completed. The order conains: ");

                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];

                    totalPrice += item.Quantity * item.Book.BookPrice;

                    sb.AppendLine(i.ToString() + ". " 
                        + item.Book.BookName + " " + item.Book.BookYear + " , genre: " + item.Book.BookGenre +
                        " with price of: " + item.Book.BookPrice + " and quantity of: " + item.Quantity);
                }

                sb.AppendLine("Total price: " + totalPrice.ToString());


                mail.Content = sb.ToString();


                BookInOrder.AddRange(result);

                foreach (var item in BookInOrder)
                {
                    this._BookInOrderRepositorty.Insert(item);
                }

                loggedInUser.ShoppingCart.BooksInShoppingCart.Clear();

                this._userRepository.Update(loggedInUser);
                this._emailMessageRepository.Insert(mail);

                return true;
            }
            return false;
        }
    }
}
