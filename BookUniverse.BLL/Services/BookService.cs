namespace BookUniverse.BLL.Services
{
    using AutoMapper;
    using BookUniverse.BLL.DTOs.BookDTOs;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.DAL.Entities;
    using BookUniverse.DAL.Repositories.BookRepository;
    using BookUniverse.DAL.Repositories.UserBookRepository;

    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserBookRepository _userBookRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public BookService(IBookRepository bookRepository, IUserBookRepository userBookRepository, IMapper mapper, ILoggingService logger)
        {
            _bookRepository = bookRepository;
            _userBookRepository = userBookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async void AddBook(AddBookDto newBook, Category category)
        {
            Book book = _mapper.Map<Book>(newBook, opt => opt.Items["CategoryId"] = category.Id);
            await _bookRepository.Create(book);
            _logger.LogInformation($"The book {newBook.Title} has been successfully created");
        }

        public List<Book> GetAllBooks()
        {
            return _bookRepository.GetAll().ToList();
        }

        public async Task<Book> GetBook(int id)
        {
            return await _bookRepository.Get(b => b.Id == id);
        }

        public List<Book> GetUserBooks(string userEmail)
        {
            return _userBookRepository.GetAllByUser(u => u.User.Email == userEmail).ToList();
        }
    }
}
