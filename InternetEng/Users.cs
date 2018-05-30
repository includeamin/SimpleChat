using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using SuperSocket.SocketBase;

namespace InternetEng
{
    class Users
    {
      
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PassWord { set; get; }
      
        public List<Tuple<string,string>> Messages { set; get; }
        
        public List<string> Friends { set; get; }
      
        public List<string> Chats { set; get; }
    }
}
