using System;
using System.Collections.Generic;
using System.Linq;
using static Globals.ProjectConstants;

namespace AdventCode2021
{

    class Day18Test {
        public Day18Test(string i, string a, int e = 1) {
            this.input = i;
            this.correctAnswer = a;
            this.part = e;
            this.answer = new Snailfish(i).ToString();
        }
        public string input { get; set; }
        public string correctAnswer { get; set; }
        public string answer { get; set; }
        public int part { get; set; }

        public bool Test() {
            System.Console.WriteLine("Test input        : {0}",this.input);
            System.Console.WriteLine("Test expect answer: {0}",this.correctAnswer);
            System.Console.WriteLine("Test got answer   : {0}",this.answer);
            return this.correctAnswer == this.answer;
        }
    }


    class Snailfish {
        private dynamic x, y;
        public Snailfish parent = null;
        public Snailfish top = null;
        public int depth;
        private bool modified = true;

        public Snailfish(Snailfish a, Snailfish b) {
            this.x = a;
            this.y = b;
            a.parent = this;
            b.parent = this;
            IncrementDepth();
            depth--;
            SetTop(this);
            Stabilize();
        }

        public Snailfish(int a, int b, int d, Snailfish p = null) {
            this.x = a;
            this.y = b;
            this.parent = p;
            this.depth = d;
            if(this.parent != null) {
                this.top = this.parent.top;
            }
        }

        public Snailfish(string input, int d = 0, Snailfish iTop = null) {
            int index = 0;
            object tempVarX = null;
            object tempVarY = null;
            this.depth = d;
            if(this.depth == 0) { 
                this.top = this;
            } else {
                this.top = iTop;
            }
            index = Parse(input, ref tempVarX);
            if(tempVarX.GetType() == typeof(Snailfish)) {
                this.x = (Snailfish)tempVarX;
                this.x.parent = this;
                //this.top = this.x.parent.top;
            } else {
                this.x = (int)tempVarX;
            }
            if(input[index - 1] == ',') index--;
            Parse(input.Substring(index), ref tempVarY);
            if(tempVarY.GetType() == typeof(Snailfish)) {
                this.y = (Snailfish)tempVarY;
                this.y.parent = this;
                //this.top = this.y.parent.top;
            } else {
                this.y = (int)tempVarY;
            }
            if(depth == 0) {
                //System.Console.WriteLine("Got fish          : {0}", this.ToString());
                Stabilize();
            }
        }

        public long magnitude {
            get {
                long mx, my;
                if(this.x.GetType() == typeof(int)) 
                    mx = 3 * x;
                else {
                    mx = 3 * this.x.magnitude;
                }

                if(this.y.GetType() == typeof(int)) 
                    my = 2 * y;
                else {
                    my = 2 * this.y.magnitude;
                }

                return mx + my;

            }
        }

        public void IncrementDepth() {
            this.depth++;
            if(this.x.GetType() == typeof(Snailfish))
                this.x.IncrementDepth();
            if(this.y.GetType() == typeof(Snailfish))
                this.y.IncrementDepth();
        }

        public void SetTop(Snailfish t) {
            this.top = t;
            if(this.x.GetType() == typeof(Snailfish))
                this.x.SetTop(t);
            if(this.y.GetType() == typeof(Snailfish))
                this.y.SetTop(t);
        }

        private int Parse(string input, ref object refObj) {
            string tempString;
            int index = 1;
            if(input[index] == '[') {
                int brackets = 1;
                index++;
                while(brackets >= 1 || index == 2) {
                    if(input[index] == '[') brackets++;
                    else if(input[index] == ']') brackets--;
                    index++;
                }
                refObj = new Snailfish(input.Substring(1, index - 1), this.depth+1, this.top);
                index+= 1;
            } else {
                if(input.Substring(1,input.Length - 1).Split(',',StringSplitOptions.RemoveEmptyEntries).Count() > 1)
                    tempString = input.Substring(1,input.Length - 1).Split(',',StringSplitOptions.RemoveEmptyEntries)[0];
                else 
                    tempString = input.Substring(0,input.Length - 1).Split(']',StringSplitOptions.RemoveEmptyEntries)[0];
                if(tempString[0] == ',') 
                    tempString = tempString.Substring(1);
                refObj = (int)int.Parse(tempString);
                index += tempString.Length + 1;
            }
            return index;
        }

        public override string ToString() {
            return "[" + x.ToString() + "," + y.ToString() + "]";
        }

        private void Stabilize() {
            bool doExplode = true;
            while(doExplode) doExplode = this.top.Explode();
            Snailfish doSplit = CheckSplit();
            while(doSplit != null) {
                doSplit.Split();
                modified = false;
                doSplit = CheckSplit();
            }
        }

        public bool Split() {
            bool doExplode = true;
            modified = false;
            while(doExplode)  {
                doExplode = this.top.Explode();
            }
            if(this.x.GetType() == typeof(int) && !modified && this == top.CheckSplit()) {
                if(this.x > 9) {
                    int tempVar = this.x / 2;
                    Snailfish newFish = new Snailfish(tempVar, this.x - tempVar, this.depth + 1, this);
                    this.x = newFish;
                    modified = true;
                }
            } else if(this.x.GetType() == typeof(Snailfish) && !modified){
                this.x.Split();
            } 
            if(this.y.GetType() == typeof(int) && !modified && this == top.CheckSplit()) {
                if(this.y > 9) {
                    int tempVar = this.y / 2;
                    Snailfish newFish = new Snailfish(tempVar, this.y - tempVar, this.depth + 1, this);
                    this.y = newFish;
                    modified = true;
                }
            } else if(this.y.GetType() == typeof(Snailfish) && !modified){
                this.y.Split();
            } 
            if(modified) {
                // System.Console.WriteLine("splitted:{0}", top.ToString());
                // if(top.ToString() == "[[[[7,7],[7,8]],[[9,5],[8,0]]],[[[9,10],20],[8,[9,0]]]]"){
                //     System.Console.WriteLine("Here");
                // }
                doExplode = true;
                while(doExplode && modified)  {
                    doExplode = this.top.Explode();
                }
            }
            return false;
        }

        private Snailfish CheckSplit() {
            Snailfish returnVal = null;
            if(this.x.GetType() == typeof(int)) {
                if(this.x > 9)
                    return this;
            } else {
                returnVal = this.x.CheckSplit();
            }
            if(returnVal == null ) {
                if(this.y.GetType() == typeof(int)) {
                    if(this.y > 9)
                        return this;
                } else {
                    if(returnVal == null) return this.y.CheckSplit();
                    else return returnVal;
                }
            }
            return returnVal;
        }

        public bool isFishInTree(Snailfish searchFish) {
            bool returnVal = false;
            if(this.x.GetType() == typeof(Snailfish)) {
                if(searchFish == this.x) 
                    return true;
                else 
                    returnVal = this.x.isFishInTree(searchFish);
            } 
            if(!returnVal && this.y.GetType() == typeof(Snailfish)) {
                if(searchFish == this.y) 
                    return true;
                else 
                    return returnVal || this.y.isFishInTree(searchFish);
            } else
                return false;
        }

        public bool isFishInX(Snailfish searchFish) {
            if(this.x.GetType() == typeof(Snailfish)) {
                if(searchFish == this.x) 
                    return true;
                else 
                    return this.x.isFishInX(searchFish);
            } else 
                return false;
        }

        public bool isFishInY(Snailfish searchFish) {
            if(this.y.GetType() == typeof(Snailfish)) {
                if(searchFish == this.y) 
                    return true;
                else 
                    return this.y.isFishInY(searchFish);
            } else 
                return false;
        }

        public Snailfish FindLeftNumber(Snailfish source, bool direction, int addVal) {
            if(direction) {
                if(this.x.GetType() == typeof(int)) {
                    this.x += addVal;
                    return this;
                } else if(!this.x.isFishInX(source) && this.x != source) {
                    return this.x.FindLeftNumber(source, !direction, addVal);
                } else if(this != this.top) {
                    return this.parent.FindLeftNumber(source, direction, addVal);
                } else if(top.isFishInY(source)) {
                    return this.x.FindLeftNumber(source, !direction, addVal);
                } else if(top.y.GetType() != typeof(int) && top.y.isFishInTree(source)) {
                    return top.x.FindRightNumber(source, !direction, addVal);
                } else {
                    return null;
                }
            } else {
                if(this.y.GetType() == typeof(int)) {
                    this.y += addVal;
                    return this;
                } else {
                    //return this.x.FindLeftNumber(source, direction, addVal);
                    return this.y.FindLeftNumber(source, direction, addVal);
                }
            }
        }


        public Snailfish FindRightNumber(Snailfish source, bool direction, int addVal) {
            if(direction) {
                if(this.y.GetType() == typeof(int)) {
                    this.y += addVal;
                    return this;
                } else if(!this.y.isFishInY(source) && this.y != source) {
                    return this.y.FindRightNumber(source, !direction, addVal);
                } else if(this != this.top) {
                    return this.parent.FindRightNumber(source, direction, addVal);
                } else if(top.isFishInX(source)) {
                    return this.y.FindRightNumber(source, !direction, addVal);
                } else if(top.x.GetType() != typeof(int) && top.x.isFishInTree(source)) {
                    return top.y.FindLeftNumber(source, direction, addVal);
                } else {
                    return null;
                }
            } else {
                if(this.x.GetType() == typeof(int)) {
                    this.x += addVal;
                    return this;
                } else {
                    //return this.y.FindRightNumber(source, direction, addVal);
                    return this.x.FindRightNumber(source, direction, addVal);
                }
            }
        }


        public bool Explode() {
            bool returnVal = false;
            if(this.x.GetType() == typeof(Snailfish) && this.y.GetType() == typeof(Snailfish)){
                returnVal = this.x.Explode();
                if(returnVal) return true;
                if(!returnVal) return this.y.Explode();
            } else if(this.x.GetType() == typeof(Snailfish) && this.y.GetType() != typeof(Snailfish)){
                return this.x.Explode();
            } else if(this.y.GetType() == typeof(Snailfish)){
                return this.y.Explode();
            }
            if(!returnVal && depth >= 4) {
                Snailfish LeftFish = this.parent.FindLeftNumber(this, true, this.x);
                Snailfish RightFish = this.parent.FindRightNumber(this, true, this.y);
                if(LeftFish == null) this.parent.x = 0;
                if(RightFish == null) this.parent.y = 0;
                if(LeftFish != null && RightFish != null) {
                    if(this.parent.x.GetType() == typeof(Snailfish)){
                        if(this.parent.x == this) {
                            this.parent.x = 0;
                        }
                    } 
                    if(this.parent.y.GetType() == typeof(Snailfish)){
                        if(this.parent.y == this) {
                            this.parent.y = 0;
                        }
                    } 
                }
                //System.Console.WriteLine("exploded:{0}", top.ToString());
                // if(top.ToString() == "[[[[7,0],[7,19]],[[0,11],15]],[10,[[0,21],[0,0]]]]") {
                //     System.Console.WriteLine("Here");
                // }
                return true;
            }
            return false;
        }

    }

    class Day18
    {

        public static void D18Main() {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            string inputstring;
            bool doTests = true;
            List<Day18Test> tests = new List<Day18Test>();
             if(doTests) {
                new Day18Test("[[[[[9,8],1],2],3],4]", "[[[[0,9],2],3],4]").Test();
                new Day18Test("[7,[6,[5,[4,[3,2]]]]]", "[7,[6,[5,[7,0]]]]").Test();
                new Day18Test("[[6,[5,[4,[3,2]]]],1]", "[[6,[5,[7,0]]],3]").Test();
                new Day18Test("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[7,0]]]]").Test();
                new Day18Test("[[[[[4,3],4],4],[7,[[8,4],9]]],[1,1]]", "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]").Test();
                //[15,[0,13]]],[1,1]
             }
            inputstring = System.IO.File.ReadAllText(@".\Input\Day18Input.txt");
            //inputstring = System.IO.File.ReadAllText(@".\Input\Day18TestInput.txt");
            //inputstring = "[1,1]\r\n[2,2]\r\n[3,3]\r\n[4,4]\r\n[5,5]\r\n[6,6]";
            Snailfish answer;
            string[] inputs = inputstring.Split("\r\n",StringSplitOptions.RemoveEmptyEntries);
            answer = new Snailfish(inputs[0]);
            Snailfish tempFish1, tempFish2, tempFish3;
            long part2Answer = 0;
            for(int i = 0;i<inputs.Count();i++) {
                for(int j=0;j<inputs.Count();j++) {
                    if(i != j) {
                        tempFish1 = new Snailfish(inputs[i]);
                        tempFish2 = new Snailfish(inputs[j]);
                        tempFish3 = new Snailfish(tempFish2, tempFish1);
                        if(tempFish3.magnitude > part2Answer) part2Answer = tempFish3.magnitude;
                        tempFish3 = new Snailfish(tempFish1, tempFish2);
                        if(tempFish3.magnitude > part2Answer) part2Answer = tempFish3.magnitude;
                    }
                }
                tempFish1 = new Snailfish(inputs[i]);
                tempFish2 = new Snailfish(answer.ToString());
                answer = new Snailfish(tempFish2, tempFish1);
                //System.Console.WriteLine("Part 1 Answer: {0}", answer.ToString());
            }
            System.Console.WriteLine("Part 1 Answer: {0}", answer.magnitude);
            System.Console.WriteLine("Part 2 Answer: {0}", part2Answer);
            System.Console.WriteLine("Execution time: {0} ms",watch.ElapsedMilliseconds);

       }

    }

}
