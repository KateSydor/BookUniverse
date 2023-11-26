namespace BookUniverse.DAL.Repositories.FolderRepository
{
    using BookUniverse.DAL.Entities;
    using BookUniverse.DAL.Persistence;
    using BookUniverse.DAL.Repositories.Base;
    using Microsoft.EntityFrameworkCore;

    public class FolderRepository : Repository<Folder>, IFolderRepository
    {
        public FolderRepository(DatabaseContext context)
        : base(context)
        {
        }

        public async Task<Folder> GetLastAddedFolder()
        {
            return await dbSet
                .OrderByDescending(folder => folder.Id)
                .FirstOrDefaultAsync();
        }
    }
}
