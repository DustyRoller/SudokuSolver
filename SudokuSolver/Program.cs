using System;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("Expected 1 argument - puzzle file name");
                Environment.Exit(1);
            }

            var puzzle = Parser.Parser.ParsePuzzle(args[0]);

            if (puzzle.Solve())
            {
                Console.WriteLine("Successfully solved puzzle");
            }
            else
            {
                Console.WriteLine("Failed to solve puzzle\n");
                Console.WriteLine($"{puzzle.NumberOfUnsolvedCells} cells remain unsolved");
            }

            // Print out the puzzle.
            Console.WriteLine();
            Console.WriteLine(puzzle.ToString());
        }
    }
}
