using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2021
{

    public static class P {
        public const int X1 = 0;
        public const int Y1 = 1;
        public const int X2 = 2;
        public const int Y2 = 3;
    }

    class Line {
        private string raw;
        private int[] points = new int[4];
        private int slope;
        bool vertical = false;

        public Line(string inputstring)  {
            raw = inputstring;
            points = Array.ConvertAll(raw.Replace(" -> ", ",").Split(','), s => int.Parse(s));
            if(points[P.X1] == points[P.X2]) { 
                vertical = true; 
            } else {
                slope = (points[P.Y2]-points[P.Y1])/(points[P.X2]-points[P.X1]);
            }
        }

        public bool Intersects(int x, int y, bool part1) {
            if((points[P.X1] == points[P.X2] || points[P.Y1] == points[P.Y2]) || !part1) {
                if(((x >= points[P.X1] && x<= points[P.X2]) || (x >= points[P.X2] && x<= points[P.X1])) && ((y >= points[P.Y1] && y<= points[P.Y2]) || (y >= points[P.Y2] && y<= points[P.Y1]))) {
                    if((!vertical && (y - points[P.Y1]) == (slope * (x - points[P.X1]))) || (vertical && x == points[P.X1])) {  
                        return true;
                    }
                }
            }
            return false;
        }
    }

    class Day5
    {

        public static void D5Main() {
            string inputstring;
            List<Line> lines = new List<Line>();
            int numDangerZones = 0;
            int maxSize = 0;
            inputstring = System.IO.File.ReadAllText(@".\Input\Day5Input.txt");
            maxSize = Array.ConvertAll(inputstring.Replace("\r\n", ",").Replace(" -> ", ",").Split(","), s => int.Parse(s)).Max();
            foreach(string item in inputstring.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)) {
                lines.Add(new Line(item));
            }
            for(int x=0;x<=maxSize;x++) {
                for(int y=0;y<=maxSize;y++) {
                    if(lines.Where(l => l.Intersects(x, y, true)).Count() > 1) numDangerZones++;
                }
            }
            System.Console.WriteLine("Part 1: Number of dangerous zones is {0}", numDangerZones);
            numDangerZones = 0;
            for(int x=0;x<=maxSize;x++) {
                for(int y=0;y<=maxSize;y++) {
                    if(lines.Where(l => l.Intersects(x, y, false)).Count() > 1) numDangerZones++;
                }
            }
            System.Console.WriteLine("Part 2: Number of dangerous zones is {0}", numDangerZones);
       }

    }

}
