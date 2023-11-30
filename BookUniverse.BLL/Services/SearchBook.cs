namespace BookUniverse.BLL.Services
{
    using BookUniverse.BLL.Interfaces;
    using Google.Apis.Books.v1;
    using Google.Apis.Books.v1.Data;
    using Google.Apis.Services;
    using Microsoft.Extensions.Configuration;

    public class SearchBook : ISearchBook
    {
        private readonly BooksService _booksService;
        private readonly IConfiguration _configuration;

        public SearchBook(IConfiguration configuration)
        {
            _configuration = configuration;
            _booksService = new BooksService(new BaseClientService.Initializer
            {
                ApiKey = _configuration["GoogleBooksApiKey"],
                ApplicationName = "Book Universe",
            });
        }

        public async Task<Volumes> SearchAsync(string query)
        {
            var listRequest = _booksService.Volumes.List(query);
            return await listRequest.ExecuteAsync();
        }
    }
}
