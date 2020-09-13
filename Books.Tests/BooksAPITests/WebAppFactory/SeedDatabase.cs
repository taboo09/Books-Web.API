using System;
using System.Collections.Generic;
using System.Linq;
using Books.API.Context;
using Books.API.Entities;

namespace Books.Tests.BooksAPITests.WebAppFactory
{
    public class SeedDatabase
    {
        public static void Initialize(BookContext context)
        {
            // delete OnModelCreating seed data
            // optional
            var books = context.Books.ToList();

            foreach (var book in books)
            {
                context.Books.RemoveRange(books);
            }

            SeedBooks(context);
        }

        private static void SeedBooks(BookContext context)
        {
            var authors = new List<Author>()
            {
                new Author { Id = new Guid("B43BBE4D-9C6D-8626-C813-1D5812D31FFA"), FirstName = "James", LastName = "Clavell" },
                new Author { Id = new Guid("F6511908-F335-1BEB-1327-DFF2BFB88086"), FirstName = "Wilbur", LastName = "Smith" }
            };

            var books = new List<Book>()
            {
                new Book { Id = new Guid("1147AF5C-E2ED-B8C2-D26E-05C472C7BDBD"), Title = "Shogun", AuthorId = new Guid("B43BBE4D-9C6D-8626-C813-1D5812D31FFA"), 
                    Description = "Lord Yabu is forced to commit seppuku for his involvement with the ninja attack and personally murdering Captain Yoshinaka." },
                new Book { Id = new Guid("E7B7339B-6755-580C-E8E0-CD31D047BFB0"), Title = "Tai-Pan", AuthorId = new Guid("B43BBE4D-9C6D-8626-C813-1D5812D31FFA"), 
                    Description = "Tai-Pan is a 1966 adventure drama novel by James Clavell." },
                new Book { Id = new Guid("DF5A74D3-9504-E1F9-EAA0-9EBD04C22C89"), Title = "King of Kings", AuthorId = new Guid("F6511908-F335-1BEB-1327-DFF2BFB88086"), 
                    Description = "The Courtney series" },
                new Book { Id = new Guid("58EB14D7-5E4E-2568-A8B9-1E718FE3E405"), Title = "The Sound of Thunder", AuthorId = new Guid("F6511908-F335-1BEB-1327-DFF2BFB88086"), 
                    Description = "The Courtney series" }
            };

            foreach (var book in books)
            {
                book.Author = authors.Single(x => x.Id == book.AuthorId);
            };

            context.Books.AddRange(books);

            context.SaveChanges();
        }
    }
}