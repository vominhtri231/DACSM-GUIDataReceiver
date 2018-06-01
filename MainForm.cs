using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GUIDataReceiver.DataForm;
using static GUIDataReceiver.Server;

namespace GUIDataReceiver
{
    public partial class MainForm : Form
    {
        Server server;
        Dictionary<ServerWorker,bool> workers;
        
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
                    
            server = new Server(6969, AddToListClient,RemoveFromListClient);
            workers = new Dictionary<ServerWorker, bool>();
        }

        public void AddToListClient(ServerWorker worker)
        {
            workers[worker] = false;
            if (listClient.InvokeRequired)
            {
                HandleWorker a = new HandleWorker(AddToListClient);
                Invoke(a, new object[] { worker });
            }
            else
            {
                ListViewItem listViewItem = new ListViewItem(worker.client.RemoteEndPoint.ToString());
                listViewItem.SubItems.Add(worker.client.Connected.ToString());
                this.listClient.Items.Add(listViewItem);
            }
            
        }

        public void RemoveFromListClient(ServerWorker worker)
        {
            if (listClient.InvokeRequired)
            {
                HandleWorker a = new HandleWorker(RemoveFromListClient);
                Invoke(a, new object[] { worker });
            }
            else
            {
                string ep = worker.client.RemoteEndPoint.ToString();
                foreach (ListViewItem listViewItem in listClient.Items)
                {
                    if (listViewItem.Text.Equals(ep))
                    {
                        listClient.Items.Remove(listViewItem);
                        break;
                    }
                }
                listClient.Refresh();
            }           
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            server.End();
        }


        private void listClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach(ListViewItem listViewItem in listClient.SelectedItems)
            {
                string name=listViewItem.Text;
                ServerWorker worker =workers.Where(p => p.Key.client.RemoteEndPoint.ToString().Equals(name)).FirstOrDefault().Key;
                bool status=workers[worker];
                if (!status)
                {
                    DataForm dataForm = new DataForm(worker, EndForm);
                    dataForm.Show();
                    workers[worker] = true;
                }
            }
        }

        private void EndForm(ServerWorker worker)
        {
            workers[worker] = false;
        }
    }
}
