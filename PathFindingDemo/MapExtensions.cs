namespace PathFindingDemo
{
    /// <summary>
    /// BitMatrix represents map where true value means obstacle.
    /// </summary>
    internal static class MapExtensions
    {
        public static bool IsObstacle(this BitMatrix map, Node node) => map[node.X, node.Y];

        public static void GetNeighbors(this BitMatrix map, Node node, Span<Node> neighbors)
        {
            int x = node.X;
            int y = node.Y;
            neighbors[0] = new Node(x, y - 1);
            neighbors[1] = new Node(x - 1, y);
            neighbors[2] = new Node(x + 1, y);
            neighbors[3] = new Node(x, y + 1);
        }

        public static void PrintWithAgents(this BitMatrix map, Dictionary<Node, Agent> spaceMap)
        {
            for (int y = 0; y < map.RowsCount; y++)
            {
                for (int x = 0; x < map.ColumnsCount; x++)
                {
                    Node node = new(x, y);
                    if (map.IsObstacle(node))
                    {
                        Console.Write("#");
                    }
                    else if (spaceMap.TryGetValue(node, out Agent? agent))
                    {
                        Console.Write(agent.Name.First());
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
