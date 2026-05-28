// Código testado em https://www.jdoodle.com/compile-c-sharp-online
using System.Globalization; 

public class Program {
    public static void Main() {
        
     string inicio;        
     var calc = new CalculadoraIMC();        
        do
        {
            calc.CalculoIMC();
            
            Console.WriteLine("Deseja reiniciar? S/N");
            inicio = Console.ReadLine()?.Trim().ToUpper() ?? string.Empty;

        } while(inicio == "S");
        
        Console.WriteLine("Fim");
    }
}
public class CalculadoraIMC
{  
    public void CalculoIMC()
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;         // Configura o console para aceitar pontos decimais corretamente de acordo com a cultura local

        Console.WriteLine("Calculadora de IMC");

        double peso = PedeDadoAoUsuario("Digite seu peso em kg (ex: 70.5): ");
        double altura = PedeDadoAoUsuario("Digite sua altura em metros (ex: 1.75): ");
        double imc = CalcularIMC(peso, altura);
        string classificacaoIMC = ClassificarIMC(imc);


        // 5. Exibição dos resultados
        Console.WriteLine($"Resultado: {imc:F2} - {classificacaoIMC}");

    }

    private static double PedeDadoAoUsuario(string enunciado)
    {       
       
            bool entradaValida = true;
            double numeroDigitado;
            do
            {
                if(!entradaValida){ Console.WriteLine("Número inválido. Tente novamente.");}
                
               Console.Write(enunciado);
            entradaValida = double.TryParse(Console.ReadLine(), out numeroDigitado) && numeroDigitado > 0;

            } while (!entradaValida);
        
        return numeroDigitado;
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