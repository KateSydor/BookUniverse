namespace BookUniverse.BLL.Interfaces
{
    using Google.Apis.Drive.v3;

    public interface IGoogleDriveService
    {
        DriveService GetService();

		Task<(int, Google.Apis.Drive.v3.Data.File)> UploadFile(string _uploadFile);
    }
}
