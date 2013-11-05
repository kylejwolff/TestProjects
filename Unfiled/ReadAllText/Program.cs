using System;
using System.Collections.Generic;
using System.IO;
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

            //correct, more compact version of the above
            // using statement closes, disposes the stream no matter what
            // prevents the file from being locked if your app crashes for whatevs reason
            using (var filestream = new StreamReader(args[0]))
                Console.WriteLine(filestream.ReadToEnd());
            Console.ReadLine();

            //How I'd have done it.
            //The framework is your friend.
            Console.WriteLine(File.ReadAllText(args[0]));
            Console.WriteLine("Hit enter to exit");
            Console.ReadLine();
        }
    }
}
