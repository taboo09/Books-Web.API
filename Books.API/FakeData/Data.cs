using System;
using System.Collections.Generic;
using Bogus;
using Books.API.Entities;

namespace Books.API.FakeData
{
    public class Data
    {
        public static FakeObject GenerateFakeData(int count)
        {
            var authors = new List<Author>();

            for (int i = 0; i < count; i++)
            {
                var author = new Faker<Author>()
                    .StrictMode(true)
                    .RuleFor(x => x.Id, f => f.Random.Guid())
                    .RuleFor(x => x.FirstName, f => f.Person.FirstName)
                    .RuleFor(x => x.LastName, f => f.Person.LastName);

                authors.Add(author.Generate());
            }

            var books = new List<Book>();

            foreach(var author in authors)
            {
                for (int i = 0; i < 4; i++)
                {
                    var book = new Faker<Book>()
                        .StrictMode(false)
                        .RuleFor(x => x.Id, f => f.Random.Guid())
                        .RuleFor(x => x.Title, f => f.Commerce.Product())
                        .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
                        .RuleFor(x => x.AuthorId, f => author.Id);

                    books.Add(book.Generate());
                }
            }

            return new FakeObject 
            {
                AuthorsFake = authors.ToArray(), 
                BooksFake = books.ToArray()
            };
        }
    }

    public class FakeObject
    {
        public Author[] AuthorsFake { get; set; }
        public Book[] BooksFake { get; set; }
    }
}