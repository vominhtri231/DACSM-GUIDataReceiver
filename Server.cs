using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUIDataReceiver
{
    #region old server
    /*
    class Server
    {
        Socket socket;
        Byte[] buffer;
        EndPoint endPoint;
        public Server(int port)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            endPoint = new IPEndPoint(IPAddress.Any, port);
            socket.Bind(endPoint);

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            buffer = new byte[1024];
            socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref endPoint, new AsyncCallback(this.ReceiveData), endPoint);
            while (true) ;
        }

        private void ReceiveData(IAsyncResult asyncResult)
        {
            string s = Encoding.UTF8.GetString(buffer);
            s=s.TrimEnd();
            Console.WriteLine(s);
            buffer = new byte[1024];

            socket.EndReceiveFrom(asyncResult, ref endPoint);

            socket.BeginReceiveFrom(buffer, 0,buffer.Length, SocketFlags.None, ref endPoint, new AsyncCallback(this.ReceiveData), endPoint);
        }
    }
    */
    #endregion


    public class Server
    {
        public delegate void HandleMessage(string message);
        public delegate void SetPicture(Image image);
        public delegate void HandleWorker(ServerWorker worker);
        public delegate void Handle();

        public Socket socket;
        public List<ServerWorker> workers;
        public ServerListener listener;
        
        public Server(int port, HandleWorker addWorker, HandleWorker removeWorker)
        {
            workers = new List<ServerWorker>();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, port);
            socket.Bind(iPEndPoint);
            listener = new ServerListener(this, addWorker, removeWorker);
            new Thread(listener.Run).Start();
            
        }

        public void End()
        {
            listener.Stop();
            foreach (ServerWorker worker in workers)
            {
                worker.Stop();
            }
        }

    }
    

}
