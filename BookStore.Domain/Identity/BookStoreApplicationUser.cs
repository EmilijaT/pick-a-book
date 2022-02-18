using BookStore.Domain.DomainModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Domain.Identity
{
    public enum Role
    {
        ROLE_ADMIN,
        ROLE_USER
    }
    public class BookStoreApplicationUser : IdentityUser
    {
        public Role Role { get; set; } = Role.ROLE_USER;
        public virtual ShoppingCart ShoppingCart { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
