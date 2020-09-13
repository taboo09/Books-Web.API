using System;
using System.Threading.Tasks;
using AutoMapper;
using Books.API.Filters;
using Books.API.Models;
using Books.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Books.API.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        public BooksController(IBookRepository bookRepository, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        }

        [HttpGet]
        [BooksResultFilter]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _bookRepository.GetBooksAsync();

            return Ok(books);
        }

        [HttpGet]
        [Route("{id}", Name = "GetBook")]
        [BookWithCoversResultFilter]
        public async Task<IActionResult> GetBook(Guid id)
        {
            var book = await _bookRepository.GetBookAsync(id);

            if (book == null) return NotFound();

            var bookCovers = await _bookRepository.GetBookCoversAsync(id);

            return Ok((book: book, bookCovers: bookCovers));
        }

        [HttpPost]
        [BookResultFilter]
        public async Task<IActionResult> AddBook(BookForCreation newBook)
        {
            var book = _mapper.Map<Entities.Book>(newBook);

            _bookRepository.AddBook(book);

            await _bookRepository.SaveChangesAsync();

            // fetch the book from DB, including the author
            await _bookRepository.GetBookAsync(book.Id);

            return CreatedAtRoute("GetBook", new { id = book.Id }, book);
        }
    }
}