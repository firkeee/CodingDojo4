using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CD4_Server.Communication
{
    class ClientHandler
    {
        private Func<Socket> accept;
        private Action<string, Socket> action;
        public string Name { get; set; }
        public Socket ClientSocket { get; set; }

        public ClientHandler(Func<Socket> accept, Action<string, Socket> action)
        {
            this.accept = accept;
            this.action = action;
        }

        public void Close()
        {

        }
        public void Send(string message)
        {

        }
        public void Receive()
        {

        }
    }
}
