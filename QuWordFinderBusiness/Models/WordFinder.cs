using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

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
            try
            {
                var matrixArray = matrix.ToArray();

                if (matrixArray.Length < matrixSizeMin || matrixArray.Length > matrixSizeMax || matrixArray.Any(row => row.Length != matrixArray[0].Length))
                    throw new ArgumentException(String.Format("Matrix size must be between {0}x{0} and {1}x{1}, and all rows must have the same number of characters.", matrixSizeMin, matrixSizeMax));

                this.matrix = matrixArray;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during initialization of WordFinder: " + ex.Message);
                //Log.Insert(ex.InnerException)
                throw;
            }
        }

        /// <summary>
        /// Method to find the most repeated words from the user input stream
        /// </summary>
        /// <param name="wordstream"></param>
        /// <returns></returns>
        public IEnumerable<string> Find(IEnumerable<string> wordstream)
        {
            try
            {
                var foundWords = wordstream
                .Distinct()
                .Where(word => WordExistsHorizontally(word) || WordExistsVertically(word))
                .Take(mostRepeatedWord)
                .ToHashSet();

                return foundWords;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while finding words: " + ex.Message);
                //Log.Insert(ex.InnerException)
                return Enumerable.Empty<string>();
            }
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
            try
            {
                var rowStrings = matrix;
                return rowStrings.Any(row => row.Contains(word));
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("Error while checking horizontally for word '{0}': {1}", word, ex.Message));
                //Log.Insert(ex.InnerException)
                return false;  
            }
        }

        /// <summary>
        /// Method to search for a word in the matrix vertically
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private bool WordExistsVertically(string word)
        {
            try
            {
                int numRows = matrix.Length;
                int numCols = matrix[0].Length;
                var columnHashes = new HashSet<string>();

                for (int col = 0; col < numCols; col++)
                {
                    var columnBuilder = new StringBuilder();
                    for (int row = 0; row < numRows; row++)
                    {
                        columnBuilder.Append(matrix[row][col]);
                    }
                    columnHashes.Add(columnBuilder.ToString());
                }

                return columnHashes.Contains(word);
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("Error while checking vertically for word '{0}': {1}", word, ex.Message));
                //Log.Insert(ex.InnerException)
                return false;
            }
        }
        #endregion
    }
}