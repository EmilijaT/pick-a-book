using BookStore.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDto getShoppingCartInfo(string userId);
        bool deleteBookFromShoppingCart(string userId, Guid id);
        bool orderNow(string userId);
    }
}
