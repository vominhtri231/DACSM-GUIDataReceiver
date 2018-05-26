using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using static GUIDataReceiver.Server;

namespace GUIDataReceiver
{
    public class ServerWorker
    {
        public HandleMessage transfer;
        public SetPicture setPicture;
        public Handle closeConnect;
        public Socket client;

        private bool isRun;
        private int num;
        private string path;
        private HandleWorker removeWorker;
        

        public ServerWorker(Socket client,HandleWorker removeWorker )
        {
            this.client = client;
            this.removeWorker = removeWorker;
            isRun = true;
            path = createStorePlace();
        }



        public void Run()
        {
            while (isRun)
            {
                try
                {
                    byte[] type = new byte[1];
                    client.Receive(type);
                    byte[] data = ReceiveVarData(client);

                    switch (type[0])
                    {
                        case 0:
                            string message = Encoding.UTF8.GetString(data);
                            if (transfer != null) transfer.Invoke(message);
                            save2File(message);
                            break;
                        case 1:
                            MemoryStream ms = new MemoryStream(data);
                            Image bmp = Image.FromStream(ms);
                            if (setPicture != null) setPicture.Invoke(bmp);
                            save2File(bmp);
                            break;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    if(isRun) removeWorker(this);
                    if(closeConnect!=null) closeConnect.Invoke();
                }
            }
        }

        private byte[] ReceiveVarData(Socket client)
        {

            byte[] datasize = new byte[4];
            client.Receive(datasize, 0, 4, 0);
            int size = BitConverter.ToInt32(datasize, 0);

            int total = 0;
            int dataleft = size;
            byte[] data = new byte[size];


            while (total < size)
            {
                int recv = client.Receive(data, total, dataleft, 0);
                if (recv == 0)
                {
                    break;
                }
                total += recv;
                dataleft -= recv;
            }
            return data;
        }

        private string createStorePlace()
        {
            string directoryName = client.RemoteEndPoint.ToString();
            directoryName = directoryName.Replace(":", ".");
            string path = "data/" + directoryName;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Directory.CreateDirectory(path + "/Images");
                FileStream fileStream = File.Create(path + "/main.html");
                fileStream.Close();

            }

            return path;
        }

        private void save2File(string text)
        {
            using (StreamWriter stream = new StreamWriter(path + "/main.html", true))
            {

                stream.WriteLine("<h4> " + text + "</h4>");
            }
        }

        public void save2File(Image image)
        {
            string imagePath = this.path + "/Images/" + this.num + ".png";
            image.Save(imagePath, ImageFormat.Png);
            using (StreamWriter stream = new StreamWriter(path + "/main.html", true))
            {
                stream.WriteLine("<img src='Images/" + this.num + ".png'> ");
            }
            this.num++;
        }

        public void Stop()
        {
            isRun = false;
            client.Close();
        }

    }
}
