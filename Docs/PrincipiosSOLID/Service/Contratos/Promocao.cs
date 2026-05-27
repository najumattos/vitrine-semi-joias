namespace PrincipiosSOLID.Service.Contratos;

// Uma classe abstract é uma "base incompleta". Ela serve como um molde para outras class
public abstract class Promocao
{
    public decimal Porcentagem { get; set; }

    protected decimal CalcularValorComDesconto(decimal valorOriginal)
    {
        // O método não é abstrato porque ele JÁ TEM uma lógica pronta.
        // Usamos 'protected' para que apenas as classes filhas possam usar.
        decimal desconto = valorOriginal * (Porcentagem / 100);
        return valorOriginal - desconto;
    }
    public abstract void Processar();
    //Métodos Abstratos: Se você marcar um método como abstract, ele não tem corpo.A classe filha deve obrigatoriamente usar override para implementá-lo.
}
