using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using BookUniverse.BLL.Interfaces;
using BookUniverse.DAL.Entities;
using BookUniverse.DAL.Repositories.FolderRepository;
using global::BookUniverse.BLL.Interfaces;
using global::BookUniverse.DAL.Repositories.UserRepository;

namespace BookUniverse.BLL.Services
{



    public class FolderService : IFolderService
   {
        private readonly IFolderRepository _folderRepository;

        public FolderService(IFolderRepository fService)
        {
            _folderRepository = fService;
        }

        public List<Folder> GetAllFolders()
        {
            return _folderRepository.GetAll().ToList();
        }

        
    }
}


