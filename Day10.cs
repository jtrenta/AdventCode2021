using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2021
{

    class Day10
    {

        public static void D10Main() {
            string inputstring;
            inputstring = System.IO.File.ReadAllText(@".\Input\Day10Input.txt");
            // inputstring =   "[({(<(())[]>[[{[]{<()<>>\r\n" + 
            //                 "[(()[<>])]({[<{<<[]>>(\r\n" +
            //                 "{([(<{}[<>[]}>{[]{[(<()>\r\n" +
            //                 "(((({<>}<{<{<>}{[]{[]{}\r\n" +
            //                 "[[<[([]))<([[{}[[()]]]\r\n" +
            //                 "[{[{({}]{}}([{[{{{}}([]\r\n" +
            //                 "{<[[]]>}<{[{[{[]{()[[[]\r\n" +
            //                 "[<(<(<(<{}))><([]([]()\r\n" +
            //                 "<{([([[(<>()){}]>(<<{{\r\n" +
            //                 "<{([{{}}[<[[[<>{}]]]>[]]\r\n";

            Dictionary<char, char> MatchingCloser = new Dictionary<char, char>();
            Dictionary<char, int> syntaxScore = new Dictionary<char, int>();
            Dictionary<char, int> autoCompleteScore = new Dictionary<char, int>();
            Dictionary<char, int> openBrackets = new Dictionary<char, int>();

            syntaxScore.Add(')', 3);
            syntaxScore.Add(']', 57);
            syntaxScore.Add('}', 1197);
            syntaxScore.Add('>', 25137);
            syntaxScore.Add('\u0000', 0);

            autoCompleteScore.Add(')', 1);
            autoCompleteScore.Add(']', 2);
            autoCompleteScore.Add('}', 3);
            autoCompleteScore.Add('>', 4);
            autoCompleteScore.Add('\u0000', 0);

            MatchingCloser.Add('(',')');
            MatchingCloser.Add('[',']');
            MatchingCloser.Add('{','}');
            MatchingCloser.Add('<','>');

            int finalSyntaxScore = 0, currentSyntaxScore;
            long currentAutoCompleteScore;
            List<long> autoCompleteScores = new List<long>();
            string workString;
            int prevLength = 0;

            foreach(string item in inputstring.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)) {
                workString = item.ToString();
                //Iteratively remove all "primitive" chunks until none remain
                while(prevLength != workString.Length) {
                    prevLength = workString.Length;
                    workString = workString.Replace("()","").Replace("[]","").Replace("{}","").Replace("<>","");
                }
                //Remaining string is empty for valid lines, 
                //corrupted if it has a closing bracket
                //incomplete if no closing bracket found
                if(workString.Length > 1) {
                    //For corrupted strings, get the first closing bracket and add the score
                    currentSyntaxScore = syntaxScore[workString.FirstOrDefault(s => syntaxScore.ContainsKey(s))];
                    finalSyntaxScore += currentSyntaxScore;
                    //For incomplete strings (no closing brackets left)
                    if(currentSyntaxScore == 0) {
                        //reverse the order of the remaining open brackets and score them
                        currentAutoCompleteScore = 0;
                        foreach(char c in workString.Reverse()) {
                            currentAutoCompleteScore = currentAutoCompleteScore * 5 + autoCompleteScore[MatchingCloser[c]];
                        }
                        autoCompleteScores.Add(currentAutoCompleteScore);
                    }
                }
                prevLength = 0;
            }
            autoCompleteScores.Sort();
            System.Console.WriteLine("Part 1: Syntax error score: {0}", finalSyntaxScore);
            System.Console.WriteLine("Part 1: Auto complete score: {0}", autoCompleteScores.ElementAt(autoCompleteScores.Count()/2));

       }

    }

}
