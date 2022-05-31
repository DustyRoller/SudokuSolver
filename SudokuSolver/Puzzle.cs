using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SudokuSolver
{
    class Puzzle
    {
        /// <summary>
        /// Gets all of the Cells that make up the puzzle.
        /// </summary>
        public ReadOnlyCollection<Cell> Cells => cells.AsReadOnly();

        public List<Section> Columns { get; set; } = new();

        public List<Section> Rows { get; set; } = new();

        public List<Section> Squares { get; set; } = new();

        /// <summary>
        /// List of all of the cells in the puzzle.
        /// </summary>
        private readonly List<Cell> cells;

        /// <summary>
        /// Initializes a new instance of the <see cref="Puzzle"/> class.
        /// </summary>
        public Puzzle()
        {
            cells = new List<Cell>();
        }

        /// <summary>
        /// Add the given cell to this puzzle.
        /// </summary>
        /// <param name="cell">The cell to add.</param>
        public void AddCell(Cell cell)
        {
            cells.Add(cell);
        }
    }
}
