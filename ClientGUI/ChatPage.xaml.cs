using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SuperSocket.ClientEngine;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    public partial class ChatPage : Page
    {
        private string _userName;
        private ClientSession _client;
        public ChatPage(string userName,ClientSession session)
        {
            InitializeComponent();
            this._client = session;
            _userName = userName;
            _client.DataReceived+=ClientOnDataReceived;
        }

        private void ClientOnDataReceived(object sender, DataEventArgs dataEventArgs)
        {
            var message = Encoding.UTF8.GetString(dataEventArgs.Data, 0, dataEventArgs.Data.Length);
            // MessageBox.Show(message);
            //chat|amin|<message content>
            var cm = message.Split('|');
            switch (cm[0].ToLower())
            {
                case "chat":
                    addMessage(cm[2],false);
                    break;
            }
        }
        
        public void addMessage(string message , bool isMe)
        {
            if (isMe)
            {
                ChatPanel.Children.Add(new Chatblub(message, isMe)
                {
                    HorizontalAlignment = HorizontalAlignment.Right
                });
            }
            else
            {
                ChatPanel.Children.Add(new Chatblub(message, isMe)
                {
                    HorizontalAlignment = HorizontalAlignment.Left
                });
            }
        }

        private void SendMessage_OnClick(object sender, RoutedEventArgs e)
        {
            SendMssage(_client, message.Text);
            addMessage(message.Text,true);
           
        }
        public  void SendMssage(ClientSession session, string message)
        {
            var Message = Encoding.UTF8.GetBytes(message);
            var newLine = Encoding.UTF8.GetBytes(Environment.NewLine);
            session.Send(Message, 0, Message.Length);
            session.Send(newLine,0,newLine.Length);
        }
    }
}
