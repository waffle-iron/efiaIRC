using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace classIRC
{
    public class Network
    {
        const String MULTICAST_NET = "239.42.42.42";
        const int MC_PORT = 6667;
        const int IRC_PORT = 194;

        private UdpClient udpClient;
        private static Thread m_t;

        private IPEndPoint endPoint;

        public Network()
        {
            IPAddress address = IPAddress.Parse(MULTICAST_NET);  // Zieladresse

            // Generiere Endpunkt
            endPoint = new IPEndPoint(address, IRC_PORT);

            udpClient = new UdpClient(MC_PORT, AddressFamily.InterNetwork);

            udpClient.JoinMulticastGroup(address);

            Task<UdpReceiveResult> receiveTask = udpClient.ReceiveAsync();

            m_t = new Thread(new ThreadStart(ClientTarget.StartMulticastConversation));
            m_t.Start();

            new IPEndPoint(IPAddress.Any, 50);
        }

        private Message receiveMessage()
        {

        }

        public void sendMessage(Message message)
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize( ,message);

            udpClient.Send(endPoint)
        }
    }
}
