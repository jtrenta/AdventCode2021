using System;
using System.Collections.Generic;
using System.Linq;
using static Globals.ProjectConstants;

namespace AdventCode2021
{

    class PolymerRule {
        
        public PolymerRule(string input) {
            this.inputs = input.Split(" -> ",StringSplitOptions.RemoveEmptyEntries)[0];
            this.insert = input.Split(" -> ",StringSplitOptions.RemoveEmptyEntries)[1];
            this.insertChar = this.insert.ToCharArray()[0];
        }

        public string inputs {
            get;
            set;
        }

        public string insert {
            get;
            set;
        }

        public char insertChar {
            get;
            set;
        }

        public string output {
            get { 
                return this.inputs.Insert(1,this.insert); 
            }
        }

        public string[] outputs {
            get {
                return new string[] { this.output.Substring(0,2), this.output.Substring(1,2) };
            }
        }
    }

    class Day14
    {

        public static void D14Main() {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            string inputstring;
            List<PolymerRule> rules = new List<PolymerRule>();
            Dictionary<string, long> pairs = new Dictionary<string, long>();
            Dictionary<string, long> newpairs = new Dictionary<string, long>();
            Dictionary<char, long> elements = new Dictionary<char, long>();
            long Part1Answer=0, Part2Answer=0, numToChange;
            inputstring = System.IO.File.ReadAllText(@".\Input\Day14Input.txt");
            // inputstring = "NNCB\r\n\r\nCH -> B\r\nHH -> N\r\nCB -> H\r\nNH -> C\r\nHB -> C\r\nHC -> B\r\n" +
            //               "HN -> C\r\nNN -> C\r\nBH -> H\r\nNC -> B\r\nNB -> B\r\nBN -> B\r\nBB -> N\r\n" +
            //               "BC -> B\r\nCC -> N\r\nCN -> C";
            string polymer;
            PolymerRule newrule;
            polymer = inputstring.Split("\r\n",StringSplitOptions.RemoveEmptyEntries)[0];
            foreach(string item in inputstring.Replace(polymer, "").Split("\r\n", StringSplitOptions.RemoveEmptyEntries)) {
                newrule = new PolymerRule(item);
                rules.Add(newrule);
                pairs[newrule.inputs] = 0;
                pairs[newrule.outputs[0]] = 0;
                pairs[newrule.outputs[1]] = 0;
                elements[newrule.insertChar] = 0;
            }

            for(int j = 0;j<polymer.Length-1;j++) {
                pairs[rules.Single(r => r.inputs == polymer.Substring(j,2)).outputs[0]]++;
                pairs[rules.Single(r => r.inputs == polymer.Substring(j,2)).outputs[1]]++;
                elements[polymer.ToCharArray()[j]]++;
                elements[rules.Single(r => r.inputs == polymer.Substring(j,2)).insertChar]++;
            }
            
            elements[polymer.ToCharArray()[polymer.Length-1]]++;
            newpairs = new Dictionary<string, long>(pairs);
            
            for(int i = 0;i< 39;i++) {
                foreach(PolymerRule rule in rules) {
                    numToChange = pairs[rule.inputs];
                    newpairs[rule.outputs[0]] += numToChange;
                    newpairs[rule.outputs[1]] += numToChange;
                    newpairs[rule.inputs] -= numToChange;
                    elements[rule.insertChar] += numToChange;
                }
                pairs = new Dictionary<string, long>(newpairs);
                if(i==8) {
                    Part1Answer = elements.Max(e => e.Value) - elements.Min(e => e.Value);
                }
            }

            Part2Answer = elements.Max(e => e.Value) - elements.Min(e => e.Value);
            System.Console.WriteLine("Answer for Part 1: {0}", Part1Answer);
            System.Console.WriteLine("Answer for Part 2: {0}", Part2Answer);
            System.Console.WriteLine("Execution time: {0} ms",watch.ElapsedMilliseconds);
        }
    }
}
