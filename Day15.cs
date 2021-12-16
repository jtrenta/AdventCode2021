using System;
using System.Collections.Generic;
using System.Linq;
using static Globals.ProjectConstants;

namespace AdventCode2021
{

    class Day15Map {
        private int[,] riskMap;
        private int width, height;
        private int[] start;
        private int Part1Answer, Part2Answer;
        public Day15Map(string input, int[] startPos) {
            string[] strings = input.Split("\r\n",StringSplitOptions.RemoveEmptyEntries);
            width = strings[0].Length;
            height = strings.Count();
            riskMap = new int[width*5, height*5];
            for(int y = 0; y < height;y++) {
                for(int x = 0; x < width;x++) {
                    riskMap[x,y] = strings[y].ToCharArray()[x] - '0';
                }
            }
            start = startPos;
            Part1Answer = Solve(width - 1, height - 1);

            for(int k=0;k<5;k++) {
                for(int i=1;i<5;i++){
                    for(int y = 0; y < strings.Count();y++) {
                        for(int x = 0; x < strings[y].Length;x++) {
                            for(int j=1;j<5;j++) {
                                riskMap[j*width+x,k*height+y] = riskMap[(j-1)*width+x,k*height+y]+1;
                                if(riskMap[j*width+x,k*height+y] > 9) riskMap[j*width+x,k*height+y] = 1;
                                riskMap[k*width+x,i*height+y] = riskMap[k*width+x,(i-1)*height+y]+1;
                                if(riskMap[k*width+x,i*height+y] > 9) riskMap[k*width+x,i*height+y] = 1;
                            }
                        }
                    }
                }
            }
            width = width*5;
            height = height*5;
            Part2Answer = Solve(width-1, height - 1);
        }

        public void PrintMap(int[,] values) {
            for(int y = 0; y < height;y++) {
                for(int x = 0; x < width;x++) {
                    System.Console.Write(values[x,y]+",");
                }
                System.Console.Write("\n");
            }
        }

        public int Part1 {
            get { return Part1Answer; }
        }

        public int Part2 {
            get { return Part2Answer; }
        }

        public int Solve(int x, int y) {
            int returnVal = 0;
            bool change = true;
            int[,] newMap = (int[,])riskMap.Clone();

            for(int i = 0;i < height;i++ ) {
                for(int j=0;j < width;j++) {
                    newMap[j,i] = int.MaxValue - 10;
                }
            }
            newMap[0,0] = 0;
            int changes = 0;

            while(change) {
                change = false;
                for(int i = 0;i < width;i++ ) {
                    for(int j=0;j < height;j++) {
                        if (j < width - 1) { 
                            if(newMap[j+1,i] + riskMap[j,i] < newMap[j,i] ) { 
                                newMap[j,i] = newMap[j+1,i] + riskMap[j,i]; 
                                change = true; changes++;
                                //PrintMap(newMap);
                            } 
                        }
                        if (j > 0 ) { 
                            if(newMap[j-1,i] + riskMap[j,i] < newMap[j,i]) { 
                                newMap[j,i] = newMap[j-1,i] + riskMap[j,i]; 
                                change = true; changes++;
                                //PrintMap(newMap);
                            } 
                        }
                        if (i < height - 1) { 
                            if(newMap[j,i+1] + riskMap[j,i] < newMap[j,i] ) { 
                                newMap[j,i] = newMap[j,i+1] + riskMap[j,i]; 
                                change = true; changes++;
                                //PrintMap(newMap);
                            } 
                        }
                        if (i > 0) { 
                            if(newMap[j,i-1] + riskMap[j,i] < newMap[j,i]) { 
                                newMap[j,i] = newMap[j,i-1] + riskMap[j,i]; 
                                change = true; changes++;
                                //PrintMap(newMap);
                            } 
                        }
                    }
                }
            }
            //System.Console.WriteLine("Changes: {0}",changes);
            //PrintMap(newMap);
            returnVal = newMap[x, y];
            return returnVal;
        }
    }

    class Day15
    {

        public static void D15Main() {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            string inputstring;
            inputstring = System.IO.File.ReadAllText(@".\Input\Day15Input.txt");
            // inputstring =   "1163751742\r\n" +
            //                 "1381373672\r\n" +
            //                 "2136511328\r\n" +
            //                 "3694931569\r\n" +
            //                 "7463417111\r\n" +
            //                 "1319128137\r\n" +
            //                 "1359912421\r\n" +
            //                 "3125421639\r\n" +
            //                 "1293138521\r\n" +
            //                 "2311944581";
            var map = new Day15Map(inputstring, new int[] { 0, 0});
            System.Console.WriteLine("Answer Part 1: {0}", map.Part1);
            System.Console.WriteLine("Answer Part 2: {0}", map.Part2);
            System.Console.WriteLine("Execution time: {0} ms",watch.ElapsedMilliseconds);

       }

    }

}
