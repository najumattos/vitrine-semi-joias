using System.Globalization;

namespace FDEVS;

public class Calculadora
{
    public void CalculadoraOperacoes()
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        Console.WriteLine("Calculadora Simples FDEVS");

        bool entradaValida;

        // 1. Entrada do Primeiro Número
        double numero1;
        do
        {
            Console.Write("Digite o primeiro número: ");
            entradaValida = double.TryParse(Console.ReadLine(), out numero1);

            if (!entradaValida)
            {
                Console.WriteLine("Número inválido. Tente novamente.");
            }
        } while (!entradaValida);

        // 2. Entrada do Operador
        string operacao;
        do
        {
            Console.Write("Digite a operação desejada (+, -, *, /): ");
            operacao = Console.ReadLine().Trim();

            entradaValida = operacao == "+" || operacao == "-" || operacao == "*" || operacao == "/";

            if (!entradaValida)
            {
                Console.WriteLine("Operação inválida! Escolha entre +, -, * ou /.");
            }
        } while (!entradaValida);

        // 3. Entrada do Segundo Número (com validação extra para divisão por zero)
        double numero2;
        do
        {
            Console.Write("Digite o segundo número: ");
            entradaValida = double.TryParse(Console.ReadLine(), out numero2);

            if (!entradaValida)
            {
                Console.WriteLine("Número inválido. Tente novamente.");
            }
            // Impede matematicamente a divisão por zero
            else if (operacao == "/" && numero2 == 0)
            {
                Console.WriteLine("Erro: Não é possível dividir por zero! Digite outro valor.");
                entradaValida = false;
            }
        } while (!entradaValida);

        // 4. Processamento do cálculo utilizando Switch Expression
        double resultado = operacao switch
        {
            "+" => Somar(numero1, numero2),
            "-" => Subtrair(numero1, numero2),
            "*" => Multiplicar(numero1, numero2),
            "/" => Dividir(numero1, numero2),
            _ => 0 // Cenário de salvaguarda do compilador
        };

        // 5. Exibição do Resultado
        Console.WriteLine("\n=== Resultado ===");
        Console.WriteLine($"{numero1} {operacao} {numero2} = {resultado.ToString("F2")}");
    }

    // Métodos especialistas simplificados (Expression-bodied members)
    private static double Somar(double n1, double n2) => n1 + n2;
    private static double Subtrair(double n1, double n2) => n1 - n2;
    private static double Multiplicar(double n1, double n2) => n1 * n2;
    private static double Dividir(double n1, double n2) => n1 / n2;
}
