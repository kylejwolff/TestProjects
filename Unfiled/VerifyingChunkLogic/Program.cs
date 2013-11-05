using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyingChunkLogic
{
    class Program
    {
        static void Main(string[] args)
        {
            // trying to verify that this logic works correctly
            // I want to chunk a collection and process each chunk without losing anybody or throwing exceptions
            var input = Enumerable.Range(1, 100);
            var take = 7;
            int skip = 0;
            int count = input.Count();
            while(skip < count)
            {
                Console.WriteLine(string.Join("", input.Skip(skip).Take(take)));
                skip += take;
            }
            Console.ReadLine();
        }
    }
}
