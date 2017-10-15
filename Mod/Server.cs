using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Mod
{
    public class Server
    {
        /*
        private readonly TcpListener _listener;

        public Server(int port)
        {
            if (_listener != null) return;
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();
            new Thread(Handler).Start();
        }

        public void Stop()
        {
            _listener.Stop();
        }

        private void Handler()
        {
            while (true)
            {
                TcpClient client = _listener.AcceptTcpClient();
                using (NetworkStream stream = client.GetStream())
                {
                    var data = new byte[client.ReceiveBufferSize];
                    int bytesRead = stream.Read(data, 0, client.ReceiveBufferSize);
                    string request = Encoding.UTF8.GetString(data, 0, bytesRead);
                    Match match = new Regex(@"(\d+) (\d+.\d+.\d+.\d+)").Match(request);
                    if (match.Success)
                    {
                        if (!Core.PlayersIp.ContainsKey(match.Groups[1].Value.toInt()))
                            Core.PlayersIp.Add(match.Groups[1].Value.toInt(), match.Groups[2].Value);
                        var msg = Encoding.Default.GetBytes("200 OK");
                        stream.Write(msg, 0, msg.Length);
                    }
                    else
                    {
                        var msg = Encoding.Default.GetBytes("400 Bad Request");
                        stream.Write(msg, 0, msg.Length);
                    }
                    
                }
                client.Close();
            }
        }*/
    }
}
