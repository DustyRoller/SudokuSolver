using NUnit.Framework;
using System.IO;

namespace SudokuSolver.UnitTests
{
    [TestFixture]
    public class PuzzleUnitTests
    {
        private const string TestPuzzleDir = "TestPuzzles";

        [Test]
        public void Puzzle_Solve_FailsToSolveEmptyPuzzle()
        {
            var testFile = Path.Combine(TestPuzzleDir, "EmptyPuzzle.txt");

            Assert.IsTrue(File.Exists(testFile));

            var puzzle = Parser.Parser.ParsePuzzle(testFile);

            Assert.IsFalse(puzzle.Solve());
        }

        [TestCase("EasyPuzzle.txt")]
        [TestCase("EasyPuzzle2.txt")]
        [TestCase("MediumPuzzle.txt")]
        [TestCase("HardPuzzle.txt")]
        [TestCase("ExpertPuzzle.txt")]
        public void Puzzle_Solve_SuccessfullySolvesTestPuzzles(string testPuzzleFileName)
        {
            var testFile = Path.Combine(TestPuzzleDir, testPuzzleFileName);

            Assert.IsTrue(File.Exists(testFile));

            var puzzle = Parser.Parser.ParsePuzzle(testFile);

            Assert.IsTrue(puzzle.Solve());
        }
    }
}
