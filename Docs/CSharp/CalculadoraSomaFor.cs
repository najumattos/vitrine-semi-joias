// Código testado em https://www.jdoodle.com/compile-c-sharp-online
public class Program {
    public static void Main() {
        var calc = new CalculadoraSomaFor();        
        calc.CalcularSomaFor();                           
        Console.WriteLine("Fim");
    }
}

public class CalculadoraSomaFor
{
    public void CalcularSomaFor()
    {
        Console.WriteLine("Somar incrementos do For");
        int soma = 0;
        for (int i = 1; i <= 10; i++){ soma += i; }
        Console.WriteLine($"A soma dos números de 1 a 10 é: {soma}");
    }
}