using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.API.Context;
using Books.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Books.API.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly BookContext _context;
        public BookRepository(BookContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        public async Task<Book> GetBookAsync(Guid id)
        {
            return await _context.Books
                .Include(x => x.Author)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public IEnumerable<Book> GetBooks()
        {
            return _context.Books
                .Include(x => x.Author)
                .ToList();
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await _context.Books
                .Include(x => x.Author)
                .ToListAsync();
        }
    }
}