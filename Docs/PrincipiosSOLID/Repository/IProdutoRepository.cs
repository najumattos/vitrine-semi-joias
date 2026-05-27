using PrincipiosSOLID.Model;

namespace PrincipiosSOLID.Repository;

// Note que para transitar de uma camada para outra utiliza-se Interfaces(DIP)
public interface IProdutoRepository
{
    List<ProdutoModel> BuscarTodosProdutos();
    List<ProdutoModel> BuscarProdutosPorCategoria(int Categoria);
    ProdutoModel BuscarProdutoPorId(int Id);  
}
