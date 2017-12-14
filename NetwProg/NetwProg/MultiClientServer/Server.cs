using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

class Server
{
    public Server(int port)
    {
        // Luister op de opgegeven poort naar verbindingen
        TcpListener server = new TcpListener(IPAddress.Any, port);
        server.Start();

        // Start een aparte thread op die verbindingen aanneemt
        new Thread(() => AcceptLoop(server)).Start();
    }

    private void AcceptLoop(TcpListener handle)
    {
        while (true)
        {
            TcpClient client = handle.AcceptTcpClient();
            StreamReader clientIn = new StreamReader(client.GetStream());
            StreamWriter clientOut = new StreamWriter(client.GetStream());
            clientOut.AutoFlush = true;

            // De server weet niet wat de poort is van de client die verbinding maakt, de client geeft dus als onderdeel van het protocol als eerst een bericht met zijn poort
            int zijnPoort = int.Parse(clientIn.ReadLine().Split()[1]);

            Console.WriteLine("Client maakt verbinding: " + zijnPoort);

            // Zet de nieuwe verbinding in de verbindingslijst
            if (!NetwProg.neighs.ContainsKey(zijnPoort))
                NetwProg.neighs.Add(zijnPoort, new Connection(clientIn, clientOut));
            if (!NetwProg.routingNeighTable.ContainsKey(zijnPoort))
                NetwProg.routingNeighTable.Add(zijnPoort, Tuple.Create(zijnPoort, 1));
            else NetwProg.routingNeighTable[zijnPoort] = Tuple.Create(zijnPoort, 1);
            if (NetwProg.neighs[zijnPoort] == null)
                NetwProg.neighs[zijnPoort] = new Connection(clientIn, clientOut);
        }
    }
}
