namespace BookUniverse.BLL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.DAL.Entities;
    using BookUniverse.DAL.Repositories.FolderRepository;

    public class FolderService : IFolderService
    {
        private readonly IFolderRepository _folderRepository;
        private int minSize = 2;
        private int maxSize = 30;

        public FolderService(IFolderRepository fService)
        {
            _folderRepository = fService;
        }

        public List<Folder> GetAllFolders()
        {
            return _folderRepository.GetAll().ToList();
        }

        public async Task<Folder> AddNewFolder(Folder folder, int id)
        {
            Folder newFolder = await _folderRepository.Get(u => u.FolderName == folder.FolderName && u.UserId == id);

            if (newFolder != null)
            {
                throw new Exception("Folder with this name already exist");
            }
            if (folder.FolderName.Length < minSize || folder.FolderName.Length > maxSize)
            {
                throw new Exception("Not valid length of folder name");
            }

            return await _folderRepository.Create(folder);
        }
    }
}
