using BookStore.Domain.DomainModels;
using BookStore.Domain.DTO;
using BookStore.Domain.Identity;
using BookStore.Services.Interface;
using ClosedXML.Excel;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookStoreApplication.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IBookService _bookService;
        private readonly UserManager<BookStoreApplicationUser> _userManager;
        public AdminController(IUserService userService, IBookService bookService, UserManager<BookStoreApplicationUser> userManager)
        {
            _userManager = userManager;
            _bookService = bookService;
            _userService = userService;
        }
        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!_userService.IsAdmin(userId))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public IActionResult ImportUsers(IFormFile file)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!_userService.IsAdmin(userId))
            {
                return RedirectToAction("Index", "Home");
            }
            //make a copy
            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";

            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            //read data from copy file

            List<UserRegistrationDto> users = getAllUsersFromFile(file.FileName);

            bool status = true;
            foreach (var item in users)
            {
                var userCheck = _userManager.FindByEmailAsync(item.Email).Result;
                Role role;
                if (item.UserRole == 0)
                    role = Role.ROLE_ADMIN;
                else role = Role.ROLE_USER;

                if (userCheck == null)
                {
                    var user = new BookStoreApplicationUser
                    {
                        UserName = item.Email,
                        NormalizedUserName = item.Email,
                        Email = item.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        ShoppingCart = new ShoppingCart(),
                        Role = role
                    };
                    var result = _userManager.CreateAsync(user, item.Password).Result;

                    status = status && result.Succeeded;
                }
                else
                {
                    continue;
                }
            }

            return RedirectToAction("Index", "Admin");
        }

        private List<UserRegistrationDto> getAllUsersFromFile(string fileName)
        {

            List<UserRegistrationDto> users = new List<UserRegistrationDto>();
            string filePath = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        Role userRole;
                        if (reader.GetValue(3).Equals("ROLE_ADMIN"))
                        {
                            userRole = Role.ROLE_ADMIN;
                        }
                        else userRole = Role.ROLE_USER;
                        users.Add(new UserRegistrationDto
                        {
                            Email = reader.GetValue(0).ToString(),
                            Password = reader.GetValue(1).ToString(),
                            ConfirmPassword = reader.GetValue(2).ToString(),
                            UserRole = userRole
                        });
                    }
                }
            }
            return users;
        }

        [HttpPost]
        public FileContentResult ExportBooksFromGenre([Bind("BookGenre")] Book book)  // SO CLOSEDXML
        {
            var genre = book.BookGenre;

            string fileName = "books"+genre+".xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add(genre.ToString());

                worksheet.Cell(1, 1).Value = "Book Id";
                worksheet.Cell(1, 2).Value = "Book Name";
                worksheet.Cell(1, 3).Value = "Book Year";
                worksheet.Cell(1, 4).Value = "Book Price";
              //  worksheet.Cell(1, 5).Value = "Streaming From";
              //  worksheet.Cell(1, 6).Value = "Streaming To";


                var result = this._bookService.GetAllBooks().Where(z => z.BookGenre == genre).ToList();

                if (result.Count > 0)
                {
                    for (int i = 1; i <= result.Count(); i++)
                    {
                        var item = result[i - 1];

                        worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                        worksheet.Cell(i + 1, 2).Value = item.BookName.ToString();
                        worksheet.Cell(i + 1, 3).Value = item.BookYear.ToString();
                        worksheet.Cell(i + 1, 4).Value = item.BookPrice.ToString();
                       // worksheet.Cell(i + 1, 5).Value = item.StartDate.ToString();
                       // worksheet.Cell(i + 1, 6).Value = item.EndDate.ToString();
                    }
                }
                else
                {
                    worksheet.Cell(2, 1).Value = "No books for selected genre";
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }

            }
        }

        public FileContentResult ExportAllBooks()  // SO CLOSEDXML
        {

            string fileName = "AllBooks.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("AllBooks");

                worksheet.Cell(1, 1).Value = "Book Id";
                worksheet.Cell(1, 2).Value = "Book Name";
                worksheet.Cell(1, 3).Value = "Book Year";
                worksheet.Cell(1, 4).Value = "Book Genre";
                worksheet.Cell(1, 5).Value = "Book Price";
               // worksheet.Cell(1, 6).Value = "Streaming From";
               // worksheet.Cell(1, 7).Value = "Streaming To";


                var result = _bookService.GetAllBooks().ToList();

                if (result.Count > 0)
                {
                    for (int i = 1; i <= result.Count(); i++)
                    {
                        var item = result[i - 1];

                        worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                        worksheet.Cell(i + 1, 2).Value = item.BookName.ToString();
                        worksheet.Cell(i + 1, 3).Value = item.BookYear.ToString();
                        worksheet.Cell(i + 1, 4).Value = item.BookGenre.ToString();
                        worksheet.Cell(i + 1, 5).Value = item.BookPrice.ToString();
                      //  worksheet.Cell(i + 1, 6).Value = item.StartDate.ToString();
                      //  worksheet.Cell(i + 1, 7).Value = item.EndDate.ToString();
                    }
                }
                else
                {
                    worksheet.Cell(2, 1).Value = "No active books";
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }

            }
        }
    }
}
