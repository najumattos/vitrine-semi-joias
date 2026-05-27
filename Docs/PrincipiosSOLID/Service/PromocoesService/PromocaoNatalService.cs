using PrincipiosSOLID.Service.Contratos;

namespace PrincipiosSOLID.Service.PromocoesService;
public class PromocaoNatalService : Promocao
{
    public override void Processar()
    {
        decimal valorProduto = 100.00m;
        CalcularValorComDesconto(valorProduto);
    }
}