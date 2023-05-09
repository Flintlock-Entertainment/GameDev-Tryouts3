using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IntPair = System.ValueTuple<int, int>;

namespace TestAStar
{
    class IntGraph : IGraph<int>
    {  // IGraph is defined in the file ./IGraph.cs (a copy is found in Assets/Scripts/0-bfs/IGraph.cs)
        public IEnumerable<int> Neighbors(int node)
        {
            yield return node + 1;
            yield return node - 1;
        }
    }

    class IntPairGraph : IGraph<IntPair>
    {
        public IEnumerable<IntPair> Neighbors(IntPair node)
        {
            yield return (node.Item1, node.Item2 + 1);
            yield return (node.Item1, node.Item2 - 1);
            yield return (node.Item1 + 1, node.Item2);
            yield return (node.Item1 - 1, node.Item2);
            //yield return (node.Item1 - 1, node.Item2+1);
        }
    }

    class Test
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start BFS Test");

            var intGraph = new IntGraph();
            var path = AStar.GetPath(intGraph, 90, 95, IntGraphDistance, IntGraphDistance);
            var pathString = string.Join(",", path.ToArray());
            Console.WriteLine("path is: " + pathString);
            //Debug.Assert(pathString == "90,91,92,93,94,95");
            path = AStar.GetPath(intGraph, 85, 80, IntGraphDistance, IntGraphDistance);
            pathString = string.Join(",", path.ToArray());
            Console.WriteLine("path is: " + pathString);
            //Debug.Assert(pathString == "85,84,83,82,81,80");

            var intPairGraph = new IntPairGraph();
            var path2 = AStar.GetPath(intPairGraph, (9, 5), (7, 6), PairGraphDistance, PairGraphDistance);
            pathString = string.Join(",", path2.ToArray());
            Console.WriteLine("path is: " + pathString);
            //Debug.Assert(pathString == "(9, 5),(9, 6),(8, 6),(7, 6)");
            //Debug.Assert(path2.Count == 4);

            // Here we should get an empty path because of maxiterations:
            int maxiterations = 1000;
            path2 = AStar.GetPath(intPairGraph, (9, 5), (-7, -6), PairGraphDistance, PairGraphDistance ,maxiterations);
            pathString = string.Join(",", path2.ToArray());
            Console.WriteLine("path is: " + pathString);
            //Debug.Assert(pathString == "");

            Console.WriteLine("End AStar Test");
        }
        public static float IntGraphDistance(int x, int y)
        {
            return Math.Abs(x - y);
        }

        public static float PairGraphDistance(IntPair point1, IntPair point2)
        {
            float dx = Math.Abs(point1.Item1 - point2.Item1);
            float dy = Math.Abs(point1.Item2 - point2.Item2);
            return (float)Math.Sqrt((dx * dx) + (dy * dy));
        }
    }
}
