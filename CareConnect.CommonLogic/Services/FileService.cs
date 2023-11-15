using System.IO.Compression;
using CareConnect.CommonLogic.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;

namespace CareConnect.CommonLogic.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<FileService> _logger;

        public FileService(IWebHostEnvironment environment, ILogger<FileService> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public bool SaveFile(List<IFormFile> files, string subDirectory)
        {
            subDirectory ??= string.Empty;

            try
            {
                string imagesPath = Path.Combine(_environment.WebRootPath, "documents");

                string target = Path.Combine(imagesPath, subDirectory);

                Directory.CreateDirectory(target);

                files.ForEach(async file =>
                {
                    if (file.Length <= 0) return;
                    var filePath = Path.Combine(target, file.FileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"File upload error: {ex}");
                return false;
            }

            return true;
        }

        public async Task<bool> SaveFile(IFormFile file, string subDirectory)
        {
            subDirectory ??= string.Empty;

            try
            {
                string imagesPath = Path.Combine(_environment.WebRootPath, "documents");

                string target = Path.Combine(imagesPath, subDirectory);

                Directory.CreateDirectory(target);
               
                var filePath = Path.Combine(target, file.FileName);
                using FileStream stream = new (filePath, FileMode.Create);
                await file.CopyToAsync(stream);
            
            }
            catch (Exception ex)
            {
                _logger.LogError($"File upload error: {ex}");
                return false;
            }

            return true;
        }

        public async Task<string> SaveFile2(IFormFile file, string subDirectory)
        {
            subDirectory ??= string.Empty;
            var filePath = string.Empty;

            try
            {
                string imagesPath = Path.Combine(_environment.WebRootPath, "documents");

                string target = Path.Combine(imagesPath, subDirectory);

                Directory.CreateDirectory(target);

                filePath = Path.Combine(target, file.FileName);
                using FileStream stream = new(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

            }
            catch (Exception ex)
            {
                _logger.LogError($"File upload error: {ex}");
                return null;
            }

            return filePath;
        }

        public (string fileType, byte[] archiveData, string archiveName) FetechFiles(string subDirectory)
        {
            string zipName = $"archive-{DateTime.Now:yyyy_MM_dd-HH_mm_ss}.zip";

            var files = Directory.GetFiles(Path.Combine(_environment.WebRootPath, subDirectory)).ToList();

            using var memoryStream = new MemoryStream();
            using (ZipArchive archive = new (memoryStream, ZipArchiveMode.Create, true))
            {
                files.ForEach(file =>
                {
                    var theFile = archive.CreateEntry(file);
                    using StreamWriter streamWriter = new (theFile.Open());
                    streamWriter.Write(File.ReadAllText(file));

                });
            }

            return ("application/zip", memoryStream.ToArray(), zipName);

        }

        public string SizeConverter(long bytes)
        {
            var fileSize = new decimal(bytes);
            var kilobyte = new decimal(1024);
            var megabyte = new decimal(1024 * 1024);
            var gigabyte = new decimal(1024 * 1024 * 1024);

            return fileSize switch
            {
                var _ when fileSize < kilobyte => $"Less then 1KB",
                var _ when fileSize < megabyte => $"{Math.Round(fileSize / kilobyte, 0, MidpointRounding.AwayFromZero):##,###.##}KB",
                var _ when fileSize < gigabyte => $"{Math.Round(fileSize / megabyte, 2, MidpointRounding.AwayFromZero):##,###.##}MB",
                var _ when fileSize >= gigabyte => $"{Math.Round(fileSize / gigabyte, 2, MidpointRounding.AwayFromZero):##,###.##}GB",
                _ => "n/a",
            };
        }

        public byte[] ToZip(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location: var filePath = Path.GetTempFileName();
            var tempPath = Path.GetTempPath();
            var filePath = tempPath + "/submission/";
            var archiveFile = tempPath + "/zip/archive.zip";
            var archivePath = tempPath + "/zip/";
            if (Directory.Exists(filePath))
            {
                Directory.Delete(filePath, true);
            }
            if (Directory.Exists(archivePath))
            {
                Directory.Delete(archivePath, true);
            }

            Directory.CreateDirectory(filePath);
            Directory.CreateDirectory(archivePath);

            foreach (var formFile in files)
            {
                var fileName = filePath + formFile.FileName;
                if (formFile.Length > 0)
                {
                    using var stream = new FileStream(fileName, FileMode.Create);
                    formFile.CopyToAsync(stream);
                }
            }
            ZipFile.CreateFromDirectory(filePath, archiveFile);
            /* beapen: 2017/07/24
             * 
             * Currently A Filestream cannot be directly converted to a byte array.
             * Hence it is copied to a memory stream before serializing it.
             * This may change in the future and may require refactoring.
             * 
             */
            var stream2 = new FileStream(archiveFile, FileMode.Open);
            var memoryStream = new MemoryStream();
            stream2.CopyTo(memoryStream);
            using (memoryStream)
            {
                return memoryStream.ToArray();
            }
        }

        public string CheckInvalidChars(string fileNme)
        {
            char[] invalidFileChars = Path.GetInvalidFileNameChars();

            foreach (char someChar in invalidFileChars)
            {
                if (fileNme.Contains(someChar))
                {
                    fileNme = fileNme.Replace(someChar, Convert.ToChar(""));
                }
            }

            fileNme = fileNme.Replace(Convert.ToChar(" "), Convert.ToChar("_"));

            return fileNme;
        }

        public void FileResize(string file, string filePath, int width, int height)
        {

            //var fileName = Path.GetFileName(file.FileName);

            string path = Path.Combine(filePath, file);

            string fileName = $"thumb_{file}";

            string thumbPath = Path.Combine(filePath, fileName);

            // Image.Load(string path) is a shortcut for our default type. 
            // Other pixel formats use Image.Load<TPixel>(string path))
            using Image image = Image.Load(path);
            
            image.Mutate(x => x
                    .Resize(width, height)
            );
            image.Save(thumbPath); // Automatic encoder selected based on extension.

            return;
        }

        public string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
