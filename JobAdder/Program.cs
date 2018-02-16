using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAdder
{
    class Program
    {
        public static int solution1(int[] A)
        {
            int n = A.Length;
            int result = 0;
            for (int i = 0; i < n - 1; i++)
            {
                if (A[i] == A[i + 1])
                    result = result + 1;
            }
            int r = Int32.MinValue;
            for (int i = 0; i < n; i++)
            {
                int count = 0;
                if (i > 0) // Not the first
                {
                    if (A[i - 1] != A[i])
                        count = count + 1;
                    else
                        count = count - 1;
                }
                if (i < n - 1) // Not the last
                {
                    if (A[i + 1] != A[i])
                        count = count + 1;
                    else
                        count = count - 1;
                }
                r = Math.Max(r, count);
            }
            return result + r;
        }

        

        public static int solution(int[][] A)
        {            
            var map = new Map(A);
            
            var components = 0;

            for (int row = 0; row < A.Length; row++)
            {
                for (int col = 0; col < A[row].Length; col++)
                {
                    if (map.HasBeenVisited(row, col))
                        continue;

                    var root = new Tile(row, col);

                    var steps = BFS(root, t => map.Visit(t)).ToList();

                    components++;
                }
            }

            return components;
        }

        static void Main(string[] args)
        {
            var input = new int[][] { new[] { 5, 4, 4 }, new[] { 4, 3, 4 }, new[] { 3, 2, 4 }, new[] { 2, 2, 2 }, new[] { 3, 3, 4 }, new[] { 1, 4, 4 }, new[] { 4, 1, 1 } };
            var result = solution(input);
        }
    }

    public class Map
    {
        private int[][] values;
        private bool[][] visited;

        public Map(int[][] values)
        {
            this.values = values;
            visited = new bool[values.Length][];

            for (int i = 0; i < values.Length; i++)
            {
                visited[i] = new bool[values[i].Length];
            }
        }
        public IEnumerable<Tile> Tiles { get; set; }

        public IEnumerable<Tile> Visit(Tile tile)
        {
            var rowAdvances = new[] { -1, 1, 0, 0 };
            var colAdvances = new[] { 0, 0, -1, 1 };

            visited[tile.Row][tile.Column] = true;

            var neighbours = new List<Tile>();

            for (int i = 0; i < 4; i++)
            {
                var newRow = tile.Row + rowAdvances[i];
                var newColumn = tile.Column + colAdvances[i];

                if ((newRow != tile.Row || newColumn != tile.Column)
                    && (newRow >= 0 && newRow < values.Length)
                    && (newColumn >= 0 && newColumn < values[newRow].Length)
                    && !visited[newRow][newColumn]
                    && values[newRow][newColumn] == values[tile.Row][tile.Column])
                {
                    neighbours.Add(new Tile(newRow, newColumn));
                    
                }
            }

            return neighbours;
        }

        public bool HasBeenVisited(int row, int column)
        {
            return visited[row][column];
        }

        private IEnumerable<TNode> BFS<TNode>(TNode root, Func<TNode, IEnumerable<TNode>> children)
        {
            var q = new Queue<TNode>();
            q.Enqueue(root);

            while (q.Count > 0)
            {
                TNode current = q.Dequeue();
                yield return current;

                foreach (var child in children(current))
                    q.Enqueue(child);
            }
        }
    }
    public class Tile
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Tile(int row, int column)
        {
            this.Row = row;
            this.Column = column;            
        }
    }
}
