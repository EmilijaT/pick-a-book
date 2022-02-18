using BookStore.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Repository.Interface
{
    public interface IOrderRepository
    {
        List<Order> GetAllOrders();
        Order GetOrderDetails(Guid orderId);
    }
}
