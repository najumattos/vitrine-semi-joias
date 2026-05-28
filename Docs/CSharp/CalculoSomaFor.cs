using System;

namespace FDEVS;

public class CalculoSomaFor
{
    public void CalcularSomaFor()
    {
        Console.WriteLine("Calcular Soma do For");
        int soma = 0;

        // O laço começa em 1, executa enquanto for menor ou igual a 10, e avança de 1 em 1
        for (int i = 1; i <= 10; i++)
        {
            soma += i; // Adiciona o valor atual de 'i' ao total acumulado
        }
        // Exibe o resultado final no console
        Console.WriteLine($"A soma dos números de 1 a 10 é: {soma}");
    }
}