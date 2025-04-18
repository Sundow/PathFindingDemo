using PathFindingDemo;

// create a random map 20 x 20 with 0% obstacles
BitMatrix map = CreateMap(20, 20, 0);

Agent[] agentList = [
    new("A", new(1, 1), new(8, 1)),
    new("B", new(8, 1), new(1, 1)) ];

bool isValid = true;
Parallel.ForEach(agentList, agent =>
{
    if (!Solver.GetTrueDistanceHeuristic(agent, map, agent.Start))
    {
        isValid = false;
    }
});

if (!isValid)
{
    Console.WriteLine("Some agents failed to calculate heuristics.");
    return;
}

/*******************************************************************************
      Set some variables:
      n_agents: it's number of agents, that there are exists on the map
      all_agents_find_goal: indecator that everyone has reached their own goal
*******************************************************************************/
int nAgents = agentList.Length;
bool allAgentsFindGoal = false;

/****************************************************************************
      Start main loop, it will be work until that moment, whilst all agents
      won't reach the goals.
*****************************************************************************/
while (!allAgentsFindGoal)
{
    int nAgentsAtGoal = 0;
    foreach (Agent agent in agentList)
    {
        // if agent is at goal, skip it
        if (agent.CurrentNode == agent.Goal)
            nAgentsAtGoal++;
    }

    if( nAgentsAtGoal == nAgents )
    {
        allAgentsFindGoal = true;
        Console.WriteLine("Success! All agents reached goal!");
        break;
    }

    /**************************************************************************************************
		There we starting the main computational action of the program Let me explain a little bit.

		Time is not time like
		min or sec. Time is unit to represent a frame, a step for all
		agents. below, I used a vector. space_time_map[0] means at the begining.
		space_time_map[7] means after 7 steps, at step 8 or the end of current
		window, where are the agents located on the map. say if at time 5, 'A' is
		located at node (3,4). how to capture 'A'? space_time_map[4][{3,4}] ; I hope
		you got the idea, the syntax may be wrong. notice that, one node, one agent,
		it is not allowed to have multiple agents sitting at one node. Now I hope
		that you know what is space_time_map, and what I mean by time.
    ***************************************************************************************************/
    var spaceTimeMap = new Dictionary<Node, Agent>[Definitions.WindowSize];
    for (int i = 0; i < Definitions.WindowSize; i++)
        spaceTimeMap[i] = [];

    foreach(Agent agent in agentList)
    {
        agent.SetPortionPath(spaceTimeMap);
        Solver.UpdateSpaceMap(spaceTimeMap, agent);
    }

    PrintSpaceMap(spaceTimeMap, map);
}

void PrintSpaceMap(Dictionary<Node, Agent>[] spaceTimeMap, BitMatrix map )
{
    for(int i = 0; i < Definitions.WindowSize; i++)
    {
        Console.WriteLine($"space_map at time  {i}");
        map.PrintWithAgents(spaceTimeMap[i]);
        Console.WriteLine();
        Console.ReadLine();
    }
}

BitMatrix CreateMap(int rows, int cols, int percentObstacles)
{
    BitMatrix map = new(rows, cols);
    // set border values to false (obstacles) and also a random percent of values equal to percentObstacles, the rest is set to true - passable
    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < cols; j++)
        {
            if (i == 0 || i == rows - 1 || j == 0 || j == cols - 1)
            {
                map.Set(i, j, true);
            }
            else
            {
                map.Set(i, j, Random.Shared.Next(100) < percentObstacles);
            }
        }
    }
    return map;
}