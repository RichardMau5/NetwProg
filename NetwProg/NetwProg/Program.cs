using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class NetwProg
{
    public static int myPortNr;
    public static Dictionary<int, Connection> neighs = new Dictionary<int, Connection>();
    Dictionary<int, int> routingNeighTable = new Dictionary<int, int>();


    public NetwProg(string[] args)
    {
        myPortNr = int.Parse(args[0]);
        Console.Title = "NetChange " + myPortNr;
        for(int i = 1; i < args.Length; i++)
        {
            neighs.Add(int.Parse(args[i]), null); 
        }
        Console.ReadLine();
    }

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
        for (int i = 1; i < args.Length; i++)
        {
            int port = int.Parse(args[i]);
            neighs.Add(port, null);
        }
    }

    private static void Run()
    {
        while (true)
        {
            string[] input = Console.ReadLine().Split(new char[] { ' ' }, 3);
            switch (input[0])
            {
                case "B":
                    int portMsg = int.Parse(input[1]);
                    if (!neighs.ContainsKey(portMsg))
                    {
                        Console.WriteLine("Error: Unknown port number");
                        break;
                    }
                    if (neighs[portMsg] == null)
                        neighs[portMsg] = new Connection(portMsg);
                    neighs[portMsg].Write.WriteLine(input[2]);
                    break;
            }
        }
    }
}
