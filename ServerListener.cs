using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static GUIDataReceiver.Server;

namespace GUIDataReceiver
{
    public class ServerListener
    {
        Server server;
        bool isRun;
        HandleWorker addWorker, removeWorker;
        public ServerListener(Server server, HandleWorker addWorker, HandleWorker removeWorker)
        {
            this.server = server;
            isRun = true;
            this.addWorker = addWorker;
            this.removeWorker = removeWorker;
        }
        public void Run()
        {
            server.socket.Listen(10);
            while (isRun)
            {
                Thread.Sleep(30);
                try
                {
                    Socket client = server.socket.Accept();
                    ServerWorker worker = new ServerWorker(client, removeWorker);
                    server.workers.Add(worker);
                    new Thread(worker.Run).Start();
                    addWorker(worker);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }


            }
        }

        public void Stop()
        {
            isRun = false;
            server.socket.Close();
        }
    }
}
