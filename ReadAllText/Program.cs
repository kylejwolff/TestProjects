using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadAllText
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read the file as one string.
            System.IO.StreamReader myFile =
               new System.IO.StreamReader("c:\\TestProjects\\Truck.txt");
            string myString = myFile.ReadToEnd();

            myFile.Close();

            // Display the file contents.
            Console.WriteLine(myString);
            // Suspend the screen.
            Console.ReadLine();
        }
    }
}
