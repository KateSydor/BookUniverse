using BookUniverse.BLL.Interfaces;
using BookUniverse.DAL.Repositories.BookFolderRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookUniverse.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookUniverse.BLL.Services
{
    public class BookFolderService : IBookFolderService
    {

        private readonly IBookFolderRepository _folderRepository;

        public BookFolderService(IBookFolderRepository fService)
        {
            _folderRepository = fService;
        }

        public List<BookFolder> GetAllBookFolders()
        {
            return _folderRepository.GetAll().ToList();
        }

        public async Task AddInFolder(BookFolder folder)
        {
            int j = folder.Id;
            var g = folder.Book;
            var e = folder.BookId;
            var w = folder.Folder;
            var t = folder.FolderId;
            await _folderRepository.Create(folder);
        }
    }
    
}
