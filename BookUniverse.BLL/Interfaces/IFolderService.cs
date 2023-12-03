namespace BookUniverse.BLL.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BookUniverse.DAL.Entities;

    public interface IFolderService
    {
        List<Folder> GetAllFolders();

        Task<Folder> AddNewFolder(Folder folder, int id);
    }
}
