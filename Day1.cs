using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2021Day1
{
    class DepthReading {
        private int depth = -1;
        public DepthReading prevDepth;

        public bool initialized {
            get {
                return (depth != -1);
            }
        }

        public bool invalid {
            get {
                return (depth < 0);
            }
        }

        public DepthReading(int iDepth) {
            depth = iDepth;
        }

        public DepthReading(string inputstring){
            if(!int.TryParse(inputstring, out depth)) {
                depth = -2;
            }
        }

        public void SetDepthReading(DepthReading pDepth) {
            prevDepth = pDepth;
        }

        public int GetDepth {
            get {
                return depth;
            }
        }

        public bool Lower {
            get {
                return depth > prevDepth.GetDepth;
            }
        }

        public bool Higher {
            get {
                return depth < prevDepth.GetDepth;
            }
        }

    }

    class Sequence1 {
        private List<DepthReading> Readings = new List<DepthReading>();
        private DepthReading reading;
        private DepthReading prevReading = null;

        public Sequence1(string inputstring) {
            string[] stringSeparators = new string[] { "\r\n" };
            foreach(string item in inputstring.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries)) {
                reading = new DepthReading(item);
                if(prevReading == null) {
                    prevReading = reading;
                } 
                reading.SetDepthReading(prevReading);
                prevReading = reading;
                Readings.Add(reading);
            }
        }

        public int GetLower {
            get {
                return Readings.Where(r => r.Lower == true).Count();
            }
        }

    }

    class Sequence2 {
        private List<DepthReading> Readings = new List<DepthReading>();
        private DepthReading reading;
        private DepthReading prevReading = null;

        public Sequence2(string inputstring) {
            string[] stringSeparators = new string[] { "\r\n" };
            string[] items = inputstring.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
            for(int i = 2;i < items.Count();i++) {
                reading = new DepthReading(int.Parse(items[i-2]) + int.Parse(items[i-1]) + int.Parse(items[i]));
                if(prevReading == null) {
                    prevReading = reading;
                } 
                reading.SetDepthReading(prevReading);
                prevReading = reading;
                Readings.Add(reading);
            }
        }

        public int GetLower {
            get {
                return Readings.Where(r => r.Lower == true).Count();
            }
        }

    }

    class Program
    {

        public static void DoDay1() {
            string inputstring;
            inputstring = System.IO.File.ReadAllText(@".\Input\DayOneInput.txt");
            Sequence1 sequence = new Sequence1(inputstring);
            System.Console.WriteLine("Part 1: Readings Lower than Previous: {0}", sequence.GetLower);
            Sequence2 sequence2 = new Sequence2(inputstring);
            System.Console.WriteLine("Part 2: Readings Lower than Previous: {0}", sequence2.GetLower);
       }

    }

}
