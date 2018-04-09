using System;
using System.Linq;
using static System.Console;

namespace _8Puzzle_Console
{
    class Program
    {
        private static int[] _field = { 1, 2, 3, 4, 5, 6, 7, 8, 0 };
        
        static void Main(string[] args)
        {
            WriteLine("Пятнашки 3x3");
            PrintResult(_field);
            WriteLine(" 1.Перемешать и играть.\n" +
                      " 2.Перемешать и решить.\n");
            int option;
            if (int.TryParse(ReadLine(), out option))
            {
                switch (option)
                {
                    case 1: Game(); break;
                    case 2: AutoGame(); break;
                    default: WriteLine("Попробуйте в следующий раз."); break;
                }
            }
            else
                WriteLine("Попробуйте в следующий раз.");
            ReadKey();
        }

        private static void Game()
        {
            _field = Puzzle.shuffle(_field, 20);
            PrintResult(_field);
            WriteLine("Выберите элемент, который хотите передвинуть:");
            var status = "";
            while (status != "Успех!")
            {
                int elem;
                var input = "";
                while (!int.TryParse(input, out elem))
                    input = ReadLine();
                if (elem != 0)
                {
                    var tileId = Array.IndexOf(_field, elem);
                    var res = Puzzle.move(_field, tileId);
                    UpdateField(res, ref status);
                }
                else status = "Попробуйте передвинуть другой элемент.";
                WriteLine(status);
            }
        }

        private static void AutoGame()
        {
            _field = Puzzle.shuffle(_field, 20);
            WriteLine("Начальное состояние:");
            PrintResult(_field);
            var res = Puzzle.solve(_field, Puzzle.goalState);
            WriteLine("Решение:");
            foreach (var nextState in res)
            {
                WriteLine("======");
                WriteLine("  " + (Array.IndexOf(res,nextState)+1));
                WriteLine("======");
                PrintResult(nextState);
            }
        }


        private static void PrintResult(int[] res)
        {
            for (var i = 0; i < res.Length; i++)
            {
                Write(res[i] + " ");
                if ((i + 1) % 3 == 0) WriteLine();
            }
        }

        private static void UpdateField(int[]res, ref string stat)
        {
            if (!res.SequenceEqual(_field))
            {
                PrintResult(res);
                _field = res;
                stat = res.SequenceEqual(Puzzle.goalState) ? "Успех!" : "Выберите элемент, который хотите передвинуть:";
            }
            else stat = "Попробуйте передвинуть другой элемент.";
        }
    }
}
