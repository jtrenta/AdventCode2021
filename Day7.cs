using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2021
{

    class Day7
    {

        public static void D7Main() {
            string inputstring;
            int BestSpot = 0;
            inputstring = System.IO.File.ReadAllText(@".\Input\Day7Input.txt");
            //inputstring = "16,1,2,0,4,2,7,1,2,14";
            int[] positions = Array.ConvertAll(inputstring.Split(',', StringSplitOptions.RemoveEmptyEntries), s => int.Parse(s));
            Array.Sort(positions);
            //Best spot is the median value
            BestSpot = (positions[positions.Length / 2] + positions[(positions.Length - 1) / 2])/2;
            long FuelSpent = positions.Sum(p => Math.Abs(p - BestSpot));
            System.Console.WriteLine("Part 1: Best fuel usage is: {0}", FuelSpent);
            BestSpot = 0;

            long BestFuel = -1, CurrentFuel;
            double cost = 0.0;
            int minPos = positions.Min(), maxPos = positions.Max();
             for(int j=minPos;j<maxPos;j++) {
                CurrentFuel = 0;
                for(int i=0;i<positions.Count();i++) {
                    // Fuel usage = distance^2 + distance / 2 (formula for 1+2+3+4+..n)
                    cost = ((Math.Pow((positions[i]-j),2) + Math.Abs(positions[i]-j))/2.0);
                    CurrentFuel += (long)Math.Round(cost,MidpointRounding.AwayFromZero);
                }
                if(CurrentFuel < BestFuel || BestFuel == -1) {
                    BestFuel = CurrentFuel;
                    BestSpot = j;
                } else if(BestFuel != 1) {
                    //Once sequence hits best spot, cost will begin to rise and we can break loop
                    break;
                }
             }
            System.Console.WriteLine("Part 2: fuel usage is: {0}", BestFuel);
       }

    }

}
