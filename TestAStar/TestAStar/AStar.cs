using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KeyPair = System.ValueTuple<int, float>;

namespace TestAStar
{
    class AStar
    {
        public static void FindPath<NodeType>(
            IGraph<NodeType> graph,
            NodeType startNode, NodeType endNode,
            List<NodeType> outputPath, Func<NodeType, NodeType, float> Distance, Func<NodeType, NodeType, float> Heuristic, int maxiterations = 1000)
        {
            
            ProirityQueue<NodeWrapper<NodeType>> openQueue = new ProirityQueue<NodeWrapper<NodeType>>();
            HashSet<NodeType> openSet = new HashSet<NodeType>();
            Dictionary<NodeType, NodeType> previous = new Dictionary<NodeType, NodeType>();
            NodeWrapper<NodeType> start = new NodeWrapper<NodeType>(startNode, 0 ,0);
            openQueue.Enqueue(start, 0.00001f);
            openSet.Add(startNode);
            int i; for (i = 0; i < maxiterations; ++i)
            { // After maxiterations, stop and return an empty path
                if (openQueue.Count == 0)
                {
                    break;
                }
                else
                {
                    var searchFocus = openQueue.Dequeue();

                    if (searchFocus.node.Equals(endNode))
                    {
                        // We found the target -- now construct the path:
                        outputPath.Add(endNode);
                        while (previous.ContainsKey(searchFocus.node))
                        {
                            searchFocus.node = previous[searchFocus.node];
                            outputPath.Add(searchFocus.node);
                        }
                        outputPath.Reverse();
                        break;
                    }
                    else
                    {
                        // We did not found the target yet -- develop new nodes.
                        foreach (var neighbor in graph.Neighbors(searchFocus.node))
                        {
                            if (openSet.Contains(neighbor))
                            {
                                continue;
                            }
                            float dist = Distance(searchFocus.node, neighbor);
                            float h = Heuristic(searchFocus.node, endNode);
                            var neighborWrap = new NodeWrapper<NodeType>(neighbor, dist + searchFocus.g, h);
                            openQueue.Enqueue(neighborWrap, neighborWrap.f);
                            openSet.Add(neighbor);
                            previous[neighbor] = searchFocus.node;
                        }
                    }
                }
            }
        }
        public static List<NodeType> GetPath<NodeType>(IGraph<NodeType> graph, NodeType startNode,
            NodeType endNode, Func<NodeType, NodeType, float> Distance,
            Func<NodeType, NodeType, float> Heuristic, int maxiterations = 1000)
        {
            List<NodeType> path = new List<NodeType>();
            FindPath(graph, startNode, endNode, path, Distance, Heuristic, maxiterations);
            return path;
        }
    }

    class NodeWrapper<NodeType>
    {
        public NodeType node;
        public float g;
        public float h;
        public float f;

        public NodeWrapper(NodeType node, float g, float h)
        {
            this.node = node;
            this.g = g;
            this.h = h;
            this.f = g + h;
        }

    }

    class ProirityQueue<T>
    {
        private SortedList<KeyPair, T> q;
        public int Count => q.Count;

        int counter;

        public ProirityQueue()
        {
            q = new SortedList<KeyPair, T>(new KeyComperer());
            counter = 0;
        }

        public void Enqueue(T node, float key)
        {
            Console.WriteLine(key);
            q.Add(new KeyPair(counter,key), node);
            counter++;
        }

        public T Dequeue()
        {
            T node = q[q.Keys[0]];
            q.RemoveAt(0);
            return node;
        }
    }
    class KeyComperer : IComparer<KeyPair>
    {
        public int Compare(KeyPair pair1, KeyPair pair2) 
            {
                if (pair1.Item2 > pair2.Item2)
                {
                    return 1;
                }
                if (pair1.Item2 < pair2.Item2)
                {
                    return -1;
                }
                return pair1.Item1.CompareTo(pair2.Item1);
                }
    }
}

