namespace BookUniverse.BLL.Services
{
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.DAL.Repositories.BookRepository;
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Drive.v3;
    using Google.Apis.Services;
    using Google.Apis.Util.Store;
    using PdfSharp.Pdf.IO;

    public class GoogleDriveService : IGoogleDriveService
    {
        public static string[] Scopes = { DriveService.Scope.Drive, DriveService.Scope.DriveFile };

        private readonly IBookRepository _bookRepository;

        public GoogleDriveService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public DriveService GetService()
        {
            var applicationName = "Book Universe";
            var username = "bookuniverse34@gmail.com";

            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            new ClientSecrets
            {
                ClientId = "clientId",
                ClientSecret = "secret"
            }, Scopes,
            username, CancellationToken.None, new FileDataStore("token")).Result;

            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            });

            return service;
        }

        public async Task<(int, Google.Apis.Drive.v3.Data.File)> UploadFile(string _uploadFile)
        {
            DriveService _service = GetService();
            if (File.Exists(_uploadFile))
            {
                Google.Apis.Drive.v3.Data.File body = new Google.Apis.Drive.v3.Data.File();
                body.Name = Path.GetFileName(_uploadFile);
                body.MimeType = GetMimeType(_uploadFile);
                byte[] byteArray = File.ReadAllBytes(_uploadFile);
                MemoryStream stream = new MemoryStream(byteArray);
                int pageCount = GetPageCount(body, stream);
                try
                {
                    FilesResource.CreateMediaUpload request = _service.Files.Create(body, stream, body.MimeType);
                    request.SupportsTeamDrives = true;
                    await request.UploadAsync();
                    await SetPublicReadPermission(request.ResponseBody.Id, _service);
                    Google.Apis.Drive.v3.Data.File shareableFile = await GetShareableLink(request.ResponseBody.Id, _service);
                    return (pageCount, shareableFile);
                }
                catch
                {
                    throw new Exception("Error during uppload");
                }
            }
            else
            {
                throw new Exception("File doesn't exist");
            }
        }

        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);

            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();

            System.Diagnostics.Debug.WriteLine(mimeType);
            return mimeType;
        }

        private int GetPageCount(Google.Apis.Drive.v3.Data.File body, MemoryStream stream)
        {
            if (body.MimeType == "application/pdf")
            {
                using var pdfDocument = PdfReader.Open(stream, PdfDocumentOpenMode.ReadOnly);
                return pdfDocument.PageCount;
            }

            return -1;
        }

        private async Task SetPublicReadPermission(string fileId, DriveService service)
        {
            Google.Apis.Drive.v3.Data.Permission permission = new Google.Apis.Drive.v3.Data.Permission()
            {
                Role = "reader",
                Type = "anyone",
            };

            await service.Permissions.Create(permission, fileId).ExecuteAsync();
        }

        private async Task<Google.Apis.Drive.v3.Data.File> GetShareableLink(string fileId, DriveService service)
        {
            var fileRequest = service.Files.Get(fileId);
            fileRequest.Fields = "id,name,webViewLink";

            return await fileRequest.ExecuteAsync();
        }
    }
}
