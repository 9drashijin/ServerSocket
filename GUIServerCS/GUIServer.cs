using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Xml;

namespace GUIServerCS
{
    public partial class ServerForm : Form
    {
        public class StateObject
        {
            public Socket workSocket = null;
            public byte[] buffer = new byte[1024];
            public StringBuilder str = new StringBuilder();
        }
        public class Sequence
        {
            public String FuncID;
            public String Param;
            public String Input;
            public String Expect;
            public String Description;
        }
        public class TestCase
        {
            public List<Sequence> seq = new List<Sequence>();
            public String Module;
            public String Category;
            public String TestCases;
        }
        public class Module
        {
            public TestCase[] tc = new TestCase[128];
        }

        //Global Variable
        Socket serverSocket;
        Socket ClientHandler;
        IPEndPoint clientIP;
        StateObject state = new StateObject();
        public static ManualResetEvent ConnectionDone = new ManualResetEvent(false);
        System.Threading.ManualResetEvent workerBusy = new System.Threading.ManualResetEvent(false);
        String childParentTxt = String.Empty;
        String expectedResult = String.Empty, description = String.Empty;
        String failedTest = String.Empty;
        String currentTestCases = String.Empty, currentTestCasePadded = String.Empty;
        Module[] Mod = new Module[11]; // total module size
        String content = String.Empty; // Received string data
        bool SendButtonCheck = false, stopSendFlag = false;
        int counter = 0, tempCounter = 0, GparentIndex, GchildIndex, passTest = 0, tempPass = 0;
        int pBarMax = 0;
        StreamWriter report = new StreamWriter("ResultLog.txt", true); //Text file at current directory

        public ServerForm()
        {
            try
            {
                // Init GUI
                InitializeComponent();

                // Init Tree View 
                buildTreeView();

                // Background Server
                backgroundWorker1.RunWorkerAsync();
            }
            catch (Exception) { throw; }
            finally { Application.Exit(); }
        }
        public void buildTreeView()
        {
            TestCase[] TC = new TestCase[128];
            Sequence[] Seq = new Sequence[128];
            int i = 0, j = 0, k = 0, l = 0, tcIndex = 1;
            int tempI = 0, tempJ = 0, tempK = 0, tempL = 0;

            // Get the xml Directory
            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.xml");

            // Must call BeginUpdate when updating the tree
            TestMenuTree.BeginUpdate();
            foreach (string file in files)
            {
                // check for file existing
                if (!File.Exists(file))
                {
                    MessageBox.Show("File Not Found");
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(file); // load the file to the form

                //Cases to avoid multiple creation of TestMenu for multiple file
                if (!TestMenuTree.Nodes.ContainsKey("TestMenu"))
                {
                    TestMenuTree.Nodes.Add(new TreeNode("TestMenu") { Name = "TestMenu" });
                }

                // Loop to build the tree from the xml file
                foreach (XmlNode xNode in xmlDoc.SelectNodes("TestMenu"))
                {
                    Mod[j] = new Module();
                    if (!TestMenuTree.Nodes[0].Nodes.ContainsKey(xNode.SelectSingleNode("Category").InnerText))
                    {
                        TestMenuTree.Nodes[0].Nodes.Add(new TreeNode(xNode.SelectSingleNode("Category").InnerText) { Name = xNode.SelectSingleNode("Category").InnerText });
                    }
                    if (!TestMenuTree.Nodes[0].Nodes[0].Nodes.ContainsKey(xNode.SelectSingleNode("Module").InnerText))
                    {
                        TestMenuTree.Nodes[0].Nodes[0].Nodes.Add(new TreeNode(xNode.SelectSingleNode("Module").InnerText) { Name = xNode.SelectSingleNode("Module").InnerText });
                    }
                    else
                    {
                        i = tempI;
                        j = tempJ;
                        k = tempK;
                        l = tempL;
                    }

                    XmlNodeList testCaseList = xmlDoc.GetElementsByTagName("TestCase");
                    foreach (XmlNode tcNode in testCaseList)
                    {
                        TestMenuTree.Nodes[0].Nodes[0].Nodes[i].Nodes.Add(new TreeNode(tcNode.Attributes["tc"].InnerText));
                    }

                    foreach (XmlNode xNode1 in xNode.SelectNodes("TestCase"))
                    {
                        Mod[j].tc[k] = new TestCase();
                        Mod[j].tc[k].Category = xNode.SelectSingleNode("Category").InnerText;
                        Mod[j].tc[k].Module = xNode.SelectSingleNode("Module").InnerText;
                        Mod[j].tc[k].TestCases = xNode1.SelectSingleNode("//TestCase[" + tcIndex + "]").Attributes["tc"].Value;
                        foreach (XmlNode xNode2 in xNode1.SelectNodes("SeqNum"))
                        {

                            Seq[l] = new Sequence();
                            Seq[l].FuncID = xNode2.SelectSingleNode("FuncID").InnerText;
                            Seq[l].Param = xNode2.SelectSingleNode("Param").InnerText;
                            Seq[l].Expect = xNode2.SelectSingleNode("Expect").InnerText;
                            Seq[l].Description = xNode2.SelectSingleNode("Desc").InnerText;
                            Mod[j].tc[k].seq.Add(Seq[l]);
                            l++;
                        }
                        k++;
                        tcIndex++;
                        tempL = l;
                        l = 0;
                    }
                    tempK = k;
                    k = 0;
                    tcIndex = 1;
                }
                tempJ = j;
                tempI = i;
                j++;
                i++;
            }
            TestMenuTree.EndUpdate();
        }
        public void DisplayServerInfo()
        {
            IPAddress IpAddressv6 = Dns.GetHostAddresses(Dns.GetHostName())[0];
            IPAddress IpAddressv4 = Dns.GetHostAddresses(Dns.GetHostName())[1];
            //Some delays to avoid GetHostAddresses fail
            Thread.Sleep(100);
            ServerInfo.AppendText("Host Name    : " + Dns.GetHostName() + Environment.NewLine);
            ServerInfo.AppendText("IPv4 Address : " + IpAddressv4.ToString() + Environment.NewLine);
            ServerInfo.AppendText("IPv6 Address : " + IpAddressv6.ToString() + Environment.NewLine);
            ServerInfo.AppendText("Server Port  : 8888" + Environment.NewLine);
        }
        public void DisplayClientInfo()
        {
            //Get the client information.
            clientIP = (IPEndPoint)ClientHandler.RemoteEndPoint;
            ClientInfo.AppendText("IPv4 Address : " + clientIP.Address + Environment.NewLine);
            ClientInfo.AppendText("Client Port  : " + clientIP.Port + Environment.NewLine);
        }
        public void ServerListen()
        {
            IPEndPoint localIP = new IPEndPoint(IPAddress.Any, 8888); // Server Port : 8888
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                serverSocket.Bind(localIP);
                serverSocket.Listen(10); // maximum 10 client
                receiveDisplay.AppendText("Waiting for connecntion..." + Environment.NewLine);

                ConnectionDone.Reset();

                serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), serverSocket);
                ConnectionDone.WaitOne();

            }
            catch (Exception) { }
        }
        public void AcceptCallBack(IAsyncResult asyn)
        {
            ConnectionDone.Set();
            Socket ClientListener = (Socket)asyn.AsyncState;
            ClientHandler = ClientListener.EndAccept(asyn);

            receiveDisplay.AppendText("A Client has joined the server." + Environment.NewLine);
            state.workSocket = ClientHandler;
        }
        public void Read(StateObject state)
        {
            ClientHandler.BeginReceive(state.buffer, 0, 1024, 0, new AsyncCallback(ReadCallBack), state);
        }
        public void ReadCallBack(IAsyncResult asyn)
        {
            StateObject state = (StateObject)asyn.AsyncState;
            Socket handler = state.workSocket;

            int bytesRead = handler.EndReceive(asyn);
            if (bytesRead > 0)
            {
                //ProgressBar function
                toolStripProgressBar1.Maximum = pBarMax;
                toolStripProgressBar1.Increment(1);

                // To find the progress bar percentage
                int percent = (int)(((double)toolStripProgressBar1.Value / (double)toolStripProgressBar1.Maximum) * 100);
                percentageDisplay.Text = percent.ToString() + "%";

                // To smoothen the updating of the progress bar
                int value = toolStripProgressBar1.Value;
                if (value == toolStripProgressBar1.Maximum)//To correctly update progress bar
                {
                    toolStripProgressBar1.Maximum = value + 1;
                    toolStripProgressBar1.Value = value + 1;
                    toolStripProgressBar1.Maximum = value;
                }
                else
                {
                    toolStripProgressBar1.Value = value + 1;
                }
                toolStripProgressBar1.Value = value;

                currentTest.Text = "Current Test : " + (counter).ToString();
                // get the data and transfer to string builder
                state.str.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                content = state.str.ToString();
                
                if (expectedResult == "PROMPT_USER_INPUT")
                {
                    workerBusy.Reset();
                    content = String.Empty;
                    DialogResult result = MessageBox.Show(currentTestCases + Environment.NewLine +
                        "Please confirm if the test pass." + Environment.NewLine + description, "User Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        content += "PASS";
                    }
                    else if (result == DialogResult.No)
                    {
                        content += "FAIL";
                    }
                    else
                    {
                        content += "ABORTED";
                        stopSendFlag = true;
                        UncheckAll(TestMenuTree.Nodes);
                        CheckTicked(TestMenuTree.Nodes);
                    }
                    receiveDisplay.AppendText(currentTestCasePadded + content + Environment.NewLine);
                    report.WriteLine(currentTestCasePadded + content);
                }
                else if (expectedResult == "")
                {
                    tempPass++;
                }
                else if (content.Trim() == expectedResult)
                {
                    if(content == "TRUE" || content == "FALSE")
                    {
                        content = String.Empty;
                        content = "PASS";
                    }
                    else
                    {
                        content += " PASS";
                    }
                    receiveDisplay.AppendText(currentTestCasePadded + content + Environment.NewLine);
                    report.WriteLine(currentTestCasePadded + content);
                }
                else
                {
                    content += " FAIL";
                    receiveDisplay.AppendText(currentTestCasePadded + content + Environment.NewLine);
                    report.WriteLine(currentTestCasePadded + content);
                }
                if (content.Contains("PASS")) tempPass++;
                workerBusy.Set();
                expectedResult = String.Empty;
                content = String.Empty;
                state.str.Clear();
            }
        }
        public void Send(Socket handler, String data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallBack), handler);
        }
        public void SendCallBack(IAsyncResult asyn)
        {
            //do nothing
        }
        public void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            DisplayServerInfo();
            ServerListen();
            backgroundWorker2.RunWorkerAsync();
            DisplayClientInfo();

            int i = 0;
            while (worker.CancellationPending != true)
            {
                counter = 0;
                tempCounter = 0;
                pBarMax = 0;
                Thread.Sleep(100);

                if (SendButtonCheck == true)
                {
                    try
                    {
                        i++;
                        report.WriteLine("Test Result " + i + " - " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + "\r\n--------------------------------------");
                        toolStripProgressBar1.Value = 0;
                        CheckTotalTick(TestMenuTree.Nodes);
                        totalTest.Text = "Total Test : " + tempCounter.ToString();
                        CheckTicked(TestMenuTree.Nodes);
                        report.WriteLine();
                        receiveDisplay.AppendText("\r\n-------------------------\r\nOVERALL UNIT TEST SUMMARY\r\n-------------------------\r\nTESTED : "
                            + tempCounter + "\r\nPASSED : " + passTest + "\r\nFAILED : " + (tempCounter - passTest) + "\r\n-------------------------\r\n\r\n");
                    }
                    catch (Exception) { break; }
                }
                tempPass = 0;
                passTest = 0;
                SendButtonCheck = false;
                if (((state.workSocket.Poll(1, SelectMode.SelectRead) && (state.workSocket.Available == 0)) || !state.workSocket.Connected))
                {
                    state.workSocket.Close();
                    break;
                }
            }
            receiveDisplay.AppendText(clientIP.Address + ":" + clientIP.Port + " has disconnected from the server!" + Environment.NewLine);
        }
        public void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            while ((worker.CancellationPending != true))
            {
                Read(state);
                Thread.Sleep(100);
                if (((state.workSocket.Poll(1, SelectMode.SelectRead) && (state.workSocket.Available == 0)) || !state.workSocket.Connected))
                {
                    state.workSocket.Close();
                    break;
                }
            }
        }
        public void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            reConnect();
        }
        public void reConnect()
        {
            state.workSocket.Close();
            ClientHandler.Close();
            serverSocket.Close();
            ServerInfo.Clear();
            ClientInfo.Clear();
            backgroundWorker1.CancelAsync();
            backgroundWorker2.CancelAsync();
            backgroundWorker1.RunWorkerAsync();
        }
        private void TestMenuTree_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            DescBox.Clear();
            ExpectBox.Clear();

            TreeNode selNode = (TreeNode)TestMenuTree.GetNodeAt(TestMenuTree.PointToClient(Cursor.Position));
            if (selNode.Level == 0 || selNode.Level == 1 || selNode.Level == 2) { ; }
            else if (selNode != null)
            {
                int i = 0;
                for (i = 0; i < Mod[selNode.Parent.Index].tc[selNode.Index].seq.Count; i++)
                {
                    DescBox.AppendText(Mod[selNode.Parent.Index].tc[selNode.Index].seq[i].Description + Environment.NewLine);
                    ExpectBox.AppendText(Mod[selNode.Parent.Index].tc[selNode.Index].seq[i].Expect + Environment.NewLine);
                }
            }
        }
        private void TestMenuTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            DescBox.Clear();
            ExpectBox.Clear();
            CheckSelect(TestMenuTree.Nodes);
        }
        public void TestMenuTree_CheckedChanged(object sender, TreeViewEventArgs e)
        {
            TestMenuTree.BeginUpdate();

            TreeNode node = e.Node;
            foreach (TreeNode childNode in e.Node.Nodes)
            {
                childNode.Checked = e.Node.Checked;
            }
            TestMenuTree.SelectedNode = node;

            TestMenuTree.EndUpdate();
        }
        public void UncheckAll(TreeNodeCollection theNodes)
        {
            if (theNodes != null)
            {
                foreach (TreeNode child in theNodes)
                {
                    if (child.Checked)
                    {
                        child.Checked = false;
                    }
                    else
                    {
                        UncheckAll(child.Nodes);
                    }
                }
            }
        }
        public void CheckTicked(TreeNodeCollection theNodes)
        {
            if (theNodes != null)
            {
                foreach (TreeNode child in theNodes)
                {
                    if (child.Checked)
                    {
                        if (child.LastNode != null)
                        {
                            CheckTicked(child.Nodes);
                        }
                        else
                        {
                            try
                            {
                                GparentIndex = child.Parent.Index;
                                GchildIndex = child.Index;
                                checkIndex(child.Index, child.Parent.Index);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Test Cases not found!" + Environment.NewLine + e);
                            }
                        }
                    }
                    else
                    {
                        CheckTicked(child.Nodes);
                    }
                }
            }
        }
        public void CheckSelect(TreeNodeCollection theNodes)
        {
            if (theNodes != null)
            {
                foreach (TreeNode child in theNodes)
                {
                    if (child.IsSelected)
                    {
                        if (child.LastNode != null)
                        {
                            CheckSelect(child.Nodes);
                        }
                        else
                        {
                            int i = 0;
                            for (i = 0; i < Mod[child.Parent.Index].tc[child.Index].seq.Count; i++)
                            {
                                DescBox.AppendText(Mod[child.Parent.Index].tc[child.Index].seq[i].Description + Environment.NewLine);
                                ExpectBox.AppendText(Mod[child.Parent.Index].tc[child.Index].seq[i].Expect + Environment.NewLine);
                            }
                        }
                    }
                    else
                    {
                        CheckSelect(child.Nodes);
                    }
                }
            }
        }
        public void CheckTotalTick(TreeNodeCollection theNodes)
        {
            if (theNodes != null)
            {
                foreach (TreeNode child in theNodes)
                {
                    if (child.Checked)
                    {
                        if (child.LastNode != null)
                        {
                            CheckTotalTick(child.Nodes);
                        }
                        else
                        {
                            pBarMax += Mod[child.Parent.Index].tc[child.Index].seq.Count;
                            tempCounter++;
                        }
                    }
                    else
                    {
                        CheckTotalTick(child.Nodes);
                    }
                }
            }
        }
        public void checkIndex(int childIndex, int parentIndex)
        {
            stopSendFlag = false;
            currentModule.Text = "Current Module : " + Mod[parentIndex].tc[childIndex].Module;
            counter++;
            String inputData = String.Empty;
            int i = 0;
            description = String.Empty;
            DescBox.Clear();
            ExpectBox.Clear();
            for (i = 0; i < Mod[parentIndex].tc[childIndex].seq.Count; i++)
            {
                DescBox.AppendText(Mod[parentIndex].tc[childIndex].seq[i].Description + Environment.NewLine);
                ExpectBox.AppendText(Mod[parentIndex].tc[childIndex].seq[i].Expect + Environment.NewLine);
                if (stopSendFlag == true) break;
                expectedResult = String.Empty;
                inputData = Mod[parentIndex].tc[childIndex].seq[i].FuncID + Mod[parentIndex].tc[childIndex].seq[i].Param;
                expectedResult = Mod[parentIndex].tc[childIndex].seq[i].Expect;
                description += Mod[parentIndex].tc[childIndex].seq[i].Description + Environment.NewLine;
                currentTestCases = Mod[parentIndex].tc[childIndex].Module + " - " + Mod[parentIndex].tc[childIndex].TestCases;
                currentTestCasePadded = (Mod[parentIndex].tc[childIndex].Module).PadRight(18, ' ') + " - "
                                        + Mod[parentIndex].tc[childIndex].TestCases + "|Seq - " + (i + 1).ToString("D4") + " - ";
                failedTest = Mod[parentIndex].tc[childIndex].Module + " - " + Mod[parentIndex].tc[childIndex].TestCases
                             + " Failed , Sequence : " + (i + 1).ToString("D4");
                switch (Mod[parentIndex].tc[childIndex].Module.ToLower())
                {
                    case "annunciator":
                        Send(state.workSocket, "3001");
                        Thread.Sleep(100);
                        Send(state.workSocket, inputData);
                        if (Mod[parentIndex].tc[childIndex].seq[i].FuncID == "8000" || Mod[parentIndex].tc[childIndex].seq[i].FuncID == "0001")
                        {
                            Thread.Sleep((Int32.Parse(Mod[parentIndex].tc[childIndex].seq[i].Param)) + 100);
                        }
                        else
                        {
                            Thread.Sleep(100);
                        }
                        Send(state.workSocket, "exit");
                        Thread.Sleep(100);
                        break;

                    case "card reader":
                        throw new Exception("Error");
                    case "clock":
                        throw new Exception("Error");
                    case "contactless reader":
                        Send(state.workSocket, "3004");
                        Thread.Sleep(100);
                        Send(state.workSocket, inputData);
                        if (Mod[parentIndex].tc[childIndex].seq[i].FuncID == "8000")
                        {
                            Thread.Sleep((Int32.Parse(Mod[parentIndex].tc[childIndex].seq[i].Param)) + 100);
                        }
                        else
                        {
                            Thread.Sleep(100);
                        }
                        Send(state.workSocket, "exit");
                        Thread.Sleep(100);
                        break;

                    case "crypto":
                        Send(state.workSocket, "3005");
                        Thread.Sleep(100);
                        Send(state.workSocket, inputData);
                        Thread.Sleep(50);
                        Send(state.workSocket, "exit");
                        Thread.Sleep(100);
                        break;

                    case "display":
                        Send(state.workSocket, "3006");
                        Thread.Sleep(100);
                        Send(state.workSocket, inputData);
                        Send(state.workSocket, "exit");
                        Thread.Sleep(100);
                        break;

                    case "file":
                        throw new Exception("Error");
                    case "hardware layer":
                        Send(state.workSocket, "3008");
                        Thread.Sleep(100);
                        Send(state.workSocket, inputData);
                        Send(state.workSocket, "exit");
                        Thread.Sleep(100);
                        break;

                    case "keypad":
                        Send(state.workSocket, "3009");
                        Thread.Sleep(100);
                        Send(state.workSocket, inputData);
                        Send(state.workSocket, "exit");
                        Thread.Sleep(100);
                        break;

                    case "magstr reader":
                        Send(state.workSocket, "3010");
                        Thread.Sleep(100);
                        Send(state.workSocket, inputData);
                        Send(state.workSocket, "exit");
                        Thread.Sleep(100);
                        break;

                    case "printer":
                        Send(state.workSocket, "3011");
                        Thread.Sleep(100);
                        Send(state.workSocket, inputData);
                        Send(state.workSocket, "exit");
                        Thread.Sleep(100);
                        break;

                    default: break;
                }
                workerBusy.WaitOne();
                expectedResult = String.Empty;
            }
            passTest += tempPass / Mod[parentIndex].tc[childIndex].seq.Count;
            tempPass = 0;
        }
        public void SendButton_Click(object sender, EventArgs e)
        {
            SendButtonCheck = true;
            receiveDisplay.Focus();
        }
        public void ClearButton_Click(object sender, EventArgs e)
        {
            UncheckAll(TestMenuTree.Nodes);
            receiveDisplay.Clear();
            toolStripProgressBar1.Value = 0;
            percentageDisplay.Text = "";
            currentTest.Text = "Current Test : 0";
            totalTest.Text = "Total Test : 0";
            currentModule.Text = "Current Module : ";
            DescBox.Text = String.Empty;
            ExpectBox.Text = String.Empty;
        }
        public void timer1_Tick(object sender, EventArgs e)
        {
            timerDisplay.Text = "Server Time : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
        }
        public void ServerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult closing = MessageBox.Show("Do you want to Restart the Program?", "Program Terminating...", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
            if (closing == DialogResult.Yes) Application.Restart();
            report.Close();
            backgroundWorker1.CancelAsync();
            backgroundWorker2.CancelAsync();
            backgroundWorker1.Dispose();
            backgroundWorker2.Dispose();
            System.Environment.Exit(1);
        }
    }
}
