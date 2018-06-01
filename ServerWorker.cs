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
        public HandleMessage transfer,setOutput,addFile,addDir;
        public SetPicture setPicture;
        public Handle closeConnect,completeTransInfo;
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
                        case 2:
                            string output = Encoding.UTF8.GetString(data);
                            if (setOutput != null) setOutput.Invoke(output);
                            break;
                        case 3:
                            string dirName = Encoding.UTF8.GetString(data);
                            if (addDir != null) addDir.Invoke(dirName);
                            break;
                        case 4:
                            string fileName = Encoding.UTF8.GetString(data);
                            if (addFile != null) addFile.Invoke(fileName);
                            break;
                        case 9:
                            if (completeTransInfo != null) completeTransInfo.Invoke();
                            break;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    if (isRun)
                    {
                        if (closeConnect != null) closeConnect.Invoke();
                        removeWorker(this);
                        isRun = false;
                    }
                    
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
            using (StreamWriter stream = new StreamWriter(path + "/main.html", true, Encoding.UTF8))
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


        public void SendCommand(string command)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(command);
            client.Send(new byte[] { 0 });
            SendVarData(buffer);
        }

        public void SendPath(string path)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(path);
            client.Send(new byte[] { 1 });
            SendVarData(buffer);
        }

        private void SendVarData(byte[] data)
        {
            int total = 0;
            int size = data.Length;
            int dataleft = size;


            byte[] datasize = new byte[4];
            datasize = BitConverter.GetBytes(size);
            client.Send(datasize);

            while (total < size)
            {
                int sent = client.Send(data, total, dataleft, SocketFlags.None);
                total += sent;
                dataleft -= sent;
            }

        }

        public void Stop()
        {
            isRun = false;
            client.Close();
        }

    }
}
