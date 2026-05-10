namespace VitrineSemiJoias.Services.Interfaces;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file, string subDirectory = "uploads");
    bool DeleteFileAsync(string filePath);
    string GetFileUrl(string fileName);
}
