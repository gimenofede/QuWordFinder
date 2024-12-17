using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace QuWordFinderBusiness.Models
{
    public class WordFinder
    {
        private readonly string[] matrix;
        private int matrixSizeMax = int.Parse(ConfigurationManager.AppSettings["MatrixSizeMax"]);
        private int matrixSizeMin = int.Parse(ConfigurationManager.AppSettings["MatrixSizeMin"]);
        private int mostRepeatedWord = int.Parse(ConfigurationManager.AppSettings["MostRepeatedWords"]);

        #region "Public Methods"
        public WordFinder(IEnumerable<string> matrix)
        {
            var matrixArray = matrix.ToArray();

            if (matrixArray.Length < matrixSizeMin || matrixArray.Length > matrixSizeMax || matrixArray.Any(row => row.Length != matrixArray[0].Length))
                throw new ArgumentException(String.Format("Matrix size must be between {0}x{0} and {1}x{1}, and all rows must have the same number of characters.", matrixSizeMin, matrixSizeMax));

            this.matrix = matrixArray;
        }

        /// <summary>
        /// Method to find the most repeated words from the user input stream
        /// </summary>
        /// <param name="wordstream"></param>
        /// <returns></returns>
        public IEnumerable<string> Find(IEnumerable<string> wordstream)
        {
            var foundWords = wordstream
                .Distinct()
                .Where(word => WordExistsHorizontally(word) || WordExistsVertically(word))
                .Take(mostRepeatedWord)
                .ToHashSet();

            return foundWords;
        }

        /// <summary>
        /// Method to get the matrix.
        /// </summary>
        public string[] GetMatrix()
        {
            return matrix;
        }
        #endregion

        #region "Private Methods"
        /// <summary>
        /// Method to search for a word in the matrix horizontally
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private bool WordExistsHorizontally(string word)
        {
            return matrix.Any(row => row.Contains(word));
        }

        /// <summary>
        /// Method to search for a word in the matrix vertically
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private bool WordExistsVertically(string word)
        {
            return Enumerable.Range(0, matrix[0].Length) // Iterate through columns
                             .Any(col => string.Concat(matrix.Select(row => row[col])).Contains(word));
        }
        #endregion
    }
}