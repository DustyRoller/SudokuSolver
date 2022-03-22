using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SudokuSolverUnitTests")]

namespace SudokuSolver.Parser
{
    /// <summary>
    /// Class to parse Sudoku puzzles from text files.
    /// </summary>
    /// <remarks>
    /// Puzzle files will be text format using the following format:
    ///   |-|-|-|-|-|-|-|-|-|
    ///   |-|-|-|-|-|-|-|-|-|
    ///   |-|-|-|-|-|-|-|-|-|
    ///   |-|-|-|-|-|-|-|-|-|
    ///   |-|-|-|-|-|-|-|-|-|
    ///   |-|-|-|-|-|-|-|-|-|
    ///   |-|-|-|-|-|-|-|-|-|
    ///   |-|-|-|-|-|-|-|-|-|
    ///   |-|-|-|-|-|-|-|-|-|.
    /// </remarks>
    internal static class Parser
    {
        /// <summary>
        /// Parse the given file to generate a Puzzle, ready to be solved.
        /// </summary>
        /// <param name="puzzleFilePath">The path to the file containing the puzzle.</param>
        /// <returns>A Puzzle object.</returns>
        public static Puzzle ParsePuzzle(string puzzleFilePath)
        {
            ValidateInputFile(puzzleFilePath);

            var puzzle = new Puzzle();

            // Now read in the puzzle.
            var lines = File.ReadAllLines(puzzleFilePath);

            ValidatePuzzleSize(lines);

            for (var row = 0u; row < lines.Length; ++row)
            {
                // Cells within the line will be delimited by '|'.
                var cellsStr = lines[row].Split('|', StringSplitOptions.RemoveEmptyEntries);

                for (var column = 0u; column < cellsStr.Length; ++column)
                {
                    var cell = ParseCell(cellsStr[column]);
                    cell.Coordinate = new Coordinate(column, row);

                    puzzle.AddCell(cell);
                }
            }

            ParseSections(puzzle);

            return puzzle;
        }

        /// <summary>
        /// Parse the given cell string to generate a Cell object.
        /// </summary>
        /// <param name="cellStr">The cell string to be parsed.</param>
        /// <returns>A Cell object.</returns>
        private static Cell ParseCell(string cellStr)
        {
            var cell = new Cell();

            // Square will be either empty or already have a value in it.
            if (cellStr != "-")
            {
                if (!uint.TryParse(cellStr, out uint cellValue))
                {
                    throw new ParserException($"Failed to parse cell value: {cellStr}");
                }

                cell.CellValue = cellValue;
            }

            return cell;
        }

        private static void ParseSections(Puzzle puzzle)
        {
            // Get all the column and row sections.
            for (int i = 0; i < 9; ++i)
            {
                puzzle.Columns.Add(new Section
                {
                    Cells = puzzle.Cells.Where(c => c.Coordinate.Y == i).ToList(),
                });

                puzzle.Rows.Add(new Section
                {
                    Cells = puzzle.Cells.Where(c => c.Coordinate.X == i).ToList(),
                });
            }

            // Get all of the 3x3 squares from within the puzzle,
            // this is a pretty ugly way of doing it but works for now.
            var squareStartingIndexes = new List<int>()
            {
                0, 3, 6, 27, 30, 33, 54, 57, 60,
            };

            foreach (var startingIndex in squareStartingIndexes)
            {
                var index = startingIndex;
                var cells = new List<Cell>();
                for (int x = 0; x < 3; ++x)
                {
                    for (int y = 0; y < 3; ++y)
                    {
                        cells.Add(puzzle.Cells[index]);
                        ++index;
                    }
                    index += 6;
                }

                puzzle.Squares.Add(new Section
                {
                    Cells = cells,
                });
            }
        }

        /// <summary>
        /// Validate that the input file contains a valid puzzle.
        /// </summary>
        /// <param name="puzzleFilePath">The path to the file containing the puzzle.</param>
        private static void ValidateInputFile(string puzzleFilePath)
        {
            if (!File.Exists(puzzleFilePath))
            {
                throw new FileNotFoundException("Unable to find puzzle file.", puzzleFilePath);
            }

            const string puzzleFileExtension = ".txt";
            if (Path.GetExtension(puzzleFilePath) != puzzleFileExtension)
            {
                throw new ArgumentException($"Invalid file type, expected {puzzleFileExtension}.", nameof(puzzleFilePath));
            }

            // Make sure the file actually contains some data.
            if (new FileInfo(puzzleFilePath).Length == 0)
            {
                throw new ArgumentException("Puzzle file is empty.", nameof(puzzleFilePath));
            }
        }

        /// <summary>
        /// Validate that the given puzzle file is the right size.
        /// </summary>
        /// <param name="puzzle">The lines that make up the puzzle.</param>
        private static void ValidatePuzzleSize(string[] puzzle)
        {
            const int SectionSize = 9;

            // Make sure we have 9 rows.
            if (puzzle.Length != SectionSize)
            {
                // Create parser exception for this.
                throw new ParserException("Puzzle does not have 9 rows.");
            }

            // Make sure every row has 9 columns.
            foreach (var line in puzzle)
            {
                if (line.Length != (SectionSize * 2) + 1)
                {
                    throw new ParserException("Not all rows have 9 columns.");
                }
            }
        }
    }
}
