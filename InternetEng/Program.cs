using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LiteDB;
using MessagePack;
using Microsoft.Win32;
using SuperSocket.Common;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;
using ZeroMQ;


namespace InternetEng
{
    [MessagePackObject()]
    public class Protocol
    {
        [Key(0)]
        public string Command { get; set; }
        [Key(1)]
        public object ProtocolObj { get; set; }
    }
    class Program
    {
      
        private static Dictionary<AppSession, Users> _onlineUser = new Dictionary<AppSession, Users>();

        static void Main(string[] args)
        {
            Console.WriteLine("*Handmade Chat Server*");
            Console.WriteLine("load Configs");





            AppServer server = new AppServer();
            server.Setup("127.0.0.1", 1212);
            server.NewRequestReceived += ServerOnNewRequestReceived;
            server.NewSessionConnected += ServerOnNewSessionConnected;

            server.Start();
            


            Console.Read();

        }

      
        private static void ServerOnNewSessionConnected(AppSession session)
        {
            Console.WriteLine(session.SessionID + " is Connected");

            session.Send(session.SessionID);
            
           

        }
       
        private static void ServerOnNewRequestReceived(AppSession session, StringRequestInfo requestInfo)
        {
            Console.WriteLine(requestInfo.Key);

            Console.WriteLine(session.SessionID);
            var cm = requestInfo.Key.Split('|');
            switch (cm[0].ToLower())
            {//regiser|username|password
                case "login":
                        using (var db = new LiteDatabase("ChatApp.db"))
                          {
                              Users user;
                              
                              var users = db.GetCollection<Users>("users");
                            

                              if (users.Exists(u => u.UserName == cm[1]))
                              {
                                 
                                  if (users.Exists(u => u.UserName == cm[1] & u.PassWord == cm[2]))
                                  {
                                      user = users.FindOne(u => u.UserName == cm[1]);
                                      SendMssage(session, $"Serverrespon|Welacom {user.UserName} |");
                                      if (!_onlineUser.ContainsValue(user))
                                      {
                                         _onlineUser.Add(session, user);
                                          Console.WriteLine($"[{_onlineUser.Count} users is online]");

                                      }
                                  }
                                  else
                                  {
                                   SendMssage(session, $"Serverrespon|password is wrong|");
                                  }
                                
                                  
                              }
                              else
                              {
                                  user = new Users()
                                  {
                                      // Session = session,
                                      PassWord = cm[2],
                                      UserName = cm[1],
                                      Friends = new List<string>(),
                                      Chats = new List<string>(),
                                      Messages = new List<Tuple<string, string>>()
                                  };
                                  users.Insert(user);
                                  
                                  SendMssage(session,"ServerRespon|Regsiter Succes|");
                                  if (!_onlineUser.ContainsValue(user))
                                  {
                                      _onlineUser.Add(session,user);
                                      Console.WriteLine($"[{_onlineUser.Count} users is online]");

                                  }
                              }
                        }

                   
                      
                  
                 
                 break;
                //todo: add other command like all,send message , broadcasting and ...
                //add friends addf|username
                case "addf":
                    using (var db = new LiteDatabase("ChatApp.db"))
                    {
                        Users user;

                        var users = db.GetCollection<Users>("users");
                        if (users.Exists(u => u.UserName == cm[1]))
                        {
                            var tempcurrentUser = _onlineUser[session];
                            var current = users.FindOne(u => u.UserName == tempcurrentUser.UserName);
                            user = users.FindOne(u => u.UserName == cm[1]);
                            user.Friends.Add(current.UserName);
                            current.Friends.Add(user.UserName);
                            users.Update(current);
                            users.Update(user);

                          

                            //todo; add acc or ignor for adding user
                            SendMssage(session,$"{cm[1]}  and you are friend now");

                        }
                        else
                        {
                            SendMssage(session,$"serverRespon|user with this [{cm[1]}] not found|");
                        }

                    }

                    break;
                case "loadf":
                    var temp_user = _onlineUser[session];
                    string output = "loadf|";
                    using (var db = new LiteDatabase("ChatApp.db"))
                    {
                        var users = db.GetCollection<Users>("users");
                        var user = users.FindOne(u => u.UserName == temp_user.UserName);
                        foreach (var friend in user.Friends)
                        {
                            output += friend + "|";
                        }

                    }


                    Console.WriteLine(output);
                    SendMssage(session,output);
                   

                    break;
                case "chat":
                    

                    break;
                

            }


        }

        public static void SendMssage(AppSession session, string message)
        {
            var Message = Encoding.UTF8.GetBytes(message);
            session.Send(Message, 0, Message.Length);
        }
    }

}

