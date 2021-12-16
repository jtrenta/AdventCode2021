using System;
using System.Collections.Generic;
using System.Linq;
using static Globals.ProjectConstants;

namespace AdventCode2021
{
    class Day16Test {
        public Day16Test(string i, int d, int e = 1) {
            this.input = i;
            this.answer = d;
            this.part = e;
        }
        public string input { get; set; }
        public int answer { get; set; }
        public int part { get; set; }
    }

    class BITS_Message {
        private string unpacked;
        private int version;
        private int type;
        private string message;
        private List<BITS_Message> subPackets;

        public BITS_Message(string input, bool inBinary = false) {
            string tempData = "";
            int subPacketLength = 0;
            int subPacketBits = 0;
            int tempPacketBits = 0;

            if(!inBinary)
                unpacked = String.Join(String.Empty,input.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
            else
                unpacked = input.ToString();

            version = Convert.ToInt32(unpacked.Substring(0,3), 2);
            type = Convert.ToInt32(unpacked.Substring(3,3), 2);

            subPackets = new List<BITS_Message>();

            if(type == 4) {
                int i = 0;

                while(i < rawDataString.Length) {
                    tempData += rawDataString.Substring(i+1, 4);
                    i+=5;
                    if(rawDataCharA[i-5] == '0') {
                        break;
                    }
                }

                this.length = i+6;
                message = Convert.ToString(Convert.ToInt64(tempData,2));
                part1Answer = version;
                part2Answer = Convert.ToInt64(message);

            } else {
                BITS_Message nextMessage;
                int i;
                if(rawDataCharA[0] == '0') 
                    subPacketBits = 16; 
                else 
                    subPacketBits = 12;
                
                i = subPacketBits;
                subPacketLength = Convert.ToInt32(rawDataString.Substring(1, subPacketBits-1),2);
                
                //If length type is 0, then length value means total bytes
                if(rawDataCharA[0] == '0') {
                    while(i < subPacketBits + subPacketLength) {
                        nextMessage = new BITS_Message(rawDataString.Substring(i), true);
                        subPackets.Add(nextMessage);
                        i += nextMessage.length;
                    }
                //Else length type is 1, which means loop length number times
                } else {
                    for(int j = 0;j < subPacketLength;j++) {
                        nextMessage = new BITS_Message(rawDataString.Substring(i), true);
                        subPackets.Add(nextMessage);
                        i += nextMessage.length;
                    }
                }

                part1Answer = version + subPackets.Sum(s => s.part1Answer);
                length = 6 + i;
                
                if(type == 0) {
                    part2Answer = subPackets.Sum(s => s.part2Answer);
                } else if(type == 1) {
                    part2Answer = subPackets.Select(x => x.part2Answer).Aggregate((x, a) => a * x);
                } else if(type == 2) {
                    part2Answer = subPackets.Min(s => s.part2Answer);
                } else if(type == 3) {
                    part2Answer = subPackets.Max(s => s.part2Answer);
                } else if(type == 5) {
                    part2Answer = subPackets[0].part2Answer > subPackets[1].part2Answer ? 1 : 0;
                } else if(type == 6) {
                    part2Answer = subPackets[0].part2Answer < subPackets[1].part2Answer ? 1 : 0;
                } else if(type == 7) {
                    part2Answer = subPackets[0].part2Answer == subPackets[1].part2Answer ? 1 : 0;
                }
            }
        }

        private string rawDataString { 
            get {
                return unpacked.Substring(6);
            }
        }

        private char[] rawDataCharA {
            get {
                return unpacked.Substring(6).ToCharArray();
            }
        }
        
        public int length { get; set; }
        public int part1Answer { get; set; }
        public long part2Answer { get; set; }
    }

    class Day16
    {

        public static void D16Main() {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            List<Day16Test> tests = new List<Day16Test>();
            bool doTests = false;
            string inputstring;
            inputstring = System.IO.File.ReadAllText(@".\Input\Day16Input.txt");
            BITS_Message msg;
            if(doTests) {
                tests.Add(new Day16Test("D2FE28", 6));
                tests.Add(new Day16Test("38006F45291200", 9));
                tests.Add(new Day16Test("EE00D40C823060", 14));
                tests.Add(new Day16Test("8A004A801A8002F478", 16));
                tests.Add(new Day16Test("620080001611562C8802118E34", 12));
                tests.Add(new Day16Test("C0015000016115A2E0802F182340", 23));
                tests.Add(new Day16Test("A0016C880162017C3686B18A3D4780", 31));
                tests.Add(new Day16Test("C200B40A82", 3, 2));
                tests.Add(new Day16Test("04005AC33890", 54, 2));
                tests.Add(new Day16Test("880086C3E88112", 7, 2));
                tests.Add(new Day16Test("CE00C43D881120", 9, 2));
                tests.Add(new Day16Test("D8005AC2A8F0", 1, 2));
                tests.Add(new Day16Test("F600BC2D8F", 0, 2));
                tests.Add(new Day16Test("9C005AC2F8F0", 0, 2));
                tests.Add(new Day16Test("9C0141080250320F1802104A08", 1, 2));
                foreach(Day16Test test in tests.Where(t => t.part == 1)) {
                    msg = new BITS_Message(test.input);
                    System.Console.WriteLine("Test input: {0}",test.input);
                    System.Console.WriteLine("Test expect answer: {0}",test.answer);
                    System.Console.WriteLine("Test got answer: {0}",msg.part1Answer);
                }
                foreach(Day16Test test in tests.Where(t => t.part == 2)) {
                    msg = new BITS_Message(test.input);
                    System.Console.WriteLine("Test input: {0}",test.input);
                    System.Console.WriteLine("Test expect answer: {0}",test.answer);
                    System.Console.WriteLine("Test got answer: {0}",msg.part2Answer);
                }
            }
            msg = new BITS_Message(inputstring);
            System.Console.WriteLine("Part 1 Answer: {0}",msg.part1Answer);
            System.Console.WriteLine("Part 2 Answer: {0}",msg.part2Answer);
            System.Console.WriteLine("Execution time: {0} ms",watch.ElapsedMilliseconds);

       }

    }

}
