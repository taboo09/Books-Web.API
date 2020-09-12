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
        Task<IEnumerable<Book>> GetBooksAsync(IEnumerable<Guid> bookIds);
        IEnumerable<Book> GetBooks();
        void AddBook(Entities.Book book);
        Task<bool> SaveChangesAsync();
    }
}