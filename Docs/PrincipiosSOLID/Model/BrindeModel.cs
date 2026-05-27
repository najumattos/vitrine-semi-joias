namespace PrincipiosSOLID.Model;

public class BrindeModel : ProdutoModel      
{
    public int IdBrinde { get; set; }
    public string TipoBrinde { get; set; }
    public DateTime DataValidade { get; set; }
    //outras props
}
