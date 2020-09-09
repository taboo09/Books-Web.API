using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Books.API.Entities;

namespace Books.API.Services
{
    public interface IBookRepository
    {
        Task<Book> GetBookAsync(Guid id);
        Task<IEnumerable<Book>> GetBooksAsync();
    }
}