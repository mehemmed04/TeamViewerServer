using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TeamViewerServer.NetworkHelper
{
    public class Network
    {
        public static string IP;
        public static int PORT;
        public static TcpListener listener = null;
        public static BinaryReader br = null;
        public static List<TcpClient> Clients { get; set; } = new List<TcpClient>();

        public static void Connect()
        {
            IP = StaticMembers.GetLocalIpAddress();
            PORT = StaticMembers.GetLocalPort();
            var ip = IPAddress.Parse(IP);
            Clients = new List<TcpClient>();
            var ep = new IPEndPoint(ip, PORT);
            listener = new TcpListener(ep);
            listener.Start();

            MessageBox.Show($"Listening on {listener.LocalEndpoint}");

           

        }
    }
}