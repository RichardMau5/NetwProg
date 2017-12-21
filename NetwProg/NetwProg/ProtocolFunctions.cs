using System;

public static class ProtocolFunctions
{
    public static void PrintRoutingTable()
    {
        var tupleList = NetChange.routingTable.GetAsTupleList();
        foreach (Tuple<int, int, int> entry in tupleList)
        {
            if (entry.Item3 != 0 && entry.Item3 != 20)                                      //Check if num of hops are not "infinity" or 0 (local)
            {
                Console.WriteLine(entry.Item1 + " " + entry.Item3 + " " + entry.Item2);
                continue;
            }
            if (entry.Item3 >= 20)                                                          //An unreachable node has a hops count of 20
                continue;
            Console.WriteLine(entry.Item1 + " " + 0 + " local");                            //The only case left is that the hop count is 0
        }
    }

    public static void MessageService(string destinationPort, string message)
    {
        int portMsg = int.Parse(destinationPort);
        if (!NetwProg.neighs.ContainsKey(portMsg))
        {
            Console.WriteLine("Poort " + destinationPort + " is niet bekend");
            return;
        }
        NetwProg.neighs[portMsg].Write.WriteLine("Msg " + destinationPort + " " + message);
    }

    public static void NewConnect(int newNeighPortNr)
    {
        NetwProg.neighs.Add(newNeighPortNr, new Connection(newNeighPortNr));
        NetwProg.routingTable.SetEntry(newNeighPortNr, newNeighPortNr, 1);
    }

    public static void DeleteConnect(int neighDisconnect, bool sendToNeigh = true)
    {
        if (!NetwProg.neighs.ContainsKey(neighDisconnect))
        {
            Console.WriteLine("Poort " + neighDisconnect + " is niet bekend");
            return;
        }
        if (sendToNeigh)
            NetwProg.neighs[neighDisconnect].Write.WriteLine("Del " + NetwProg.myPortNr);
        NetwProg.neighs.Remove(neighDisconnect);
        NetChange.neighRoutingTable.RemoveNeigh(neighDisconnect);
        NetChange.Disconnect(neighDisconnect);
        Console.WriteLine("Verbroken " + neighDisconnect);
    }

    internal static void TempWriteAllNeighs()
    {
        foreach (int x in NetChange.allNodes)
            Console.WriteLine("// Node " + x + " is in Network");
    }
}
