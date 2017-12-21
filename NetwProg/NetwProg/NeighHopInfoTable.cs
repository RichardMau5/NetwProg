using System;
using System.Collections.Generic;

class NeighHopInfoTable
{
    Dictionary<int, Dictionary<int, int>> hopInfoTable = new Dictionary<int, Dictionary<int, int>>();

    public NeighHopInfoTable(Dictionary<int, Connection> neighs)
    {
        foreach (KeyValuePair<int, Connection> kvp in neighs)
            hopInfoTable.Add(kvp.Key, new Dictionary<int, int>());
    }

    public void Update(int neighbour, int destination, int hops)
    {
        if (!hopInfoTable.ContainsKey(neighbour))
            hopInfoTable.Add(neighbour, new Dictionary<int, int>());
        if (!hopInfoTable[neighbour].ContainsKey(destination))
            hopInfoTable[neighbour].Add(destination, hops);
        else
        {
            lock(hopInfoTable[neighbour])
                hopInfoTable[neighbour][destination] = hops;
        }
    }

    public Tuple<int, int> GetMinimumDistanceTo(int toPort)
    {
        int minimum = NetChange.N;
        int port = -1;
        foreach (KeyValuePair<int, Dictionary<int, int>> kvp in hopInfoTable)
        {
            if (!kvp.Value.TryGetValue(toPort, out int aValue))
                aValue = NetChange.N;
            if (aValue < minimum)
            {
                port = kvp.Key;
                minimum = aValue;
            }
        }
        return Tuple.Create(port, minimum);
    }

    public void RemoveNeigh(int neighPort)
    {
        if (hopInfoTable.ContainsKey(neighPort))
            hopInfoTable.Remove(neighPort);
    }
}