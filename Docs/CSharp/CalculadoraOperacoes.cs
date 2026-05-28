// Código testado em https://www.jdoodle.com/compile-c-sharp-online
using System.Globalization;
public class Program {
    public static void Main() {
        
        string inicio;
        var calc = new CalculadoraOperacoes();
        do
        {
            calc.CalcularOperacao();
            Console.WriteLine("Deseja reiniciar? S/N");
            inicio = Console.ReadLine()?.Trim().ToUpper() ?? string.Empty;
        } while(inicio == "S");
        
        Console.WriteLine("Fim");
    }
}

public class CalculadoraOperacoes
{
    public void CalcularOperacao()
    {
        Console.WriteLine("Calculadora Simples FDEVS");

        string operacao = PedirOperacaoAoUsuario(); 
        double[] numeros = PedirNumerosAoUsuario(2, operacao);    //2 é o tamanho da array/ qtd de numeros    
        double resultado = Calcular(numeros, operacao);

        // 5. Exibição do Resultado
        Console.WriteLine("\n=== Resultado ===");
        string equacao = string.Join($" {operacao} ", numeros.Select(n => n.ToString(CultureInfo.InvariantCulture)));
        Console.WriteLine($"{equacao} = {resultado.ToString("F2", CultureInfo.InvariantCulture)}");
    }
    
    private static string PedirOperacaoAoUsuario()
    {
        string operacao;
        bool entradaValida = true;
        do
        {
            if (!entradaValida){ Console.WriteLine("Operação inválida! Escolha entre +, -, * ou /.");}
            
            Console.Write("Digite a operação desejada (+, -, *, /): ");
            operacao = Console.ReadLine()?.Trim() ?? string.Empty;

            entradaValida = operacao == "+" || operacao == "-" || operacao == "*" || operacao == "/";           
        
            
        }while (!entradaValida);
        
        return operacao;
    }
    
    private static double[] PedirNumerosAoUsuario(int qtd, string operacao)
    {
        double[] numeros = new double[qtd];
        for (int i = 0; i < qtd; i++)
        {
            bool entradaValida = true;
            double numeroDigitado;
            do
            {
                if(!entradaValida){ Console.WriteLine("Número inválido. Tente novamente.");}
                
                Console.Write($"Digite o {i + 1}° número: ");                
                entradaValida = double.TryParse(
                    Console.ReadLine(), 
                    CultureInfo.InvariantCulture, 
                    out numeroDigitado
                );

               if (entradaValida && operacao == "/" && i == 1 && numeroDigitado == 0)
                {
                    Console.WriteLine("Erro: Em uma divisão, o 2° número não pode ser zero!");
                    entradaValida = false; 
                }

            }while (!entradaValida);
            
            numeros[i] = numeroDigitado;
        }
        return numeros;
    }
    
    private static double Calcular(double[] numeros, string operacao)
    {
        if (numeros == null || numeros.Length == 0) return 0;
        return operacao switch
        {
            "+" => numeros.Sum(),
            "-" => numeros.Aggregate((acumulado, proximo) => acumulado - proximo),
            "*" => numeros.Aggregate((acumulado, proximo) => acumulado * proximo),
            "/" => numeros.Aggregate((acumulado, proximo) => acumulado / proximo),            
            _ => 0
        };
    }
  
}