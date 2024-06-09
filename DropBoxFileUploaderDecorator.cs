using Dropbox.Api.Files;
using Dropbox.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uml_hw_two
{
    public class DropBoxFileUploaderDecorator : BaseFileUploaderDecorator
    {
        private readonly BaseFileUploader _wrappedUploader;
        private readonly string _accessToken;

        public DropBoxFileUploaderDecorator(BaseFileUploader wrappedUploader, string accessToken)
        {
            _wrappedUploader = wrappedUploader;
            _accessToken = accessToken;
        }

        public override async Task UploadFileAsync(string filePath)
        {
            await _wrappedUploader.UploadFileAsync(filePath);

            using (var dbx = new DropboxClient(_accessToken))
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open))
                {
                    var fileName = Path.GetFileName(filePath);
                    var dropboxPath = $"/{fileName}";

                    await dbx.Files.UploadAsync(
                        dropboxPath,
                        WriteMode.Overwrite.Instance,
                        body: fileStream);
                }
            }
        }
    }
}
