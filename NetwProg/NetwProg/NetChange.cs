using System;
using System.Collections.Generic;

class NetChange
{
    public static int N = 20;                                                                                                     //Number of max Nodes
    public static IDaDTable routingTable = new IDaDTable();                                                                      //Your own routing table
    public static NeighHopInfoTable neighRoutingTable = new NeighHopInfoTable(NetwProg.neighs);     //The preferred routes of your neighbours

    public static void Init()
    {
        routingTable.SetEntry(NetwProg.myPortNr, NetwProg.myPortNr, 0);
        foreach (KeyValuePair<int, Connection> kvp in NetwProg.neighs)
            kvp.Value.Write.WriteLine("mdist " + NetwProg.myPortNr + " " + NetwProg.myPortNr + " " + 0);
    }

    private static void Recompute(int toPort)
    {
        int oldD = routingTable.GetHops(toPort);
        if (toPort == NetwProg.myPortNr)
            routingTable.SetEntry(toPort, toPort, 0);
        else
        {
            Tuple<int, int> portAndMinDist = neighRoutingTable.GetMinimumDistanceTo(toPort);
            int d = 1 + portAndMinDist.Item2;
            if(d < N)
                routingTable.SetEntry(toPort, portAndMinDist.Item1, d);
            if(d != oldD)
                foreach(KeyValuePair<int, Connection> kvp in NetwProg.neighs)
                    kvp.Value.Write.WriteLine("mdist " + NetwProg.myPortNr + " " + toPort + " " + d);
        }
    }

    public static void DistanceChange(int fromPort, int toPort, int distance)
    {
        neighRoutingTable.Update(fromPort, toPort, distance);
        Recompute(toPort);
    }
}
