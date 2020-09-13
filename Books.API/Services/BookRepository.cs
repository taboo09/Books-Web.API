using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Books.API.Context;
using Books.API.Entities;
using Books.API.ExternalModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Books.API.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly BookContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly ILogger<BookRepository> _logger;

        public BookRepository(BookContext context, IHttpClientFactory httpClientFactory, ILogger<BookRepository> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddBook(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            _context.Add(book);
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

        public async Task<IEnumerable<Book>> GetBooksAsync(IEnumerable<Guid> bookIds)
        {
            return await _context.Books
                .Where(x => bookIds.Contains(x.Id))
                .Include(x => x.Author)
                .ToListAsync();
        }

        public async Task<BookCover> GetBookCoverAsync(string coverId)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.GetAsync($"https://localhost:6001/api/bookcovers/{coverId}");

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<BookCover>(await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }

            return null;
        }

        public async Task<IEnumerable<BookCover>> GetBookCoversAsync(Guid bookId)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var bookCovers = new List<BookCover>();

            _cancellationTokenSource = new CancellationTokenSource();

            // create a list of fake bookcovers
            var bookCoversUrls = new[]
            {
                $"https://localhost:6001/api/bookcovers/{bookId}-dummycover1",
                $"https://localhost:6001/api/bookcovers/{bookId}-dummycover2",
                // $"https://localhost:6001/api/bookcovers/{bookId}-dummycover3?returnFault=true",
                $"https://localhost:6001/api/bookcovers/{bookId}-dummycover4",
                $"https://localhost:6001/api/bookcovers/{bookId}-dummycover5",
            };

            // create the tasks
            var downloadBookCoverTasksQuery =
                from bookCoverUrl
                in bookCoversUrls
                select DownloadBookCoverAsync(httpClient, bookCoverUrl, _cancellationTokenSource.Token);    

            // start the tasks
            var downloadBookCoverTasks = downloadBookCoverTasksQuery.ToList();        

            try
            {
                return await Task.WhenAll(downloadBookCoverTasks);
            }
            catch (OperationCanceledException operationCanceledException)
            {
                _logger.LogInformation($"{operationCanceledException.Message}");

                foreach (var task in downloadBookCoverTasks)
                {
                    _logger.LogInformation($"Task {task.Id} has status {task.Status}");
                }
                
                return new List<BookCover>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");

                throw;
            }
            

            // foreach(var bookCoverUrl in bookCoversUrls)
            // {
            //     var response = await httpClient.GetAsync(bookCoverUrl);

            //     if (response.IsSuccessStatusCode)
            //     {
            //         bookCovers.Add(JsonSerializer.Deserialize<BookCover>(await response.Content.ReadAsStringAsync(),
            //             new JsonSerializerOptions
            //             {
            //                 PropertyNameCaseInsensitive = true
            //             }));
            //     }
            // }

            // return bookCovers;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        private async Task<BookCover> DownloadBookCoverAsync(HttpClient httpClient, string bookCoverUrl,
            CancellationToken cancellationToken)
        {
            var response = await httpClient.GetAsync(bookCoverUrl, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var bookCover = JsonSerializer.Deserialize<BookCover>(await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                return bookCover;
            }

            _cancellationTokenSource.Cancel();

            return null;
        }
    }
}