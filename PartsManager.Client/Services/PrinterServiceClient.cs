using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace PartsManager.Client.Services
{
    public class PrinterServiceClient : IDisposable
    {
        private TcpClient _client;
        private string _ip;
        private int _port;

        public PrinterServiceClient(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        public bool Connect()
        {
            try
            {
                if (_client != null && _client.Connected) return true;
                _client = new TcpClient();
                var result = _client.BeginConnect(_ip, _port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2));
                if (!success) return false;
                _client.EndConnect(result);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SendPrintRequest(string template, Dictionary<string, string> fields)
        {
            if (!Connect()) return false;

            try
            {
                var cmd = new
                {
                    Type = "PRINT",
                    Template = template,
                    Fields = fields
                };

                string json = JsonConvert.SerializeObject(cmd);
                byte[] payload = Encoding.UTF8.GetBytes(json);
                byte[] header = new byte[5];
                header[0] = 0x02; // STX
                
                // Length (Big-endian)
                byte[] lenBytes = BitConverter.GetBytes(payload.Length);
                if (BitConverter.IsLittleEndian) Array.Reverse(lenBytes);
                Array.Copy(lenBytes, 0, header, 1, 4);

                NetworkStream stream = _client.GetStream();
                stream.Write(header, 0, 5);
                stream.Write(payload, 0, payload.Length);
                stream.WriteByte(0x03); // ETX
                return true;
            }
            catch
            {
                _client?.Close();
                _client = null;
                return false;
            }
        }

        public void Dispose()
        {
            _client?.Close();
        }
    }
}
