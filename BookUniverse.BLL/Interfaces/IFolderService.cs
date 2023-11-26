using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookUniverse.BLL.DTOs.BookDTOs;
using BookUniverse.DAL.Entities;
using global::BookUniverse.BLL.DTOs.BookDTOs;


namespace BookUniverse.BLL.Interfaces
{
    public interface IFolderService
    {
        List<Folder> GetAllFolders();
        Task AddNewFolder(Folder folder);
        Task<Folder> GetLastFolder();

    }
}


