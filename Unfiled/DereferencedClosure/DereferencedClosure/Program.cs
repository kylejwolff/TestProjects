using System;
using System.Collections.Generic;
using System.Linq;

namespace DereferencedClosure
{
    class Program
    {
        private static readonly Derp[] Derps = new Derp[]
        {
            new Derp { Text = "one" },
            new Derp { Text = "two" },
            new Derp { Text = "three" },
        };

        static void Main(string[] args)
        {
            List<Action> derpers = new List<Action>();
            //Derp derp = null;
            for(int i = 0; i < Derps.Length; i++)
            {
                var derp = Derps[i];
                derpers.Add(() => { derp.Herp(); });
            }
            foreach(var deep in derpers)
                deep();
            Console.Read();
        }
    }

    class Derp
    {
        public string Text { get; set; }
        public void Herp()
        {
            Console.WriteLine(Text);
        }
    }
}
