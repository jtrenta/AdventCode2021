using System;
using System.Collections.Generic;
using System.Linq;
using static Globals.ProjectConstants;

namespace AdventCode2021
{
    
    class Day17
    {

        public static void D17Main() {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            GridPoint start = new GridPoint(0, 0);
            GridPoint[] targetArea = new GridPoint[2] { new GridPoint(0, 0), new GridPoint(0, 0)};
            int maxCurrentYVal = 0, part1YMax = int.MinValue;
            bool hitTarget, missedTarget;
            List<GridPoint> points = new List<GridPoint>();
            List<int> xVelocities = new List<int>();
            GridPoint currentPosition = new GridPoint(0,0), currentVelocity = new GridPoint(0,0);
            int part2Count;
            string inputstring;
            inputstring = "target area: x=253..280, y=-73..-46";
            //inputstring = "target area: x=20..30, y=-10..-5";
            foreach(string item in inputstring.Replace("target area: ", "").Split(", ",StringSplitOptions.RemoveEmptyEntries)) {
                if(item.Split("=")[0] == "x") {
                    targetArea[0].x = int.Parse(item.Split("=")[1].Split("..")[0]) > int.Parse(item.Split("=")[1].Split("..")[1]) ? int.Parse(item.Split("=")[1].Split("..")[1]) : int.Parse(item.Split("=")[1].Split("..")[0]);
                    targetArea[1].x = int.Parse(item.Split("=")[1].Split("..")[0]) > int.Parse(item.Split("=")[1].Split("..")[1]) ? int.Parse(item.Split("=")[1].Split("..")[0]) : int.Parse(item.Split("=")[1].Split("..")[1]);
                } else {
                    targetArea[0].y = int.Parse(item.Split("=")[1].Split("..")[0]) > int.Parse(item.Split("=")[1].Split("..")[1]) ? int.Parse(item.Split("=")[1].Split("..")[1]) : int.Parse(item.Split("=")[1].Split("..")[0]);
                    targetArea[1].y = int.Parse(item.Split("=")[1].Split("..")[0]) > int.Parse(item.Split("=")[1].Split("..")[1]) ? int.Parse(item.Split("=")[1].Split("..")[0]) : int.Parse(item.Split("=")[1].Split("..")[1]);
                }
            }
            //Get valid X Velocities so we don't loop through all the Y Velocities for invalid X Velocities
            for(int i = targetArea[1].x;i > 0;i--) {
                currentVelocity.x = i;
                currentPosition.x = start.x;
                int t = 1;
                while(currentPosition.x < targetArea[1].x && currentVelocity.x > 0) {
                    currentPosition.x += currentVelocity.x;
                    if(currentVelocity.x > 0) currentVelocity.x--;
                    if(currentPosition.x >= targetArea[0].x && currentPosition.x <= targetArea[1].x) {
                        xVelocities.Add(i);
                        if(currentVelocity.x == 0) break;
                    }
                    t++;
                }
            }
            foreach(int XVel in xVelocities) {
                for(int i = targetArea[0].y;i<11000;i++) {
                    hitTarget = missedTarget = false;
                    currentVelocity.y = i;
                    currentPosition.y = start.y;
                    currentVelocity.x = XVel;
                    currentPosition.x = start.x;
                    maxCurrentYVal = start.y;
                    for(int t = 1;!missedTarget;t++) {
                        currentPosition.y += currentVelocity.y;
                        currentVelocity.y--;
                        currentPosition.x += currentVelocity.x;
                        if(currentVelocity.x > 0) currentVelocity.x--;
                        if(currentVelocity.y == 0) maxCurrentYVal = currentPosition.y;
                        if(currentPosition.y >= targetArea[0].y && currentPosition.y <= targetArea[1].y && currentPosition.x >= targetArea[0].x && currentPosition.x <= targetArea[1].x) {
                            if(maxCurrentYVal > part1YMax) {
                                part1YMax = maxCurrentYVal;
                            }
                            if(!hitTarget) {
                                points.Add(new GridPoint(XVel, i));
                            }
                            hitTarget = true;
                        }
                        if(currentPosition.y < targetArea[0].y && currentVelocity.y < 0 || currentPosition.x > targetArea[1].x) missedTarget = true;
                    }
                }
            }
            part2Count = points.Distinct(new GridPointComparer()).Count();
            System.Console.WriteLine("Part 1 Answer: {0}", part1YMax);
            System.Console.WriteLine("Part 2 Answer: {0}", part2Count);
            System.Console.WriteLine("Execution time: {0} ms",watch.ElapsedMilliseconds);

       }

    }

}
