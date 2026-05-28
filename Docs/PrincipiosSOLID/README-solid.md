# Princípios SOLID
Este repositorio contem a aplicação prática dos Princípios SOLID.

> Leia no blog: https://blog-najuliarmattos.vercel.app/#/post/11

Os princípios **SOLID** são cinco diretrizes da programação orientada a objetos que visam tornar o software mais compreensível, flexível e fácil de manter. Eles são fundamentais para quem trabalha com arquiteturas robustas e código limpo.

## S - Single Responsibility Principle (SRP)
> **Princípio da Responsabilidade Única:** Uma classe deve ter um único motivo para mudar.

### Violação
Aqui, o método faz a busca, conhece a regra de desconto e executa o cálculo. Se a regra de promoção mudar (ex: "agora é 15% para prata"), você precisa mexer no método de busca.
```csharp
public class ProdutoService(){
	public List<Produto> BuscarTodosProdutos()
	{
	    var produtos = context.produto.ToList(); //VIOLAÇÃO: *Busca* no banco
	    foreach (var produto in produtos)
	    {
	        if (produto.TemPromocao == true) 
	        {
	            produto.PrecoFinal = produto.PrecoBase * 0.90m; //VIOLAÇÃO: *Calcula* Desconto
	        }
	    }
	    return new List<Produto>();
	}
}
```

### Correção
Agora,  o método `ListarVitrineAtualizada()` agora se comporta como um **orquestrador**. Ele não sabe _como_ o banco de dados busca os itens e nem sabe _qual_ é a porcentagem do desconto. Ele apenas gerencia o fluxo: "pegue aqui" e "processe ali".
```csharp
public class ProdutoService(ICalcularDescontoService calcularDesconto){
	public List<Produto> ListarVitrineAtualizada()
	{
	    var produtos = repositorio.BuscarTodosProdutos(); //CORREÇÃO: *Chama* o método de busca
	    foreach (var produto in produtos)
	    {
	        calcularDesconto.ProcessarPreco(produto); // CORREÇÃO: *Chama* o método de calculo
	    }
	    
	    return produtos;
	}
}
```


## O - Open/Closed Principle (OCP)
>**Princípio Aberto/Fechado:** Objetos ou entidades devem estar abertos para extensão, mas fechados para modificação.

Vamos Adentrar a classe CalcularDesconto(), note como existe a necessidade de adicionar else if para cada nova promoção:
### Violação
```csharp
public class CalcularDesconto(){
	public void ProcessarPreco(Produto p)
	{
	    if (tipoPromocao == "Natal") 
	        p.PrecoFinal = p.PrecoBase * 0.80m;
	    else if (tipoPromocao == "BlackFriday")
	        p.PrecoFinal = p.PrecoBase * 0.50m;
	    // Toda vez que mudar a regra, essa classe sofre alteração.
	}
}
```
### Correção
Para resolver isso, usamos **Interfaces** ou **Classes Abstratas**. Criamos um "contrato" para as promoções.
```csharp
public interface IPromocao
{
    void AplicarDesconto(Produto p); //A Interface tem um metodo que recebe um produto
}
```

Implementamos essa interface
```csharp
public class PromocaoNatal : IPromocao 
{
//A classe implementa a Interface e aplica suas próprias regras de desconto
    public void AplicarDesconto(ProdutoModel p) => p.PrecoFinal = p.PrecoBase * 0.80m;  
}
//Se eu quiser adicionar uma nova promoção é só criar outra classe que implemente IPromoção. Assim não polui o código com "else if"
public class PromocaoPrata : IPromocao
{
    public void AplicarDesconto(ProdutoModel p) => p.PrecoFinal = p.PrecoBase * 0.90m;
}
```

E como fica ProcessarPreco()?
```csharp
public class CalcularDesconto(List<IPromocao> promocoes)
{
     public void ProcessarPreco(Produto p)
    {
    // O orquestrador apenas percorre as regras existentes
        foreach (var promocao in promocoes) 
        {
            promocao.AplicarDesconto(p);
        }
    }
}
```
Note como a `CalcularDesconto()` se tornou "imune" ao tempo: não importa se a loja criar 100 tipos de promoções novas (Dia das Mães, Queima de Estoque, etc.), você **nunca mais** precisará abrir o arquivo da `CalcularDesconto()` para alterar sua lógica central.

## L - Liskov Substitution Principle (LSP)
>**Princípio da Substituição de Liskov:** Uma classe derivada deve ser substituível por sua classe base.

o LSP é sobre **comportamento esperado**.  

Para exemplificar vamos criar uma classe abstrata que vai servir como base para exemplificar a violação e a correção do Principio. Uma classe `abstract` é uma "base incompleta". Ela serve como um molde para outras classes.
```csharp
public abstract class Promocao
{
    public decimal Porcentagem { get; set; }

    protected decimal CalcularValorComDesconto(decimal valorOriginal)
    {
        // O MÉTODO não é abstrato porque ele JÁ TEM uma lógica pronta.
        decimal desconto = valorOriginal * (Porcentagem / 100);
        return valorOriginal - desconto;
    }
    public abstract void Processar();   //método abstrato
}

```
### Violação
```csharp
public class PromocaoFuncionarioService : Promocao
{
    public override void Processar()
    {
        decimal valorProduto = 50.00m;

        // VIOLAÇÃO 1: Lançar uma exceção inesperada para um comportamento que a base deveria suportar.
        if (valorProduto < 100)
        {
            throw new Exception("Esta promoção não se aplica a valores baixos!");
        }

        // VIOLAÇÃO 2: Alterar drasticamente o estado ou ignorar o contrato.
        // Se quem chama a classe base espera que o desconto seja aplicado, mas aqui você decide que o desconto é sempre zero ignorando a 'Porcentagem'.
        decimal valorFinal = valorProduto; 
    }
}
```

Sempre que você sentir vontade de escrever um `throw new NotImplementedException()` dentro de um método que veio de uma classe pai ou interface, pare tudo. Você provavelmente está violando o LSP.

### Correção
O ponto crucial aqui é a **confiança**: O polimorfismo só funciona de verdade se pudermos confiar que qualquer "filho" se comporta como o "pai" prometeu.
```csharp
public class DiaDasMaesService : Promocao
{
    public override void Processar()
    {   
		    decimal valorProduto = 666.00m;
        CalcularValorComDesconto(valorProduto);
    }
}

public class PromocaoNatalService : Promocao
{
    public override void Processar()
    {
        decimal valorProduto = 100.00m;
        CalcularValorComDesconto(valorProduto);
    }
}

public class BlackFridayService : Promocao
{
    public override void Processar()
    {
        decimal valorProduto = 999.00m;
        CalcularValorComDesconto(valorProduto);
    }
}
```


### O que isso significa?
Esse principio é sobre não quebrar o fluxo de execução do programa. Se você tiver um código que percorre uma lista de promoções para processá-las, ele "quebraria" quando chegasse no PromocaoFuncionarioService().
```csharp
List<Promocao> listaPromocao = new List<Promocao>{ new DiaDasMaesService(), new PromocaoFuncionarioService() }; 

foreach (var promo in listaPromocao) {
promo.Processar(); 
}
```
> Esse é o principio mais chatinho de conseguir visualizar na prática, então assista esse vídeo para compreender melhor: [LSP]

## I - Interface Segregation Principle (ISP)
>**Princípio da Segregação de Interface:** Uma classe não deve ser forçada a implementar interfaces e métodos que não utiliza.

### Violação
```csharp
public interface IProdutoService
{
    void AplicarDesconto(Produto p); 
    void BuscarTodosProdutos();
    //outros metodos
}

// O Brinde é forçado a implementar AplicarDesconto mesmo que não utilize
public class BrindeService : IProdutoService
{
    void AplicarDesconto(Produto p);  //somente produto utiliza
    void BuscarTodosProdutos(); //ambos utilizam
    //outros metodos
}
```
interfaces "gordas" (com métodos demais) criam acoplamentos desnecessários. Quando você força uma classe como `BrindeService` a implementar algo que ela não usa, você está criando um código frágil: qualquer mudança na assinatura de `AplicarDesconto` forçaria você a mexer no código de Brinde, mesmo que a lógica de lá não tenha nada a ver com descontos.

### Correção
```csharp
public interface IProdutoService
{
    void BuscarTodosProdutos();
    void BuscarProdutosPorCategoria(Categoria categoria)
    void BuscarProdutoPorId(int id)
    //outros metodos compartilhados por tudo que for produto
}

public class BrindeService : IProdutoService
{
    void BuscarTodosProdutos();
    void BuscarProdutosPorCategoria(Categoria categoria)
    void BuscarProdutoPorId(int id)
    //outros metodos compartilhados por tudo que for produto
}

public class ProdutoService : IProdutoService, ICalcularDesconto
{
  // Metodos compartilhados por IProdutoService
    void BuscarTodosProdutos();
    void BuscarProdutosPorCategoria(Categoria categoria)
    void BuscarProdutoPorId(int id)  

//Métodos compartilhados por IPromocoes

}
```

## D - Dependency Inversion Principle (DIP)
>**Princípio da Inversão de Dependência:** Dependa de abstrações e não de implementações.

O Princípio defende que:
1.   Módulos de alto nível não devem depender de módulos de baixo nível. Ambos devem depender de **abstrações**.
 2.  Abstrações não devem depender de detalhes. **Detalhes devem depender de abstrações.**

### Violação

Na violação o banco de dados é chamado diretamente no controller, causando o acoplamento, mistura regra de negocio e da ao controller responsabilidades que não é dele, violando tambem o SRP. Olhando assim parece simples mas em um sistema real o controller ficaria rapidamente sobrecarregado pelas funções que pertencem ao SERVICE
```csharp
 public async Task<IActionResult> Create(ProductViewModel product)
    {
       await context.AddAsync(product);
       await context.SaveChangesAsync();
       
       return View(product);
    }

```

### Correção
A estrutura a seguir é o que chamamos de **Arquitetura em Camadas** (ou os primeiros passos para uma _Clean Architecture_):

O Controller chama a INTERFACE do Service
```csharp
    namespace Exemplo.Controllers;
    public class ProdutosController(IProdutoService service) : Controller
	{
	    public async Task<IActionResult> Create(ProdutoModel produto)
	    {
		     var result = await service.AddProductAsync(produto);       
		     return View(produto);
	    }
     }
```

O Service implementa a INTERFACE `IProdutoService`, aplica as regras de negócio e chama a INTERFACE do repository
 ```csharp    
    namespace Exemplo.Services;
    public class ProdutoService(IProdutoRepository repository) : IProdutoService
	{
	    public async Task<ProdutoModel> AddProductAsync(ProdutoModel produto)
	    {
	        if (produto == null)
	        {
	            return "Produto inválido ou não informado.";
	        }
	        await repository.AddProductAsync(produto);      
	        return  ProductViewModel();
	     }
    }
```

O repository implementa a INTERFACE `IProdutoRepository` e sua única função é bater no banco e enviar a resposta ao service. O "AppDbContext" é como o porteiro do banco de dados.
```csharp
public class ProdutoRepository(AppDbContext context) : IProdutoRepository
{
	public async TaskProdutoModel AddProductAsync(ProdutoModel produto)
    {      
        await context.Produto.AddAsync(produto);
        await context.SaveChangesAsync();
        return produto;              
     }
 }
```

Visualizar e entender esse fluxo é bem  importante, o próximo passo seria implementar o uso de DTOs e fazer os tratamentos de erros e execessoes, que também devem ser tratados no momento correto para não tornar o código confuso e bagunçado.

O Service não deve mandar no Repository, ele apenas confia no contrato que a interface estabeleceu. RESUMINDO:
-   **Módulo de Alto Nível (Controller/Service):** Define as regras de negócio. Ele diz: "Eu preciso de alguém que saiba salvar um produto, não me importa se é no SQL ou no Excel".
    
-   **Abstração (Interface):** É o contrato. Ela define o método `AddProductAsync`.
    
-   **Módulo de Baixo Nível (Repository):** É o detalhe técnico. Ele implementa o contrato e lida com o `AppDbContext`.

## Resumindo
-   **RP:** "Cada um no seu quadrado."
    
-   **OCP:** "Adicione peças, não mude o tabuleiro."
    
-   **LSP:** "Não me dê sustos ao trocar um objeto por outro."
    
-   **ISP:** "Não me obrigue a assinar o que não vou usar."
    
-   **DIP:** "Peça o que você precisa, não fabrique você mesmo."

## Conclusão
Todos os principios conversam entre si, é menos complicado do que parece e o principio mais dificil de entender é o LSP.

O SOLID é uma ferramenta para lidar com a **mudança**. Se você aplica SOLID em um código que não vai mudar, você está apenas adicionando complexidade (mais arquivos, mais saltos de memória, mais dificuldade de depuração). SOLID é indispensavel mas cuidado para não criar um **Design prematuro** ao tentar aplicar todos os princípios de forma rigorosa logo no primeiro dia, criando dezenas de interfaces e classes pequenas antes mesmo de entender o domínio do problema. o SOLID serve para facilitar a evolução, mas o excesso dele antes da hora vira um obstáculo.

[LSP]:https://youtu.be/kt1AqWcxoA0?si=iuEX1AAVmbUWR_sW
