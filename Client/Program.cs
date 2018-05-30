using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace Client
{
    public class ClientChat
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string Name { get; set; }
        public TcpClient Client { get; set; }
        public event EventHandler MessageRec;
        private Thread MRS;
        public ClientChat(string hostname,int port,string name)
        {
            Port = port;
            HostName = hostname;
            Name = name;
            Client = Connect(HostName, Port, Name);
          
        }


        public  TcpClient Connect(string hostName, int port, string id)
        {
            TcpClient client = new TcpClient();
            client.Connect(hostName, port);
            if (client.Connected)
            {
                Send(id, client.GetStream());
            }
            MRS = new Thread(MessageReceiver);
            MRS.Start();
            return client;

        }

        public  void Send(string data, NetworkStream stream)
        {
           
            var str = Encoding.ASCII.GetBytes(data);
            var newLine = Encoding.ASCII.GetBytes(Environment.NewLine);
            stream.Write(str, 0, str.Length);
            stream.Write(newLine, 0, newLine.Length);

        }
        public void Send(string data)
        {
            var stream = this.Client.GetStream();
            var str = Encoding.ASCII.GetBytes(data);
            var newLine = Encoding.ASCII.GetBytes(Environment.NewLine);
            stream.Write(str, 0, str.Length);
            stream.Write(newLine, 0, newLine.Length);

        }

        public void SendDirectMessage(string userName, string message)
        {
            string temp = $"DM|{userName}|{message}";
            Send(temp);
        }

        public void JoinToGp(string gpName)
        {
            string temp = $"JPG|{gpName}";
            Send(temp);
        }

        public void SendMessageToGroup(string groupName, string message)
        {
            string temp = $"GP|{groupName}|{message}";
            Send(temp);
        }

        public void GetOnlineUserList()
        {
            string temp = "GU|";
            Send(temp);
        }
        public void MessageReceiver()
        {
            try
            {
                string sData;
                StreamReader sReader = new StreamReader(Client.GetStream(), Encoding.ASCII);
                while (Client.Connected)
                {
                    // setname 'name'
                    // reads from stream
                    sData = sReader.ReadLine();


                    // shows content on the console.
                    Console.WriteLine($"[{DateTime.Now.ToString()}]:{sData} ");

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
               
            }
        }
    }

    class Program
    {
        public static ClientChat Client;
        static void Main(string[] args)
        {


            Console.WriteLine("Simple Chat console application");
            Console.WriteLine("Enter your name:");
            var name = Console.ReadLine();
            Client = new ClientChat("178.63.169.234", 5555, name);
            Console.WriteLine("Menu");
            Console.WriteLine("[1]  Show Menu");
            Console.WriteLine("[2]  Direct Message");
            Console.WriteLine("[3]  Join to group");
            Console.WriteLine("[4]  Send message to groups ");
            Console.WriteLine("[5]  Get online user list");
           
            
            Console.WriteLine("Select Menu number:");

            

            try
            {
                while (true)
                {
                    var number = Convert.ToInt32(Console.ReadLine());
                    switch (number)
                    {
                        case 1:
                            Menu();
                            break;
                        case 2:
                            DM();
                            break;
                        case 3:
                            JG();
                            break;
                        case 4:
                            GM();
                            break;
                        case 5:
                            break;
                    }


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        






        }

        public static void DM()
        {
            Console.WriteLine("username of your friend:");
            var userName = Console.ReadLine();
            while (true)
            {
                if (Client.Client.Connected)
                {
                    Console.WriteLine("Direct message");
                    Console.WriteLine("Enter your message");
                    var content = Console.ReadLine();
                    if (content.ToLower() == "endchat")
                    {
                        break;
                    }
                  
                    Client.SendDirectMessage(userName,content);


                }
                else
                {
                    Console.WriteLine("Not connected");
                }
                
            }
        }
        public static void JG()
        {
            Console.WriteLine("Enter group name:");
            var gpName = Console.ReadLine();
            Client.JoinToGp(gpName);

        }

        public static void GM()
        {
            Console.WriteLine("Enter group name:");
            var gpName = Console.ReadLine();
            Console.WriteLine("Type message and then press Enter");
            while (true)
            {

                if (Client.Client.Connected)
                {
                    var message = Console.ReadLine();
                    if (message.ToLower() == "endchat")
                    {
                        break;
                    }
                    Client.SendMessageToGroup(gpName,message);
                }
            }
        }

        public static void GU()
        {
            if (Client.Client.Connected)
            {
                Client.GetOnlineUserList();
            }
            else
            {
                Console.WriteLine("client in offline");
            }
        }
        public static void Menu()
        {
            Console.Clear();
            Console.WriteLine("Menu");
            
            Console.WriteLine("[1]  Connect to server");
            Console.WriteLine("[2]  Direct Message");
            Console.WriteLine("[3]  Join to group");
            Console.WriteLine("[4]  Send message to groups ");
            Console.WriteLine("[5]  Get online user list");
        }
     
      
    }
}
