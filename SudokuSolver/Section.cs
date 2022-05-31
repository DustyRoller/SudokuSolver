using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    class Section
    {
        /// <summary>
        /// Gets the list of Cells that make up this section.
        /// </summary>
        public List<Cell> Cells { get; set; }

        /// <summary>
        /// Is this Section solved with valid answers.
        /// </summary>
        /// <returns>True if section is solved, otherwise false.</returns>
        public bool IsSolved()
        {
            // Make sure that all the cells are solved and that they all have
            // unique values.
            return Cells.All(pc => pc.Solved) &&
                   Cells.Select(pc => pc.CellValue).Distinct().Count() == Cells.Count;
        }
    }
}

