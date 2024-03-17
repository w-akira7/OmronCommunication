using System.Net;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using OmronCommunication;
using OmronCommunication.DataTypes;
using OmronCommunication.Resource;
using OmronCommunication.TinyNet;


namespace OmronCommunication.Protocol
{
    public sealed class FinsUdpDevice : AbstractFinsDevice
    {
        private readonly INetDevice _netudpclient;
        private IPAddress? _localIP;
        private IPAddress? _remoteIP;

        public FinsUdpDevice(IPAddress remoteIP, int remotePort) : base()
        {
            RemoteIP = remoteIP;
            RemotePort = remotePort;
            var remoteAddress= new IPEndPoint(RemoteIP, RemotePort);
            _netudpclient = new NetUdpClient(remoteAddress);
        }
        public FinsUdpDevice(int localPort, IPAddress remoteIP, int remotePort):base()
        {
            LocalPort = localPort;
            RemoteIP = remoteIP;
            RemotePort = remotePort; 
            var remoteAddress = new IPEndPoint(RemoteIP, RemotePort);
            _netudpclient = new NetUdpClient(remoteAddress);
        }
        /// <summary>
        /// 上位机网络地址
        /// </summary>
        public IPAddress? LocalIP 
        {
            get => _localIP;
            private set
            {
                _localIP = value;
                Header.SA1 = value!.GetAddressBytes()[3];
            }
        }
        /// <summary>
        /// 上位机的端口
        /// </summary>
        public int LocalPort
        {
            get;
            private set;
        }
        /// <summary>
        /// 目标网络设备的IP地址
        /// </summary>
        /// 
        public IPAddress? RemoteIP
        {
            get => _remoteIP;
            private set
            {
                _remoteIP = value;
                Header.DA1 = value!.GetAddressBytes()[3];
            }
        }
        /// <summary>
        /// 目标网络设备的端口, omron plc 默认 9600
        /// </summary>
        public int RemotePort { get; private set; } = 9600;
        /// <summary>
        /// 接收超时
        /// </summary>
        public int ReceiveTimeout { get; set; } = 3000;
        public override INetDevice NetDevice => _netudpclient;

        /// <summary>
        /// 建立连接
        /// </summary>
        public override Task ConnectAsync()
        {
           NetDevice.InitWithNoBind(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp);

           //TODO 如果超出长度会出错，需要更完善的网络库
           NetDevice.ReceiveBufferSize = 1024;
           return NetDevice.ConnectAsync(); 
        }

        /// <summary>
        /// 关闭并释放连接
        /// </summary>
        public override void Close()
        {
            NetDevice.Close();
        }

        public override FinsResponse AnalyzeFinsResponse(byte[] result)
        {
            var response = new FinsResponse();
            // a correct fins/udp response contains at least 14 bytes including: fins header, command code, end code 
            if (result.Length >= 14)
            {
                // header
                response.Header.ICF = result[0];
                response.Header.RSV = result[1];
                response.Header.GCT = result[2];
                response.Header.DNA = result[3];
                response.Header.DA1 = result[4];
                response.Header.DA2 = result[5];
                response.Header.SNA = result[6];
                response.Header.SA1 = result[7];
                response.Header.SA2 = result[8];
                response.Header.SID = result[9];
                // command code
                response.CommandCode.MR = result[10];
                response.CommandCode.SR = result[11];
                // end code
                response.EndCode.MainCode = result[12];
                response.EndCode.SubCode = result[13];
                // TODO 处理错误码

                // text
                response.hasText = false;
                if (result.Length > 14)
                {
                    var buffer = new byte[result.Length - 14];
                    Array.Copy(result, 14, buffer, 0, buffer.Length);
                    response.Text = buffer;
                    response.hasText = true;
                }
                return response;
            }
            throw new Exception("The response was not a fins response.");
        }
    }
}
