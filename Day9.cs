using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2021
{

    class MapPoint {
        private int risk;
        private Basin b = null;
        private int[] location;
        private List<MapPoint> neighbors;

        public MapPoint(int ir, int ix, int iy) {
            risk = ir;
            location = new int[2] { ix, iy };
        }

        public void AddNeighbors(List<MapPoint> iPoints) {
            neighbors = iPoints;
        }

        public Basin basin {
            get {
                return b;
            }
        }

        public void UpdateBasin(Basin newBasin) {
            if(risk < 9) {
                b = newBasin;
                newBasin.AddPoint(this);
            }
            foreach(MapPoint p in neighbors) {
                if(p.getRisk < 9 && !p.inBasin) {
                    p.UpdateBasin(newBasin);
                    newBasin.AddPoint(p);
                }
            }
        }

        public bool inBasin {
            get {
                return !(b == null);
            }
        }

        public int getRisk {
            get {
                return risk;
            }
        }

        public int[] getLoc {
            get {
                return location.ToArray();
            }
        }

        public int x {
            get {
                return location[0];
            }
        }

        public int y {
            get {
                return location[1];
            }
        }

    }

    class Basin {
        List<MapPoint> points;

        public Basin(MapPoint first) {
            points = new List<MapPoint>();
            points.Add(first);
            first.UpdateBasin(this);
        }

        public void AddPoint(MapPoint p) {
            if(!points.Contains(p)) { 
                points.Add(p);
            }
        }

        public int size {
            get {
                return points.Count();
            }
        }
    }

    class Day9
    {

        public static void D9Main() {
            string inputstring;
            string[] inputstrings;
            int[,] map;
            int[] checkIndexes = { 0, 0, 0, 0 };
            int riskLevel = 0, width = 0, length = 0, currentRiskLevel = 0;
            Basin basin;
            List<MapPoint> points = new List<MapPoint>();
            List<Basin> basins = new List<Basin>();
            inputstring = System.IO.File.ReadAllText(@".\Input\Day9Input.txt");
            //inputstring = "2199943210\r\n3987894921\r\n9856789892\r\n8767896789\r\n9899965678";
            inputstrings = inputstring.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            length = inputstrings.Count();
            width = inputstrings[0].Length;
            map = new int[length, width];
            
            for(int i = 0;i < length; i++) {
                for(int j = 0;j < width; j++) {
                    map[i,j] = int.Parse(inputstrings[i].ElementAt(j).ToString());
                    points.Add(new MapPoint(map[i,j], i, j));
                }
            }

            //Part1
            for(int i = 0;i < length; i++) {
                for(int j = 0;j < width; j++) {
                    checkIndexes = new int[] { i-1 >= 0 ? i-1 : i+1, i+1 < length ? i+1 : i-1, j-1 >= 0 ? j-1 : j+1, j+1 < width ? j+1 : j-1 };
                    currentRiskLevel = map[i,j];
                    if(map[checkIndexes[0],j] > currentRiskLevel &&
                       map[checkIndexes[1],j] > currentRiskLevel &&
                       map[i,checkIndexes[2]] > currentRiskLevel &&
                       map[i,checkIndexes[3]] > currentRiskLevel) {
                           riskLevel += 1 + currentRiskLevel;
                       }
                }
            }

            //Part2
            //Create links between points so basins can be created via recursion
            for(int i = 0;i < length; i++) {
                for(int j = 0;j < width; j++) {
                    points.Where(p => (p.x == i && p.y == j)).FirstOrDefault().AddNeighbors(
                        points.Where(n => ((n.x >= i-1 && n.x <= i+1 && n.y == j) || (n.y >= j-1 && n.y <= j+1 && n.x == i)) && !(n.x == i && n.y == j)).ToList<MapPoint>());
                }
            }
            //For each point without a basin already, create a new basin and traverse the linked points until all points in basin have been added
            foreach(MapPoint mp in points) {
                if(mp.getRisk < 9 && !mp.inBasin) {
                    basin = new Basin(mp);
                    basins.Add(basin);
                    mp.UpdateBasin(basin);
                }
            }

            basins.Sort((x, y) => y.size.CompareTo(x.size));

            System.Console.WriteLine("Part 1: Risk level: {0}", riskLevel);
            System.Console.WriteLine("Part 2: Size of three largest basins multiplied: {0}", basins.ElementAt(0).size * basins.ElementAt(1).size * basins.ElementAt(2).size);

       }

    }

}
