using BookStore.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Interface
{
    public interface IUserService
    {
        bool ChangeUserRole(string userId);
        List<BookStoreApplicationUser> findAll();
        bool IsAdmin(string userId);
    }
}
