using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Books.API.Models;
using Books.Tests.BooksAPITests.WebAppFactory;
using FluentAssertions;
// using Newtonsoft.Json;
using Xunit;

namespace Books.Tests.BooksAPITests.Controllers
{
    public class BooksControllerTests : IntegrationTests
    {
        [Fact]
        public async Task GetBooks_ReturnsOk_WhenCalled()
        {
            var response = await _client.GetAsync("books");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetBooks_ReturnsBooksListOf4BooksModel_WhenCalled()
        {
            var response = await _client.GetAsync("books");

            var books = new List<Books.API.Models.Book>();

            if (response.IsSuccessStatusCode)
            {
                books =  JsonSerializer.Deserialize<List<Books.API.Models.Book>>(await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }

            books.Should().HaveCount(4);
            books.Should().BeOfType(typeof(List<Book>));
        }

        [Fact]
        public async Task GetBook_ReturnsBookWithCovers_WhenIdIsPassed()
        {
            var bookId = new Guid("1147AF5C-E2ED-B8C2-D26E-05C472C7BDBD");
            var response = await _client.GetAsync($"books\\{bookId}");

            var book = new BookWithCovers();

            if (response.IsSuccessStatusCode)
            {
                book =  JsonSerializer.Deserialize<BookWithCovers>(await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }

            book.Should().BeOfType(typeof(BookWithCovers));
            book.BookCovers.Should().HaveCount(4);
            book.Id.Should().Be(bookId);
            book.Author.Should().Be("James Clavell");
        }

        [Fact]
        public async Task GetBook_ReturnsNotFound_WhenBookIsNull()
        {
            var bookId = Guid.NewGuid();
            var response = await _client.GetAsync($"books\\{bookId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddBook_ReturnsCreatedAtRoute_WhenNewBookIsPassedAsParameter()
        {
            var newBook = CreateBookForCreation();

            string jsonObject = Newtonsoft.Json.JsonConvert.SerializeObject(newBook);
            var payload = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"books", payload);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task AddBook_ReturnBookWithCovers_WhenNewBookIsPassedAsParameter()
        {
            var newBook = CreateBookForCreation();

            string jsonObject = Newtonsoft.Json.JsonConvert.SerializeObject(newBook);
            var payload = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"books", payload);

            var bookWithCovers = new BookWithCovers();

            if (response.IsSuccessStatusCode)
            {
                bookWithCovers =  JsonSerializer.Deserialize<Books.API.Models.BookWithCovers>(await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }

            bookWithCovers.Title.Should().Be(newBook.Title);
            bookWithCovers.Id.Should().NotBe(new Guid());
        }

        private BookForCreation CreateBookForCreation()
        {
            return new BookForCreation()
            {
                AuthorId = Guid.NewGuid(),
                Title = "Test Book",
                Description = "Description for Test"
            };
        }
    }
}