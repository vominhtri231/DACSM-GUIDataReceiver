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
    public partial class CommandMaker : Form
    {
        ServerWorker worker;
        HandleMessage sendCommand;
        int pos;
        public CommandMaker(ServerWorker worker)
        {
            this.worker = worker;
            this.pos = 0;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string command=this.textBox1.Text;
            if(sendCommand!=null) sendCommand.Invoke(command);
            receiveOuput(command);
        }

        private void CommandMaker_Load(object sender, EventArgs e)
        {
            sendCommand = worker.SendCommand;
            worker.setOutput = receiveOuput;
        }

        public void receiveOuput(string output)
        {
            if (this.panel1.InvokeRequired)
            {
                HandleMessage a = new HandleMessage(receiveOuput);
                Invoke(a, new object[] { output });
            }
            else
            {
                Label label = new Label();
                label.Text = output;
                label.Size = new Size(600, 20);
                label.Location = new Point(0, pos);
                pos += label.Height;
                label.Parent = panel1;
                panel1.Controls.Add(label);
            }
        }

        private void CommandMaker_FormClosed(object sender, FormClosedEventArgs e)
        {
            worker.setOutput = null;
        }
    }
}
