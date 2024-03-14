using System.Net;
using System.Net.Sockets;
using OmronCommunication;
using OmronCommunication.DataTypes;
using OmronCommunication.Resource;
using OmronCommunication.src.TinyNet;
using OmronCommunication.Thread;
using OmronCommunication.TinyNet;


namespace OmronCommunication.Profinet
{
    public class FinsUdp : FinsCommand, IProfinet
    {
        private readonly INetDevice _netdevice;

        private UdpClient? _udpclient;
        private IPAddress? _localIP;
        private IPAddress? _remoteIP;
        private bool _isActive = false;

        public FinsUdp(IPAddress remoteIP, int remotePort)
        {
            RemoteIP = remoteIP;
            RemotePort = remotePort;
            var remoteAddress= new IPEndPoint(RemoteIP, RemotePort);
            _netdevice = new NetUdpDevice(remoteAddress);
        }
        public FinsUdp(int localPort, IPAddress remoteIP, int remotePort)
        {
            LocalPort = localPort;
            RemoteIP = remoteIP;
            RemotePort = remotePort;
            var remoteAddress = new IPEndPoint(RemoteIP, RemotePort);
            _netdevice = new NetUdpDevice(remoteAddress);
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
                SA1 = value!.GetAddressBytes()[3];
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
                DA1 = value!.GetAddressBytes()[3];
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
        public INetDevice NetDevice => _netdevice;
        /// <summary>
        /// 建立连接
        /// </summary>
        public Task ConnectAsync()
        {
           NetDevice.InitWithNoBind(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp);
           NetDevice.ReceiveBufferSize = 1024;
           return NetDevice.ConnectAsync(); 
        }

        /// <summary>
        /// 关闭并释放连接
        /// </summary>
        public void Close()
        {
            NetDevice.Close();
        }

       
        #region Read Write

        public async Task Write(string address, byte[] data, bool isBit)
        {
            //BuildWriteCommand
            var command = BuildFinsWriteCommand(address, data, isBit);

            //ReadFromPLC
            var response = await NetDevice.ResqusetWaitResponse(command);

            //DataAnalysis
            AnalyzeFinsResponse(response);
        }

        public async Task<byte[]> Read(string address, ushort length, bool isBit)
        {
            //BuildReadCommand
            var command = BuildFinsReadCommand(address, length, isBit);

            //ReadFromPLC
            var response = await NetDevice.ResqusetWaitResponse(command);
           
            //DataAnalysis
            var result = AnalyzeFinsResponse(response);
            return result.Data;
        }
        #endregion
    }
}
