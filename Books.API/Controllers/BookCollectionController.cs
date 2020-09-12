using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Books.API.Filters;
using Books.API.ModelBinder;
using Books.API.Models;
using Books.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Books.API.Controllers
{
    [ApiController]
    [Route("api/bookcollection")]
    [BooksResultFilter]
    public class BookCollectionController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        public BookCollectionController(IBookRepository bookRepository, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        }

        // api/bookcollection/(id1, id2,...)
        [HttpGet("({ids})", Name = "GetBookCollection")]
        public async Task<IActionResult> GetBookCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]
            IEnumerable<Guid> ids)
        {
            var books = await _bookRepository.GetBooksAsync(ids);

            if (ids.Count() != books.Count())
            {
                return NotFound();
            }

            return Ok(books);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBookCollection(IEnumerable<BookForCreation> newBooks)
        {
            var books = _mapper.Map<IEnumerable<Entities.Book>>(newBooks);

            foreach (var book in books)
            {
                _bookRepository.AddBook(book);
            }

            await _bookRepository.SaveChangesAsync();

            var booksToReturn = await _bookRepository.GetBooksAsync(books.Select(x => x.Id).ToList());

            var bookIds = string.Join(",", booksToReturn.Select(x => x.Id));

            return CreatedAtRoute("GetBookCollection", new { ids = bookIds }, booksToReturn);
        }
    }
}