using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GUIDataReceiver.Server;

namespace GUIDataReceiver
{
    public partial class DataForm : Form
    {
        ServerWorker worker;
        HandleWorker endWorking;
        CommandMaker commandMaker;
        FileSystem fileSystem;
        int pos;

        public DataForm(ServerWorker worker, HandleWorker endWorking)
        {
            InitializeComponent();
            this.worker = worker;
            this.endWorking = endWorking;
            this.labelName.Text = worker.client.RemoteEndPoint.ToString();
            pos = 0;
        }

        private void DataForm_Load(object sender, EventArgs e)
        {
            worker.transfer = UpdateList;
            worker.setPicture = SetIma;
            worker.closeConnect = EndConnection;
        }

        private void DataForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            worker.transfer = null;
            worker.setPicture = null;
            worker.closeConnect = null;
            Invoke(endWorking, new object[] { this.worker });
        }

        private void EndConnection()
        {
            if (this.InvokeRequired)
            {
                Handle a = new Handle(EndConnection);
                Invoke(a);
            }
            else
            {
                if(commandMaker!=null) commandMaker.Dispose();
                if (fileSystem != null) fileSystem.Dispose();
                this.Dispose();
            }
        }

        private void UpdateList(string message)
        {
            if (mainPanel.InvokeRequired)
            {
                HandleMessage a = new HandleMessage(UpdateList);
                Invoke(a, new object[] { message });
            }
            else
            {
                Label label = new Label();
                label.Text = message;
                label.Size = new Size(1200, 50);
                label.Location = new Point(0, pos);
                pos += label.Height  ;
                label.Parent = mainPanel;
                mainPanel.Controls.Add(label);
            }
            
            
        }

        private void SetIma(Image image)
        {
            if (mainPanel.InvokeRequired)
            {
                SetPicture a = new SetPicture(SetIma);
                Invoke(a, new object[] { image });
            }
            else
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.Width = 1200;
                pictureBox.Height = (int)((double)1200/image.Size.Width*image.Size.Height);
                pictureBox.Image = (Image)(new Bitmap(image, pictureBox.Size));
                pictureBox.Location = new Point(0, pos );
                pos += pictureBox.Height + 30;
                pictureBox.Parent = mainPanel;
                mainPanel.Controls.Add(pictureBox);
            }
            
        }

        private void button_makeCm_Click(object sender, EventArgs e)
        {
            commandMaker = new CommandMaker(this.worker);
            commandMaker.Show();
        }

        private void but_viewfile_Click(object sender, EventArgs e)
        {
            fileSystem = new FileSystem(this.worker);
            fileSystem.Show();
        }
    }
}
