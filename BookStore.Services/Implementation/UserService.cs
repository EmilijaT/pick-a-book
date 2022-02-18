using BookStore.Domain.Identity;
using BookStore.Repository.Interface;
using BookStore.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool ChangeUserRole(string userId)
        {
            throw new NotImplementedException();
        }

        public List<BookStoreApplicationUser> findAll()
        {
            return (List<BookStoreApplicationUser>)_userRepository.GetAll();
        }

        public bool IsAdmin(string userId)
        {
            BookStoreApplicationUser user = _userRepository.Get(userId);
            if (user.Role == Role.ROLE_ADMIN) return true;
            return false;
        }
    }
}
