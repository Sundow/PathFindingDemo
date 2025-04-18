 This is an implementation of WHCA* (Windowed Hierarchical Cooperative A*) pathfinding.
 
 Path-finding is one of the basic problems in the field of artificial intelligence and computer science in general. In the most basic variant, the task is to find a path for an agent through a given graph, so that the agent moves from vertex to vertex along the edges of the graph and ends in a specified goal vertex. A natural extension of the basic path-finding problem is multi-agent pathfinding. In this problem, we are given multiple agents, each with its own goal, and the task is to again guide them toward their goals along the edges of a given graph. Agents have knowledge of each others plans, which allows them to cooperate. The amount of cooperation required between agents depends on the problem instance, and it can vary from very high on instances where the number of agents is close to the number of available vertices to very low on instances where the number of agents is small relative to the size of the graph and there is enough space between the agents.
 
To a big extent this code is a C# conversion of the C++ code present here: https://github.com/JuiceFV/Cooperative_Pathfinding

I even kept a number of the original author's comments, no matter how frivolous they were.

