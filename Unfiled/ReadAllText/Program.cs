using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadAllText
{
    /// <summary>
    /// Simple Project called <c>ReadAllText</c>
    /// </summary>
    /// <remarks>This project prints the text from "Truck.txt" to a Console Window</remarks>
    class Program
    {
        /// <summary>
        /// The entry point for he application
        /// </summary>
        /// <param name="args">A list of command line arguments</param>
        static void Main(string[] args)
        {
            // Calls method WriteLine from the Console class,
            // which writes "Truck.txt" by calling method ReadAllText
            // from the File class which reads "Truck.text" because it
            // is the first Argument in the Command line.
            Console.WriteLine(File.ReadAllText(args[0]));
            Console.WriteLine("Hit enter to exit");
            Console.ReadLine();
        }
    }
}
