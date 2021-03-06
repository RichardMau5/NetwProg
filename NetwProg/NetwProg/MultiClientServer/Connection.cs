﻿using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

class Connection
{
    public StreamReader Read;
    public StreamWriter Write;

    // Connection heeft 2 constructoren: deze constructor wordt gebruikt als wij CLIENT worden bij een andere SERVER
    public Connection(int port)
    {
        TcpClient client = new TcpClient("localhost", port);
        Read = new StreamReader(client.GetStream());
        Write = new StreamWriter(client.GetStream());
        Write.AutoFlush = true;

        // De server kan niet zien van welke poort wij client zijn, dit moeten we apart laten weten
        Write.WriteLine("Poort: " + NetwProg.myPortNr);

        // Start het reader-loopje
        new Thread(ReaderThread).Start();
    }

    // Deze constructor wordt gebruikt als wij SERVER zijn en een CLIENT maakt met ons verbinding
    public Connection(StreamReader read, StreamWriter write)
    {
        Read = read; Write = write;

        // Start het reader-loopje
        new Thread(ReaderThread).Start();
    }

    // LET OP: Nadat er verbinding is gelegd, kun je vergeten wie er client/server is (en dat kun je aan het Connection-object dus ook niet zien!)

    // This loop receives messages formatted according to protocol
    public void ReaderThread()
    {
        try
        {
            while (true)
            {
                string test = Read.ReadLine();
                Console.WriteLine("// " + test);
                string[] line = test.Split(new char[] { ' ' }, 3);
                switch (line[0])
                {
                    case "Msg":
                        if (int.Parse(line[1]) == NetwProg.myPortNr)
                            Console.WriteLine(line[2]);
                        else
                            ProtocolFunctions.MessageService(line[1], line[2]);
                        break;
                    case "Del":
                        ProtocolFunctions.DeleteConnect(int.Parse(line[1]), false);
                        break;
                    case "mdist":
                        string[] lineEx = line[2].Split(new char[] { ' ' }, 3);
                        int fromPort = int.Parse(line[1]);
                        int toPort = int.Parse(lineEx[0]);
                        int distance = int.Parse(lineEx[1]);
                        Console.WriteLine("// mdist fromPort: " + fromPort + " toPort: " + toPort + " distance: " + distance);
                        NetChange.DistanceChange(fromPort, toPort, distance);
                        break;
                }          
            }
        }
        catch { } // Connection apperently has been closed
    }
}
