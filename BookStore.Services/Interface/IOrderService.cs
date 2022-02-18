using BookStore.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Interface
{
    public interface IOrderService
    {
        List<Order> GetAllOrders();
        Order GetOrderDetails(Guid orderId);
    }
}
