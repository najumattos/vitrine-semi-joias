using PrincipiosSOLID.Model;
using PrincipiosSOLID.Service.Contratos;

namespace PrincipiosSOLID.Service;

public class ProdutoService() : IProdutoService
{  
 
    public ProdutoModel BuscarProdutoPorId(int id)
    {
        // IProdutoService: Aplica regras de negócio, service conversa com repository
        return new ProdutoModel();
    }

    public List<ProdutoModel> BuscarProdutosPorCategoria(string categoria)
    {
        // IProdutoService: Aplica regras de negócio, service conversa com repository
        return new List<ProdutoModel>();
    }

    public List<ProdutoModel> BuscarTodosProdutos()
    {
        // IProdutoService: Aplica regras de negócio, service conversa com repository
        return new List<ProdutoModel>();
    }
}
