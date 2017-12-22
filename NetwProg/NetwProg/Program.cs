using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class NetwProg
{
    public static int myPortNr;
    public static Dictionary<int, Connection> neighs = new Dictionary<int, Connection>();
    public static IDaDTable routingTable = new IDaDTable();                                                                //Your own routing table
    public static ConcurrentDictionary<int, IDaDTable> neighRoutingTable = new ConcurrentDictionary<int, IDaDTable>();     //The preferred routes of your neighbours

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
        routingTable.SetEntry(myPortNr, myPortNr, 0);                                         //Trivial case
        int maxTry = 15;
        for (int i = 1; i < args.Length; i++)
        {
            int neighPortNr = int.Parse(args[i]);
            EstablishConnection(neighPortNr, maxTry);
        }
        NetChange.Init();
    }

    private static void Run()
    {
        while (true)
        {
            string[] input = Console.ReadLine().Split(new char[] { ' ' }, 3);
            switch (input[0])
            {
                case "R":
                    ProtocolFunctions.PrintRoutingTable();
                    break;
                case "B":
                    ProtocolFunctions.MessageService(input[1], input[2]);
                    break;
                case "C":
                    ProtocolFunctions.NewConnect(int.Parse(input[1]));
                    break;
                case "D":
                    ProtocolFunctions.DeleteConnect(int.Parse(input[1]));
                    break;
                case "Q":
                    ProtocolFunctions.TempWriteAllNeighs();
                    break;
            }
        }
    }

    private static void EstablishConnection(int neighPortNr, int maxTry)
    {
        int tries = 0;
        while (!neighs.ContainsKey(neighPortNr) && tries < maxTry)
        {
            try
            {
                neighs.Add(neighPortNr, new Connection(neighPortNr));
            }
            catch
            {
                tries++;
            }
        }
        routingTable.SetEntry(neighPortNr, neighPortNr, 1);
        NetChange.DistanceChange(neighPortNr, myPortNr, 1);
    }
}