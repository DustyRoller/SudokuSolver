using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace SudokuSolver.Parser.UnitTests
{
    [TestFixture]
    public class ParserUnitTests
    {
        private const string TestPuzzleFileName = "TestPuzzle.txt";

        private const string TestPuzzleDir = "TestPuzzles";

        [Test]
        public void Parser_ParsePuzzle_FailsWithNonExistantFile()
        {
            var ex = Assert.Throws<FileNotFoundException>(() => Parser.ParsePuzzle("randomfile"));

            Assert.AreEqual("Unable to find puzzle file.", ex.Message);
        }

        [Test]
        public void Parser_ParsePuzzle_FailsWithInvalidFileExtension()
        {
            var fileName = "test.fdg";

            File.Create(fileName).Close();

            var ex = Assert.Throws<ArgumentException>(() => Parser.ParsePuzzle(fileName));

            Assert.AreEqual("Invalid file type, expected .txt. (Parameter 'puzzleFilePath')", ex.Message);

            File.Delete(fileName);
        }

        [Test]
        public void Parser_ParsePuzzle_FailsWithEmptyFile()
        {
            File.Create(TestPuzzleFileName).Close();

            var ex = Assert.Throws<ArgumentException>(() => Parser.ParsePuzzle(TestPuzzleFileName));

            Assert.AreEqual("Puzzle file is empty. (Parameter 'puzzleFilePath')", ex.Message);

            File.Delete(TestPuzzleFileName);
        }

        [Test]
        public void Parser_ParsePuzzle_FailsWithPuzzleWithInvalidNumberOfColumns()
        {
            var sb = new StringBuilder();

            // Create puzzle with 8 columns.
            sb.AppendLine("|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|");

            File.WriteAllText(TestPuzzleFileName, sb.ToString());

            var ex = Assert.Throws<ParserException>(() => Parser.ParsePuzzle(TestPuzzleFileName));

            Assert.AreEqual("Not all rows have 9 columns.", ex.Message);

            File.Delete(TestPuzzleFileName);
        }

        [Test]
        public void Parser_ParsePuzzle_FailsWithPuzzleWithInvalidNumberOfRows()
        {
            var sb = new StringBuilder();

            // Create puzzle with 8 rows.
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");

            File.WriteAllText(TestPuzzleFileName, sb.ToString());

            var ex = Assert.Throws<ParserException>(() => Parser.ParsePuzzle(TestPuzzleFileName));

            Assert.AreEqual("Puzzle does not have 9 rows.", ex.Message);

            File.Delete(TestPuzzleFileName);
        }

        [Test]
        public void Parser_ParsePuzzle_FailsWithPuzzleWithInvalidCharacters()
        {
            var sb = new StringBuilder();

            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|?|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");

            File.WriteAllText(TestPuzzleFileName, sb.ToString());

            var ex = Assert.Throws<ParserException>(() => Parser.ParsePuzzle(TestPuzzleFileName));

            Assert.AreEqual("Failed to parse cell value: ?", ex.Message);

            File.Delete(TestPuzzleFileName);
        }

        [Test]
        public void Parser_ParsePuzzle_Successful()
        {
            var testFile = Path.Combine(TestPuzzleDir, "EasyPuzzle.txt");

            Assert.IsTrue(File.Exists(testFile));

            var puzzle = Parser.ParsePuzzle(testFile);

            Assert.AreEqual(81, puzzle.Cells.Count);
            Assert.AreEqual(9, puzzle.Columns.Count);
            Assert.AreEqual(9, puzzle.Rows.Count);
            Assert.AreEqual(9, puzzle.Squares.Count);

            // Assert that the cell coordinates are correct.
            var index = 0;

            for (var y = 0u; y < 9; ++y)
            {
                for (var x = 0u; x < 9; ++x)
                {
                    Assert.AreEqual(new Coordinate(x, y), puzzle.Cells[index].Coordinate);

                    index++;
                }
            }

            // Assert that all the columns and rows have the expected coordinates.
            for (int i = 0; i < 9; ++i)
            {
                Assert.IsTrue(puzzle.Columns[i].Cells.All(c => c.Coordinate.Y == i));
                Assert.IsTrue(puzzle.Rows[i].Cells.All(c => c.Coordinate.X == i));
            }

            Assert.AreEqual(0u, puzzle.Cells[0].CellValue);
            Assert.AreEqual(4u, puzzle.Cells[1].CellValue);
            Assert.AreEqual(2u, puzzle.Cells[2].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[3].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[4].CellValue);
            Assert.AreEqual(5u, puzzle.Cells[5].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[6].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[7].CellValue);
            Assert.AreEqual(6u, puzzle.Cells[8].CellValue);

            Assert.AreEqual(1u, puzzle.Cells[9].CellValue);
            Assert.AreEqual(9u, puzzle.Cells[10].CellValue);
            Assert.AreEqual(7u, puzzle.Cells[11].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[12].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[13].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[14].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[15].CellValue);
            Assert.AreEqual(4u, puzzle.Cells[16].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[17].CellValue);

            Assert.AreEqual(5u, puzzle.Cells[18].CellValue);
            Assert.AreEqual(6u, puzzle.Cells[19].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[20].CellValue);
            Assert.AreEqual(4u, puzzle.Cells[21].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[22].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[23].CellValue);
            Assert.AreEqual(1u, puzzle.Cells[24].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[25].CellValue);
            Assert.AreEqual(9u, puzzle.Cells[26].CellValue);

            Assert.AreEqual(8u, puzzle.Cells[27].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[28].CellValue);
            Assert.AreEqual(1u, puzzle.Cells[29].CellValue);
            Assert.AreEqual(3u, puzzle.Cells[30].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[31].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[32].CellValue);
            Assert.AreEqual(2u, puzzle.Cells[33].CellValue);
            Assert.AreEqual(6u, puzzle.Cells[34].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[35].CellValue);

            Assert.AreEqual(9u, puzzle.Cells[36].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[37].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[38].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[39].CellValue);
            Assert.AreEqual(7u, puzzle.Cells[40].CellValue);
            Assert.AreEqual(1u, puzzle.Cells[41].CellValue);
            Assert.AreEqual(4u, puzzle.Cells[42].CellValue);
            Assert.AreEqual(5u, puzzle.Cells[43].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[44].CellValue);

            Assert.AreEqual(0u, puzzle.Cells[45].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[46].CellValue);
            Assert.AreEqual(3u, puzzle.Cells[47].CellValue);
            Assert.AreEqual(2u, puzzle.Cells[48].CellValue);
            Assert.AreEqual(5u, puzzle.Cells[49].CellValue);
            Assert.AreEqual(6u, puzzle.Cells[50].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[51].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[52].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[53].CellValue);

            Assert.AreEqual(0u, puzzle.Cells[54].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[55].CellValue);
            Assert.AreEqual(5u, puzzle.Cells[56].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[57].CellValue);
            Assert.AreEqual(3u, puzzle.Cells[58].CellValue);
            Assert.AreEqual(2u, puzzle.Cells[59].CellValue);
            Assert.AreEqual(7u, puzzle.Cells[60].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[61].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[62].CellValue);

            Assert.AreEqual(0u, puzzle.Cells[63].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[64].CellValue);
            Assert.AreEqual(4u, puzzle.Cells[65].CellValue);
            Assert.AreEqual(5u, puzzle.Cells[66].CellValue);
            Assert.AreEqual(9u, puzzle.Cells[67].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[68].CellValue);
            Assert.AreEqual(6u, puzzle.Cells[69].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[70].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[71].CellValue);

            Assert.AreEqual(0u, puzzle.Cells[72].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[73].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[74].CellValue);
            Assert.AreEqual(7u, puzzle.Cells[75].CellValue);
            Assert.AreEqual(6u, puzzle.Cells[76].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[77].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[78].CellValue);
            Assert.AreEqual(8u, puzzle.Cells[79].CellValue);
            Assert.AreEqual(0u, puzzle.Cells[80].CellValue);
        }
    }
}
