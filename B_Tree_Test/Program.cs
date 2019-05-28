using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace B_Tree_Test
{
    class Program
    {
        static long comparisons = 0;
        static void Main(string[] args)
        {
            //I've never done B Trees before and have no idea what I'm doing!
            Console.WriteLine("Let's start this B Tree thing!");

            //why use b tree?
            // keeps keys in sorted order for sequential traversing
            // uses a hierarchical index to minimize the number of disk reads
            // uses partially full blocks to speed insertions and deletions
            // keeps the index balanced with a recursive algorithm


            //Ok - so it looks like it's an accumulation of key/value pairs...
            //It must create objects of itself


            //TODO:
            //Let's see how fast we can make our traversals
            //1. get a basic addition/deletion set up
            //2. set up testing and log two things:
            //      a. how long it takes on average (and max)
            //      b. how many comparisons we make on average (and max)


            var numbers_count = 10000000;
            var iterations_count = 1;

            Console.WriteLine("Press Key to Start!");
            Console.ReadKey();

            //weird - looks like they're close to even - struct might be slightly faster at this volume
            //could test at a much larger count of numbers... but don't want to take the time
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("Creating random numbers");
                var rnd = new Random();
                //create random numbers for each iteration - no duplicates
                var randomNumbers = new HashSet<int>();
                while (randomNumbers.Count < numbers_count)
                {
                    //generate random value to store
                    randomNumbers.Add(rnd.Next());
                }
                //keep it in a list so each method is exactly the same (numbers in same order)
                var randNumList = randomNumbers.ToList();

                Thread.Sleep(1000); //give it one second to settle

                Test_B_Tree_Struct(randNumList, iterations_count);
                Thread.Sleep(1000); //give it one second to settle
                Test_B_Tree_Object(randNumList, iterations_count);
            }

            Console.WriteLine("Finished");
            Console.ReadKey();
        }

        private static void Test_B_Tree_Struct(List<int> randomNumbers, int num_of_iterations)
        {
            Console.WriteLine();
            Console.Write("Starting Struct... ");
            Stopwatch sw = new Stopwatch();
            HashSet<long> comparison_set = new HashSet<long>();
            HashSet<long> milliseconds_set = new HashSet<long>();

            //run XX tests and find average results
            for (int i = 0; i < num_of_iterations; i++)
            {
                sw.Start();

                //now store numbers in B Tree
                BinaryNode b_tree = new BinaryNode();
                foreach (var num in randomNumbers)
                {
                    b_tree.Add(num);
                }

                sw.Stop();

                //gather information for info later
                comparison_set.Add(comparisons);
                milliseconds_set.Add(sw.ElapsedMilliseconds);

                //reset local values
                comparisons = 0;
                sw.Reset();
            }

            
            Console.WriteLine($"Finished!");
            Console.WriteLine($"Average Adding Nodes in {milliseconds_set.Average()}ms.");
            Console.WriteLine($"Average of {comparison_set.Average()} comparisons.");
            Console.WriteLine();
            //Console.WriteLine($"Max Adding Nodes in {milliseconds_set.Max()}ms.");
            //Console.WriteLine($"Max of {comparison_set.Max()} comparisons.");
            //Console.WriteLine();
        }

        private static void Test_B_Tree_Object(List<int> randomNumbers, int num_of_iterations)
        {
            Console.WriteLine();
            Console.Write("Starting Object... ");
            Stopwatch sw = new Stopwatch();
            HashSet<long> comparison_set = new HashSet<long>();
            HashSet<long> milliseconds_set = new HashSet<long>();

            //run XX tests and find average results
            for (int i = 0; i < num_of_iterations; i++)
            {
                sw.Start();

                //now store numbers in B Tree
                BinaryNodeOBJ b_tree = new BinaryNodeOBJ();
                foreach (var num in randomNumbers)
                {
                    b_tree.Add(num);
                }

                sw.Stop();

                //gather information for info later
                comparison_set.Add(comparisons);
                milliseconds_set.Add(sw.ElapsedMilliseconds);

                //reset local values
                comparisons = 0;
                sw.Reset();
            }
            
            Console.WriteLine($"Finished!");
            Console.WriteLine($"Average Adding Nodes in {milliseconds_set.Average()}ms.");
            Console.WriteLine($"Average of {comparison_set.Average()} comparisons.");
            Console.WriteLine();
            //Console.WriteLine($"Max Adding Nodes in {milliseconds_set.Max()}ms.");
            //Console.WriteLine($"Max of {comparison_set.Max()} comparisons.");
            //Console.WriteLine();
        }

        struct BinaryNode
        {
            public int Key;
            public BinaryNode[] Values;
            
            public void Add(int num) //return number of comparisons
            {
                //Create first Node
                if (Key == 0)
                {
                    Key = num;
                    return; //set the key - now break out
                }
                else
                {
                    if (Values == null)
                        Values = new BinaryNode[2]; //create values if not created yet

                    if (num < Key)
                        Values[0].Add(num); //less than
                    else
                        Values[1].Add(num); //greater than

                    comparisons++;
                }
            }
        }


        class BinaryNodeOBJ
        {
            public int Key;
            public BinaryNode[] Values;

            public void Add(int num) //return number of comparisons
            {
                //Create first Node
                if (Key == 0)
                {
                    Key = num;
                    return; //set the key - now break out
                }
                else
                {
                    if (Values == null)
                        Values = new BinaryNode[2]; //create values if not created yet

                    if (num < Key)
                        Values[0].Add(num); //less than
                    else
                        Values[1].Add(num); //greater than

                    comparisons++;
                }
            }
        }

        //NOT USED
        //good ol' stack overflow suggests this: 
        //https://stackoverflow.com/questions/9132944/how-can-a-b-tree-node-be-represented
        //however, someone else mentioned to use structs in an array - optimization purposes
        //though - lists are more flexible 
        class Node
        {
            public int Key { get; set; }
            public bool IsRoot { get; set; }
            public bool IsLeaf { get; set; }
            private List<Node> children = new List<Node>();
            public List<Node> Children { get { return this.children; } }
        }
    }
}
