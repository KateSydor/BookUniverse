using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookUniverse.DAL.Entities;

namespace BookUniverse.BLL.Interfaces
{
    public interface IBookFolderService
    {
        List<BookFolder> GetAllBookFolders();
        Task AddInFolder(BookFolder folder);
    }
}
