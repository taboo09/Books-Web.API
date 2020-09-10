using System;
using Books.API.Filters;
using Books.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Books.API.Controllers
{
    [ApiController]
    [Route("api/synchronousbooks")]
    public class SynchronousBooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        public SynchronousBooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        }

        [HttpGet]
        [BooksResultFilter]
        public IActionResult GetBooks()
        {
            var books = _bookRepository.GetBooks();

            return Ok(books);
        }
    }
}