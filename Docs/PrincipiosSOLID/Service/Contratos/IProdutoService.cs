using PrincipiosSOLID.Model;

namespace PrincipiosSOLID.Service.Contratos;

public interface IProdutoService
{
    List<ProdutoModel> BuscarTodosProdutos();
    List<ProdutoModel> BuscarProdutosPorCategoria(string Categoria);
    ProdutoModel BuscarProdutoPorId(int Id);
}
