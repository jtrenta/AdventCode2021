using System;
using System.Collections.Generic;
using System.Linq;
using static Globals.Day5Constants;

namespace AdventCode2021
{

    class Line {
        private int[] points = new int[4];
        private int slope;

        public Line(string inputstring)  {
            points = Array.ConvertAll(inputstring.Replace(" -> ", ",").Split(','), s => int.Parse(s));
            if(points[X1] != points[X2]) { 
                slope = (points[Y2]-points[Y1])/(points[X2]-points[X1]);
            }
        }

        private bool vertical {
            get {
                return points[X1] == points[X2];
            }
        }

        private bool InRange(int x, int y) {
            return (    ((x >= points[X1] && x<= points[X2]) || (x >= points[X2] && x<= points[X1])) 
                    &&  ((y >= points[Y1] && y<= points[Y2]) || (y >= points[Y2] && y<= points[Y1])) );
        }

        private bool StraightLine {
            get {
                return points[X1] == points[X2] || points[Y1] == points[Y2];
            }
        }

        public bool Intersects(int x, int y, bool checkDiagonals) {
            if(StraightLine || checkDiagonals) {
                if(InRange(x, y)) {
                    if(     (!vertical && (y - points[Y1]) == (slope * (x - points[X1]))) 
                        ||  ( vertical && x == points[X1])) {  
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
                    if(lines.Where(l => l.Intersects(x, y, false)).Count() > 1) numDangerZones++;
                }
            }
            System.Console.WriteLine("Part 1: Number of dangerous zones is {0}", numDangerZones);
            numDangerZones = 0;
            for(int x=0;x<=maxSize;x++) {
                for(int y=0;y<=maxSize;y++) {
                    if(lines.Where(l => l.Intersects(x, y, true)).Count() > 1) numDangerZones++;
                }
            }
            System.Console.WriteLine("Part 2: Number of dangerous zones is {0}", numDangerZones);
       }

    }

}
