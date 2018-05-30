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

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for Chatblub.xaml
    /// </summary>
    public partial class Chatblub : UserControl
    {
        public Chatblub()
        {
            InitializeComponent();
        }
        public Chatblub(string message , bool isme)
        {
            InitializeComponent();

            Pm.Text = message;
           
            Time.Text = $"{DateTime.Now.Hour.ToString()}:{DateTime.Now.Minute}";

            
        }
    }
}
