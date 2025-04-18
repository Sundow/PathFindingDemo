namespace PathFindingDemo
{
    internal class Solver
    {
        /*****Reverse Resumable A Star as described in ************************************************
        RRA* is a Backwards Search ignoring other agents the g(n) value
        (measured distance to goal) is the true distance heuristic value. if g(n)
        value of requested node is not known, set goal at requested_node.
        ******************************************************************************/
        public static bool GetTrueDistanceHeuristic(Agent agent, BitMatrix map, Node requestNode)
        {
            bool goalFound = false;

            /*******************************************************
            Hmmm, what if we were fucked up and passed the
            obstacle as request_node. Ok, nevermind, I will
            check it anyway.
            ********************************************************/
            if( map.IsObstacle(requestNode) )
            {
                Console.WriteLine($"Requested node {requestNode} is an obstacle.");
                agent.ClosedSet.Add(requestNode, int.MaxValue);
                return false;
            }

            // Simple logic. IT'S RRA* (REVERSE!!!)
            Node start = agent.Goal;
            Node goal = requestNode;

            /*************************************************
            1) Initially, only the start node is known.
            2) The distance between start and start is 0
            3) Heuristic distance between start and goal
            *************************************************/
            agent.OpenSet.Enqueue(start, 0);
            agent.GScore[start] = 0;
            agent.FScore[start] = ManhattanHeuristic(start, goal);

            // The Search start...
            Span<Node> neighbors = stackalloc Node[4];
            while (agent.OpenSet.Count > 0)
            {
                Node current = agent.OpenSet.Dequeue();

                if(current == goal)
                    goalFound = true;

                agent.ClosedSet[current] = agent.FScore[current];

                /**********************************************************************
                Explore the neighbors around the current node. And we don't care what
                exactly the neighbor (Either obstacle or empty or other agent).
                **********************************************************************/
                map.GetNeighbors(current, neighbors);

                for(int i = 0; i < neighbors.Length; i++)
                {
                    Node neighbor = neighbors[i];

                    // if neighbors do not exist, create new nodes
                    if (!agent.CameFrom.ContainsKey(neighbor))
                    {
                        agent.CameFrom[neighbor] = current;
                        if(!agent.GScore.ContainsKey(neighbor))
                            agent.GScore[neighbor] = int.MaxValue;
                        if (!agent.FScore.ContainsKey(neighbor))
                            agent.FScore[neighbor] = int.MaxValue;
                    }

                    if(agent.ClosedSet.ContainsKey(neighbor))
                        continue;

                    // if neighbor node is obstacle, paste to closed_set and continue.
                    if (map.IsObstacle(neighbor))
                    {
                        agent.ClosedSet[neighbor] = int.MaxValue;
                        continue;
                    }

                    // a distance to neighbor is always 10 (could have been different for diagonal moves, but they are not supported).
                    int tenativeGScore = agent.GScore[current] + 10;
                    if( tenativeGScore > agent.GScore[neighbor])
                        continue;

                    agent.CameFrom[neighbor] = current;
                    agent.GScore[neighbor] = tenativeGScore;

                    /************************************************************************
                    If goal is already found, manhattan_heuristic is inaccurate to
                    measure f_score. because manhanttan heuristic ignore obstacles. If goal
                    is found, we simply add 20, why? for example, if we know node A's
                    f_score and node B is A's neighbor, and if A is the only parent of B,
                    then the cost of going from A to B is 10, so B_f_score = A_f_sore + 10;
                    in order for B to reach goal, it must go back to A, so add 10 to
                    B_f_score again, total is 20;
                    ***********************************************************************/
                    int newFScore;
                    if (goalFound)
                    {
                        newFScore = agent.FScore[current] + 20;
                        agent.FScore[neighbor] = newFScore;
                    }
                    else
                    {
                        newFScore = tenativeGScore + ManhattanHeuristic(neighbor, goal);
                        agent.FScore[neighbor] = newFScore;
                    }
                    agent.OpenSet.Enqueue(neighbors[i], newFScore);
                }
            }

            return goalFound;
        }

        private static int ManhattanHeuristic(Node a, Node b) => 10 * (Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y));

        public static void UpdateSpaceMap(Dictionary<Node, Agent>[] spaceMap, Agent agent)
        {
            for(int i = 0; i < Definitions.WindowSize; i++)
                spaceMap[i][agent.PortionPath[i]] = agent;
        }
    }
}
