using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class NetwProg
{
    public static int myPortNr;
    public static Dictionary<int, Connection> neighs = new Dictionary<int, Connection>();
    public static Dictionary<int, Tuple<int, int>> routingNeighTable = new Dictionary<int, Tuple<int, int>>();            //KEY: destinationPort, VALUE: (neighbour to send to, #Hops)

    static void Main(string[] args)
    {
        if (args.Length != 0)
        {
            Init(args);
            Run();
        }
        else { Console.WriteLine("Please provide at least a socket nr."); }
    }

    private static void Init(string[] args)
    {
        myPortNr = int.Parse(args[0]);
        Console.Title = "NetChange " + myPortNr;
        new Server(myPortNr);
        routingNeighTable.Add(myPortNr, Tuple.Create(myPortNr, 0));                                         //Trivial case
        for (int i = 1; i < args.Length; i++)
        {
            int neighPortNr = int.Parse(args[i]);
            neighs.Add(neighPortNr, null);
            routingNeighTable.Add(neighPortNr, Tuple.Create(neighPortNr, 1));
        }
    }

    private static void Run()
    {
        while (true)
        {
            string[] input = Console.ReadLine().Split(new char[] { ' ' }, 3);
            switch (input[0])
            {
                case "R":
                    PrintRoutingTable();
                    break;
                case "B":
                    MessageService(input[1], input[2]);
                    break;
                case "C":
                    NewConnect(int.Parse(input[1]));
                    break;
            }
        }
    }

    private static void PrintRoutingTable()
    {
        foreach (KeyValuePair<int, Tuple<int, int>> entry in routingNeighTable)
        {
            if (entry.Value.Item2 != 0 && entry.Value.Item2 != 20)                                      //Check if num of hops are not "infinity" or 0 (local)
            {                                        
                Console.WriteLine(entry.Key + " " + entry.Value.Item2 + " " + entry.Value.Item1);
                continue;
            }
            if (entry.Value.Item2 != 0)
                continue;
            Console.WriteLine(entry.Key + " " + 0 + " local");
        }
    }

    public static void MessageService(string destinationPort, string message)
    {
        int portMsg = int.Parse(destinationPort);
        if (!neighs.ContainsKey(portMsg))
        {
            Console.WriteLine("Error: Unknown port number");
            return;
        }
        if (neighs[portMsg] == null)
            neighs[portMsg] = new Connection(portMsg);
        neighs[portMsg].Write.WriteLine("Msg " + destinationPort + " " + message);
    }

    public static void NewConnect(int newNeighPortNr)
    {
        neighs.Add(newNeighPortNr, new Connection(newNeighPortNr));
        if (!routingNeighTable.ContainsKey(newNeighPortNr))                                                 //Could be calculated via NetChange but this is a slightly bit more efficient
        {
            routingNeighTable.Add(newNeighPortNr, Tuple.Create(newNeighPortNr, 1));
            return;
        }
        routingNeighTable[newNeighPortNr] = Tuple.Create(newNeighPortNr, 1);
    }
}
