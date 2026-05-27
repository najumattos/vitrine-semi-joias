using PrincipiosSOLID.Model;
using PrincipiosSOLID.Service.Contratos;
namespace PrincipiosSOLID.Service.PromocoesService;

public class BlackFridayService : Promocao
{
    public override void Processar()
    {
        decimal valorProduto = 999.00m;
        CalcularValorComDesconto(valorProduto);
    }
}
