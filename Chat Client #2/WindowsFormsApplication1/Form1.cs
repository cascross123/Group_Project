using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        NetworkStream serverStream = default(NetworkStream);
        string readData = null;
        bool conGo = false;
        bool first = true;
        bool usage = false;
        bool reply1 = false;
        bool reply2 = false;
        bool reply3 = false;
        bool reply4 = false;
        bool finish = false;
        bool aiActivate = false;
        bool joined = true;
        int repName = 0;
        int spellMis1 = 0;
        int spellMiss2 = 0;
        int spellMiss3 = 0;
        string pcParts;
        string budgetS;
        string speedOrStorage;
        string caseS;
        bool exi = false;
        bool typing = false;



        public Form1()
        {
            
                InitializeComponent();
            
            this.AcceptButton = buttonSend;
         
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            try {

                for (int n = listBoxChat.Items.Count - 1; n >= 0; --n)
                {
                    if (listBoxChat.Items[n].ToString().ToLower().Contains("is typing...".ToLower()))
                    {
                        listBoxChat.Items.RemoveAt(n);
                    }
                }

                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes(textBoxMsg.Text + "$");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();

                textBoxMsg.Clear();

                typing = false;
            }
            catch
            {
                MessageBox.Show("Please enter a name.", "Fatal Windows Forms Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            
            Random rnd = new Random();
            repName = rnd.Next(4);
            spellMis1 = rnd.Next(100);
            spellMiss2 = rnd.Next(100);
            spellMiss3 = rnd.Next(100);


            if (textBoxName.Text == "PCPartPickerG10")
            {
                aiActivate = true;
                //textBoxName.Text = "PCPartPicker";
                switch(repName)
                {
                    case 1:
                        {
                            textBoxName.Text = "Bob";
                            break;
                        }
                    case 2:
                        {
                            textBoxName.Text = "Jim";
                            break;
                        }
                    case 3:
                        {
                            textBoxName.Text = "Emma";
                            break;
                        }
                    default:
                        {
                            textBoxName.Text = "Emma";
                            break;
                        }
                }

            }
           
                readData = "Conected to Server, a representative will be with you shortly...";
                msg();
                /////////////////////////////////////////////////////////////// - CHANGE THIS IP
                //ipv4 address uni labs - , uni lib - .
                clientSocket.Connect("127.0.0.1", 8888);
                ///////////////////////////////////////////////////////////////hjhj
                serverStream = clientSocket.GetStream();

                byte[] outstream = System.Text.Encoding.ASCII.GetBytes(textBoxName.Text + "$");
                serverStream.Write(outstream, 0, outstream.Length);
                serverStream.Flush();

                if (conGo == true)
                {

                    buttonConnect.Text = "Connected!";
                    buttonConnect.Enabled = false;
                    textBoxMsg.Focus();
                }



                Thread ctThread = new Thread(getMessage);
                ctThread.Start();
           
        }

        private void getMessage()
        {
            while (true)
            {
                serverStream = clientSocket.GetStream();
                int buffSize = 20025;
                byte[] inStream = new byte[20025];
                //buffSize = clientSocket.ReceiveBufferSize;
                serverStream.Read(inStream, 0, buffSize);
                string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                readData = "" + returndata;
                msg();
            }
        }

        private void msg()
        {

            for (int n = listBoxChat.Items.Count - 1; n >= 0; --n)
            {

                if (typing == false && listBoxChat.Items[n].ToString().ToLower().Contains("is typing...".ToLower()))
                {
                    listBoxChat.Items.RemoveAt(n);
                }
            }

            if (first == true)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(msg));

                }
                else
                {
                    listBoxChat.Items.Add("Server" + " >> " + readData);

                }
                first = false;
            }
            else
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(msg));

                }
                else
                {
                    listBoxChat.Items.Add(readData);
                    listBoxChatChanged();
                }

            }

        }

        //private string GetLocalIp()
        //{

        //    IPHostEntry host;

        //    host = Dns.GetHostEntry(Dns.GetHostName());

        //    foreach (IPAddress ip in host.AddressList)
        //    {
        //        if (ip.AddressFamily == AddressFamily.InterNetwork)
        //        {
        //            return ip.ToString();
        //        }

        //    }

        //    return "127.0.0.1";

        //}

        private void listBoxChat_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                e.DrawBackground();
                e.DrawFocusRectangle();
                e.Graphics.DrawString(
                     (string)listBoxChat.Items[e.Index],
                     e.Font,
                     new SolidBrush(e.ForeColor),
                     e.Bounds);
            }
            catch
            {
                textBoxName.Focus();
                MessageBox.Show("Please reopen, enter a name and connect", "Windows Forms Fatal Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
                exi = true;
            }
            finally
            {
                if (exi == true)
                {
                    Application.Exit();
                }
            }
        
            
        }

        private void listBoxChat_MeasureItem(object sender, MeasureItemEventArgs e)
        {
              
                e.ItemHeight = 13 * GetLinesNumber((string)listBoxChat.Items[e.Index]);
            
        }

        private int GetLinesNumber(string text)
        {
            int count = 1;
            int pos = 0;
            while ((pos = text.IndexOf("\r\n", pos)) != -1) { count++; pos += 2; }
            return count;
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (textBoxName.Text != String.Empty)
            {
                buttonConnect.Enabled = true;
                conGo = true;
            }
        }

        public void waitTime()
        {
            Thread.Sleep(5000);

        }



        private void listBoxChatChanged()
        {
            listBoxChat.SelectedIndex = listBoxChat.Items.Count - 1;
            listBoxChat.SelectedIndex = -1;

          

                if (aiActivate == true)
                {



                for (int n = listBoxChat.Items.Count - 1; n >= 0; --n)
                {

                    if (joined == true)
                    {
                        if (listBoxChat.Items[n].ToString().Contains("Joined") && !listBoxChat.Items[n].ToString().Contains("Emma") && !listBoxChat.Items[n].ToString().Contains("Bob") && !listBoxChat.Items[n].ToString().Contains("Jim") || listBoxChat.Items[n].ToString().ToLower().Contains("Hello".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("Hey".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("Hi".ToLower()))
                        {
                            Thread.Sleep(2000);
                            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                            serverStream.Write(outStream, 0, outStream.Length);
                            serverStream.Flush();
                            Thread.Sleep(5000);


                            switch (repName)
                            {
                                case 1:
                                    outStream = System.Text.Encoding.ASCII.GetBytes("Hello, my name is Bob, i will be your PC Part Picker \r\nassistant today." + "$");
                                    serverStream.Write(outStream, 0, outStream.Length);
                                    serverStream.Flush();
                                    break;

                                case 2:
                                    outStream = System.Text.Encoding.ASCII.GetBytes("Hello, my name is Jim, i will be your PC Part Picker \r\nassistant today." + "$");
                                    serverStream.Write(outStream, 0, outStream.Length);
                                    serverStream.Flush();
                                    break;

                                case 3:
                                    outStream = System.Text.Encoding.ASCII.GetBytes("Hello, my name is Emma, i will be your PC Part Picker \r\nassistant today." + "$");
                                    serverStream.Write(outStream, 0, outStream.Length);
                                    serverStream.Flush();
                                    break;

                                default:
                                    outStream = System.Text.Encoding.ASCII.GetBytes("Hello, my name is Emma, i will be your PC Part Picker \r\nassistant today." + "$");
                                    serverStream.Write(outStream, 0, outStream.Length);
                                    serverStream.Flush();
                                    break;


                            }
                            usage = true;
                            joined = false;
                        }
                    }


                    if (usage == true)
                    {
                        Thread.Sleep(2000);
                        byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                        serverStream.Write(outStream, 0, outStream.Length);
                        serverStream.Flush();
                        Thread.Sleep(5000);
                        outStream = System.Text.Encoding.ASCII.GetBytes("To start with can i ask you, what will be primarily using \r\nthe computer for?\r\n- Gaming\r\n- Web and Word\r\n- Video Editing\r\n- Home Theater\r\n- All rounder" + "$");
                        serverStream.Write(outStream, 0, outStream.Length);
                        serverStream.Flush();

                        usage = false;
                        reply1 = true;
                    }

                    if (reply1 == true)
                    {
                        if (!listBoxChat.Items[n].ToString().ToLower().Contains("-"))
                        {
                            if (listBoxChat.Items[n].ToString().ToLower().Contains("Gaming".ToLower()))
                            {
                                Thread.Sleep(2000);
                                byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                Thread.Sleep(5000);
                                outStream = System.Text.Encoding.ASCII.GetBytes("Right'o, and what budget do you have for your gaming pc? \r\n(GBP)\r\n- Low (300.00 - 500.00)\r\n- Medium (500.00 - 800.00)\r\n- High (800.00 - 1500.00)" + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                pcParts = "gaming";
                                reply1 = false;
                                reply2 = true;
                            }
                        }
                    }

                    if (reply1 == true)
                    {
                        if (!listBoxChat.Items[n].ToString().ToLower().Contains("-"))
                        {
                            if (listBoxChat.Items[n].ToString().ToLower().Contains("Web and Word".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("Web".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("Word".ToLower()))
                            {
                                Thread.Sleep(2000);
                                byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                Thread.Sleep(5000);
                                outStream = System.Text.Encoding.ASCII.GetBytes("Right'o, and what budget do you have for your Web and Word pc? \r\n(GBP)\r\n- Low (300.00 - 500.00)\r\n- Medium (500.00 - 800.00)" + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                pcParts = "web and word";
                                reply1 = false;
                                reply2 = true;
                            }
                        }
                    }
                    if (reply1 == true)
                    {
                        if (!listBoxChat.Items[n].ToString().ToLower().Contains("-"))
                        {

                            if (listBoxChat.Items[n].ToString().ToLower().Contains("Video Editing".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("Video".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("Editing".ToLower()))
                            {
                                Thread.Sleep(2000);
                                byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                Thread.Sleep(5000);
                                outStream = System.Text.Encoding.ASCII.GetBytes("Right'o, and what budget do you have for your Video Editing pc? \r\n(GBP)\r\n- Low (300.00 - 500.00)\r\n- Medium (500.00 - 800.00)\r\n- High (800.00 - 1500.00)" + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                pcParts = "video editing";
                                reply1 = false;
                                reply2 = true;
                            }
                        }
                    }

                    if (reply1 == true)
                    {
                        if (!listBoxChat.Items[n].ToString().ToLower().Contains("-"))
                        {
                            if (listBoxChat.Items[n].ToString().ToLower().Contains("Home Theater".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("Home".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("Theater".ToLower()))
                            {
                                Thread.Sleep(2000);
                                byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                Thread.Sleep(5000);
                                outStream = System.Text.Encoding.ASCII.GetBytes("Right'o, and what budget do you have for your Home Theater pc? \r\n(GBP)\r\n- Low (300.00 - 500.00)\r\n- Medium (500.00 - 800.00)" + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                pcParts = "home theater";
                                reply1 = false;
                                reply2 = true;
                            }
                        }
                    }

                    if (reply1 == true)
                    {
                        if (!listBoxChat.Items[n].ToString().ToLower().Contains("-"))
                        {
                            if (listBoxChat.Items[n].ToString().ToLower().Contains("All Rounder".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("All".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("Rounder".ToLower()))
                            {
                                Thread.Sleep(2000);
                                byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                Thread.Sleep(5000);
                                outStream = System.Text.Encoding.ASCII.GetBytes("Right'o, and what budget do you have for your All Rounder pc? \r\n(GBP)\r\n- Low (300.00 - 500.00)\r\n- Medium (500.00 - 800.00)" + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                pcParts = "all rounder";
                                reply1 = false;
                                reply2 = true;
                            }
                        }

                    }

                    
                    if (reply2 == true)
                    {
                        if (!listBoxChat.Items[n].ToString().ToLower().Contains("-"))
                        {
                            if (listBoxChat.Items[n].ToString().ToLower().Contains("Low".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("300".ToLower()))
                            {
                                Thread.Sleep(2000);
                                byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                Thread.Sleep(5000);
                                outStream = System.Text.Encoding.ASCII.GetBytes("Ok, do you prefer your computer to be -FAST \r\nor have a large amount of -STORAGE?" + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                budgetS = "low";
                                reply2 = false;
                                reply3 = true;
                            }
                        }
                    }

                    if (reply2 == true)
                    {
                        if (!listBoxChat.Items[n].ToString().ToLower().Contains("-"))
                        {
                            if (listBoxChat.Items[n].ToString().ToLower().Contains("Medium".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("500".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("Mid".ToLower()))
                            {
                                Thread.Sleep(2000);
                                byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                Thread.Sleep(5000);
                                outStream = System.Text.Encoding.ASCII.GetBytes("Ok, do you prefer your computer to be -FAST \r\nor have a large amount of -STORAGE?" + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                budgetS = "mid";
                                reply2 = false;
                                reply3 = true;
                            }
                        }
                    }

                    if (reply2 == true)
                    {
                        if (!listBoxChat.Items[n].ToString().ToLower().Contains("-"))
                        {
                            if (listBoxChat.Items[n].ToString().ToLower().Contains("High".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("800".ToLower()))
                            {
                                Thread.Sleep(2000);
                                byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                Thread.Sleep(5000);
                                outStream = System.Text.Encoding.ASCII.GetBytes("Ok, do you prefer your computer to be -FAST \r\nor have a large amount of -STORAGE?" + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                budgetS = "high";
                                reply2 = false;
                                reply3 = true;
                            }

                        }
                    }

                    if (reply3 == true)
                    {
                        if (!listBoxChat.Items[n].ToString().ToLower().Contains("-"))
                        {
                            if (pcParts == "video editing" || pcParts == "gaming")
                            {
                                if (listBoxChat.Items[n].ToString().ToLower().Contains("Not Sure".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("Dont know".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("for me".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("Pick".ToLower()))
                                {
                                    Thread.Sleep(2000);
                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                    serverStream.Write(outStream, 0, outStream.Length);
                                    serverStream.Flush();
                                    Thread.Sleep(5000);
                                    outStream = System.Text.Encoding.ASCII.GetBytes("Honestly in a " + pcParts + " PC I would go for a speed over storage." + "$");
                                    serverStream.Write(outStream, 0, outStream.Length);
                                    serverStream.Flush();                                   
                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                    serverStream.Write(outStream, 0, outStream.Length);
                                    serverStream.Flush();
                                    Thread.Sleep(5000);
                                    outStream = System.Text.Encoding.ASCII.GetBytes("Finally, in terms of physical size, would you prefer your computer to be \r\n- Large \r\n- Small" + "$");
                                    serverStream.Write(outStream, 0, outStream.Length);
                                    serverStream.Flush();
                                    speedOrStorage = "speed";
                                    reply3 = false;
                                    reply4 = true;
                                }
                            }
                        }
                    }
                    
                    if (reply3 == true)
                    {
                        if (!listBoxChat.Items[n].ToString().ToLower().Contains("-"))
                        {
                            if (pcParts == "video editing" || pcParts == "gaming")
                            {
                                if (listBoxChat.Items[n].ToString().ToLower().Contains("fast".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("speed".ToLower()))
                                {
                                    Thread.Sleep(2000);
                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                    serverStream.Write(outStream, 0, outStream.Length);
                                    serverStream.Flush();
                                    Thread.Sleep(5000);
                                    outStream = System.Text.Encoding.ASCII.GetBytes("Fantastic, finally, in terms of physical size, would you prefer your computer to be \r\n- Large \r\n- Small" + "$");
                                    serverStream.Write(outStream, 0, outStream.Length);
                                    serverStream.Flush();
                                    speedOrStorage = "speed";
                                    reply3 = false;
                                    reply4 = true;
                                }
                            }
                        }
                    }
                    
                    if (reply3 == true)
                    {
                        if (!listBoxChat.Items[n].ToString().ToLower().Contains("-"))
                        {
                            if (pcParts == "video editing" || pcParts == "gaming")
                            {
                                if (listBoxChat.Items[n].ToString().ToLower().Contains("storage".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("space".ToLower()))
                                {
                                    Thread.Sleep(2000);
                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                    serverStream.Write(outStream, 0, outStream.Length);
                                    serverStream.Flush();
                                    Thread.Sleep(5000);
                                    outStream = System.Text.Encoding.ASCII.GetBytes("Fantastic, finally, in terms of physical size, would you prefer your computer to be \r\n- Large \r\n- Small" + "$");
                                    serverStream.Write(outStream, 0, outStream.Length);
                                    serverStream.Flush();
                                    speedOrStorage = "storage";
                                    reply3 = false;
                                    reply4 = true;
                                }
                            }
                        }
                    }
                    
                    if (reply3 == true)
                    {
                        if (!listBoxChat.Items[n].ToString().ToLower().Contains("-"))
                        {
                            if (listBoxChat.Items[n].ToString().ToLower().Contains("Not Sure".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("Dont know".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("for me".ToLower()))
                            {
                                Thread.Sleep(2000);
                                byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                Thread.Sleep(5000);
                                outStream = System.Text.Encoding.ASCII.GetBytes("Honestly in a" + pcParts + " PC I would go for storage." + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();                                
                                outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                Thread.Sleep(3000);
                                outStream = System.Text.Encoding.ASCII.GetBytes("Finally, in terms of physical size, would you prefer your computer to be \r\n- Large \r\n- Small" + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                speedOrStorage = "storage";
                                reply3 = false;
                                reply4 = true;
                            }
                        }
                    }
                    
                    if (reply3 == true)
                    {
                        if (!listBoxChat.Items[n].ToString().ToLower().Contains("-"))
                        {
                            if (listBoxChat.Items[n].ToString().ToLower().Contains("fast".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("speed".ToLower()))
                            {
                                Thread.Sleep(2000);
                                byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                Thread.Sleep(5000);
                                outStream = System.Text.Encoding.ASCII.GetBytes("Fantastic, finally, in terms of physical size, would you prefer your computer to be \r\n- Large\r\n- Medium\r\n- Small" + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                speedOrStorage = "speed";
                                reply3 = false;
                                reply4 = true;
                            }
                        }
                    }
                    
                        if (reply3 == true)
                    {
                        if (!listBoxChat.Items[n].ToString().ToLower().Contains("-"))
                        {
                            if (listBoxChat.Items[n].ToString().ToLower().Contains("storage".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("space".ToLower()))
                            {
                                Thread.Sleep(2000);
                                byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                Thread.Sleep(5000);
                                outStream = System.Text.Encoding.ASCII.GetBytes("Fantastic, finally, in terms of physical size, would you prefer your computer to be \r\n- Large\r\n- Medium\r\n- Small" + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                speedOrStorage = "storage";
                                reply3 = false;
                                reply4 = true;

                            }
                        }

                    }



                    if (reply4 == true)
                    {
                        if (!listBoxChat.Items[n].ToString().ToLower().Contains("-"))
                        {
                            if (pcParts == "video editing" || pcParts == "gaming")
                            {
                                if (listBoxChat.Items[n].ToString().ToLower().Contains("Suggest".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("Suggestion".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("Think".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("recommend".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("Would You".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("choose".ToLower()))
                                {
                                    Thread.Sleep(2000);
                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                    serverStream.Write(outStream, 0, outStream.Length);
                                    serverStream.Flush();
                                    Thread.Sleep(5000);
                                    outStream = System.Text.Encoding.ASCII.GetBytes("with a " + pcParts + " PC i would go for a large case." + "$");
                                    serverStream.Write(outStream, 0, outStream.Length);
                                    serverStream.Flush();                                    
                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                    serverStream.Write(outStream, 0, outStream.Length);
                                    serverStream.Flush();
                                    Thread.Sleep(3000);
                                    outStream = System.Text.Encoding.ASCII.GetBytes("Thats great, please give me a moment to gather\r\n your pc parts using the info you have provided me." + "$");
                                    serverStream.Write(outStream, 0, outStream.Length);
                                    serverStream.Flush();
                                    caseS = "not";
                                    pcBuild(pcParts, budgetS, speedOrStorage, caseS);
                                    reply4 = false;
                                    finish = true;
                                }
                            }
                        }
                    }

                    if (reply4 == true)
                    {
                        if (!listBoxChat.Items[n].ToString().ToLower().Contains("-"))
                        {                            
                                if (listBoxChat.Items[n].ToString().ToLower().Contains("Suggest".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("Suggestion".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("Think".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("recommend".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("Would You".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("choose".ToLower()))
                                {
                                Thread.Sleep(2000);
                                byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                Thread.Sleep(5000);
                                    outStream = System.Text.Encoding.ASCII.GetBytes("with a " + pcParts + " PC i would go for a small case." + "$");
                                    serverStream.Write(outStream, 0, outStream.Length);
                                    serverStream.Flush();
                               
                                outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                Thread.Sleep(2000);
                                outStream = System.Text.Encoding.ASCII.GetBytes("Thats great, please give me a moment to gather\r\n your pc parts using the info you have provided me." + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                    serverStream.Flush();
                                    caseS = "very";
                                    pcBuild(pcParts, budgetS, speedOrStorage, caseS);
                                    reply4 = false;
                                    finish = true;
                                }                            
                        }
                    }

                    if (reply4 == true)
                    {
                        if (!listBoxChat.Items[n].ToString().ToLower().Contains("-"))
                        {
                            if (listBoxChat.Items[n].ToString().ToLower().Contains("Small".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("Little".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("Smaller".ToLower()))
                            {
                                Thread.Sleep(2000);
                                byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                Thread.Sleep(5000);
                                outStream = System.Text.Encoding.ASCII.GetBytes("Thats great, please give me a moment to gather\r\n your pc parts using the info you have provided me." + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                caseS = "very";
                                pcBuild(pcParts, budgetS, speedOrStorage, caseS);
                                reply4 = false;
                                finish = true;
                            }
                        }
                    }

                    if (reply4 == true)
                    {
                        if (!listBoxChat.Items[n].ToString().ToLower().Contains("-"))
                        {
                            if (listBoxChat.Items[n].ToString().ToLower().Contains("Large".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("Bigger".ToLower()))
                            {
                                Thread.Sleep(2000);
                                byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                Thread.Sleep(5000);
                                outStream = System.Text.Encoding.ASCII.GetBytes("Thats great, please give me a moment to gather\r\n your pc parts using the info you have provided me." + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                caseS = "not";
                                pcBuild(pcParts, budgetS, speedOrStorage, caseS);
                                reply4 = false;
                                finish = true;
                            }
                        }
                    }

                    if (reply4 == true)
                    {
                        if (!listBoxChat.Items[n].ToString().ToLower().Contains("-"))
                        {
                            if (listBoxChat.Items[n].ToString().ToLower().Contains("Medium".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("Not".ToLower()) || listBoxChat.Items[n].ToString().ToLower().Contains("Middle".ToLower()))
                            {
                                Thread.Sleep(2000);
                                byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                Thread.Sleep(5000);
                                outStream = System.Text.Encoding.ASCII.GetBytes("Thats great, please give me a moment to gather\r\nyour pc parts using the info you have provided me." + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                caseS = "somewhat";
                                pcBuild(pcParts, budgetS, speedOrStorage, caseS);
                                reply4 = false;
                                finish = true;
                            }
                        }
                    }
                    
                
                            if (reply4 == false && finish == true)
                            {
                        
                                byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                Thread.Sleep(5000);
                                outStream = System.Text.Encoding.ASCII.GetBytes("Thanks for using PC PART PICKER, I hope i have been some\r\nhelp in the creation of your pc.Please leave any feedback on this chat. farewell!" + "$");
                                serverStream.Write(outStream, 0, outStream.Length);
                                serverStream.Flush();
                                finish = false;
                            }


                        }


                    }

                
            }
            
        


        void pcBuild(string pcParts, string budgetS, string speedOrStorage, string caseS)
        {
            

            switch (pcParts)
            {
                case "gaming":
                {
                        switch(caseS)
                        {
                            case "very":
                                {
                                    switch(budgetS)
                                    {
                                        case "high":
                                            {
                                                if(speedOrStorage == "speed")
                                                {

                                                    //high cost small case with ssd gaming here                                                    
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);                                                    
                                                    Thread.Sleep(10000);                                                    
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Core i7-6700K 4.0GHz Quad-Core Processor(Aria PC)\r\nCOOLER:\r\nNoctua NH-L9i 57.5 CFM CPU Cooler(Ebuyer)\r\nMOTHERBOARD:\r\nGigabyte GA-Z170MX-Gaming 5 Micro ATX LGA1151 Motherboard(More Computers)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);                                                    
                                                    Thread.Sleep(3000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nG.Skill TridentZ Series 16GB (2 x 8GB) DDR4-3200 Memory(More Computers)\r\nSTORAGE:\r\nSamsung 850 EVO-Series 250GB Solid State Drive(Amazon UK)\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nVIDEO CARD:\r\nEVGA GeForce GTX 980 Ti 6GB Superclocked Video Card(Amazon UK)\r\nCASE:\r\nCorsair 350D Window MicroATX Mid Tower Case(Amazon UK)\r\nPOWER SUPPLY:\r\nEVGA SuperNOVA G2 650W 80+ Gold Certified Fully-Modular ATX Power Supply(Amazon UK)\r\nOPTICAL DRIVE:\r\nSamsung SH-224DB/BEBE DVD/CD Writer(Ebuyer)/r/nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n1458.18(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();                                                    
                                                    
                                                }

                                                if(speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);                                                    
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Core i7-6700K 4.0GHz Quad-Core Processor(Aria PC)\r\nCOOLER:\r\nNoctua NH-L9i 57.5 CFM CPU Cooler(Ebuyer)\r\nMOTHERBOARD:\r\nGigabyte GA-Z170MX-Gaming 5 Micro ATX LGA1151 Motherboard(More Computers)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);                                                    
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nG.Skill TridentZ Series 16GB (2 x 8GB) DDR4-3200 Memory(More Computers)\r\nSTORAGE:\r\nWestern Digital Caviar Blue 2TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nVIDEO CARD:\r\nEVGA GeForce GTX 980 Ti 6GB Superclocked Video Card(Amazon UK)\r\nCASE:\r\nCorsair 350D Window MicroATX Mid Tower Case(Amazon UK)\r\nPOWER SUPPLY:\r\nEVGA SuperNOVA G2 650W 80+ Gold Certified Fully-Modular ATX Power Supply(Amazon UK)\r\nOPTICAL DRIVE:\r\nSamsung SH-224DB/BEBE DVD/CD Writer(Ebuyer)/r/nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n1438.18(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                break;
                                            }
                                        case "mid":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:Intel Core i5-6400 2.7GHz Quad-Core Processor(Aria PC)\r\nCOOLER:\r\nCooler Master Hyper TX3 54.8 CFM Sleeve Bearing CPU Cooler(Amazon UK)\r\nMOTHERBOARD:\r\nAsus H110I-PLUS D3/CSM Mini ITX LGA1151 Motherboard(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1600 Memory(More Computers)\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nVIDEO CARD:\r\nMSI Radeon R9 390 8GB Video Card(Amazon UK)\r\nCASE:\r\nZalman M1 Mini ITX Tower Case(More Computers)\r\nPOWER SUPPLY:\r\n: EVGA SuperNOVA NEX 750W 80+ Bronze Certified Semi-Modular ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n780.32(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:Intel Core i5-6400 2.7GHz Quad-Core Processor(Aria PC)\r\nCOOLER:\r\nCooler Master Hyper TX3 54.8 CFM Sleeve Bearing CPU Cooler(Amazon UK)\r\nMOTHERBOARD:\r\nAsus H110I-PLUS D3/CSM Mini ITX LGA1151 Motherboard(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1600 Memory(More Computers)\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nVIDEO CARD:\r\nMSI Radeon R9 390 8GB Video Card(Amazon UK)\r\nCASE:\r\nZalman M1 Mini ITX Tower Case(More Computers)\r\nPOWER SUPPLY:\r\n: EVGA SuperNOVA NEX 750W 80+ Bronze Certified Semi-Modular ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n753.32(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                        case "low":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD A8-5500 3.2GHz Quad-Core Processor(Amazon UK)\r\nMOTHERBOARD:\r\nGigabyte GA-F2A88XN-WIFI Mini ITX FM2+ Motherboard(Amazon UK)\r\nMEMORY:\r\n Corsair Vengeance 8GB (2 x 4GB) DDR3-1600 Memory(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nVIDEO CARD:\r\nEVGA GeForce GTX 750 Ti 2GB Superclocked Video Card(Dabs)\r\nCASE:\r\nZalman M1 Mini ITX Tower Case(More Computers)\r\nPOWER SUPPLY:\r\nEVGA 500W 80+ Bronze Certified ATX Power Supply(Amazon UK)\r\nOPTICAL DIRVE:\r\nLite-On iHAS124-04 DVD/CD Writer(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n491.24(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD A8-5500 3.2GHz Quad-Core Processor(Amazon UK)\r\nMOTHERBOARD:\r\nGigabyte GA-F2A88XN-WIFI Mini ITX FM2+ Motherboard(Amazon UK)\r\nMEMORY:\r\n Corsair Vengeance 8GB (2 x 4GB) DDR3-1600 Memory(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nVIDEO CARD:\r\nEVGA GeForce GTX 750 Ti 2GB Superclocked Video Card(Dabs)\r\nCASE:\r\nZalman M1 Mini ITX Tower Case(More Computers)\r\nPOWER SUPPLY:\r\nEVGA 500W 80+ Bronze Certified ATX Power Supply(Amazon UK)\r\nOPTICAL DIRVE:\r\nLite-On iHAS124-04 DVD/CD Writer(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n474.24(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case "not":
                                {
                                    switch(budgetS)
                                    {
                                        case "high":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Core i7-6700K 4.0GHz Quad-Core Processor(Aria PC)\r\nCOOLER:\r\nCooler Master Hyper 212 EVO 82.9 CFM Sleeve Bearing CPU Cooler(Novatech)\r\nMOTHERBOARD:\r\nMSI Z170A GAMING M5 ATX LGA1151 Motherboard(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nG.Skill TridentZ Series 16GB (2 x 8GB) DDR4-3200 Memory(More Computers)\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nSamsung 850 EVO-Series 250GB 2.5 Solid State Drive(Amazon UK)\r\nVIDEO CARD:\r\nMSI GeForce GTX 980 Ti 6GB Video Card(Amazon UK)\r\nCASE:\r\nFractal Design Define R5 w/Window (Black) ATX Mid Tower Case(Novatech)\r\nPOWER SUPPLY:\r\nEVGA SuperNOVA G2 650W 80+ Gold Certified Fully-Modular ATX Power Supply(Amazon UK)\r\nOPTICAL DRIVE:\r\nSamsung SH-224DB/BEBE DVD/CD Writer(Ebuyer)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n1470.22(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Core i7-6700K 4.0GHz Quad-Core Processor(Aria PC)\r\nCOOLER:\r\nCooler Master Hyper 212 EVO 82.9 CFM Sleeve Bearing CPU Cooler(Novatech)\r\nMOTHERBOARD:\r\nMSI Z170A GAMING M5 ATX LGA1151 Motherboard(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nG.Skill TridentZ Series 16GB (2 x 8GB) DDR4-3200 Memory(More Computers)\r\nSTORAGE:\r\nWestern Digital Caviar Blue 2TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nVIDEO CARD:\r\nMSI GeForce GTX 980 Ti 6GB Video Card(Amazon UK)\r\nCASE:\r\nFractal Design Define R5 w/Window (Black) ATX Mid Tower Case(Novatech)\r\nPOWER SUPPLY:\r\nEVGA SuperNOVA G2 650W 80+ Gold Certified Fully-Modular ATX Power Supply(Amazon UK)\r\nOPTICAL DRIVE:\r\nSamsung SH-224DB/BEBE DVD/CD Writer(Ebuyer)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n1450.22(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                        case "mid":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD FX-8350 4.0GHz 8-Core Processor(Amazon UK)\r\nCOOLER:\r\nCooler Master Hyper TX3 54.8 CFM Sleeve Bearing CPU Cooler(Amazon UK)\r\nMOTHERBOARD:\r\nMSI 970 GAMING ATX AM3+ Motherboard(Novatech)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1600 Memory(More Computers)\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nVIDEO CARD:\r\nMSI Radeon R9 390 8GB Video Card(Amazon UK)\r\nCASE:\r\nCorsair SPEC-01 RED ATX Mid Tower Case(Ebuyer)\r\nPOWER SUPPLY:\r\nEVGA SuperNOVA NEX 750W 80+ Bronze Certified Semi-Modular ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n789.96(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD FX-8350 4.0GHz 8-Core Processor(Amazon UK)\r\nCOOLER:\r\nCooler Master Hyper TX3 54.8 CFM Sleeve Bearing CPU Cooler(Amazon UK)\r\nMOTHERBOARD:\r\nMSI 970 GAMING ATX AM3+ Motherboard(Novatech)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1600 Memory(More Computers)\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nVIDEO CARD:\r\nMSI Radeon R9 390 8GB Video Card(Amazon UK)\r\nCASE:\r\nCorsair SPEC-01 RED ATX Mid Tower Case(Ebuyer)\r\nPOWER SUPPLY:\r\nEVGA SuperNOVA NEX 750W 80+ Bronze Certified Semi-Modular ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n776.96(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                        case "low":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD FX-6300 3.5GHz 6-Core Processor(Ebuyer)\r\nMOTHERBOARD:\r\nAsus M5A97 R2.0 ATX AM3+ Motherboard(Dabs)\r\nMEMORY:\r\nCorsair Vengeance 8GB (2 x 4GB) DDR3-1600 Memory(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nVIDEO CARD:\r\n:EVGA GeForce GTX 750 Ti 2GB Superclocked Video Card(Dabs)\r\nCASE:\r\nCooler Master Force 500 ATX Mid Tower Case(Amazon UK)\r\nPOWER SUPPLY:\r\n EVGA 500W 80+ Bronze Certified ATX Power Supply(Amazon UK)\r\nOPTICAL DRIVE:\r\nLite-On iHAS124-04 DVD/CD Writer(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n490.83(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD FX-6300 3.5GHz 6-Core Processor(Ebuyer)\r\nMOTHERBOARD:\r\nAsus M5A97 R2.0 ATX AM3+ Motherboard(Dabs)\r\nMEMORY:\r\nCorsair Vengeance 8GB (2 x 4GB) DDR3-1600 Memory(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK) \r\nVIDEO CARD:\r\n:EVGA GeForce GTX 750 Ti 2GB Superclocked Video Card(Dabs)\r\nCASE:\r\nCooler Master Force 500 ATX Mid Tower Case(Amazon UK)\r\nPOWER SUPPLY:\r\n EVGA 500W 80+ Bronze Certified ATX Power Supply(Amazon UK)\r\nOPTICAL DRIVE:\r\nLite-On iHAS124-04 DVD/CD Writer(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n477.83(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                    }
                                    break;
                                }
                        }

                        break;
                }

                case "web and word":
                {
                        switch (caseS)
                        {
                            case "very":
                                {
                                    switch(budgetS)
                                    {                                        
                                        case "mid":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Core i5-6400 2.7GHz Quad-Core Processor(Aria PC)\r\nCOOLER:\r\nCooler Master Hyper TX3 54.8 CFM Sleeve Bearing CPU Cooler(Amazon UK)\r\nMOTHERBOARD\r\nAsus H110I-PLUS D3/CSM Mini ITX LGA1151 Motherboard(Amazon UK)\r\nAsus H110I-PLUS D3/CSM Mini ITX LGA1151 Motherboard(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1866 Memory(More Computers)\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nVIDEO CARD:\r\nMSI Radeon R9 380 4GB Video Card(Amazon UK)\r\nCASE:\r\nCorsair SPEC-01 RED ATX Mid Tower Case(Ebuyer)\r\nPOWER SUPPLY:\r\nEVGA SuperNOVA NEX 650W 80+ Gold Certified Fully-Modular ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n634.68(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Core i5-6400 2.7GHz Quad-Core Processor(Aria PC)\r\nCOOLER:\r\nCooler Master Hyper TX3 54.8 CFM Sleeve Bearing CPU Cooler(Amazon UK)\r\nMOTHERBOARD\r\nAsus H110I-PLUS D3/CSM Mini ITX LGA1151 Motherboard(Amazon UK)\r\nAsus H110I-PLUS D3/CSM Mini ITX LGA1151 Motherboard(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1866 Memory(More Computers)\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nVIDEO CARD:\r\nMSI Radeon R9 380 4GB Video Card(Amazon UK)\r\nCASE:\r\nCorsair SPEC-01 RED ATX Mid Tower Case(Ebuyer)\r\nPOWER SUPPLY:\r\nEVGA SuperNOVA NEX 650W 80+ Gold Certified Fully-Modular ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n617.68(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                        case "low":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Pentium G3258 3.2GHz Dual-Core Processor(Amazon UK)\r\nMOTHERBOARD:/r/n: MSI H81I Mini ITX LGA1150 Motherboard(Amazon UK)\r\nMEMORY:\r\nPatriot Signature 8GB (2 x 4GB) DDR3-1600 Memory(Amazon UK) " + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nCASE:\r\nZalman M1 Mini ITX Tower Case(More Computers) \r\nPOWER SUPPLY:\r\nEVGA 430W 80+ Certified ATX Power Supply(Ebuyer)\r\nOPTICAL DRIVE:\r\nLite-On iHAS124-04 DVD/CD Writer(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n322.83(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Pentium G3258 3.2GHz Dual-Core Processor(Amazon UK)\r\nMOTHERBOARD:/r/n: MSI H81I Mini ITX LGA1150 Motherboard(Amazon UK)\r\nMEMORY:\r\nPatriot Signature 8GB (2 x 4GB) DDR3-1600 Memory(Amazon UK) " + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nCASE:\r\nZalman M1 Mini ITX Tower Case(More Computers) \r\nPOWER SUPPLY:\r\nEVGA 430W 80+ Certified ATX Power Supply(Ebuyer)\r\nOPTICAL DRIVE:\r\nLite-On iHAS124-04 DVD/CD Writer(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n305.83(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                    }

                                    break;
                                }
                            case "somewhat":
                                {
                                    switch (budgetS)
                                    {                                        
                                        case "mid":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD FX-8350 4.0GHz 8-Core Processor(Amazon UK)\r\nCOOLER:\r\nCooler Master Hyper TX3 54.8 CFM Sleeve Bearing CPU Cooler(Amazon UK)\r\nMOTHERBOARD:\r\nASRock 970M PRO3 Micro ATX AM3+/AM3 Motherboard(More Computers)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1866 Memory(More Computers)\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nVIDEO CARD:\r\nMSI Radeon R9 380 4GB Video Card(Amazon UK)\r\nCASE:\r\nCooler Master N200 MicroATX Mid Tower Case(Amazon UK)\r\nPOWER SUPPLY:\r\nEVGA SuperNOVA NEX 650W 80+ Gold Certified Fully-Modular ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n619.23(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD FX-8350 4.0GHz 8-Core Processor(Amazon UK)\r\nCOOLER:\r\nCooler Master Hyper TX3 54.8 CFM Sleeve Bearing CPU Cooler(Amazon UK)\r\nMOTHERBOARD:\r\nASRock 970M PRO3 Micro ATX AM3+/AM3 Motherboard(More Computers)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1866 Memory(More Computers)\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nVIDEO CARD:\r\nMSI Radeon R9 380 4GB Video Card(Amazon UK)\r\nCASE:\r\nCooler Master N200 MicroATX Mid Tower Case(Amazon UK)\r\nPOWER SUPPLY:\r\nEVGA SuperNOVA NEX 650W 80+ Gold Certified Fully-Modular ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n602.23(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                        case "low":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Pentium G3258 3.2GHz Dual-Core Processor(Amazon UK)\r\nMOTHERBOARD:/r/n: MSI H81I Mini ITX LGA1150 Motherboard(Amazon UK)\r\nMEMORY:\r\nPatriot Signature 8GB (2 x 4GB) DDR3-1600 Memory(Amazon UK) " + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nCASE:\r\nCooler Master N200 MicroATX Mid Tower Case(Amazon UK)\r\nPOWER SUPPLY:\r\nEVGA 430W 80+ Certified ATX Power Supply(Ebuyer)\r\nOPTICAL DRIVE:\r\nLite-On iHAS124-04 DVD/CD Writer(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n334.20(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Pentium G3258 3.2GHz Dual-Core Processor(Amazon UK)\r\nMOTHERBOARD:/r/n: MSI H81I Mini ITX LGA1150 Motherboard(Amazon UK)\r\nMEMORY:\r\nPatriot Signature 8GB (2 x 4GB) DDR3-1600 Memory(Amazon UK) " + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nCASE:\r\nCooler Master N200 MicroATX Mid Tower Case(Amazon UK)\r\nPOWER SUPPLY:\r\nEVGA 430W 80+ Certified ATX Power Supply(Ebuyer)\r\nOPTICAL DRIVE:\r\nLite-On iHAS124-04 DVD/CD Writer(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n317.20(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case "not":
                                {
                                    switch (budgetS)
                                    {                                        
                                        case "mid":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD FX-8350 4.0GHz 8-Core Processor(Amazon UK)\r\nCOOLER:\r\nCooler Master Hyper TX3 54.8 CFM Sleeve Bearing CPU Cooler(Amazon UK)\r\nMOTHERBOARD:\r\n: MSI 970 GAMING ATX AM3+ Motherboard(Novatech)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1866 Memory(More Computers)\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nVIDEO CARD:\r\nMSI Radeon R9 380 4GB Video Card(Amazon UK)\r\nCASE:\r\nCorsair SPEC-01 RED ATX Mid Tower Case(Ebuyer)\r\nPOWER SUPPLY:\r\nEVGA SuperNOVA NEX 650W 80+ Gold Certified Fully-Modular ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n634.68(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD FX-8350 4.0GHz 8-Core Processor(Amazon UK)\r\nCOOLER:\r\nCooler Master Hyper TX3 54.8 CFM Sleeve Bearing CPU Cooler(Amazon UK)\r\nMOTHERBOARD:\r\n: MSI 970 GAMING ATX AM3+ Motherboard(Novatech)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1866 Memory(More Computers)\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK) \r\nVIDEO CARD:\r\nMSI Radeon R9 380 4GB Video Card(Amazon UK)\r\nCASE:\r\nCorsair SPEC-01 RED ATX Mid Tower Case(Ebuyer)\r\nPOWER SUPPLY:\r\nEVGA SuperNOVA NEX 650W 80+ Gold Certified Fully-Modular ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n617.68(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                        case "low":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Pentium G3258 3.2GHz Dual-Core Processor(Amazon UK)\r\nMOTHERBOARD:\r\nASRock H81 Pro BTC ATX LGA1150 Motherboard(More Computers)\r\nMEMORY:\r\nPatriot Signature 8GB (2 x 4GB) DDR3-1600 Memory(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nCASE:\r\nCiT Vantage ATX Mid Tower Case(Ebuyer)\r\nPOWER SUPPLY:\r\nEVGA 430W 80+ Certified ATX Power Supply(Ebuyer)\r\nOPTICAL DRIVE:\r\nLite-On iHAS124-04 DVD/CD Writer(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n322.16(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Pentium G3258 3.2GHz Dual-Core Processor(Amazon UK)\r\nMOTHERBOARD:\r\nASRock H81 Pro BTC ATX LGA1150 Motherboard(More Computers)\r\nMEMORY:\r\nPatriot Signature 8GB (2 x 4GB) DDR3-1600 Memory(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK) \r\nCASE:\r\nCiT Vantage ATX Mid Tower Case(Ebuyer)\r\nPOWER SUPPLY:\r\nEVGA 430W 80+ Certified ATX Power Supply(Ebuyer)\r\nOPTICAL DRIVE:\r\nLite-On iHAS124-04 DVD/CD Writer(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n305.49(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                    }
                                    break;
                                }

                        }
                        break;
                }
                case "video editing":
                {
                        switch (caseS)
                        {
                            case "very":
                                {
                                    switch (budgetS)
                                    {
                                        case "high":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Core i7-5820K 3.3GHz 6-Core Processor(Ebuyer)\r\nCOOLER:\r\nCooler Master Hyper 212 EVO 82.9 CFM Sleeve Bearing CPU Cooler(Novatech)\r\nMOTHERBOARD:\r\nGigabyte GA-X99M-GAMING 5 Micro ATX LGA2011-3 Motherboard(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nG.Skill TridentZ Series 16GB (2 x 8GB) DDR4-3200 Memory(More Computers)\r\nSTORAGE:\r\nSamsung 850 EVO-Series 250GB 2.5 Solid State Drive(Amazon UK)\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nVIDEO CARD:\r\nEVGA GeForce GTX 980 Ti 6GB Superclocked Video Card(Amazon UK)\r\nCASE:\r\nFractal Design Arc Mini MicroATX Mini Tower Case(Amazon UK)\r\nPOWER SUPPLY:\r\nEVGA SuperNOVA G2 650W 80+ Gold Certified Fully-Modular ATX Power Supply(Amazon UK)\r\nOPTICAL DRIVE:\r\nSamsung SH-224DB/BEBE DVD/CD Writer(Ebuyer)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n1498.81(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Core i7-5820K 3.3GHz 6-Core Processor(Ebuyer)\r\nCOOLER:\r\nCooler Master Hyper 212 EVO 82.9 CFM Sleeve Bearing CPU Cooler(Novatech)\r\nMOTHERBOARD:\r\nGigabyte GA-X99M-GAMING 5 Micro ATX LGA2011-3 Motherboard(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nG.Skill TridentZ Series 16GB (2 x 8GB) DDR4-3200 Memory(More Computers)\r\nSTORAGE:\r\nWestern Digital Caviar Blue 2TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nVIDEO CARD:\r\nEVGA GeForce GTX 980 Ti 6GB Superclocked Video Card(Amazon UK)\r\nCASE:\r\nFractal Design Arc Mini MicroATX Mini Tower Case(Amazon UK)\r\nPOWER SUPPLY:\r\nEVGA SuperNOVA G2 650W 80+ Gold Certified Fully-Modular ATX Power Supply(Amazon UK)\r\nOPTICAL DRIVE:\r\nSamsung SH-224DB/BEBE DVD/CD Writer(Ebuyer)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n1488.81(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                break;
                                            }
                                        case "mid":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Core i7-4770K 3.5GHz Quad-Core Processor(Amazon UK)\r\nCOOLER:\r\nCooler Master Hyper TX3 54.8 CFM Sleeve Bearing CPU Cooler(Amazon UK)\r\nMOTHERBOARD:\r\nASRock Z97M-ITX/AC Mini ITX LGA1150 Motherboard(Amazon UK) " + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1866 Memory(More Computers)\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nVIDEO CARD:\r\nAsus Radeon R9 380 4GB Video Card(Amazon UK)\r\nCASE:\r\nZalman M1 Mini ITX Tower Case(More Computers)\r\nPOWER SUPPLY:\r\nCorsair CX 500W 80+ Bronze Certified Semi-Modular ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL\r\n789.74(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Core i7-4770K 3.5GHz Quad-Core Processor(Amazon UK)\r\nCOOLER:\r\nCooler Master Hyper TX3 54.8 CFM Sleeve Bearing CPU Cooler(Amazon UK)\r\nMOTHERBOARD:\r\nASRock Z97M-ITX/AC Mini ITX LGA1150 Motherboard(Amazon UK) " + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1866 Memory(More Computers)\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nVIDEO CARD:\r\nAsus Radeon R9 380 4GB Video Card(Amazon UK)\r\nCASE:\r\nZalman M1 Mini ITX Tower Case(More Computers)\r\nPOWER SUPPLY:\r\nCorsair CX 500W 80+ Bronze Certified Semi-Modular ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL\r\n772.74(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                        case "low":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Core i5-6400 2.7GHz Quad-Core Processor(Aria PC)\r\nMOTHERBOARD:\r\nAsus H110I-PLUS D3/CSM Mini ITX LGA1151 Motherboard(Amazon UK)\r\nMEMORY:\r\nCorsair Vengeance 8GB (2 x 4GB) DDR3-1600 Memory(Ebuyer)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK\r\nVIDEO CARD:\r\nMSI Radeon R7 240 2GB Video Card(Overclockers.co.uk)\r\nCASE:\r\nZalman M1 Mini ITX Tower Case(More Computers)\r\nPOWER SUPPLY:\r\nEVGA 500W 80+ Bronze Certified ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n484.53(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Core i5-6400 2.7GHz Quad-Core Processor(Aria PC)\r\nMOTHERBOARD:\r\nAsus H110I-PLUS D3/CSM Mini ITX LGA1151 Motherboard(Amazon UK)\r\nMEMORY:\r\nCorsair Vengeance 8GB (2 x 4GB) DDR3-1600 Memory(Ebuyer)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK) \r\nVIDEO CARD:\r\nMSI Radeon R7 240 2GB Video Card(Overclockers.co.uk)\r\nCASE:\r\nZalman M1 Mini ITX Tower Case(More Computers)\r\nPOWER SUPPLY:\r\nEVGA 500W 80+ Bronze Certified ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n467.53(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                    }

                                    break;
                                }
                            
                            case "not":
                                {
                                    switch (budgetS)
                                    {
                                        case "high":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Core i7-5820K 3.3GHz 6-Core Processor(Ebuyer)\r\nCOOLER:\r\nCooler Master Hyper 212 EVO 82.9 CFM Sleeve Bearing CPU Cooler(Novatech)\r\nMOTHERBOARD:\r\nASRock X99 Extreme4 ATX LGA2011-3 Motherboard(More Computers)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nG.Skill TridentZ Series 16GB (2 x 8GB) DDR4-3200 Memory(More Computers)\r\nSTORAGE:\r\nSamsung 850 EVO-Series 250GB 2.5 Solid State Drive(Amazon UK)\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nVIDEO CARD:\r\nEVGA GeForce GTX 980 Ti 6GB Superclocked Video Card(Amazon UK)\r\nCASE:\r\nCorsair Graphite Series 230T Black ATX Mid Tower Case(Dabs)\r\nPOWER SUPPLY:\r\nEVGA SuperNOVA G2 650W 80+ Gold Certified Fully-Modular ATX Power Supply(Amazon UK)\r\nOPTICAL DRIVE:\r\nSamsung SH-224DB/BEBE DVD/CD Writer(Ebuyer)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n1499.55(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Core i7-5820K 3.3GHz 6-Core Processor(Ebuyer)\r\nCOOLER:\r\nCooler Master Hyper 212 EVO 82.9 CFM Sleeve Bearing CPU Cooler(Novatech)\r\nMOTHERBOARD:\r\nASRock X99 Extreme4 ATX LGA2011-3 Motherboard(More Computers)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nG.Skill TridentZ Series 16GB (2 x 8GB) DDR4-3200 Memory(More Computers)\r\nSTORAGE:\r\nWestern Digital Caviar Blue 2TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nVIDEO CARD:\r\nEVGA GeForce GTX 980 Ti 6GB Superclocked Video Card(Amazon UK)\r\nCASE:\r\nCorsair Graphite Series 230T Black ATX Mid Tower Case(Dabs)\r\nPOWER SUPPLY:\r\nEVGA SuperNOVA G2 650W 80+ Gold Certified Fully-Modular ATX Power Supply(Amazon UK)\r\nOPTICAL DRIVE:\r\nSamsung SH-224DB/BEBE DVD/CD Writer(Ebuyer)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n1489.55(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                break;
                                            }
                                        case "mid":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Core i7-4770K 3.5GHz Quad-Core Processor(Amazon UK)\r\nCOOLER:\r\nCooler Master Hyper TX3 54.8 CFM Sleeve Bearing CPU Cooler(Amazon UK)\r\nMOTHERBOARD:\r\nASRock Z87 Pro4 ATX LGA1150 Motherboard(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1866 Memory(More Computers)\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nVIDEO CARD:\r\nAsus Radeon R9 380 4GB Video Card(Amazon UK)\r\nCASE:\r\nCorsair SPEC-01 RED ATX Mid Tower Case(Ebuyer)\r\nPOWER SUPPLY:\r\nCorsair CX 500W 80+ Bronze Certified Semi-Modular ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n786.00(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Core i7-4770K 3.5GHz Quad-Core Processor(Amazon UK)\r\nCOOLER:\r\nCooler Master Hyper TX3 54.8 CFM Sleeve Bearing CPU Cooler(Amazon UK)\r\nMOTHERBOARD:\r\nASRock Z87 Pro4 ATX LGA1150 Motherboard(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1866 Memory(More Computers)\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nVIDEO CARD:\r\nAsus Radeon R9 380 4GB Video Card(Amazon UK)\r\nCASE:\r\nCorsair SPEC-01 RED ATX Mid Tower Case(Ebuyer)\r\nPOWER SUPPLY:\r\nCorsair CX 500W 80+ Bronze Certified Semi-Modular ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n769.00(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                        case "low":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD FX-8370 4.0GHz 8-Core Processor(Ebuyer)\r\nMOTHERBOARD:\r\nMSI 970A-G43 ATX AM3+ Motherboard(Ebuyer)\r\nMEMORY:\r\nCorsair Vengeance 8GB (2 x 4GB) DDR3-1600 Memory(Ebuyer)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nVIDEO CARD:\r\nMSI Radeon R7 240 2GB Video Card(Overclockers.co.uk)\r\nCASE:\r\nCorsair SPEC-01 RED ATX Mid Tower Case(Ebuyer)\r\nPOWER SUPPLY:\r\nEVGA 500W 80+ Bronze Certified ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n499.90(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD FX-8370 4.0GHz 8-Core Processor(Ebuyer)\r\nMOTHERBOARD:\r\nMSI 970A-G43 ATX AM3+ Motherboard(Ebuyer)\r\nMEMORY:\r\nCorsair Vengeance 8GB (2 x 4GB) DDR3-1600 Memory(Ebuyer)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nVIDEO CARD:\r\nMSI Radeon R7 240 2GB Video Card(Overclockers.co.uk)\r\nCASE:\r\nCorsair SPEC-01 RED ATX Mid Tower Case(Ebuyer)\r\nPOWER SUPPLY:\r\nEVGA 500W 80+ Bronze Certified ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n482.90(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                    }
                                    break;
                                }

                        }
                        break;
                }
                case "home theater":
                {
                        switch (caseS)
                        {
                            case "very":
                                {
                                    switch (budgetS)
                                    {
                                        
                                        case "mid":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Core i5-6400 2.7GHz Quad-Core Processor(Aria PC)\r\nMOTHERBOARD:\r\nAsus H110I-PLUS D3/CSM Mini ITX LGA1151 Motherboard(Amazon UK)\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1600 Memory(More Computers)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nVIDEO CARD:\r\nEVGA GeForce GTX 750 Ti 2GB Superclocked Video Card(Dabs)\r\nCASE:\r\nZalman M1 Mini ITX Tower Case(More Computers)\r\nPOWER SUPPLY:\r\nEVGA 600B 600W 80+ Bronze Certified ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n560.39(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Core i5-6400 2.7GHz Quad-Core Processor(Aria PC)\r\nMOTHERBOARD:\r\nAsus H110I-PLUS D3/CSM Mini ITX LGA1151 Motherboard(Amazon UK)\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1600 Memory(More Computers)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nVIDEO CARD:\r\nEVGA GeForce GTX 750 Ti 2GB Superclocked Video Card(Dabs)\r\nCASE:\r\nZalman M1 Mini ITX Tower Case(More Computers)\r\nPOWER SUPPLY:\r\nEVGA 600B 600W 80+ Bronze Certified ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n543.39(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                        case "low":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD A8-7600 3.1GHz Quad-Core Processor(More Computers)\r\nMOTHERBOARD:\r\nMSI A88XI AC V2 Mini ITX FM2+ Motherboard(Ebuyer)\r\nMEMORY:\r\nPatriot Signature 8GB (2 x 4GB) DDR3-1600 Memory(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nCASE:\r\nZalman M1 Mini ITX Tower Case(More Computers)\r\nPOWER SUPPLY:\r\nEVGA 430W 80+ Certified ATX Power Supply(Ebuyer)\r\nOPTICAL DRIVE:\r\nLite-On iHAS124-04 DVD/CD Writer(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n349.25(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD A8-7600 3.1GHz Quad-Core Processor(More Computers)\r\nMOTHERBOARD:\r\nMSI A88XI AC V2 Mini ITX FM2+ Motherboard(Ebuyer)\r\nMEMORY:\r\nPatriot Signature 8GB (2 x 4GB) DDR3-1600 Memory(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nCASE:\r\nZalman M1 Mini ITX Tower Case(More Computers)\r\nPOWER SUPPLY:\r\nEVGA 430W 80+ Certified ATX Power Supply(Ebuyer)\r\nOPTICAL DRIVE:\r\nLite-On iHAS124-04 DVD/CD Writer(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n332.25(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                    }

                                    break;
                                }
                            case "somewhat":
                                {
                                    switch (budgetS)
                                    {
                                        
                                        case "mid":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD FX-8350 4.0GHz 8-Core Processor(Amazon UK)\r\nMOTHERBOARD:\r\n: Gigabyte GA-78LMT-USB3 Micro ATX AM3+ Motherboard(Novatech)\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1600 Memory(More Computers)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nVIDEO CARD:\r\nEVGA GeForce GTX 750 Ti 2GB Superclocked Video Card(Dabs)\r\nCASE:\r\nCooler Master N200 MicroATX Mid Tower Case(Amazon UK)\r\nPOWER SUPPLY:\r\nEVGA 600B 600W 80+ Bronze Certified ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n542.89(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD FX-8350 4.0GHz 8-Core Processor(Amazon UK)\r\nMOTHERBOARD:\r\n: Gigabyte GA-78LMT-USB3 Micro ATX AM3+ Motherboard(Novatech)\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1600 Memory(More Computers)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nVIDEO CARD:\r\nEVGA GeForce GTX 750 Ti 2GB Superclocked Video Card(Dabs)\r\nCASE:\r\nCooler Master N200 MicroATX Mid Tower Case(Amazon UK)\r\nPOWER SUPPLY:\r\nEVGA 600B 600W 80+ Bronze Certified ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n525.89(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                        case "low":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD A8-7600 3.1GHz Quad-Core Processor(More Computers)\r\nMOTHERBOARD:\r\nASRock FM2A68M-DG3+ Micro ATX FM2+ Motherboard(More Computers)\r\nMEMORY:\r\nPatriot Signature 8GB (2 x 4GB) DDR3-1600 Memory(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nCASE:\r\nCooler Master N200 MicroATX Mid Tower Case(Amazon UK)\r\nPOWER SUPPLY:\r\nEVGA 430W 80+ Certified ATX Power Supply(Ebuyer)\r\nOPTICAL DRIVE:\r\nLite-On iHAS124-04 DVD/CD Writer(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n336.27(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD A8-7600 3.1GHz Quad-Core Processor(More Computers)\r\nMOTHERBOARD:\r\nASRock FM2A68M-DG3+ Micro ATX FM2+ Motherboard(More Computers)\r\nMEMORY:\r\nPatriot Signature 8GB (2 x 4GB) DDR3-1600 Memory(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nCASE:\r\nCooler Master N200 MicroATX Mid Tower Case(Amazon UK)\r\nPOWER SUPPLY:\r\nEVGA 430W 80+ Certified ATX Power Supply(Ebuyer)\r\nOPTICAL DRIVE:\r\nLite-On iHAS124-04 DVD/CD Writer(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n319.27(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case "not":
                                {
                                    switch (budgetS)
                                    {
                                        
                                        case "mid":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD FX-8350 4.0GHz 8-Core Processor(Amazon UK)\r\nMOTHERBOARD:\r\nGigabyte GA-970A-DS3P ATX AM3+ Motherboard(Amazon UK)\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1600 Memory(More Computers)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nVIDEO CARD:\r\nEVGA GeForce GTX 750 Ti 2GB Superclocked Video Card(Dabs)\r\nCASE:\r\nCorsair SPEC-01 RED ATX Mid Tower Case(Ebuyer)\r\nPOWER SUPPLY:\r\nEVGA 600B 600W 80+ Bronze Certified ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n551.25(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD FX-8350 4.0GHz 8-Core Processor(Amazon UK)\r\nMOTHERBOARD:\r\nGigabyte GA-970A-DS3P ATX AM3+ Motherboard(Amazon UK)\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1600 Memory(More Computers)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nVIDEO CARD:\r\nEVGA GeForce GTX 750 Ti 2GB Superclocked Video Card(Dabs)\r\nCASE:\r\nCorsair SPEC-01 RED ATX Mid Tower Case(Ebuyer)\r\nPOWER SUPPLY:\r\nEVGA 600B 600W 80+ Bronze Certified ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n536.25(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                        case "low":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD A8-7600 3.1GHz Quad-Core Processor(More Computers)\r\nMOTHERBOARD:\r\nASRock FM2A78 PRO3+ ATX FM2+ Motherboard(Amazon UK)\r\nMEMORY:\r\nPatriot Signature 8GB (2 x 4GB) DDR3-1600 Memory(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nCASE:\r\nCorsair SPEC-01 RED ATX Mid Tower Case(Ebuyer)\r\nPOWER SUPPLY:\r\nEVGA 430W 80+ Certified ATX Power Supply(Ebuyer)\r\nOPTICAL DRIVE:\r\nLite-On iHAS124-04 DVD/CD Writer(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n344.65(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD A8-7600 3.1GHz Quad-Core Processor(More Computers)\r\nMOTHERBOARD:\r\nASRock FM2A78 PRO3+ ATX FM2+ Motherboard(Amazon UK)\r\nMEMORY:\r\nPatriot Signature 8GB (2 x 4GB) DDR3-1600 Memory(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nCASE:\r\nCorsair SPEC-01 RED ATX Mid Tower Case(Ebuyer)\r\nPOWER SUPPLY:\r\nEVGA 430W 80+ Certified ATX Power Supply(Ebuyer)\r\nOPTICAL DRIVE:\r\nLite-On iHAS124-04 DVD/CD Writer(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n327.65(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                    }
                                    break;
                                }

                        }
                        break;
                }
                case "all rounder":
                {
                        switch (caseS)
                        {
                            case "very":
                                {
                                    switch (budgetS)
                                    {
                                        
                                        case "mid":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Core i5-6400 2.7GHz Quad-Core Processor(Aria PC)\r\nCOOLER:\r\nCooler Master Hyper TX3 54.8 CFM Sleeve Bearing CPU Cooler(Amazon UK)\r\nMOTHERBOARD:\r\nAsus H110I-PLUS D3/CSM Mini ITX LGA1151 Motherboard(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1866 Memory(More Computers)\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nVIDEO CARD:\r\nMSI Radeon R9 380 4GB Video Card(Amazon UK)\r\nCASE:\r\nZalman M1 Mini ITX Tower Case(More Computers)\r\nPOWER SUPPLY:\r\nEVGA SuperNOVA NEX 650W 80+ Gold Certified Fully-Modular ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n613.82(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Core i5-6400 2.7GHz Quad-Core Processor(Aria PC)\r\nCOOLER:\r\nCooler Master Hyper TX3 54.8 CFM Sleeve Bearing CPU Cooler(Amazon UK)\r\nMOTHERBOARD:\r\nAsus H110I-PLUS D3/CSM Mini ITX LGA1151 Motherboard(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1866 Memory(More Computers)\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK) \r\nVIDEO CARD:\r\nMSI Radeon R9 380 4GB Video Card(Amazon UK)\r\nCASE:\r\nZalman M1 Mini ITX Tower Case(More Computers)\r\nPOWER SUPPLY:\r\nEVGA SuperNOVA NEX 650W 80+ Gold Certified Fully-Modular ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n596.82(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                        case "low":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Pentium G3470 3.6GHz Dual-Core Processor(Dabs)\r\nMOTHERBOARD:\r\nMSI H81I Mini ITX LGA1150 Motherboard(Amazon UK)\r\nMEMORY:\r\nCorsair Vengeance 8GB (2 x 4GB) DDR3-1600 Memory(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nVIDEO CARD:\r\n: MSI Radeon R7 240 2GB Video Card(Overclockers.co.uk)\r\nCASE:\r\nZalman M1 Mini ITX Tower Case(More Computers)\r\nPOWER SUPPLY:\r\nEVGA 430W 80+ Certified ATX Power Supply(Ebuyer)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n400.49(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nIntel Pentium G3470 3.6GHz Dual-Core Processor(Dabs)\r\nMOTHERBOARD:\r\nMSI H81I Mini ITX LGA1150 Motherboard(Amazon UK)\r\nMEMORY:\r\nCorsair Vengeance 8GB (2 x 4GB) DDR3-1600 Memory(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nVIDEO CARD:\r\n: MSI Radeon R7 240 2GB Video Card(Overclockers.co.uk)\r\nCASE:\r\nZalman M1 Mini ITX Tower Case(More Computers)\r\nPOWER SUPPLY:\r\nEVGA 430W 80+ Certified ATX Power Supply(Ebuyer)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n383.49(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                    }

                                    break;
                                }
                            case "somewhat":
                                {
                                    switch (budgetS)
                                    {
                                    
                                        case "mid":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD FX-8350 4.0GHz 8-Core Processor(Amazon UK)\r\nCOOLER:\r\nCooler Master Hyper TX3 54.8 CFM Sleeve Bearing CPU Cooler(Amazon UK)\r\nMOTHERBOARD:\r\nASRock 970M PRO3 Micro ATX AM3+/AM3 Motherboard(More Computers)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1866 Memory(More Computers)\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nVIDEO CARD:\r\nMSI Radeon R9 380 4GB Video Card(Amazon UK)\r\nCASE:\r\nCooler Master N200 MicroATX Mid Tower Case(Amazon UK)\r\nPOWER SUPPLY:\r\nEVGA SuperNOVA NEX 650W 80+ Gold Certified Fully-Modular ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n619.23(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD FX-8350 4.0GHz 8-Core Processor(Amazon UK)\r\nCOOLER:\r\nCooler Master Hyper TX3 54.8 CFM Sleeve Bearing CPU Cooler(Amazon UK)\r\nMOTHERBOARD:\r\nASRock 970M PRO3 Micro ATX AM3+/AM3 Motherboard(More Computers)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1866 Memory(More Computers)\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK) \r\nVIDEO CARD:\r\nMSI Radeon R9 380 4GB Video Card(Amazon UK)\r\nCASE:\r\nCooler Master N200 MicroATX Mid Tower Case(Amazon UK)\r\nPOWER SUPPLY:\r\nEVGA SuperNOVA NEX 650W 80+ Gold Certified Fully-Modular ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n602.23(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                        case "low":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD FX-6300 3.5GHz 6-Core Processor(Ebuyer)\r\nMOTHERBOARD:\r\nAsus M5A78L-M/USB3 Micro ATX AM3+ Motherboard(Amazon UK)\r\nMEMORY:\r\nCorsair Vengeance 8GB (2 x 4GB) DDR3-1600 Memory(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nVIDEO CARD:\r\nMSI Radeon R7 240 2GB Video Card(Overclockers.co.uk)\r\nCASE:\r\nCooler Master N200 MicroATX Mid Tower Case(Amazon UK)\r\nPOWER SUPPLY:\r\nEVGA 430W 80+ Certified ATX Power Supply(Ebuyer)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n410.73(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD FX-6300 3.5GHz 6-Core Processor(Ebuyer)\r\nMOTHERBOARD:\r\nAsus M5A78L-M/USB3 Micro ATX AM3+ Motherboard(Amazon UK)\r\nMEMORY:\r\nCorsair Vengeance 8GB (2 x 4GB) DDR3-1600 Memory(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nVIDEO CARD:\r\nMSI Radeon R7 240 2GB Video Card(Overclockers.co.uk)\r\nCASE:\r\nCooler Master N200 MicroATX Mid Tower Case(Amazon UK)\r\nPOWER SUPPLY:\r\nEVGA 430W 80+ Certified ATX Power Supply(Ebuyer)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n393.73(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case "not":
                                {
                                    switch (budgetS)
                                    {
                                       
                                        case "mid":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD FX-8350 4.0GHz 8-Core Processor(Amazon UK)\r\nCOOLER:\r\nCooler Master Hyper TX3 54.8 CFM Sleeve Bearing CPU Cooler(Amazon UK)\r\nMOTHERBOARD:\r\nMSI 970 GAMING ATX AM3+ Motherboard(Novatech)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1866 Memory(More Computers)\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nVIDEO CARD:\r\nMSI Radeon R9 380 4GB Video Card(Amazon UK)\r\nCASE:\r\nCorsair SPEC-01 RED ATX Mid Tower Case(Ebuyer)\r\nPOWER SUPPLY:\r\nEVGA SuperNOVA NEX 650W 80+ Gold Certified Fully-Modular ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n634.68(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD FX-8350 4.0GHz 8-Core Processor(Amazon UK)\r\nCOOLER:\r\nCooler Master Hyper TX3 54.8 CFM Sleeve Bearing CPU Cooler(Amazon UK)\r\nMOTHERBOARD:\r\nMSI 970 GAMING ATX AM3+ Motherboard(Novatech)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nMEMORY:\r\nKingston HyperX Fury Black 16GB (2 x 8GB) DDR3-1866 Memory(More Computers)\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nVIDEO CARD:\r\nMSI Radeon R9 380 4GB Video Card(Amazon UK)\r\nCASE:\r\nCorsair SPEC-01 RED ATX Mid Tower Case(Ebuyer)\r\nPOWER SUPPLY:\r\nEVGA SuperNOVA NEX 650W 80+ Gold Certified Fully-Modular ATX Power Supply(Amazon UK)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n617.68(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                        case "low":
                                            {
                                                if (speedOrStorage == "speed")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD FX-6300 3.5GHz 6-Core Processor(Ebuyer)\r\nMOTHERBOARD:\r\nAsus M5A97 LE R2.0 ATX AM3+ Motherboard(Ebuyer)\r\nMEMORY:\r\nCorsair Vengeance 8GB (2 x 4GB) DDR3-1600 Memory(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nKingston Savage 240GB 2.5 Solid State Drive(Amazon UK)\r\nVIDEO CARD:\r\nMSI Radeon R7 240 2GB Video Card(Overclockers.co.uk)\r\nCASE:\r\nCiT Vantage ATX Mid Tower Case(Ebuyer)\r\nPOWER SUPPLY:\r\nEVGA 430W 80+ Certified ATX Power Supply(Ebuyer)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n408.48(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }

                                                if (speedOrStorage == "storage")
                                                {
                                                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(10000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nCPU:\r\nAMD FX-6300 3.5GHz 6-Core Processor(Ebuyer)\r\nMOTHERBOARD:\r\nAsus M5A97 LE R2.0 ATX AM3+ Motherboard(Ebuyer)\r\nMEMORY:\r\nCorsair Vengeance 8GB (2 x 4GB) DDR3-1600 Memory(Amazon UK)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    Thread.Sleep(4000);
                                                    outStream = System.Text.Encoding.ASCII.GetBytes("\r\nSTORAGE:\r\nWestern Digital Caviar Blue 1TB 3.5 7200RPM Internal Hard Drive(Amazon UK)\r\nVIDEO CARD:\r\nMSI Radeon R7 240 2GB Video Card(Overclockers.co.uk)\r\nCASE:\r\nCiT Vantage ATX Mid Tower Case(Ebuyer)\r\nPOWER SUPPLY:\r\nEVGA 430W 80+ Certified ATX Power Supply(Ebuyer)\r\nOPERATING SYSTEM:\r\nMicrosoft Windows 8.1 OEM (64-bit)(Amazon UK)\r\nTOTAL:\r\n391.48(GBP)" + "$");
                                                    serverStream.Write(outStream, 0, outStream.Length);
                                                    serverStream.Flush();
                                                }
                                                break;
                                            }
                                    }
                                    break;
                                }

                        }
                        break;
                }
            }
        }
        
        private void CheckEnter(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == e.KeyChar)
            {
                if (typing == false)
                {
                   byte[] outStream = System.Text.Encoding.ASCII.GetBytes("is typing..." + "$");
                    serverStream.Write(outStream, 0, outStream.Length);
                    serverStream.Flush();
                    typing = true;
                }
            }
                        
        }
    }
}
