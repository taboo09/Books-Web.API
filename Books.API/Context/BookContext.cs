using Books.API.Entities;
using Books.API.FakeData;
using Microsoft.EntityFrameworkCore;

namespace Books.API.Context
{
    public class BookContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public BookContext(DbContextOptions<BookContext> options) :  base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var data = Data.GenerateFakeData(10);

            modelBuilder.Entity<Author>().HasData(data.AuthorsFake);
            modelBuilder.Entity<Book>().HasData(data.BooksFake);
        }        
    }
}