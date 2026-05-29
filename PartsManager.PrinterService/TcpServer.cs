using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;

namespace PartsManager.PrinterService
{
    public class TcpServer
    {
        private TcpListener _listener;
        private Thread _listenThread;
        private bool _isRunning;
        private int _port;
        private List<TcpClient> _clients = new List<TcpClient>();

        public event Action<string> MessageReceived;
        public event EventHandler StatusChanged;

        public bool IsClientConnected => _clients.Count > 0;

        public TcpServer(int port)
        {
            _port = port;
        }

        public void Start()
        {
            _isRunning = true;
            _listener = new TcpListener(IPAddress.Any, _port);
            _listener.Start();
            _listenThread = new Thread(ListenForClients);
            _listenThread.IsBackground = true;
            _listenThread.Start();
        }

        public void Stop()
        {
            _isRunning = false;
            _listener?.Stop();
            foreach (var client in _clients) client.Close();
            _clients.Clear();
        }

        private void ListenForClients()
        {
            while (_isRunning)
            {
                try
                {
                    TcpClient client = _listener.AcceptTcpClient();
                    lock (_clients) _clients.Add(client);
                    StatusChanged?.Invoke(this, EventArgs.Empty);
                    
                    Thread clientThread = new Thread(HandleClientComm);
                    clientThread.IsBackground = true;
                    clientThread.Start(client);
                }
                catch { }
            }
        }

        private void HandleClientComm(object clientObj)
        {
            TcpClient tcpClient = (TcpClient)clientObj;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] buffer = new byte[4096];
            List<byte> messageData = new List<byte>();

            while (_isRunning && tcpClient.Connected)
            {
                int bytesRead = 0;
                try
                {
                    bytesRead = clientStream.Read(buffer, 0, 4096);
                }
                catch { break; }

                if (bytesRead == 0) break;

                for (int i = 0; i < bytesRead; i++)
                {
                    byte b = buffer[i];
                    if (b == 0x02) // STX
                    {
                        messageData.Clear();
                    }
                    else if (b == 0x03) // ETX
                    {
                        // 修正： messageData 內應包含 [Length(4)] + [Type(1)] + [Payload(N)]
                        // 所以總長度必須 > 5
                        if (messageData.Count > 5)
                        {
                            try
                            {
                                // 略過前 5 Bytes (Length + Type)，擷取 JSON Payload
                                string json = Encoding.UTF8.GetString(messageData.ToArray(), 5, messageData.Count - 5);
                                MessageReceived?.Invoke(json);
                            }
                            catch (Exception ex)
                            {
                                // 可以在這裡記錄 Encoding 錯誤
                            }
                        }
                        messageData.Clear();
                    }
                    else
                    {
                        messageData.Add(b);
                    }
                }
            }

            lock (_clients) _clients.Remove(tcpClient);
            StatusChanged?.Invoke(this, EventArgs.Empty);
            tcpClient.Close();
        }
    }
}
