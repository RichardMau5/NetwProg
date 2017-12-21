using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

/// <summary>
/// (Intermediate) Destination and Distance Table
/// </summary>
class IDaDTable
{
    private Dictionary<int, Tuple<int, int>> routingNeighTable = new Dictionary<int, Tuple<int, int>>();

    public void SetEntry(int destination, int bestNeighbour, int hops)
    {
        if (!routingNeighTable.ContainsKey(destination))
            routingNeighTable.Add(destination, Tuple.Create(bestNeighbour, hops));
        else
            lock (routingNeighTable[destination])
                routingNeighTable[destination] = Tuple.Create(bestNeighbour, hops);
    }

    public int GetBestNeigh(int destination) => routingNeighTable[destination].Item1;

    public int GetHops(int destination)
    {
        if (routingNeighTable.ContainsKey(destination))
            return routingNeighTable[destination].Item2;
        return NetChange.N;
    }

    public List<Tuple<int, int, int>> GetAsTupleList()
    {
        List<Tuple<int, int, int>> tupleList = new List<Tuple<int, int, int>>();
        foreach (KeyValuePair<int, Tuple<int, int>> kvp in routingNeighTable)
        {
            Tuple<int, int, int> tuple = Tuple.Create(kvp.Key, kvp.Value.Item1, kvp.Value.Item2);
            tupleList.Add(tuple);
        }
        return tupleList;
    }
}
