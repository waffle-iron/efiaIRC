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
using System.IO;

namespace classIRC
{
    public class Network
    {
        private const String MULTICAST_NET = "239.42.42.42";
        private const int MC_PORT = 6667;
        private const int IRC_PORT = 194;

        private UdpClient udpClient;
        private IPEndPoint endPoint;

        private static Thread m_t;

        public Network()
        {
            IPAddress address = IPAddress.Parse(MULTICAST_NET);  // Zieladresse
            endPoint = new IPEndPoint(address, MC_PORT);
            udpClient = new UdpClient(MC_PORT, AddressFamily.InterNetwork);

            udpClient.JoinMulticastGroup(address);

            Task<UdpReceiveResult> receiveTask = udpClient.ReceiveAsync();

            m_t = new Thread(new ThreadStart(ClientTarget.StartMulticastConversation));
            m_t.Start();

            new IPEndPoint(IPAddress.Any, 50);
        }

        private Message receiveMessage()
        {
            byte[] data = udpClient.Receive(ref endPoint);

            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();

            object message = formatter.Deserialize(stream);
            return (Message) message;
        }

        public void sendMessage(Message message)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream ,message);

            byte[] data = stream.ToArray();

            udpClient.Send(data, data.Length, endPoint);
        }
    }
}
