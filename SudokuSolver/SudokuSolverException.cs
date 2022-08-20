using System;

namespace SudokuSolver
{
    /// <summary>
    /// Exception class for any exception thrown within the project that
    /// relates to the solving of a Sudoku puzzle.
    /// </summary>
    [Serializable]
    public class SudokuSolverException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SudokuSolverException"/> class.
        /// </summary>
        public SudokuSolverException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SudokuSolverException"/> class.
        /// </summary>
        /// <param name="message">The message associated with the exception.</param>
        public SudokuSolverException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SudokuSolverException"/> class.
        /// </summary>
        /// <param name="message">The message associated with the exception.</param>
        /// <param name="innerException">The inner Excepton associated with the exception.</param>
        public SudokuSolverException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
