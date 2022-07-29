using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    /// <summary>
    /// An object representing a section within a sudoku puzzle. Each section
    /// is made up of nine cells and can represent either a column, row or 3x3
    /// square within a puzzle.
    /// </summary>
    internal class Section
    {
        private static readonly List<uint> ValidValues = new ()
        {
            1u, 2u, 3u, 4u, 5u, 6u, 7u, 8u, 9u,
        };

        /// <summary>
        /// Gets or sets the list of Cells that make up this section.
        /// </summary>
        public List<ICell> Cells { get; set; }

        /// <summary>
        /// Gets a value indicating whether this section is solved or not.
        /// </summary>
        public bool Solved => Cells.All(pc => pc.Solved) &&
                              Cells.Select(pc => pc.CellValue).Distinct().Count() == Cells.Count;

        /// <summary>
        /// Gets the list of possible values that have not been assigned to any
        /// of the Cells within this section.
        /// </summary>
        /// <returns>The list of possible values remaining for this section.</returns>
        public List<uint> GetPossibleValues()
        {
            return ValidValues.Except(Cells.Where(c => c.Solved)
                              .Select(c => c.CellValue))
                              .ToList();
        }

        /// <summary>
        /// Attempt to solve any of the cells within the section.
        /// </summary>
        public void Solve()
        {
            // See if any of the cells in this section have a unique value.
            var allPossibleValues = new List<uint>();
            Cells.ForEach(c => allPossibleValues.AddRange(c.GetPossibleValues()));

            var uniqueValues = allPossibleValues
              .GroupBy(x => x)
              .Where(x => x.Count() == 1)
              .Select(grp => grp.Key)
              .ToList();

            // If there are any unique values, find the cell it belongs to and
            // set that as its value.
            uniqueValues.ForEach(uv => Cells.First(c => c.GetPossibleValues().Contains(uv)).CellValue = uv);
        }
    }
}
