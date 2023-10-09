using BookUniverse.DAL.Entities;
using BookUniverse.DAL.Persistence;
using BookUniverse.DAL.Repositories.Base;

namespace BookUniverse.DAL.Repositories.FolderRepository
{
    public class FolderRepository : Repository<Folder>, IFolderRepository
    {
        public FolderRepository(DatabaseContext context)
        : base(context)
        {
        }
    }
}
