namespace VitrineSemiJoias.Services.Interfaces;
/// <summary>
/// Define os contratos de infraestrutura para manipulação física de arquivos no servidor local de hospedagem.
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Realiza o upload, gera um nome único baseado em GUID e salva o arquivo de forma assíncrona no disco.
    /// </summary>
    /// <param name="file">O arquivo enviado via formulário HTTP de entrada.</param>
    /// <param name="subDirectory">O subdiretório dentro do diretório raiz onde o arquivo deve ser gravado (padrão: "uploads").</param>
    /// <returns>Uma string contendo o caminho relativo final do arquivo (ex: "img/products/nome.jpg") ou nulo caso o arquivo seja inválido.</returns>
    Task<string> SaveFileAsync(IFormFile file, string subDirectory = "uploads");
  
  /// <summary>
    /// Verifica a existência e remove fisicamente um arquivo armazenado no servidor com base em seu caminho relativo.
    /// </summary>
    /// <param name="filePath">O caminho relativo do arquivo mapeado no sistema.</param>
    /// <returns>Verdadeiro se o arquivo existia e foi excluído com sucesso; caso contrário, falso.</returns>
    Task<bool> DeleteFileAsync(string filePath);
    
    /// <summary>
    /// Converte um caminho relativo gravado no banco de dados em uma URL absoluta acessível pelo navegador do cliente.
    /// </summary>
    /// <param name="filePath">O caminho relativo do arquivo armazenado (ex: "img/products/foto.jpg").</param>
    /// <returns>A string com a URL completa formatada (ex: "https://localhost:7233/img/products/foto.jpg") ou nulo se o caminho for vazio.</returns>
    string GetFileUrl(string fileName);
}
