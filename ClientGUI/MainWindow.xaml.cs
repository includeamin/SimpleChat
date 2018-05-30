using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
using MaterialDesignThemes.Wpf;
using MessagePack;
using SuperSocket.ClientEngine;

namespace ClientGUI
{
    [MessagePackObject()]
    public class Protocol
    {
        [Key(0)]
        public string Command { get; set; }
        [Key(1)]
        public object ProtocolObj { get; set; }
    }
    class Register
    {
        public string UserName { get; set; }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string _host;
        private readonly int _port;
        private ClientSession _client;
        private string _SessionId;
        private bool _FirstConnect;
        public MainWindow()
        {
            InitializeComponent();
            _host = "127.0.0.1";
            _port = 1212;
            _FirstConnect = true;
            Task.Factory.StartNew(Init);
            
        }


        public void Init()
        {
            _client = new AsyncTcpSession();
            _client.Closed+=ClientOnClosed;
            _client.Connected+=ClientOnConnected;
            _client.DataReceived+=ClientOnDataReceived;
            _client.Error+=ClientOnError;

            
            _client.Connect(new DnsEndPoint(host:_host,port:_port));
            if (!_client.IsConnected)
            {
                Dispatcher.Invoke(() => { ConnectionStatusLbl.Text = "Online";});
            }
            else
            {
                Dispatcher.Invoke(() => { ConnectionStatusLbl.Text = "Offline";});
            }
            
            
        }
        
        public void LoadF(string friends)
        {

        }

        private void ClientOnError(object sender, ErrorEventArgs errorEventArgs)
        {
          
        }

        private void ClientOnDataReceived(object sender, DataEventArgs dataEventArgs)
        {
          
            if (_FirstConnect)
            {
                _SessionId =  Encoding.UTF8.GetString(dataEventArgs.Data, 0, dataEventArgs.Data.Length);
            
                _FirstConnect = false;

            }
            else
            {
                var message = Encoding.UTF8.GetString(dataEventArgs.Data, 0, dataEventArgs.Data.Length);
               // MessageBox.Show(message);
                var cm = message.Split('|');
                switch (cm[0].ToLower())
                {
                    case "serverrespon":
                        Dispatcher.Invoke(() => { ServerStatus.Text = cm[1]; });
                  
                        break;
                    case "loadf":
                       
                        for (int i =1; i < cm.Length-2; i++)
                        {
                            Dispatcher.Invoke(() => { FriendView.Items.Add(new Chip()
                            {
                                Content = cm[i],
                                Icon = cm[i][0],
                                Foreground =(Brush)new  BrushConverter().ConvertFromString("#DDEBE5E5"),
                                //#FFF07A00
                                IconBackground = (Brush)new BrushConverter().ConvertFromString("#FFF07A00")

                            }); });
                        }
                        break;
                }

            }
        }

        private void ClientOnConnected(object sender, EventArgs eventArgs)
        {
           
        }

        private void ClientOnClosed(object sender, EventArgs eventArgs)
        {
            
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            const string message = "loadf|aminjamal1|9518867";
            var bin = Encoding.UTF8.GetBytes(message);
            var newLine = Encoding.UTF8.GetBytes(Environment.NewLine);
            _client.Send(bin,0,bin.Length);
            _client.Send(newLine,0,newLine.Length);
        }

        private void Loign_OnMouseLeftButtonDown(object sender, RoutedEventArgs routedEventArgs)
        {
            Frame.Content = new Login(_client);
        }

        private void AddF_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Content = new addf(_client);
        }

        private void Chattest_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Content = new ChatPage("aminjamal",_client);
        }
    }
}
