using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2021
{
    class D1DepthReading {
        private int depth = -1;
        public D1DepthReading prevDepth;

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

        public D1DepthReading(int iDepth) {
            depth = iDepth;
        }

        public D1DepthReading(string inputstring){
            if(!int.TryParse(inputstring, out depth)) {
                depth = -2;
            }
        }

        public void SetDepthReading(D1DepthReading pDepth) {
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

    class D1P1Sequence {
        private List<D1DepthReading> Readings = new List<D1DepthReading>();
        private D1DepthReading reading;
        private D1DepthReading prevReading = null;

        public D1P1Sequence(string inputstring) {
            string[] stringSeparators = new string[] { "\r\n" };
            foreach(string item in inputstring.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries)) {
                reading = new D1DepthReading(item);
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

    class D1P2Sequence {
        private List<D1DepthReading> Readings = new List<D1DepthReading>();
        private D1DepthReading reading;
        private D1DepthReading prevReading = null;

        public D1P2Sequence(string inputstring) {
            string[] stringSeparators = new string[] { "\r\n" };
            string[] items = inputstring.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
            for(int i = 2;i < items.Count();i++) {
                reading = new D1DepthReading(int.Parse(items[i-2]) + int.Parse(items[i-1]) + int.Parse(items[i]));
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

    class Day1
    {

        public static void D1Main() {
            string inputstring;
            inputstring = System.IO.File.ReadAllText(@".\Input\Day1Input.txt");
            D1P1Sequence sequence = new D1P1Sequence(inputstring);
            System.Console.WriteLine("Part 1: Readings Lower than Previous: {0}", sequence.GetLower);
            D1P2Sequence sequence2 = new D1P2Sequence(inputstring);
            System.Console.WriteLine("Part 2: Readings Lower than Previous: {0}", sequence2.GetLower);
       }

    }

}
