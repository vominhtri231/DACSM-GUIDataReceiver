using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GUIDataReceiver.Server;

namespace GUIDataReceiver
{
    public partial class FileSystem : Form
    {
        ServerWorker worker;
        HandleMessage sendPath;
        TreeNode treeNode;
        public FileSystem(ServerWorker worker)
        {
            this.worker = worker;
            InitializeComponent();
        }


        private void FileSystem_Load(object sender, EventArgs e)
        {
            sendPath = worker.SendPath;
            worker.addDir = AddDirectoryInTree;
            worker.addFile = AddFileInTree;
            worker.completeTransInfo = Complete;


            tree.ImageList = imageList;
            TreeNode node = new TreeNode("c:\\");
            node.ImageKey = "folder";
            node.SelectedImageKey = "folder";
            this.tree.Nodes.Add(node);
        }

        private void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tree.SelectedNode.ImageKey == "folder"&& sendPath != null)
            {          
                sendPath.Invoke(tree.SelectedNode.FullPath);
                treeNode = tree.SelectedNode;
                tree.Enabled = false;
            }              
        }

        public void AddDirectoryInTree(string directoryName)
        {
            if (tree.InvokeRequired)
            {
                HandleMessage handleMessage = new HandleMessage(AddDirectoryInTree);
                Invoke(handleMessage, new object[] { directoryName });
            }
            else
            {
                TreeNode subNode = new TreeNode(directoryName);
                subNode.ImageKey = "folder";
                subNode.SelectedImageKey = "folder";
                treeNode.Nodes.Add(subNode);
            }
            
        }

        public void AddFileInTree(string fileName)
        {
            if (tree.InvokeRequired)
            {
                HandleMessage handleMessage = new HandleMessage(AddFileInTree);
                Invoke(handleMessage, new object[] { fileName });
            }
            else
            {
                TreeNode subNode = new TreeNode(fileName);
                subNode.ImageKey = "file";
                subNode.SelectedImageKey = "file";
                treeNode.Nodes.Add(subNode);
            }
            
        }

        public void Complete()
        {
            if (tree.InvokeRequired)
            {
                Handle handle = new Handle(Complete);
                Invoke(handle);
            }
            else
            {
                treeNode.Expand();
                tree.Enabled = true;
            }
            
        }

        private void FileSystem_FormClosed(object sender, FormClosedEventArgs e)
        {
            worker.addDir = null;
            worker.addFile = null;
            worker.completeTransInfo = null;
        }
    }
}
