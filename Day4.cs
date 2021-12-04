using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2021
{

    class BingoSet {
        private int[] vals = { 0, 0, 0, 0, 0 };
    }

    class BingoBoard {
        private string raw;
        private int[] board = { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        private List<int> called = new List<int>();
        private bool won = false;

        public BingoBoard(string inputstring) {
            raw = inputstring.ToString();
            raw = raw.Replace("  ", " ").Replace(" ",",").Replace(",,",",");
            int i = 0;
            foreach(string item in raw.Split(",")) {
                board[i++] = int.Parse(item);
            }
        }

        public int addCall(int num) {
            int[,] winners = { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } };
            called.Add(num);
            for(int i = 0;i < 5;i++) {
                for(int j = 0;j < 5;j++) {
                    if(winners[0,i] >= 0) { // Check rows
                        if(!called.Contains(board[i + j*5])) {
                            winners[0,i] = -1;
                        }
                    }
                    if(winners[1,i] >= 0) { // Check cols
                        if(!called.Contains(board[j + i*5])) {
                            winners[1,i] = -1;
                        }
                    }
                }
                if(winners[0,i] == 0 || winners[1,i] == 0) {
                    won = true;
                    break;
                }
            }
            int returnval = 0;
            if(won) {
                foreach(int n in board) {
                    if(!called.Contains(n)) { returnval += n; }
                }
            }
            return returnval * num;
        }

        public bool hasWon {
            get {
                return won;
            }
        }

        public void reset() {
            called = new List<int>();
            won = false;
        }

    }

    class Day4
    {

        public static void D4Main() {
            string callstring, inputstring;
            string[] boardstrings;
            string boardstring;
            int returncall = -1, returnval = 0;
            List<BingoBoard> boards = new List<BingoBoard>();
            inputstring = System.IO.File.ReadAllText(@".\Input\Day4Input.txt");
            callstring = inputstring.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[0];
            int[] calls = Array.ConvertAll(callstring.Split(","), int.Parse);
            boardstrings = inputstring.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();
            for(int i = 0;i < boardstrings.Length / 5;i++) {
                boardstring = string.Join(",", boardstrings.Skip(i*5).Take(5)).Trim();
                boards.Add(new BingoBoard(boardstring));
            }
            foreach(int call in calls) {
                foreach(BingoBoard board in boards) {
                    returnval = board.addCall(call);
                    if(returnval > 0) {
                        returncall = call;
                        break;
                    }
                }
                if(returnval > 0) {
                    break;
                }
            }
            System.Console.WriteLine("Part 1: First winning call is {0}, Top Score is {1}", returncall, returnval);

            boards.ForEach(x => x.reset());
            returnval = 0;
            returncall = -1;
            int wonBoards = 0;
            foreach(int call in calls) {
                foreach(BingoBoard board in boards) {
                    if(!board.hasWon) {
                        returnval = board.addCall(call);
                    }
                    if(returnval > 0) {
                        wonBoards++;
                        if(wonBoards == boards.Count()) {
                            returncall = call;
                            break;
                        } else {
                            returnval = 0;
                        }
                    }
                }
                if(wonBoards == boards.Count()) {
                    break;
                } 
            }
            System.Console.WriteLine("Part 2: Final winning call is {0}, Bottom Score is {1}", returncall, returnval);

       }

    }

}
