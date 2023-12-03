namespace BookUniverse.BLL.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BookUniverse.DAL.Entities;

    public interface IBookFolderService
    {
        List<BookFolder> GetAllBookFolders();

        Task AddInFolder(BookFolder folder);
    }
}
