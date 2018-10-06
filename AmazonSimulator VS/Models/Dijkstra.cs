using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;

namespace Models
{
    /// <summary>
    /// Based on code from MrBurst. Source: https://github.com/mburst/dijkstras-algorithm/blob/master/dijkstras.cs
    /// </summary>
    public class Dijkstra
    {
      

        public void ConvertNodesToGraph(List<Node> list)
        {
            // Probably not going to finish this function in time, on the backburner
           // foreach (var node in NodeList)
         //   {
                // Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
                //add_vertex(node.name)
         //   }
        }
        Dictionary<char, Dictionary<char, int>> vertices = new Dictionary<char, Dictionary<char, int>>();

        public Dijkstra()
        {
            // Sets the path of the world
            this.add_vertex('A', new Dictionary<char, int>() { { 'B',  15 }, { 'D', 30 } });
            this.add_vertex('B', new Dictionary<char, int>() { { 'A', 15 }, { 'C', 15 },{'F',5 } });
            this.add_vertex('C', new Dictionary<char, int>() { { 'B', 15 }, { 'O', 15 } });
            this.add_vertex('D', new Dictionary<char, int>() { { 'A', 30 }, { 'N', 10 } });
            this.add_vertex('E', new Dictionary<char, int>() { { 'N', 20 }, { 'O', 20 } });

            this.add_vertex('F', new Dictionary<char, int>() { { 'B', 5 }, { 'G', 3 },{ 'F',5} });
            this.add_vertex('G', new Dictionary<char, int>() { { 'F', 3 } });
            this.add_vertex('H', new Dictionary<char, int>() { { 'F', 5 }, { 'I', 3 },{'J',5 } });
            this.add_vertex('I', new Dictionary<char, int>() { { 'H', 3 } });
            this.add_vertex('J', new Dictionary<char, int>() { { 'H', 5 }, { 'K', 3 }, { 'O', 5 }, { 'L', 10 } });
            this.add_vertex('K', new Dictionary<char, int>() { { 'J', 3 }  });
            this.add_vertex('L', new Dictionary<char, int>() { { 'J', 10 }, { 'M', 15 } });
            this.add_vertex('M', new Dictionary<char, int>() { { 'L', 15 }, { 'N', 5 } });

            //Bonus Junctions
            this.add_vertex('N', new Dictionary<char, int>() { { 'M', 5 }, { 'D', 10 },{'E',20 } });
            this.add_vertex('O', new Dictionary<char, int>() { { 'J', 5 }, { 'C', 10 },{'E',20 } });
            // Sets the 3d-points of the world
        }




        public void add_vertex(char name, Dictionary<char, int> edges)
        {
            vertices[name] = edges;
        }

        public List<char> shortest_path(char start, char finish)
        {
            var previous = new Dictionary<char, char>();
            var distances = new Dictionary<char, int>();
            var nodes = new List<char>();

            List<char> path = null;

            foreach (var vertex in vertices)
            {
                if (vertex.Key == start)
                {
                    distances[vertex.Key] = 0;
                }
                else
                {
                    distances[vertex.Key] = int.MaxValue;
                }

                nodes.Add(vertex.Key);
            }

            while (nodes.Count != 0)
            {
                nodes.Sort((x, y) => distances[x] - distances[y]);

                var smallest = nodes[0];
                nodes.Remove(smallest);

                if (smallest == finish)
                {
                    path = new List<char>();
                    while (previous.ContainsKey(smallest))
                    {
                        path.Add(smallest);
                        smallest = previous[smallest];
                    }

                    break;
                }

                if (distances[smallest] == int.MaxValue)
                {
                    break;
                }

                foreach (var neighbor in vertices[smallest])
                {
                    var alt = distances[smallest] + neighbor.Value;
                    if (alt < distances[neighbor.Key])
                    {
                        distances[neighbor.Key] = alt;
                        previous[neighbor.Key] = smallest;
                    }
                }
            }
            return path;
        }
    }
}