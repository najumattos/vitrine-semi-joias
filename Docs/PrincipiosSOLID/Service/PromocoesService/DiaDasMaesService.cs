using PrincipiosSOLID.Service.Contratos;

namespace PrincipiosSOLID.Service.PromocoesService;

// Aberta pra extensao(add novas promocoes), fechada pra modificação (OCP)
// os métodos só mudarão se mudar a lógica de desconto (SRP)
public class DiaDasMaesService : Promocao
{
    public override void Processar()
    {   //É aqui que você escreveria a lógica específica de Dia das Maes (ex: verificar se hoje é dezembro).
        // A classe filha não precisa saber COMO calcular o desconto, 
        // ela apenas chama o método que já está pronto na base.
        decimal valorProduto = 666.00m;
        CalcularValorComDesconto(valorProduto);
    }
}
