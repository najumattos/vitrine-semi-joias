using VitrineSemiJoias.Services.Interfaces;

namespace VitrineSemiJoias.Services;

public class FileService(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor) : IFileService
{
    public async Task<string> SaveFileAsync(IFormFile file, string subDirectory)
    {
        if (file == null || file.Length == 0)
            return null;

        // Criar diretório se não existir
        var uploadsPath = Path.Combine(environment.WebRootPath, subDirectory);
        if (!Directory.Exists(uploadsPath))
            Directory.CreateDirectory(uploadsPath);

        // Gerar nome único para o arquivo
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(subDirectory, fileName); // Salva o path relativo
        var fullPath = Path.Combine(environment.WebRootPath, filePath);

        // Salvar arquivo
        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return filePath; // Retorna o path relativo: "img/usuarios/arquivo.png"
    }

    public bool DeleteFileAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            return false;

        var fullPath = Path.Combine(environment.WebRootPath, filePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            return true;
        }
        return false;
    }

    public string GetFileUrl(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            return null;

        var request = httpContextAccessor.HttpContext.Request;
        return $"{request.Scheme}://{request.Host}/{filePath.Replace("\\", "/")}";
    }
}
