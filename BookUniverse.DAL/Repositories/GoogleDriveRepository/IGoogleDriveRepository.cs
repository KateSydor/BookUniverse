using BookUniverse.DAL.Entities;
using Google.Apis.Drive.v3;

namespace BookUniverse.DAL.Repositories.GoogleDriveRepository
{
    public interface IGoogleDriveRepository
    {
        DriveService GetService();

        List<GoogleDrive> GetDriveFiles();

        void DeleteFile(GoogleDrive files);

    }
}
