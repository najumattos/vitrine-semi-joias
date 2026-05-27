using PrincipiosSOLID.Model;
using PrincipiosSOLID.Service.Contratos;

namespace PrincipiosSOLID.Service;

public class BrindeService : IProdutoService    //BrindeService implementa apenas a interface IProdutoService, não precisa implementar os métodos de Promocao
{
  
    public ProdutoModel BuscarProdutoPorId(int id)
    {
        //  Aplica Regras de negócio e chama repository          
        return new ProdutoModel();
    }

    public List<ProdutoModel> BuscarProdutosPorCategoria(string categoria)
    {
        //  Aplica Regras de negócio e chama repository  
        return new List<ProdutoModel>();
    }

    public List<ProdutoModel> BuscarTodosProdutos()
    {
        //  Aplica Regras de negócio e chama repository  
        return new List<ProdutoModel>();
    }
}
