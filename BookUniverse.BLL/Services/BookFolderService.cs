﻿namespace BookUniverse.BLL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.DAL.Entities;
    using BookUniverse.DAL.Repositories.BookFolderRepository;

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
            var bookFolders = _folderRepository.GetAll().ToList();
            foreach (var bookFolder in bookFolders)
            {
                if (bookFolder.FolderId == folder.FolderId && bookFolder.BookId == folder.BookId)
                {
                    throw new Exception("Book is already in this folder");
                }
            }

            await _folderRepository.Create(folder);
        }
    }
}
