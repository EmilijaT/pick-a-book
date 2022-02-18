using BookStore.Domain.DomainModels;
using BookStore.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Interface
{
    public interface IBookService
    {
        List<Book> GetAllBooks();
        Book GetDetailsForBook(Guid? id);
        void CreateNewBook(Book t);
        void UpdateExistingBook(Book t);
        void DeleteBook(Guid id);
        AddToShoppingCartDto GetShoppingCartInfo(Guid? id);
        bool AddToShoppingCart(AddToShoppingCartDto item, string userID);
    }
}
