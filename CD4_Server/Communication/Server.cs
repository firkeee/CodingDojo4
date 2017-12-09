using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CD4_Server.Communication
{
    class Server
    {
        Socket serverSocket;
        List<ClientHandler> clients = new List<ClientHandler>();
        Thread acceptingThread;
        Action<string> GUIUpdater;

        public Server(Action<string> guiUpdater)
        {
            GUIUpdater = guiUpdater;
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Loopback, 9090));
            serverSocket.Listen(10);
        }

        public void StartAccepting()
        {
            acceptingThread = new Thread(new ThreadStart(Accept));
            acceptingThread.IsBackground = true;
            acceptingThread.Start();
        }
        public void Accept()
        {
            while (acceptingThread.IsAlive)
            {
                clients.Add(new ClientHandler(serverSocket.Accept(), new Action<string, Socket>(NewMessageReceived)));
            }
        }
        public void StopAccepting()
        {
            serverSocket.Close();
            acceptingThread.Abort();
            foreach (var item in clients)
            {
                item.Close();
            }
            clients.Clear();
        }
        public void DisconnectClient(string name)
        {
            foreach (var item in clients)
            {
                if (item.Name.Equals(name))
                {
                    item.Close();
                    clients.Remove(item);
                    break;
                }
            }
        }

        private void NewMessageReceived(string message, Socket senderSocket)
        {
            //update gui
            GUIUpdater(message);
            //write message to all clients
            foreach (var item in clients)
            {
                if (item.ClientSocket != senderSocket)
                {
                    item.Send(message);
                }
            }
        }
    }
}
