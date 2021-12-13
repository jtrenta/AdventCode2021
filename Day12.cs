using System;
using System.Collections.Generic;
using System.Linq;
using static Globals.ProjectConstants;
using static Globals.Day12Constants;

namespace AdventCode2021
{

    class Test {
        private string iString;
        private int[] iAnswer = { 0, 0 };

        public Test(string input, int a, int a2) {
            iString = input.ToString();
            iAnswer[0] = a;
            iAnswer[1] = a2;
        }

        public string paths { get { return iString; } }
        public int[] answer { get { return iAnswer; } }
    }

    class Node {
        private string name;
        private List<Node> exits;
        private bool size;

        public Node(string iName) {
            name = iName.ToString();
            size = char.IsUpper(name[0]);
            exits = new List<Node>();
        }

        public void AddNode(Node newNode) {
            exits.Add(newNode);
        }

        public string Name { get { return name; } }
        public bool Size { get { return size; } }
        public List<Node> Exits { get { return exits; } }
        public bool isStart { get { return (name == "start"); } }
        public bool isEnd { get { return (name == "end"); } }
    }

    class Day12
    {

        public static void D12Main() {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            string inputstring = System.IO.File.ReadAllText(@".\Input\Day12Input.txt");

            if(TEST) {
                string testinputstring = System.IO.File.ReadAllText(@".\Input\Day12TestInput.txt");
                string[] testinputs = testinputstring.Split(':',StringSplitOptions.RemoveEmptyEntries);
                List<Test> tests = new List<Test>();
                for(int i=0;i < testinputs.Count();i+=3) {
                    tests.Add(new Test(testinputs[i], int.Parse(testinputs[i+1]), int.Parse(testinputs[i+2])));
                }
                for(int i=0;i < tests.Count();i++) {
                    System.Console.WriteLine("Test {0}:",i);
                    if(DEBUG) System.Console.WriteLine("Paths:\n{0}\n",tests[i].paths);
                    System.Console.WriteLine("Part 1 Answer Found:{0}",FindPaths(tests[i].paths, 1));
                    System.Console.WriteLine("Correct Part 1 Answer:{0}",tests[i].answer[0]);
                    System.Console.WriteLine("Part 2 Answer Found:{0}",FindPaths(tests[i].paths, 2));
                    System.Console.WriteLine("Correct Part 2 Answer:{0}",tests[i].answer[1]);
                }
            }

            int part1Answer = FindPaths(inputstring, 1);
            int part2Answer = FindPaths(inputstring, 2);

            watch.Stop();
            System.Console.WriteLine("Part 1 Number of paths: {0}",part1Answer);
            System.Console.WriteLine("Part 2 Number of paths: {0}",part2Answer);
            System.Console.WriteLine("Execution time: {0} ms",watch.ElapsedMilliseconds);

       }

        static int FindPaths(string input, int part) {
            List<Node> nodes = new List<Node>();
            Dictionary<string, Node> getNode = new Dictionary<string, Node>();
            Node currentNode;
            string[] currentLink;
            //Find all unique Nodes and add to list
            foreach(string item in input.Replace("\r\n","-").Split("-",StringSplitOptions.RemoveEmptyEntries)) {
                if(nodes.Where(n => n.Name == item).Count() == 0) {
                    currentNode = new Node(item);
                    nodes.Add(currentNode);
                    getNode.Add(item, currentNode);
                }
            }

            //Create map by adding exits to each Node
            foreach(string item in input.Split("\r\n",StringSplitOptions.RemoveEmptyEntries)) {
                currentLink = item.Split("-",StringSplitOptions.RemoveEmptyEntries);
                getNode[currentLink[0]].Exits.Add(getNode[currentLink[1]]);
                getNode[currentLink[1]].Exits.Add(getNode[currentLink[0]]);
            }

            List<List<Node>> paths = new List<List<Node>>();
            List<Node> currentPath = new List<Node>();

            //Buildpath iteratively builds paths, so we call it for each exit from the starting Node
            foreach(Node n in getNode["start"].Exits) {
                currentPath = new List<Node>();
                currentPath.Add(getNode["start"]);
                BuildPath(paths, currentPath.ToList(), n, part == 1);
            }

            //Return all distinct paths that end at the end
            return paths.Where(p => p.Last().isEnd).ToList().Count();
        }

        //returnVal is the list of finished paths we've generated
        //currentPath is the path we're currently building
        //start is the new Node we're evaluating to add to paths
        //smallCaves is a boolean for part 2, set to true when we've visited a single small cave twice
        static void BuildPath(List<List<Node>> returnVal, List<Node> currentPath, Node start, bool smallCave) {
            List<Node> path = currentPath.ToList();

            //If this node is the end, or all the exits are smallCaves we've been to before
            if(start.isEnd || (start.Exits.All(e => currentPath.Where(c => c.Size == SMALL && e == c).Count() > 0) && smallCave)) {
                //Only add this path to the final list of paths if this is the end
                if(!path.Contains(start) && start.isEnd) { 
                    path.Add(start);
                    returnVal.Add(path);
                    
                    if(DEBUG) {
                        foreach(Node n in path) {
                            System.Console.Write("{0}-",n.Name);
                        }
                        System.Console.Write("\n");
                    }
                }
                return;

            } else {
                //This node isn't an end, so add the node to the path
                path.Add(start);

                //For each exit from the current node, build paths
                //Filter out small caves we've already been to unless it's part 2 and we haven't been to a smallCave twice
                foreach(Node n in start.Exits.Where(s => !s.isStart && (currentPath.Where(c => c.Size == SMALL && s == c).Count() < 1 || !smallCave))) {
                    BuildPath(returnVal, path, n, smallCave || currentPath.Where(c => c.Size == SMALL && n == c).Count() > 0);
                }
            }
            
        }

    }


}
