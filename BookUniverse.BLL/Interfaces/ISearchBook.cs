using Google.Apis.Books.v1.Data;

namespace BookUniverse.BLL.Interfaces
{
    public interface ISearchBook
    {
        public Task<Volumes> SearchAsync(string query);
    }
}
