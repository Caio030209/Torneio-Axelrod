using System;
using System.Collections.Generic;

class Program
{
    enum Move { Cooperate, Defect }

    static void Main()
    {
        Console.WriteLine("Torneio de Axelrod");
        Console.WriteLine("Escolhas: C = Cooperar, T = Trair (Defect). Digite 'sair' para encerrar.\n");

// Pontuações
        var payoff = new Dictionary<(Move you, Move other), (int youScore, int otherScore)>()
        {
            {(Move.Cooperate, Move.Cooperate), (3,3)},
            {(Move.Cooperate, Move.Defect),    (0,5)},
            {(Move.Defect,    Move.Cooperate), (5,0)},
            {(Move.Defect,    Move.Defect),    (1,1)}
        };

        int totalYou = 0, totalMachine = 0;
        List<(Move you, Move machine)> history = new();

        bool primeiroTurno = true;
        while (true)
        {
            Console.Write("Sua jogada (C/T ou 'sair'): ");
            string input = Console.ReadLine()?.Trim().ToLowerInvariant();
            if (input == null) continue;
            if (input == "sair" || input == "exit" ) break;

            Move playerMove;
            if (input == "c" || input == "cooperar" || input == "cooperate")
                playerMove = Move.Cooperate;
            else if (input == "t" || input == "trair" || input == "defect")
                playerMove = Move.Defect;
            else
            {
                Console.WriteLine("Entrada inválida — digite 'C' para cooperar, 'T' para trair, ou 'sair'.");
                continue;
            }

            // Máquina: Tit-for-Tat, mas inicia traindo.
            Move machineMove;
            if (primeiroTurno)
            {
                machineMove = Move.Defect; // começa traindo
                primeiroTurno = false;
            }
            else
            {
                // copia a jogada anterior do jogador
                var last = history[^1].you;
                machineMove = last;
            }

            history.Add((playerMove, machineMove));

            var scores = payoff[(playerMove, machineMove)];
            totalYou += scores.youScore;
            totalMachine += scores.otherScore;

            Console.WriteLine($"\nVocê: {FormatMove(playerMove)}  |  Máquina: {FormatMove(machineMove)}");
            Console.WriteLine($"Pontuação desta rodada -> Você: {scores.youScore} | Máquina: {scores.otherScore}");
            Console.WriteLine($"Pontuação total -> Você: {totalYou} | Máquina: {totalMachine}\n");

            // Mostrar histórico resumido
            Console.WriteLine("Histórico (Você / Máquina):");
            for (int i = 0; i < history.Count; i++)
            {
                Console.WriteLine($"{i+1}: {FormatMove(history[i].you)} / {FormatMove(history[i].machine)}");
            }
            Console.WriteLine(new string('-', 40));
        }

        Console.WriteLine("\nJogo finalizado.");
        Console.WriteLine($"Pontuação final -> Você: {totalYou} | Máquina: {totalMachine}");
        Console.WriteLine("Obrigado por jogar!");
    }

    static string FormatMove(Move m)
    {
        return m == Move.Cooperate ? "C (Cooperar)" : "T (Trair)";
    }
}
