using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace SudokuSolver.UnitTests
{
    [TestFixture]
    public class SectionUnitTests
    {
        [Test]
        public void Section_GetPossibleValues_ReturnsAllPotentialValuesWithNoSolvedCells()
        {
            var section = new Section
            {
                Cells = new List<ICell>
                {
                    new Cell(),
                },
            };

            var expectedPossibleValues = new List<uint>()
            {
                1u, 2u, 3u, 4u, 5u, 6u, 7u, 8u, 9u,
            };

            CollectionAssert.AreEqual(expectedPossibleValues, section.GetPossibleValues());
        }

        [Test]
        public void Section_GetPossibleValues_ReturnsValidPotentialValuesWithSolvedCells()
        {
            var section = new Section
            {
                Cells = new List<ICell>
                {
                    new Cell
                    {
                        CellValue = 1u,
                    },
                    new Cell
                    {
                        CellValue = 3u,
                    },
                    new Cell
                    {
                        CellValue = 5u,
                    },
                },
            };

            var expectedPossibleValues = new List<uint>()
            {
                2u, 4u, 6u, 7u, 8u, 9u,
            };

            CollectionAssert.AreEqual(expectedPossibleValues, section.GetPossibleValues());
        }

        [Test]
        public void Section_Solved_ReturnsFalseIfNotAllCellsAreSolved()
        {
            var section = new Section
            {
                Cells = new List<ICell>
                {
                    new Cell(),
                },
            };

            Assert.IsFalse(section.Solved);
        }

        [Test]
        public void Section_Solve_DoesNotSolveSectionWithNoUniqueValues()
        {
            var cell1 = new Mock<ICell>();
            var cell2 = new Mock<ICell>();

            var possibleValues = new List<uint> { 1, 2, };

            cell1.Setup(c => c.GetPossibleValues()).Returns(possibleValues);
            cell2.Setup(c => c.GetPossibleValues()).Returns(possibleValues);

            var section = new Section
            {
                Cells = new List<ICell>
                {
                    cell1.Object,
                    cell2.Object,
                },
            };

            section.Solve();

            // Assert that none of the cells got solved.
            cell1.VerifySet(c => c.CellValue = It.IsAny<uint>(), Times.Never());
            cell2.VerifySet(c => c.CellValue = It.IsAny<uint>(), Times.Never());
        }

        [Test]
        public void Section_Solve_SolvesCellWithUniqueValues()
        {
            var cell1 = new Mock<ICell>();
            var cell2 = new Mock<ICell>();

            var cell1PossibleValues = new List<uint> { 1, 2, };
            var cell2PossibleValues = new List<uint> { 1, 2, 3, };

            cell1.Setup(c => c.GetPossibleValues()).Returns(cell1PossibleValues);
            cell2.Setup(c => c.GetPossibleValues()).Returns(cell2PossibleValues);

            var section = new Section
            {
                Cells = new List<ICell>
                {
                    cell1.Object,
                    cell2.Object,
                },
            };

            section.Solve();

            // Assert that none of the cells got solved.
            cell1.VerifySet(c => c.CellValue = It.IsAny<uint>(), Times.Never());
            cell2.VerifySet(c => c.CellValue = 3u, Times.Once());
        }

        [Test]
        public void Section_Solved_ReturnsTrueIfAllCellsAreSolved()
        {
            var section = new Section
            {
                Cells = new List<ICell>
                {
                    new Cell
                    {
                        CellValue = 3u,
                    },
                },
            };

            Assert.IsTrue(section.Solved);
        }
    }
}
