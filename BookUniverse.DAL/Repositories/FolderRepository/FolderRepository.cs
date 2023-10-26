namespace BookUniverse.DAL.Repositories.FolderRepository
{
    using BookUniverse.DAL.Entities;
    using BookUniverse.DAL.Persistence;
    using BookUniverse.DAL.Repositories.Base;

    public class FolderRepository : Repository<Folder>, IFolderRepository
    {
        public FolderRepository(DatabaseContext context)
        : base(context)
        {
        }
    }
}
