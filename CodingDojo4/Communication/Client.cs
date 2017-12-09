using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CD4_Client.Communication
{
    class Client
    {
        public byte[] buffer = new byte[256];
        Socket ClientSocket;
        Action<string> MessageInformer;
        Action AbortInformer;

        public Client(Action<string> messageInformer, Action abortInformer)
        {
            try
            {
                MessageInformer = messageInformer;
                AbortInformer = abortInformer;
                TcpClient client = new TcpClient();
                client.Connect(IPAddress.Loopback, 9090);
                ClientSocket = client.Client;
                StartReceiving();

            }
            catch (Exception)
            {

                messageInformer("Server not ready");
                AbortInformer();
            }

        }
        public void StartReceiving()
        {
            Task.Factory.StartNew(Receive);
        }
        public void Receive()
        {
            string message = "";
            while (!message.Equals("@quit"))
            {
                int length = ClientSocket.Receive(buffer);
                message = Encoding.UTF8.GetString(buffer, 0, length);
                //MessageInformer(message);
            }
            Close();
        }
        public void Send(string message)
        {
            if (ClientSocket != null)
            {
                ClientSocket.Send(Encoding.UTF8.GetBytes(message));
            }
        }
        public void Close()
        {
            ClientSocket.Close();
            AbortInformer();
        }
    }
}
