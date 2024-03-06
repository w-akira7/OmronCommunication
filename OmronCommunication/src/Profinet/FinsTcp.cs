using System.Net.Sockets;
using System.Net;
using OmronCommunication.DataTypes;
using System.IO;

namespace OmronCommunication.Profinet
{
    internal class FinsTcp : FinsCommand, IProfinet
    {
        private readonly TcpClient _tcpClient;

        public FinsTcp(string remoteIP, int remotePort)
        {
            _tcpClient = new TcpClient();
            RemoteIP = IPAddress.Parse(remoteIP);
            RemotePort = remotePort;
            LocalIP = ((IPEndPoint)TcpClient.Client.LocalEndPoint!).Address;
        }

        public TcpClient TcpClient { get { return _tcpClient; } }

        public bool IsTcp()
        {
            return true;
        }

        public bool IsUdp()
        {
            return false;
        }

        public IPAddress LocalIP
        {
            get => LocalIP;
            private set
            {
                LocalIP = value;
                SA1 = value.GetAddressBytes()[3];
            }
        }

        /// <summary>
        /// 目标网络设备的IP地址
        /// </summary>
        public IPAddress RemoteIP
        {
            get => RemoteIP;
            set
            {
                RemoteIP = value;
                DA1 = value.GetAddressBytes()[3];
            }
        }

        /// <summary>
        /// 目标网络设备的端口, omron plc 默认 9600
        /// </summary>
        public int RemotePort { get; set; } = 9600;


        #region HandSignal

        private byte[] _handSignal =
        {
            0x46, 0x49, 0x4E, 0x53, // FINS
            0x00, 0x00, 0x00, 0x0C, // 后面的命令长度
            0x00, 0x00, 0x00, 0x00, // 命令码
            0x00, 0x00, 0x00, 0x00, // 错误码
            0x00, 0x00, 0x00, 0x01  // 节点号
        };

        public byte[] HandSignal { get => _handSignal; }

        #endregion


        #region Connect

        public OperationResult ConnectPLC()
        {
            try
            {
                TcpClient.Connect(RemoteIP, RemotePort);
                return new OperationResult(true);

            }
            catch (Exception ex)
            {
                return new OperationResult(false)
                {
                    Message = ex.Message,
                };
            }
        }

        public OperationResult ClosePLC()
        {
            try
            {
                TcpClient.Close();
                return new OperationResult(true);

            }
            catch (Exception ex)
            {
                return new OperationResult(false)
                {
                    Message = ex.Message,
                };
            }
        }

        public OperationResult Write(string address, byte[] data, bool isBit)
        {
            throw new NotImplementedException();
        }

        public OperationResult<byte[]> Read(string address, ushort length, bool isBit)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Send & Receive

        public void Send(byte[] data)
        {
            var stream = new NetworkStream(TcpClient.Client);

            stream.Write(data, 0, data.Length);
        }

        public byte[] Receive()
        {
            var stream = new NetworkStream(TcpClient.Client);
            var buff = new byte[2048];
            stream.Read(buff);
            return buff;
        }


        #endregion

        public override OperationResult<byte[]> AnalyzeFinsResponse(OperationResult<byte[]> result)
        {
            return base.AnalyzeFinsResponse(result);
        }

    }
}
