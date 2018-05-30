using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
  


    class MyClass
    {
        public TcpClient Client { get; set; }
        public string Name { set; get; }
    }

    class TcpServer
    {
        private TcpListener _server;
        private Boolean _isRunning;
        private string _tempName;
        //group lists
        private Dictionary<string,List<string>> _groups = new Dictionary<string, List<string>>();
        //online userlist
        private Dictionary<string,NetworkStream> _onlineUser = new Dictionary<string, NetworkStream>();
        public TcpServer(int port)
        {
            _server = new TcpListener(IPAddress.Any, port);
            _server.Start();

            _isRunning = true;

            LoopClients();
        }
        
        public void LoopClients()
        {
            while (_isRunning)
            {
                // wait for client connection
                TcpClient newClient = _server.AcceptTcpClient();
             
                MyClass temp;

                StreamReader reader = new StreamReader(newClient.GetStream());
   

                    string name= reader.ReadLine();
                    temp = new MyClass()
                    {
                        Client = newClient,
                        Name = name
                    };

                Console.WriteLine($"[{name}] is connected");

                // client found.
                // create a thread to handle communication
                if (_onlineUser.ContainsKey(name))
                {
                    StreamWriter sWriter = new StreamWriter(newClient.GetStream(), Encoding.ASCII);
                    Console.WriteLine("blah blah");
                    sWriter.WriteLine("this username exit choos another one");
                    sWriter.Flush();
                }
                else
                {
                    Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                    t.Start(temp);
                }
               
            }
        }

        

        public void HandleClient(object obj )
        {
            MyClass inObj = (MyClass) obj;

            TcpClient client = inObj.Client;
            try
            {
                
               

                // sets two streams
                StreamWriter sWriter = new StreamWriter(client.GetStream(), Encoding.ASCII);
                StreamReader sReader = new StreamReader(client.GetStream(), Encoding.ASCII);
               
                if (_onlineUser.ContainsKey(inObj.Name))
                {
                    sWriter.WriteLine("this username exit choos another one");
                    sWriter.Flush();
                    sReader.Close();
                    sWriter.Close();

                }
                else
                {
                    _onlineUser.Add(inObj.Name, client.GetStream());
                    Boolean bClientConnected = true;
                    String sData = null;

                    while (bClientConnected)
                    {
                        // setname 'name'
                        // reads from stream
                        sData = sReader.ReadLine();

                        CommandHandling(sData,inObj.Name,inObj.Client.GetStream());

                        // shows content on the console.
                        Console.WriteLine($"Client [{inObj.Name}]; " + sData);

                        // to write something back.
                        sWriter.WriteLine("Your message recieved by server");
                        sWriter.Flush();


                       







                    }
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                client.Dispose();
               
            }
        }

        ///  <summary>
        ///   group message   GP|'GPNAME'|'message content'
        /// direct message  DM|'username'|'message content'
        /// get online userlist UL|
        /// JPG|'group name'
        ///  </summary>
        ///  <param name="command"></param>
        /// <param name="senderName"></param>
        /// <param name="stream"></param>
        public void CommandHandling(string command , string senderName , NetworkStream stream)
        {
            var cm = command.Split('|');
            switch (cm[0].ToLower())
            {
                case "jpg":
                    //join group
                    var gpName = cm[1];
                    if (_groups.ContainsKey(gpName))
                    {
                        _groups[gpName].Add(senderName);
                        Send($"You join to [{gpName}]",stream);
                        Console.WriteLine($"[{senderName}] joined to [{gpName}] Group");
                    }
                    else
                    {
                        Console.WriteLine($"NEW group create [{gpName}]");
                        _groups.Add(gpName,new List<string>(){senderName});
                        Send("new group  create [group name not exist]",stream);
                        
                    }
                    break;
                case "gp":
                    //GP|'Group name'|'message content'

                    if (_groups.ContainsKey(cm[1]))
                    {
                        
                        MultiCast(_groups[cm[1]],$"[group message][{cm[1]}][{senderName}][{cm[2]}]");
                    }
                    else
                    {
                        Send("this gp name not exist",stream);
                    }

                    break;
                case "dm":
                    var tempname = cm[1];
                    var content = cm[2];
                    if (_onlineUser.ContainsKey(tempname))
                    {
                        Console.WriteLine(tempname);

                        var tempStream = _onlineUser[tempname];
                        Send($"[Direct Message][{senderName}][{content}]",tempStream);
                    }
                    else
                    {
                        Send("sending direct message error [user notfound]",stream);
                    }

                    break;
                case "gu":
                    //get online usersname  GU|
                    string outPut = "";
                    foreach (var user in _onlineUser)
                    {
                        if (user.Value.CanWrite)
                        {
                            outPut += user.Key + Environment.NewLine;
                        }   
                    }

                    Send(outPut,stream);
                    break;
            }
        }
        public void Send(string data, NetworkStream stream)
        {

            var str = Encoding.ASCII.GetBytes(data);
            var newLine = Encoding.ASCII.GetBytes(Environment.NewLine);
            stream.Write(str, 0, str.Length);
            stream.Write(newLine, 0, newLine.Length);

        }

        public void MultiCast(List<string> userNames, string message)
        {
            foreach (var username in userNames)
            {
                if (_onlineUser.ContainsKey(username))
                {
                    var stream = _onlineUser[username];
                    Send(message,stream);
                }
               
            }
        }


    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Multi-Threaded TCP Chat Server ");

            TcpServer server = new TcpServer(5555);
            
        }
    }
}
