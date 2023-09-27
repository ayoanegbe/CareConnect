namespace CareConnect.Interfaces
{
    public interface IFileService
    {
        bool SaveFile(List<IFormFile> files, string subDirectory);
        Task<bool> SaveFile(IFormFile file, string subDirectory);
        (string fileType, byte[] archiveData, string archiveName) FetechFiles(string subDirectory);
        string SizeConverter(long bytes);
        byte[] ToZip(List<IFormFile> files);
        string CheckInvalidChars(string fileNme);
        void FileResize(string file, string filePath, int width, int height);
        string GetContentType(string path);
    }
}
