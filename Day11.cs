using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2021
{

    class Day11
    {

        public static void D11Main() {
            string inputstring;
            bool debugPrints = false;
            inputstring = System.IO.File.ReadAllText(@".\Input\Day11Input.txt");
            //inputstring =   "5483143223\r\n" +
                            // "2745854711\r\n" +
                            // "5264556173\r\n" +
                            // "6141336146\r\n" +
                            // "6357385478\r\n" +
                            // "4167524645\r\n" +
                            // "2176841721\r\n" +
                            // "6882881134\r\n" +
                            // "4846848554\r\n" +
                            // "5283751526\r\n";
            string[] inputs = inputstring.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int width = inputs[0].Length;
            int height = inputs.Count();
            int[,] octopii = new int[width,height];
            for(int i=0;i < width;i++) {
                for(int j=0;j < height;j++) {
                    octopii[i,j] = int.Parse(inputs[i].ElementAt(j).ToString());
                }
            }

            int steps = 0, flashes = 0, stepWhenSynced = 0, flashesAt100Steps = 0;

            //Keep looping until both part 1 and part 2 are finished (100 steps for part 1, all octopii flashed for 2)
            while(steps < 100 || stepWhenSynced == 0) {
                //Increment all octopii
                for(int i=0;i < width;i++) {
                    for(int j=0;j < height;j++) {
                        octopii[i,j]++;
                    }
                }

                //If an octopus is ready to flash, call routine to flash
                for(int i=0;i < width;i++) {
                    for(int j=0;j < height;j++) {
                        if(octopii[i,j] > 9) {
                            flashes += Flash(i,j,octopii);
                        }
                    }
                }

                steps++;
                if(debugPrints) System.Console.Write("\nStep {0}:\n",steps);
                
                //Reset all flashing octopii to 0
                for(int i=0;i < width;i++) {
                    for(int j=0;j < height;j++) {
                        if(octopii[i,j] < 0) octopii[i,j]=0;
                        if(debugPrints) System.Console.Write(octopii[i,j]);
                    }
                    if(debugPrints) System.Console.Write("\n");
                }

                //Part 1 answer condition
                if(steps == 100) flashesAt100Steps = flashes;

                //Part 2 answer condition
                if(octopii.Cast<int>().Sum() == 0 && stepWhenSynced == 0) stepWhenSynced = steps;
            }

            System.Console.WriteLine("Part 1: Flashes after 100 steps: {0}",flashesAt100Steps);
            System.Console.WriteLine("Part 2: Step When Synced: {0}",stepWhenSynced);
       }

       static int Flash(int x, int y, int[,] octopii) {
            int flashes = 0;
            int[,] adjacencyMatrix = new int[8,2]{ { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };
            flashes++;
            
            //Set octopus value to min int so no further adjacent flashes will trigger it again
            octopii[x,y] = int.MinValue;

            //Increment all adjacent octopii
            for(int k = 0;k<8;k++) {
                if(x+adjacencyMatrix[k,0] >= 0 && y+adjacencyMatrix[k,1] >= 0 && x+adjacencyMatrix[k,0] < octopii.GetLength(0) && y+adjacencyMatrix[k,1] < octopii.GetLength(1)) {
                    octopii[x+adjacencyMatrix[k,0],y+adjacencyMatrix[k,1]]++;

                    //If adjacent octopus hits 10+, recursively flash
                    if(octopii[x+adjacencyMatrix[k,0],y+adjacencyMatrix[k,1]] > 9) {
                        flashes += Flash(x+adjacencyMatrix[k,0],y+adjacencyMatrix[k,1],octopii);
                    }
                }
            }

            return flashes;

       }

    }

}
