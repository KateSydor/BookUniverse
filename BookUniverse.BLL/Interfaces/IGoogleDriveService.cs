namespace BookUniverse.BLL.Interfaces
{
    using Google.Apis.Drive.v3;

    public interface IGoogleDriveService
    {
        DriveService GetService();

        //List<GoogleDrive> GetDriveFiles();

        Task<(int, Google.Apis.Drive.v3.Data.File)> UploadFile(string _uploadFile);
    }
}
