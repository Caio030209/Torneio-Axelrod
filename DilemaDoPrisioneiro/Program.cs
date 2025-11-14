using System;
using System.Collections.Generic;

class Program
{
    enum Move { Cooperar, Trair }

    static void Main()
    {
        Console.WriteLine("Torneio de Axelrod");
        Console.WriteLine("Escolhas: C = Cooperar, T = Trair. Digite 'sair' para encerrar.\n");

// Pontuações
        var pontos = new Dictionary<(Move voce, Move other), (int PontosJogador, int PontosCPU)>()
        {
            {(Move.Cooperar, Move.Cooperar), (3,3)},
            {(Move.Cooperar, Move.Trair),    (0,5)},
            {(Move.Trair,    Move.Cooperar), (5,0)},
            {(Move.Trair,    Move.Trair),    (1,1)}
        };

        int totalVoce = 0, totalCPU = 0;
        List<(Move voce, Move CPU)> historico = new();

        bool primeiroTurno = true;
        while (true)
        {
            Console.Write("Sua jogada (C/T ou 'sair'): ");
            string? input = Console.ReadLine()?.Trim().ToLowerInvariant();
            if (input == null) continue;
            if (input == "sair" || input == "exit" ) break;

            Move MovimentoJogador;
            if (input == "c")
                MovimentoJogador = Move.Cooperar;
            else if (input == "t")
                MovimentoJogador = Move.Trair;
            else
            {
                Console.WriteLine("Entrada inválida — digite 'C' para cooperar, 'T' para trair, ou 'sair'.");
                continue;
            }

            // CPU: Tit-for-Tat, mas inicia traindo.
            Move MovimentoCPU;
            if (primeiroTurno)
            {
                MovimentoCPU = Move.Trair; // começa traindo
                primeiroTurno = false;
            }
            else
            {
                // copia a jogada anterior do jogador
                var last = historico[^1].voce;
                MovimentoCPU = last;
            }

            historico.Add((MovimentoJogador, MovimentoCPU));

            var scores = pontos[(MovimentoJogador, MovimentoCPU)];
            totalVoce += scores.PontosJogador;
            totalCPU += scores.PontosCPU;

            Console.WriteLine($"\nVocê: {FormatMove(MovimentoJogador)}  |  CPU: {FormatMove(MovimentoCPU)}");
            Console.WriteLine($"Pontuação desta rodada -> Você: {scores.PontosJogador} | CPU: {scores.PontosCPU}");
            Console.WriteLine($"Pontuação total -> Você: {totalVoce} | CPU: {totalCPU}\n");

            // Mostrar histórico resumido
            Console.WriteLine("Histórico (Você / CPU):");
            for (int i = 0; i < historico.Count; i++)
            {
                Console.WriteLine($"{i+1}: {FormatMove(historico[i].voce)} / {FormatMove(historico[i].CPU)}");
            }
            Console.WriteLine(new string('-', 40));
        }

        Console.WriteLine("\nJogo finalizado.");
        Console.WriteLine($"Pontuação final -> Você: {totalVoce} | CPU: {totalCPU}");
        Console.WriteLine("Obrigado por jogar!");
    }

    static string FormatMove(Move m)
    {
        return m == Move.Cooperar ? "C (Cooperar)" : "T (Trair)";
    }
}
