using BookStore.Domain.DomainModels;
using BookStore.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Order> entities;
        string errorMessage = string.Empty;

        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Order>();
        }
        public List<Order> GetAllOrders()
        {
            return entities
                .Include(z => z.BooksInOrder)
                .Include("BooksInOrder.Book")
                .Include(z => z.User)
                .ToListAsync().Result;
        }

        public Order GetOrderDetails(Guid orderId)
        {
            return entities
                .Include(z => z.BooksInOrder)
                .Include("BooksInOrder.Book")
                .Include(z => z.User)
                .SingleOrDefaultAsync(z => z.Id == orderId).Result;
        }
    }
}
