using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2021
{

    class Digit {
        private string raw;
        private int value = -1;
        private bool output;
        private bool initialized = false;

        public Digit(string inputstring, bool isOutput) {
            raw = SortString(inputstring);
            output = isOutput;

            if(inputstring.Length == 3) { value = 7; }
            if(inputstring.Length == 2) { value = 1; }
            if(inputstring.Length == 4) { value = 4; }
            if(inputstring.Length == 7) { value = 8; }
            if(value >= 0) { initialized = true; }
        
        }

        public bool isOutput { get { return output; } }

        public bool isInitialized { get { return initialized; } }

        public string segments { get { return raw; } }

        public int length{ get { return raw.Length; } }

        public int val { get { return value; } }

        public bool isEasy { get { return (value == 1 || value == 4 || value == 7 || value == 8); } }

        private static string SortString(string input)
        {
            char[] characters = input.ToCharArray();
            Array.Sort(characters);
            return new string(characters);
        }

    }

    class Panel {
        private List<Digit> digits;
        private Dictionary<string, int> segmentsToIntMap;
        private Dictionary<int, string> intToSegmentsMap;

        public Panel(List<Digit> inputs) {
            digits = inputs.ToList();
            segmentsToIntMap = new Dictionary<string, int>();
            intToSegmentsMap = new Dictionary<int, string>();

            AddSegmentsToMaps(digits.Find(d => d.length == 2).segments, 1);
            AddSegmentsToMaps(digits.Find(d => d.length == 3).segments, 7);
            AddSegmentsToMaps(digits.Find(d => d.length == 4).segments, 4);
            AddSegmentsToMaps(digits.Find(d => d.length == 7).segments, 8);

            foreach(Digit d1 in digits.Where(d => d.length == 6)) {
                // A 9 is the only digit with 6 segments that has all the same segments as a 4
                if(intToSegmentsMap[4].ToCharArray().Where(c => d1.segments.Contains(c)).Count() == 4) {
                    AddSegmentsToMaps(d1.segments, 9);
                } else 
                // A 6 has exactly 1 segment that a 1 has
                if(d1.segments.Contains(intToSegmentsMap[1][0]) ^ d1.segments.Contains(intToSegmentsMap[1][1])) {
                    AddSegmentsToMaps(d1.segments, 6);
                } else {
                // The remaining 6 segment number is a 0
                    AddSegmentsToMaps(d1.segments, 0);
                }
            }

            foreach(Digit d1 in digits.Where(d => d.length == 5)) {
                // A 5 is the only digit with 5 segments that has 5 of the segments that a 6 has
                if(intToSegmentsMap[6].ToCharArray().Where(c => d1.segments.Contains(c)).Count() == 5) {
                    AddSegmentsToMaps(d1.segments, 5);
                } else 
                // A 3 will match both segments that a 1 has
                if(d1.segments.Contains(intToSegmentsMap[1][0]) && d1.segments.Contains(intToSegmentsMap[1][1])) {
                    AddSegmentsToMaps(d1.segments, 3);
                } else {
                // The remaining 5 segment number is a 2
                    AddSegmentsToMaps(d1.segments, 2);
                }
            }
        }

        private void AddSegmentsToMaps(string key, int value) {
            if(segmentsToIntMap.ContainsKey(key)) {
                if(segmentsToIntMap[key] != value) {
                    System.Console.Write("Error setting key for {0} to {1} - it already exists with a different value ({2})\n", key, value, segmentsToIntMap[key]);
                }
            } else {
                segmentsToIntMap[key] = value;
                intToSegmentsMap[value] = key;
            }
        }

        public int CalcAnswer {
            get {
                return  segmentsToIntMap[digits.ElementAt(10).segments]*1000 +
                        segmentsToIntMap[digits.ElementAt(11).segments]*100 +
                        segmentsToIntMap[digits.ElementAt(12).segments]*10 +
                        segmentsToIntMap[digits.ElementAt(13).segments]*1;
            }
        }

        public List<Digit> GetDigits {
            get {
                return digits.ToList();
            }
        }

    }

    class Day8
    {

        public static void D8Main() {
            string inputstring;
            List<Digit> digitsInPanel;
            List<Panel> panels = new List<Panel>();
            Digit digit;
            Panel panel;
            int nonOutput = 0, easyDigits = 0;

            inputstring = System.IO.File.ReadAllText(@".\Input\Day8Input.txt");
            //inputstring = System.IO.File.ReadAllText(@".\Input\Day8TestInput.txt");
            //inputstring = "acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf";
            foreach(string item in inputstring.Replace(" | ", " ").Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)) {
                digitsInPanel = new List<Digit>();
                foreach(string panelSet in item.Split((' '), StringSplitOptions.RemoveEmptyEntries)) {
                    digit = new Digit(panelSet, nonOutput++ >= 10);
                    digitsInPanel.Add(digit);
                    //if(digit.isOutput) System.Console.Write("{0} ",digit.segment);
                }
                panel = new Panel(digitsInPanel);
                panels.Add(panel);
                //System.Console.Write(": {0}\n", panel.CalcAnswer);
                nonOutput = 0;
            }
            System.Console.WriteLine("Part 1: Easy digits count: {0}", panels.Sum(p => p.GetDigits.Where(d => d.isOutput && d.isEasy).Count()));
            System.Console.WriteLine("Part 2: Panel sum: {0}", panels.Sum(s => s.CalcAnswer));
       }

    }

}
