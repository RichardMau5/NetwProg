using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class NetwProg
{
    int myPortNr;
    List<int> neighBourPortNrs;

    public NetwProg(string[] args)
    {
        myPortNr = int.Parse(args[0]);
        Console.Title = "NetChange " + myPortNr;
        for(int i = 1; i < args.Length; i++)
        {
            //logic
        }
        Console.ReadLine();
    }

    static void Main(string[] args)
    {
        if (args.Length != 0)
        {
            NetwProg p = new NetwProg(args);
            //p.Run();
        }
        else { Console.WriteLine("Please provide at least a socket nr."); }
    }
}
