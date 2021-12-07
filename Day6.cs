using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2021
{

    class LanternfishDay {
        private long numFishThisDay = 1;
        private int daysToSpawn;
        private bool spawned = false;

        public LanternfishDay(int days, long numFish) {
            daysToSpawn = days;
            numFishThisDay = numFish;
        }

        public void Increment(long num) {
            numFishThisDay+= num;
        }

        public long AdvanceTime() {
            if(!spawned) {      //Since we change the daysToSpawn to 8, AdvanceTime will run twice on this day so this flag prevents double spawning
                daysToSpawn--;
                if(daysToSpawn < 0) {
                    daysToSpawn = 8;
                    spawned = true;
                    return numFishThisDay;
                }
            } else {
                spawned = false;
            }
            return 0;
        }

        public bool HasSpawned {
            get {
                return spawned;
            }
        }

        public int daysTillSpawn {
            get {
                return daysToSpawn;
            }
        }

        public long GetNumFish {
            get {
                return numFishThisDay;
            }
        }

    }

    class Lanternfish {
        private int daysToSpawn;

        public Lanternfish(int days = 8) {
            daysToSpawn = days;
        }

        public bool Decrement() {
            daysToSpawn--;
            if(daysToSpawn < 0) {
                daysToSpawn = 6;
                return true;
            }
            return false;
        }

        public int daysTillSpawn {
            get {
                return daysToSpawn;
            }
        }
    }

    class Day6
    {

        public static void D6Main() {
            string inputstring;
            List<Lanternfish> fish = new List<Lanternfish>();
            List<LanternfishDay> fishDays = new List<LanternfishDay>();
            int iLength = 0;
            inputstring = System.IO.File.ReadAllText(@".\Input\Day6Input.txt");
            //inputstring = "3,4,3,1,2";

            foreach(int num in Array.ConvertAll(inputstring.Split(',', StringSplitOptions.RemoveEmptyEntries), s => int.Parse(s))) {
                fish.Add(new Lanternfish(num));
            }

            for(int day = 0;day<80;day++) {
                iLength = fish.Count();
                for(int i = 0;i<iLength;i++) {
                    if(fish.ElementAt(i).Decrement()) {
                        fish.Add(new Lanternfish(8));
                    }
                }
            }

            System.Console.WriteLine("Part 1: Number of fish after 80 days: {0}", fish.Count());

            //Part 2 is same as Part 1, but Part 1 approach is too memory intensive per fish
            //Part 2 is done by storing fish summed by how many days till they spawn
            
            //Initialization
            fish = new List<Lanternfish>();
            foreach(int num in Array.ConvertAll(inputstring.Split(',', StringSplitOptions.RemoveEmptyEntries), s => int.Parse(s))) {
                fish.Add(new Lanternfish(num));
            }
            int numfish = 0;
            for(int i=0;i<9;i++) {
                numfish = fish.Where(f => f.daysTillSpawn == i).Count();
                fishDays.Add(new LanternfishDay(i, numfish));
            }
            long new6dayFish = 0;

            //Run through each day, processing each group of fish once
            for(int day = 0;day<256;day++) {
                for(int i=0;i<9;i++) {
                    foreach(LanternfishDay fd in fishDays.Where(f => f.daysTillSpawn == i)) {
                        new6dayFish += fd.AdvanceTime();
                    }
                }
                fishDays.Find(f => f.daysTillSpawn == 6).Increment(new6dayFish);
                new6dayFish = 0;
            }
            System.Console.WriteLine("Part 2: Number of fish after 256 days: {0}", fishDays.Sum(f => f.GetNumFish));

       }

    }

}
