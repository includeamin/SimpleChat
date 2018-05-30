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
    /// Interaction logic for addf.xaml
    /// </summary>
    public partial class addf : Page
    {
        private ClientSession _client;
        public addf()
        {
            InitializeComponent();
        }
        public addf(ClientSession session)
        {
            InitializeComponent();
            _client = session;
        }

        private void Addf_OnClick(object sender, RoutedEventArgs e)
        {
            string message = $"Addf|{UserName.Text}|";
            var bin = Encoding.UTF8.GetBytes(message);
            var newLine = Encoding.UTF8.GetBytes(Environment.NewLine);
            _client.Send(bin, 0, bin.Length);
            _client.Send(newLine, 0, newLine.Length);
        }
    }
}
