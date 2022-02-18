using BookStore.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<BookStoreApplicationUser> GetAll();
        BookStoreApplicationUser Get(string id);
        void Insert(BookStoreApplicationUser entity);
        void Update(BookStoreApplicationUser entity);
        void Delete(BookStoreApplicationUser entity);
    }
}
