using Microsoft.AspNetCore.Mvc;
using Bookstore.Models;
using System.Xml.Linq;

namespace Bookstore.Controllers
{
    public class BookController : Controller
    {
        private Repository<Book> data { get; set; }
        public BookController(BookstoreContext ctx) => data = new Repository<Book>(ctx);

        private const string XmlExportPath = "wwwroot/data/books.xml";

        public RedirectToActionResult Index() => RedirectToAction("List");

        public ViewResult List(BookGridData values)
        {
            // create options for querying books
            var options = new QueryOptions<Book> { 
                Includes = "Authors, Genre",
                OrderByDirection = values.SortDirection,
                PageNumber = values.PageNumber,
                PageSize = values.PageSize
            };
            if (values.IsSortByGenre) 
                options.OrderBy = b => b.GenreId;
            else if (values.IsSortByPrice) 
                options.OrderBy = b => b.Price;
            else 
                options.OrderBy = b => b.Title;

            // create view model
            var vm = new BookListViewModel { 
                Books = data.List(options),
                CurrentRoute = values,
                TotalPages = values.GetTotalPages(data.Count)
            };

            return View(vm);
        }

        public ViewResult Details(int id)
        {
            var book = data.Get(new QueryOptions<Book> {
                Where = b => b.BookId == id,
                Includes = "Authors, Genre"
            }) ?? new Book();
            return View(book);
        }

        [HttpPost]
        public RedirectToActionResult PageSize(BookGridData currentRoute)
        {
            return RedirectToAction("List", currentRoute.ToDictionary());
        }

        public IActionResult ExportToXml()
        {
            var options = new QueryOptions<Book>
            {
                Includes = "Authors, Genre"
            };
            var books = data.List(options);

            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("Books", books.Select(book =>
                {
                    string authors = "";
                    foreach (var author in book.Authors)
                    {
                        if (authors.Length > 0) authors += ", ";
                        authors += author.FullName;
                    }

                    return new XElement("Book",
                        new XElement("Id", book.BookId),
                        new XElement("Title", book.Title),
                        new XElement("Authors", authors),
                        new XElement("Price", book.Price),
                        new XElement("Genre", book.Genre?.Name ?? "N/A")
                    );
                })
                )
            );

            Directory.CreateDirectory("wwwroot/data");
            doc.Save(XmlExportPath);

            TempData["message"] = $"Exported {books.Count()} books to {XmlExportPath}";
            return RedirectToAction("List");
        }
    }   
}