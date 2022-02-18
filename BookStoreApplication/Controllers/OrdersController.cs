using BookStore.Services.Interface;
using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApplication.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }
        public IActionResult Index()
        {
            var orders = this._orderService.GetAllOrders();
            return View(orders);
        }
        public IActionResult Details(Guid orderId)
        {
            var order = this._orderService.GetOrderDetails(orderId);
            return View(order);
        }

        public IActionResult CreateInvoice(Guid orderId) //SO GEMBOX
        {
            var order = this._orderService.GetOrderDetails(orderId);

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "orderInvoice.docx");
            var document = DocumentModel.Load(templatePath);

            document.Content.Replace("{{OrderNumber}}", order.Id.ToString());
            document.Content.Replace("{{Email}}", order.User.Email);

            StringBuilder sb = new StringBuilder();
            var totalPrice = 0.0;
            foreach (var item in order.BooksInOrder)
            {
                totalPrice += item.Quantity * item.Book.BookPrice;
                sb.AppendLine(item.Book.BookName + " " + item.Book.BookYear + " with quantity of: " + item.Quantity + " and price of: " + item.Book.BookPrice + "MKD");
            }
            document.Content.Replace("{{BookList}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", totalPrice.ToString() + "MKD");

            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "OrderInvoice.pdf");
        }
    }
}
