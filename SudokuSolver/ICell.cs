using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace SudokuSolver
{
    internal interface ICell
    {
        /// <summary>
        /// Gets or sets the value of the cell, will be 0 if it hasn't been solved yet.
        /// </summary>
        uint CellValue { get; set; }

        /// <summary>
        /// Gets or sets the column section that this cell belongs to.
        /// </summary>
        Section ColumnSection { get; set; }

        /// <summary>
        /// Gets or sets the Cell's Coordinate.
        /// </summary>
        Coordinate Coordinate { get; set; }

        /// <summary>
        /// Gets the possible values for this cell.
        /// </summary>
        List<uint> PossibleValues { get; }

        /// <summary>
        /// Gets or sets the row section that this cell belongs to.
        /// </summary>
        Section RowSection { get; set; }

        /// <summary>
        /// Gets a value indicating whether this cell has been solved or not.
        /// </summary>
        bool Solved { get; }

        /// <summary>
        /// Gets or sets the square section that this cell belongs to.
        /// </summary>
        Section SquareSection { get; set; }

        /// <summary>
        /// Attempt to solve this cell.
        /// </summary>
        void Solve();
    }
}
