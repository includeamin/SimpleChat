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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private ClientSession _client;

        public Login()
        {
            InitializeComponent();
        }
        public Login(ClientSession session)
        {
            InitializeComponent();
            this._client = session;
        }
        private void LoginBtn_OnClick(object sender, RoutedEventArgs e)
        {
             string message = $"Login|{UserName.Text}|{PassWord.Password}";
            var bin = Encoding.UTF8.GetBytes(message);
            var newLine = Encoding.UTF8.GetBytes(Environment.NewLine);
            _client.Send(bin, 0, bin.Length);
            _client.Send(newLine, 0, newLine.Length);
        }

        
        
    }
}
