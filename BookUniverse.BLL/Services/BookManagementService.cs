namespace BookUniverse.BLL.Services
{
    using AutoMapper;
    using BookUniverse.BLL.DTOs.BookDTOs;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.DAL.Entities;
    using BookUniverse.DAL.Repositories.BookRepository;
    using BookUniverse.DAL.Repositories.UserBookRepository;

    public class BookManagementService : IBookManagementService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserBookRepository _userBookRepository;
        private readonly IMapper _mapper;

        public BookManagementService(IBookRepository bookRepository, IUserBookRepository userBookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _userBookRepository = userBookRepository;
            _mapper = mapper;
        }

        public async void AddBook(AddBookDto newBook, Category category)
        {
            Book book = _mapper.Map<Book>(newBook, opt => opt.Items["CategoryId"] = category.Id);
            await _bookRepository.Create(book);
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

        public async Task AddUserBook(UserBook newUserBook)
        {
            await _userBookRepository.Create(newUserBook);
        }
    }
}
