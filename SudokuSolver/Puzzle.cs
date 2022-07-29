using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    /// <summary>
    /// Class representing a sudoku puzzle.
    /// </summary>
    internal class Puzzle
    {
        /// <summary>
        /// List of all of the cells in the puzzle.
        /// </summary>
        private readonly List<ICell> cells;

        /// <summary>
        /// Initializes a new instance of the <see cref="Puzzle"/> class.
        /// </summary>
        public Puzzle()
        {
            cells = new List<ICell>();
        }

        /// <summary>
        /// Gets all of the Cells that make up the puzzle.
        /// </summary>
        public ReadOnlyCollection<ICell> Cells => cells.AsReadOnly();

        /// <summary>
        /// Gets or sets all of the column sections that make up the puzzle.
        /// </summary>
        public List<Section> Columns { get; set; } = new ();

        /// <summary>
        /// Gets the number of currently unsolved puzzle cells.
        /// </summary>
        public int NumberOfUnsolvedCells => cells.Count(pc => !pc.Solved);

        /// <summary>
        /// Gets or sets all of the row sections that make up the puzzle.
        /// </summary>
        public List<Section> Rows { get; set; } = new ();

        /// <summary>
        /// Gets or sets all of the square sections that make up the puzzle.
        /// </summary>
        public List<Section> Squares { get; set; } = new ();

        /// <summary>
        /// Add the given cell to this puzzle.
        /// </summary>
        /// <param name="cell">The cell to add.</param>
        public void AddCell(Cell cell)
        {
            cells.Add(cell);
        }

        /// <summary>
        /// Solve the puzzle.
        /// </summary>
        /// <returns>True if the puzzle was solved, otherwise false.</returns>
        public bool Solve()
        {
            var numCellsSolved = cells.Count(c => c.Solved);
            var updatedSolvedNumber = numCellsSolved;

            // Attempt to solve any sections or cells that only have single
            // possible values for a quick win.
            do
            {
                numCellsSolved = updatedSolvedNumber;

                Columns.Where(c => !c.Solved).ToList().ForEach(c => c.Solve());
                Rows.Where(r => !r.Solved).ToList().ForEach(r => r.Solve());
                Squares.Where(s => !s.Solved).ToList().ForEach(s => s.Solve());

                cells.Where(c => !c.Solved)
                     .ToList()
                     .ForEach(c => c.Solve());

                updatedSolvedNumber = cells.Count(c => c.Solved);
            }
            while (updatedSolvedNumber != numCellsSolved
                   && numCellsSolved != cells.Count);

            // If there are still unsolved cells then we can recursively
            // attempt to assign them values until we get a solution.
            if (numCellsSolved != Cells.Count)
            {
                RecursivelySolvePuzzle(cells.Where(c => !c.Solved).ToList());
            }

            // Check that all the cells and sections are solved.
            return NumberOfUnsolvedCells == 0 &&
                   Columns.All(c => c.Solved) &&
                   Rows.All(r => r.Solved) &&
                   Squares.All(s => s.Solved);
        }

        /// <summary>
        /// Get a string representation of the current state of the puzzle.
        /// </summary>
        /// <returns>String representing the current state of the puzzle.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            for (var i = 0; i < cells.Count; ++i)
            {
                sb.Append('|');

                if (i != 0 && i % 9 == 0)
                {
                    sb.AppendLine();
                    sb.Append('|');
                }

                sb.Append(cells[i].ToString());
            }

            sb.Append('|');

            return sb.ToString();
        }

        /// <summary>
        /// Recursively solve the puzzle by working our way through all of the
        /// given Cell's possible values until we get to a solved puzzle.
        /// </summary>
        /// <param name="cells">The Cells to solve.</param>
        /// <returns>True if the all the Cells are solved, otherwise false.</returns>
        private bool RecursivelySolvePuzzle(List<ICell> cells)
        {
            // Reached the end of the recursion.
            if (!cells.Any())
            {
                return true;
            }

            var success = false;

            // Check if this recursion path has provided us with more
            // possibilities to explore before continuing.
            if (cells.All(pc => pc.GetPossibleValues().Any()))
            {
                // To save the amount of recursion required keep sorting
                // the list by the number of possible values.
                var orderedCells = cells.OrderBy(pc => pc.GetPossibleValues().Count)
                                        .ToList();
                var cell = orderedCells[0];
                orderedCells.RemoveAt(0);

                foreach (var possibleValue in cell.GetPossibleValues())
                {
                    // Set the cells value to this possible value so that future
                    // cells will use this value when calculate their possible values.
                    cell.CellValue = possibleValue;

                    success = RecursivelySolvePuzzle(orderedCells);
                    if (success)
                    {
                        break;
                    }

                    cell.CellValue = 0u;
                }
            }

            return success;
        }
    }
}
