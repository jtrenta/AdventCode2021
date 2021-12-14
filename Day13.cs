using System;
using System.Collections.Generic;
using System.Linq;
using static Globals.ProjectConstants;

namespace AdventCode2021
{
    class GridPointComparer : IEqualityComparer<GridPoint> {

        public bool Equals(GridPoint g1, GridPoint g2)
        {
            if (g1.x == g2.x && g1.y == g2.y) return true;
            return false;
        }

        public int GetHashCode(GridPoint g1)
        {
            int hash = g1.x ^ g1.y;
            return hash.GetHashCode();
        }

    }

    class GridPoint {
        public GridPoint(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public int x {
            get;
            set;
        }

        public int y {
            get;
            set;
        }

        public override string ToString() {
            return "" + this.x + "," + this.y;
        }

        public int CompareTo(GridPoint that)
        {
            if (this.x == that.x && this.y == that.y) return 0;
            return 1;
        }    
    }


    class Day13
    {

        public static void D13Main() {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            string inputstring;
            inputstring = System.IO.File.ReadAllText(@".\Input\Day13Input.txt");
            //inputstring = System.IO.File.ReadAllText(@".\Input\Day13TestInput.txt");
            List<GridPoint> paper = new List<GridPoint>();
            foreach(string item in inputstring  .Split("end", StringSplitOptions.RemoveEmptyEntries)[0].Split("\r\n", StringSplitOptions.RemoveEmptyEntries)) {
                paper.Add(new GridPoint(int.Parse(item.Split(',')[0]), int.Parse(item.Split(',')[1])));
            }
            int width = paper.Max(p => p.x)+1;
            int height = paper.Max(p => p.y)+1;
            int foldIndex;
            int folds = 0;
            foreach(string item in inputstring  .Split("end", StringSplitOptions.RemoveEmptyEntries)[1].Split("\r\n", StringSplitOptions.RemoveEmptyEntries)) {
                folds++;
                foldIndex = int.Parse(item.Split('=')[1]);
                if(DEBUG){
                    for(int y=0;y < height;y++) {
                        for(int x=0;x < width;x++) {
                            Console.Write(paper.Where(p => p.x == x && p.y == y).Count() > 0 ? "#": ".");
                        }
                        Console.Write("\n");
                    }
                    Console.Write("\n");
                }
                if(item.ToCharArray()[11] == 'y') {
                    foreach(GridPoint g in paper) {
                        if(g.y > foldIndex) 
                            g.y = height - g.y - 1;
                    }
                    height = foldIndex;
                }
                if(item.ToCharArray()[11] == 'x') {
                    foreach(GridPoint g in paper) {
                        if(g.x > foldIndex) 
                            g.x = width - g.x - 1;
                    }
                    width = foldIndex;
                }
                paper = paper.Distinct(new GridPointComparer()).ToList();
                if(folds == 1) System.Console.WriteLine("Distinct points after {0} fold: {1}",folds, paper.Count());
            }

            for(int y=0;y < height;y++) {
                for(int x=0;x < width;x++) {
                    Console.Write(paper.Where(p => p.x == x && p.y == y).Count() > 0 ? "#": ".");
                }
                Console.Write("\n");
            }
            System.Console.WriteLine("Execution time: {0} ms",watch.ElapsedMilliseconds);

       }

    }

}
