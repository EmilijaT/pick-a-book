using BookStore.Domain.DomainModels;
using BookStore.Repository.Interface;
using BookStore.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            this._orderRepository = orderRepository;
        }
        public List<Order> GetAllOrders()
        {
            return this._orderRepository.GetAllOrders();
        }

        public Order GetOrderDetails(Guid orderId)
        {
            return this._orderRepository.GetOrderDetails(orderId);
        }
    }
}
