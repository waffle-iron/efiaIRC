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
        //private const int IRC_PORT = 194; // deprecated

        private UdpClient udpSender;
        private UdpClient udpListener;

        /// <summary>
        /// Set up listener (client) to reveive incoming messages
        /// </summary>
        public Network()
        {
            // Set up sender
            IPAddress multicastaddress = IPAddress.Parse(MULTICAST_NET);
            IPEndPoint serverEp = new IPEndPoint(multicastaddress, MC_PORT);            
            udpSender = new UdpClient(serverEp);
            udpSender.JoinMulticastGroup(multicastaddress);

            // Set up listener
            IPEndPoint localEp = new IPEndPoint(IPAddress.Any, MC_PORT);
            udpListener = new UdpClient(localEp);
            udpListener.JoinMulticastGroup(multicastaddress);


            // TODO: insert some wizards to deal with the magical stuff
            // add a Thread to handle udpListener.ReceiveAsync()
            // fire Event if message is incoming
            // restart receiveAsync-Thread
            // write some handler for the event
        }

        /// <summary>
        /// Client catched something (might be important)
        /// </summary>
        /// <returns>Received message</returns>
        private Message receiveMessage()
        {
            IPEndPoint localEp = new IPEndPoint(IPAddress.Any, MC_PORT);

            UdpClient udpClient = new UdpClient(localEp);
            udpClient.JoinMulticastGroup(IPAddress.Parse(MULTICAST_NET));

            byte[] data = udpClient.Receive(ref localEp);

            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();

            object message = formatter.Deserialize(stream);
            return (Message) message;
        }

        /// <summary>
        /// Set up sender to tell some tales
        /// </summary>
        /// <param name="message">Message to send</param>
        public void sendMessage(Message message)
        {
            // Serialize stuff
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream ,message);

            // message out
            byte[] data = stream.ToArray();
            udpSender.SendAsync(data, data.Length);
        }
    }
}
