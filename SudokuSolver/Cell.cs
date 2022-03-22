using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver

{
    /// <summary>
    /// The Cell class represents a cell within the puzzle.
    /// </summary>
    internal class Cell
    {
        /// <summary>
        /// Gets or sets the value of the cell, will be 0 if it hasn't been solved yet.
        /// </summary>
        public uint CellValue
        {
            get => cellValue;
            set
            {
                if (value > 9)
                {
                    throw new SudokuSolverException($"Puzzle cell value cannot be greater than 9. {Coordinate}.");
                }

                cellValue = value;
            }
        }

        /// <summary>
        /// Gets or sets the column section that this cell belongs to.
        /// </summary>
        public Section ColumnSection { get; set; }

        /// <summary>
        /// Gets or sets the Cell's Coordinate.
        /// </summary>
        public Coordinate Coordinate { get; set; }

        /// <summary>
        /// Gets the possible values for this cell.
        /// </summary>
        /// <remarks>
        /// This calculates the integer partitions for the column and row
        /// sections that this cell belongs to, and returns all of the common
        /// values into a single list.
        /// </remarks>
        public List<uint> PossibleValues => new ();

        /// <summary>
        /// Gets or sets the row section that this cell belongs to.
        /// </summary>
        public Section RowSection { get; set; }

        /// <summary>
        /// Gets a value indicating whether this cell has been solved or not.
        /// </summary>
        public bool Solved => CellValue != 0u;

        /// <summary>
        /// Gets or sets the square section that this cell belongs to.
        /// </summary>
        public Section SquareSection { get; set; }

        private uint cellValue = 0u;

        /// <summary>
        /// Get a string representation of the current state of the cell.
        /// </summary>
        /// <returns>String representing the current state of the cell.</returns>
        public override string ToString()
        {
            return Solved ? CellValue.ToString() : "-";
        }
    }
}
