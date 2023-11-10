namespace BookUniverse.BLL.Interfaces
{
    using BookUniverse.DAL.Entities;
    using Google.Apis.Drive.v3;

    public interface IGoogleDriveService
    {
        DriveService GetService();

        List<GoogleDrive> GetDriveFiles();

        Google.Apis.Drive.v3.Data.File uploadFile(string _uploadFile);

    }
}
