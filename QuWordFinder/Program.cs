using QuWordFinderBusiness.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace QuWordFinder
{
    public class Program
    {
        #region "Static Methods"
        static void Main()
        {
            string[] matrix;
            do
            {
                int matrixSize = GetMatrixSizeFromUser();
                List<string> matrixInput = GetMatrixRows(matrixSize);

                Console.WriteLine("\nMatrix input is complete. The matrix is:");

                WordFinder wordFinder = new WordFinder(matrixInput);
                matrix = wordFinder.GetMatrix();

                if (matrix != null)
                {
                    Console.WriteLine("\t " + new string('_', matrixSize));
                    foreach (var row in matrix)
                        Console.WriteLine("\t|" + row + "|");
                    Console.WriteLine("\t " + new string('-', matrixSize));
                }

                var wordsToSearch = GetWordsToSearch();

                DisplayMostRepeatedWords(wordFinder, wordsToSearch);

                Console.WriteLine("\nThanks for using QuWordFinder. Do you want to repeat? (Y/N):");
            }
            while (Console.ReadLine()?.Trim().ToUpper() == "Y");

            Console.WriteLine("Goodbye! Thanks for using QuWordFinder.");
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }

        /// <summary>
        /// Method to get the matrix size from the user with validation
        /// </summary>
        /// <returns>The validated matrix size</returns>
        static int GetMatrixSizeFromUser()
        {
            int matrixSize;
            int matrixSizeMax = int.Parse(ConfigurationManager.AppSettings["MatrixSizeMax"]);
            int matrixSizeMin = int.Parse(ConfigurationManager.AppSettings["MatrixSizeMin"]);

            Func<int, bool> isValidSize = size => size >= matrixSizeMin && size <= matrixSizeMax;

            while (true)
            {
                Console.WriteLine(String.Format("Enter the size of the matrix (between {0} and {1}):", matrixSizeMin, matrixSizeMax));
                string sizeInput = Console.ReadLine();

                if (!(int.TryParse(sizeInput, out matrixSize) && isValidSize(matrixSize)))
                    Console.WriteLine(String.Format("Invalid matrix size. Please ensure it's between {0} and {1}.{2}", matrixSizeMin, matrixSizeMax, Environment.NewLine));
                else
                    break;
            }

            return matrixSize;
        }

        /// <summary>
        /// Method to gather matrix rows from the user with validation
        /// </summary>
        /// <param name="matrixSize">The size of the matrix</param>
        /// <returns>A list of matrix rows</returns>
        static List<string> GetMatrixRows(int matrixSize)
        {
            List<string> matrixInput = new List<string>();

            Console.WriteLine(String.Format("Enter {0} rows, each with {0} characters:", matrixSize));

            for (int i = 0; i < matrixSize; i++)
            {
                string row;
                while (true)
                {
                    row = Console.ReadLine().ToLower();
                    if (row.Length != matrixSize)
                        Console.WriteLine(String.Format("Each row must have exactly {0} characters. Please enter a valid row.", matrixSize));
                    else
                    {
                        matrixInput.Add(row);
                        break;
                    }
                }
            }

            return matrixInput;
        }

        /// <summary>
        /// Method to get words from the user to search in the matrix
        /// </summary>
        /// <returns>A list of words to search for</returns>
        static string[] GetWordsToSearch()
        {
            Console.WriteLine("\nEnter words to search (separated by space, comma, or semi-colon):");
            string wordStream = Console.ReadLine().ToLower();

            return wordStream.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Method to find and display the most repeated words in the matrix
        /// </summary>
        /// <param name="wordFinder">The WordFinder instance</param>
        /// <param name="words">List of words to search for</param>
        static void DisplayMostRepeatedWords(WordFinder wordFinder, string[] words)
        {
            var result = wordFinder.Find(words);

            if (result.Any())
            {
                Console.WriteLine("\nMost repeated words found:");
                foreach (var word in result)
                    Console.WriteLine(word);
            }
            else
                Console.WriteLine("\nNo words found in the matrix.");
        }

        #endregion
    }
}
