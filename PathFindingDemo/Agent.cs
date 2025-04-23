namespace PathFindingDemo
{
    internal class Agent
    {
        private List<Node> _path = []; // the path of the agent

        public Agent(string name, Node start, Node goal)
        {
            Name = name;
            CurrentNode = Start = start;
            Goal = goal;

            _path.Add(start);
        }

        public string Name { get; }

        public Node Start { get; } // the start point

        public Node Goal { get; } // the goal point

        public Node CurrentNode { get; private set; } // the current point of the agent

        public List<Node> PortionPath { get; } = [];

        public Dictionary<Node, int> ClosedSet { get; } = [];

        public PriorityQueue<Node, int> OpenSet { get; } = new(); // Node's FScore is used as a priority, lowest on top

        public Dictionary<Node, int> GScore { get; } = [];

        public Dictionary<Node, int> FScore { get; } = [];

        public Dictionary<Node, Node> CameFrom { get; } = [];

        /// <summary>
        /// Collision solver.
        /// </summary>
        public void SetPortionPath( Dictionary<Node, Agent>[] spaceMap)
        {
            PortionPath.Clear();

            Node current = CurrentNode, prev = CurrentNode;
            Node nextBest = CurrentNode; // it's a next node wich has the best score
            int stepsLeft = Definitions.WindowSize;

            int nextBestGScore;

            if (current == Goal)
            {
                for(int i = 0; i < Definitions.WindowSize; i++)
                {
                    PortionPath.Add(current);
                }

                return;
            }

            for(int i = 0; i < Definitions.WindowSize - 1; i++)
            {
                if (current == Goal)
                    break;

                if( !CameFrom.TryGetValue(current, out nextBest))
                {
                    // came from nowhere? How?
                    break;
                }

                /****************************************************************************
                First case: next best node is not occupied, but is it safe for
                me to go there? Let's fancy just part of the map (only one line):
                ___________________
                |0|1|2|3|4|5|6|7|8|	May be the agent want to out of the map.
                <-A| | | | | | | | | It's bad. Let's check this out.
                *****************************************************************************/

                if(!spaceMap[i].ContainsKey(nextBest))
                {
                    // Probably we already out of the map?
                    if (spaceMap[i].TryGetValue(current, out Agent? anotherAgent))
                    {
                        // Ok, let's check if there another agent
                        // Let's take a beside node. Can the second walk here?

                        if (i > 0 && spaceMap[i - 1].TryGetValue(nextBest, out Agent? anotherAgent2)
                            && anotherAgent == anotherAgent2)
                        {
                            // Fuck they are going to collide
                            // Hold on! May be there are the nodes which are empty?
                            Span<Node> neighbors1 = [new(current.X - 1, current.Y), new(current.X + 1, current.Y), 
                                new(current.X, current.Y - 1), new(current.X, current.Y + 1)];

                            nextBestGScore = int.MaxValue;
                            foreach(Node neighbor in neighbors1)
                            {
                                if (neighbor == nextBest )
                                    continue; // that's what we are avoiding

                                if(neighbor == prev )
                                {
                                    // We dont want the agent go back yet though sometimes going back is the only option.
                                    continue;
                                }

                                if ( GScore[neighbor] < nextBestGScore 
                                    && !spaceMap[i].ContainsKey(neighbor)) // find best g_score and it is not in spaceMap
                                {
                                    // We found the best node where we can go!!!
                                    nextBest = neighbor;
                                    nextBestGScore = GScore[neighbor];
                                }
                            }

                            if(nextBestGScore == int.MaxValue)
                            {
                                // Fuck this we didn' find any node where the agent could to go.
                                // OK, now we consider going back!
                                nextBest = prev;
                            }
                        }
                    }

                    // Move! Let another agent go.
                    prev = current;
                    current = nextBest;
                    PortionPath.Add(current);
                    stepsLeft--;
                    CurrentNode = current;
                    continue;
                }

                /**********************************************************************************
                The second case: the next best node is occupied get non - obstacles neighbors.
                ***********************************************************************************/
                int x = current.X;
                int y = current.Y;

                Span<Node> neighbors = [new(current.X - 1, current.Y), new(current.X + 1, current.Y),
                                new(current.X, current.Y - 1), new(current.X, current.Y + 1)];
                nextBestGScore = int.MaxValue;

                foreach(Node neighbor in neighbors)
                {
                    if (neighbor == nextBest)
                        continue; // that's what we are avoiding
                    if (neighbor == prev)
                    {
                        // We dont want the agent go back though sometimes going back is the only option.
                        continue;
                    }

                    if(GScore[neighbor] < nextBestGScore
                        && !spaceMap[i].ContainsKey(neighbor)) // find best g_score and it is not in spaceMap
                    {
                        // as I had wrote before: "We found, where we can to go"
                        nextBest = neighbor;
                        nextBestGScore = GScore[neighbor];
                    }
                }

                /**************************************************************************
                f_score never updated, no neighbor except came_from which is occupied.
                The next best is to stay at current. ??? or going back?
                ***************************************************************************/
                if ( nextBestGScore == int.MaxValue )
                    break;

                // Still moving ...
                prev = current;
                current = nextBest;
                PortionPath.Add(current);
                stepsLeft--;
                CurrentNode = current; 
            }

            /********************************************************************************
            We out of from collision dection, for some reasons. Just fill the portion
            path with current node.
            *********************************************************************************/
            for(; stepsLeft > 0; stepsLeft--)
                PortionPath.Add(current);
        }
    }
}