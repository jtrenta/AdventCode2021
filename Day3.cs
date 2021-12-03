using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2021
{

    class Day3
    {
        private static bool matchesDigit(int value, int index, bool matchOne)
        {
                bool returnVal = matchOne ? (((value >> index) & 1) == 1) : (((value >> index) & 1) != 1);
                return returnVal;
        }

        private static void getGammaEpsilon(List<int> values, int size, out int gamma, out int epsilon) {
                gamma = 0;
                epsilon = 0;
                for(int j = 0;j <= size; j++ ) {
                    if(values.Where(x => matchesDigit(x, j, true)).Count() >= (values.Count()/2.0)) {
                        gamma += 1 << j;
                    } else {
                        epsilon += 1 << j;
                    }
                }
        }

        public static void D3Main() {
            string inputstring;
            inputstring = System.IO.File.ReadAllText(@".\Input\Day3Input.txt");
            List<int> inputs = new List<int>();
            List<int> oxygenList = new List<int>();
            List<int> co2List = new List<int>();
            int checkDigit = 0;
            double checkD = 0.0;
            int gamma = 0, epsilon = 0;
            int size = inputstring.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[0].Length - 1;

            foreach(string item in inputstring.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)) {
                inputs.Add(Convert.ToInt32(item, 2));
            }

            getGammaEpsilon(inputs, size, out gamma, out epsilon);

            System.Console.WriteLine("Part 1: Gamma: {0} Epsilon: {1} Answer: {2}", gamma, epsilon, gamma * epsilon);

            oxygenList = inputs.ToList<int>();
            co2List = inputs.ToList<int>();

            for(int i = size;i >= 0; i-- ) {
                if(oxygenList.Count() > 1) {
                    getGammaEpsilon(oxygenList, size, out gamma, out epsilon);
                    oxygenList = oxygenList.Where(anyItem => matchesDigit(anyItem, i, (matchesDigit(gamma, i, true)))).ToList<int>();
                }
                
                if(co2List.Count() > 1) {
                    getGammaEpsilon(co2List, size, out gamma, out epsilon);
                    co2List = co2List.Where(anyItem => matchesDigit(anyItem, i, (matchesDigit(epsilon, i, true)))).ToList<int>();
                }
            }

            System.Console.WriteLine("Part 1: Oxygen Code: {0} CO2 Code: {1} Answer: {2}", oxygenList.ElementAt(0), co2List.ElementAt(0),  oxygenList.ElementAt(0) *  co2List.ElementAt(0));

       }

    }

}
