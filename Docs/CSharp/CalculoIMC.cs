using System;
using System.Globalization; // OBRIGATÓRIO para usar o CultureInfo

namespace FDEVS;

public class CalculoIMC
{
    public void CalculadoraIMC()
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;         // Configura o console para aceitar pontos decimais corretamente de acordo com a cultura local

        Console.WriteLine("Calculadora de IMC");



        bool entradaValida;        // Variável de controle compartilhada para as validações
        // 1. Entrada de dados do Peso
        double peso;
        do
        {
            Console.Write("Digite seu peso em kg (ex: 70.5): ");
            entradaValida = double.TryParse(Console.ReadLine(), out peso) && peso > 0;

            if (!entradaValida)
            {
                Console.WriteLine("Peso inválido. Tente novamente.");
            }

        } while (!entradaValida);


        // 2. Entrada de dados da Altura
        double altura;
        do
        {
            Console.Write("Digite sua altura em metros (ex: 1.75): ");
            entradaValida = double.TryParse(Console.ReadLine(), out altura) && altura > 0;

            if (!entradaValida)
            {
                Console.WriteLine("Altura inválida. Tente novamente.");
            }

        } while (!entradaValida);

        // 3. Calcula IMC
        double imc = CalcularIMC(peso, altura);

        // 4. Classifica peso de acordo com IMC
        string classificacaoIMC = ClassificarIMC(imc);


        // 5. Exibição dos resultados
        Console.WriteLine($"Resultado: {imc:F2} - {classificacaoIMC}");

    }

    //Switch Expression combinado com Arrow Function
    private static string ClassificarIMC(double imc) => imc switch 
    {
        < 18.5 => "Abaixo do peso",
        >= 18.5 and < 25.0 => "Peso normal (saudável)",
        >= 25.0 and < 30.0 => "Sobrepeso",
        >= 30.0 and < 35.0 => "Obesidade Grau I",
        >= 35.0 and < 40.0 => "Obesidade Grau II (severa)",
        _ => "Obesidade Grau III (mórbida)" // O '_' substitui o 'default'
    };

    private static double CalcularIMC(double peso, double altura) => peso / (altura * altura); //Expression-bodied Members

}