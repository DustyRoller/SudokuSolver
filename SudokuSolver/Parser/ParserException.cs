using System;
namespace SudokuSolver.Parser
{
    /// <summary>
    /// Exception class for exceptions throw whilst parsing a puzzle.
    /// </summary>
    internal class ParserException : SudokuSolverException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParserException"/> class.
        /// </summary>
        /// <param name="message">The message to be associate with the exception.</param>
        public ParserException(string message)
            : base(message)
        {
        }
    }
}
