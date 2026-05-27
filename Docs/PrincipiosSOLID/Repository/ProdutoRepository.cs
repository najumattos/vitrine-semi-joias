using PrincipiosSOLID.Model;

namespace PrincipiosSOLID.Repository;

public class ProdutoRepository : IProdutoRepository
{

    public ProdutoModel BuscarProdutoPorId(int id)
    {
        // Lógica para buscar um produto por ID, repository conversa com o banco            
        return new ProdutoModel();
    }

    public List<ProdutoModel> BuscarProdutosPorCategoria(int categoria)
    {
        // Lógica para buscar um produto por categoria, repository conversa com o banco
        return new List<ProdutoModel>();
    }

    public List<ProdutoModel> BuscarTodosProdutos()
    {
        // Lógica para buscar todos produtos, repository conversa com o banco

        return new List<ProdutoModel>();
    }
}
