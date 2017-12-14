using System;
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

    // Deze loop leest wat er binnenkomt en print dit
    public void ReaderThread()
    {
        try
        {
            while (true)
            {
                string[] line = Read.ReadLine().Split(new char[] { ' ' }, 2);
                if (int.Parse(line[0]) == NetwProg.myPortNr)
                    Console.WriteLine(line[1]);
                else
                    NetwProg.MessageService(line[0], line[1]);
            }
        }
        catch { } // Verbinding is kennelijk verbroken
    }
}
