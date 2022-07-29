using NUnit.Framework;
using System.IO;

namespace SudokuSolver.UnitTests
{
    [TestFixture]
    public class PuzzleUnitTests
    {
        [TestCase("EasyPuzzle.txt")]
        [TestCase("EasyPuzzle2.txt")]
        [TestCase("MediumPuzzle.txt")]
        [TestCase("HardPuzzle.txt")]
        [TestCase("ExpertPuzzle.txt")]
        public void Puzzle_Solve_SuccessfullySolvesTestPuzzles(string testPuzzleFileName)
        {
            var testPuzzleDir = "TestPuzzles";
            var testFile = Path.Combine(testPuzzleDir, testPuzzleFileName);

            Assert.IsTrue(File.Exists(testFile));

            var puzzle = Parser.Parser.ParsePuzzle(testFile);

            Assert.IsTrue(puzzle.Solve());
        }
    }
}
