using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CD4_Server.Communication
{
    class ClientHandler
    {
        private Action<string, Socket> action;
        public string Name { get; private set; }
        public Socket ClientSocket { get; private set; }
        const string endMessage = "@quit";
        public byte[] buffer = new byte[256];
        private Thread clientReceiveThread;

        public ClientHandler(Socket cSocket, Action<string, Socket> action)
        {
            this.ClientSocket = cSocket;
            this.action = action;
            clientReceiveThread = new Thread(Receive);
            clientReceiveThread.Start();
        }

        public void Close()
        {
            Send(endMessage);
            ClientSocket.Close(1);
            clientReceiveThread.Abort();
        }
        public void Send(string message)
        {
            ClientSocket.Send(Encoding.UTF8.GetBytes(message));
        }
        public void Receive()
        {
            string message = "";
            while (!message.Equals(endMessage))
            {
                int length = ClientSocket.Receive(buffer);
                message = Encoding.UTF8.GetString(buffer, 0, length);

                if(Name==null && message.Contains(":"))
                {
                    Name = message.Split(':')[0];
                }
                action(message, ClientSocket);
            }
            Close();
        }
    }
}
