using System;

namespace AdventCode2021
{

    class D2Command {
        private string raw;
        private string direction = null;
        private int value = -1;
        private int[] Mult = { 0, 0, 0, 0 };
        private int X = 0, Y = 1, Z = 2, A = 3;

        public bool initialized {
            get {
                return (value >= 0 && direction != null);
            }
        }

        public D2Command(string inputstring) {
            raw = inputstring;
            direction = inputstring.Split(" ")[0];
            value = int.Parse(inputstring.Split(" ")[1]);
            if(direction == "up") Mult[A] = -1 * value;
            if(direction == "down") Mult[A] = 1 * value;
            if(direction == "forward") Mult[Y] = 1 * value;
        }
        
        public void Move1(int[] position) {
            position[Y] = position[Y] + Mult[Y];
            position[Z] = position[Z] + Mult[A];
        }

        public void Move2(int[] position) {
            position[Y] = position[Y] + Mult[Y];
            position[A] = position[A] + Mult[A];
            position[Z] = position[Z] + position[A]*Mult[Y];
        }

    }

    class D2Submarine {
        private int[] position = { 0, 0, 0, 0 };

        public D2Submarine() {
        }

        public void Move(string inputstring, int phase) {
            D2Command command = new D2Command(inputstring);
            if (phase == 1) command.Move1(position);
            if (phase == 2) command.Move2(position);
        }

        public int[] Pos {
            get {
                return position;
            }
        }
    }

    class Day2
    {

        public static void D2Main() {
            int X = 0, Y = 1, Z = 2, A=3;
            string inputstring;
            inputstring = System.IO.File.ReadAllText(@".\Input\Day2Input.txt");
            D2Submarine sub = new D2Submarine();
            string[] stringSeparators = new string[] { "\r\n" };
            foreach(string item in inputstring.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries)) {
                sub.Move(item, 1);
            }
            System.Console.WriteLine("Part 1: Position is : X:{0} Y:{1} Z:{2} Answer:{3}", sub.Pos[X], sub.Pos[Y], sub.Pos[Z], sub.Pos[Y]*sub.Pos[Z]);
            sub = new D2Submarine();
            foreach(string item in inputstring.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries)) {
                sub.Move(item, 2);
            }
            System.Console.WriteLine("Part 2: Position is : X:{0} Y:{1} Z:{2} Answer:{3}", sub.Pos[X], sub.Pos[Y], sub.Pos[Z], sub.Pos[Y]*sub.Pos[Z]);
       }

    }

}
