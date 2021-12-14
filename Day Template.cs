using System;
using System.Collections.Generic;
using System.Linq;
using static Globals.ProjectConstants;

namespace AdventCode2021
{

    class DayNum
    {

        public static void DNumMain() {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            string inputstring;
            inputstring = System.IO.File.ReadAllText(@".\Input\DayNumInput.txt");
            System.Console.WriteLine("Execution time: {0} ms",watch.ElapsedMilliseconds);

       }

    }

}
