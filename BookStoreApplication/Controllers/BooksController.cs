using BookStore.Domain.DomainModels;
using BookStore.Domain.DTO;
using BookStore.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookStoreApplication.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IUserService _userService;

        public BooksController(IBookService bookService, IUserService userService)
        {
            _bookService = bookService;
            _userService = userService;
        }
        // GET: BooksController
        public ActionResult Index()
        {
            ViewBag.IsAdmin = false;
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
                ViewBag.IsAdmin = _userService.IsAdmin(userId);

            var books = new BookDto
            {
                Books = _bookService.GetAllBooks(),
                Date = DateTime.Now
            };
            return View(books);
        }
        [HttpPost]
        public IActionResult Index(BookDto dto)
        {
            ViewBag.isAdmin = true;
            var books = _bookService.GetAllBooks()
                .Where(z => z.BookName.Contains(dto.SearchName)).ToList();
            var model = new BookDto
            {
                Books = books,
                Date = dto.Date
            };
            return View(model);
        }

        // GET: BooksController/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = this._bookService.GetDetailsForBook(id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: BooksController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BooksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("BookName,BookYear,BookGenre,BookDescription,BookImage,BookPrice,StartDate,EndDate")] Book book)
        {
            if (ModelState.IsValid)
            {
                this._bookService.CreateNewBook(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: BooksController/Edit/5
        public ActionResult Edit(Guid? t)
        {
            if (t == null)
                return NotFound();

            var book = this._bookService.GetDetailsForBook(t);

            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: BooksController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, [Bind("Id,BookName,BookYear,BookGenre,BookDescription,BookImage,BookPrice")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this._bookService.UpdateExistingBook(book);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: BooksController/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = this._bookService.GetDetailsForBook(id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: BooksController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            this._bookService.DeleteBook(id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult AddBookToCart(Guid? id)
        {
            var model = this._bookService.GetShoppingCartInfo(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBookToCart([Bind("BookId", "Quantity")] AddToShoppingCartDto item)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._bookService.AddToShoppingCart(item, userId);

            if (result)
            {
                return RedirectToAction("Index", "Books");
            }

            return View(item);
        }

        private bool BookExists(Guid id)
        {
            return this._bookService.GetDetailsForBook(id) != null;
        }
    }
}
