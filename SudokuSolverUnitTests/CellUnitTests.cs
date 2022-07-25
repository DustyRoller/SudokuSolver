using NUnit.Framework;
using System.Collections.Generic;

namespace SudokuSolver.UnitTests
{
    [TestFixture]
    internal class CellUnitTests
    {
        [Test]
        public void Cell_CellValue_ThrowsExceptionIfValueGreaterThanNine()
        {
            var cell = new Cell();

            var ex = Assert.Throws<SudokuSolverException>(() => cell.CellValue = 10u);

            Assert.AreEqual($"Cell value cannot be greater than 9. {cell.Coordinate}.", ex.Message);
        }

        [Test]
        public void Cell_PossibleValues_ReturnsAllPossibleValuesWithNoOtherSolvedCellsInSections()
        {
            var section = new Section
            {
                Cells = new List<ICell>
                {
                    new Cell(),
                },
            };

            var cell = new Cell
            {
                ColumnSection = section,
                RowSection = section,
                SquareSection = section,
            };

            var expectedPossibleValues = new List<uint>()
            {
                1u, 2u, 3u, 4u, 5u, 6u, 7u, 8u, 9u,
            };

            CollectionAssert.AreEqual(expectedPossibleValues, cell.PossibleValues);
        }

        [Test]
        public void Cell_PossibleValues_ReturnsSubsetOfPossibleValuesWithSolvedCellsInSections()
        {
            var unsolvedSection = new Section
            {
                Cells = new List<ICell>
                {
                    new Cell(),
                },
            };

            var partiallySolvedSection = new Section
            {
                Cells = new List<ICell>
                {
                    new Cell { CellValue = 1u, },
                    new Cell { CellValue = 2u, },
                    new Cell { CellValue = 3u, },
                },
            };

            var cell = new Cell
            {
                ColumnSection = unsolvedSection,
                RowSection = unsolvedSection,
                SquareSection = partiallySolvedSection,
            };

            var expectedPossibleValues = new List<uint>()
            {
                4u, 5u, 6u, 7u, 8u, 9u,
            };

            CollectionAssert.AreEqual(expectedPossibleValues, cell.PossibleValues);
        }

        [Test]
        public void Cell_Solved_ReturnsFalseIfCellValueIsZero()
        {
            var cell = new Cell();

            Assert.IsFalse(cell.Solved);
        }

        [Test]
        public void Cell_Solved_ReturnsTrueIfCellValueIsNotZero()
        {
            var cell = new Cell
            {
                CellValue = 1u,
            };

            Assert.IsTrue(cell.Solved);
        }
    }
}
