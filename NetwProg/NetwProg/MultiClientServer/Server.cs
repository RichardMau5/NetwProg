using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

class Server
{
    public Server(int port)
    {
        // Listen on your port to any incoming connections
        TcpListener server = new TcpListener(IPAddress.Any, port);
        server.Start();

        // Starts this listener on a new thread
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

            // The server doesn't know the client who's attempting to connect, it's part of the protocol of the client to let us know who he is
            int zijnPoort = int.Parse(clientIn.ReadLine().Split()[1]);

            Console.WriteLine("Verbonden: " + zijnPoort);

            // Add this newly established connection to the list
            if(!NetwProg.neighs.ContainsKey(zijnPoort))
                NetwProg.neighs.Add(zijnPoort, new Connection(clientIn, clientOut));
        }
    }
}
