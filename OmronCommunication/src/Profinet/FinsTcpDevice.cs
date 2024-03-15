using System.Net.Sockets;
using System.Net;
using OmronCommunication.TinyNet;
using OmronCommunication.DataTypes;

namespace OmronCommunication.Profinet
{
    public sealed class FinsTcpDevice : AbstractFinsDevice, IDevice
    {
        private readonly INetDevice _nettcpclient;

        private IPAddress? _localIP;
        private IPAddress? _remoteIP;
        private readonly byte[] _handSignal =
        {
            0x46, 0x49, 0x4E, 0x53, 
            0x00, 0x00, 0x00, 0x0C, 
            0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x01  
        };

        public FinsTcpDevice(IPAddress remoteIP, int remotePort)
        {

            RemoteIP = remoteIP;
            RemotePort = remotePort;
            var remoteAddress = new IPEndPoint(RemoteIP, RemotePort);
            _nettcpclient = new NetUdpClient(remoteAddress);
        }
        public FinsTcpDevice(int localPort, IPAddress remoteIP, int remotePort)
        {
            LocalPort = localPort;
            RemoteIP = remoteIP;
            RemotePort = remotePort;
            var remoteAddress = new IPEndPoint(RemoteIP, RemotePort);
            _nettcpclient = new NetUdpClient(remoteAddress);
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
                Header.SA1 = value.GetAddressBytes()[3];
            }
        }
        /// <summary>
        /// 上位机的端口
        /// </summary>
        public int LocalPort { get; set; }
        /// <summary>
        /// 目标网络设备的IP地址
        /// </summary>
        public IPAddress? RemoteIP
        {
            get => _remoteIP;
            private set
            {
                _remoteIP = value;
                Header.DA1 = value.GetAddressBytes()[3];
            }
        }
        /// <summary>
        /// 目标网络设备的端口, omron plc 默认 9600
        /// </summary>
        public int RemotePort { get; private set; } = 9600;
        public int ReceiveTimeout { get; set; } = 3000;
        public int ReceiveBufferSize { get; set; } = 1024;
        /// <summary>
        /// Fins/Tcp握手信号，每次传输都要加上
        /// </summary>
        public byte[] HandSignal { get => _handSignal; }
        public override INetDevice NetDevice => _nettcpclient;

        /// <summary>
        /// 建立连接
        /// </summary>
        public Task ConnectAsync()
        {
            NetDevice.InitWithNoBind(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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


        /// <summary>
        /// 按 TCP 传输组合完整的 FINS 数据包
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public override byte[] BuildCompleteCommand(byte[] command)
        {
            //基于UDP Command构建 并在开头加上TCP握手信号16字节
            var udpCommand = base.BuildCompleteCommand(command);
            var completeCommand = new byte[udpCommand.Length + 16];
            Array.Copy(HandSignal, 0, completeCommand, 0, 16);
            Array.Copy(udpCommand, 0, completeCommand, 16, udpCommand.Length);
            var dataLength = BitConverter.GetBytes(completeCommand.Length - 8);
            Array.Reverse(dataLength);
            Array.Copy(dataLength, 0, completeCommand, 4, dataLength.Length);
            completeCommand[11] = 0x02;
            return completeCommand;
        }

        public override FinsResponse AnalyzeFinsResponse(byte[] result)
        {
            var response = new FinsResponse();
            // a correct fins/tcp response contains at least 30 bytes including:handsignal, fins header, command code, end code 
            if (result.Length >= 30)
            {
                // header
                response.Header.ICF = result[16];
                response.Header.RSV = result[17];
                response.Header.GCT = result[18];
                response.Header.DNA = result[19];
                response.Header.DA1 = result[20];
                response.Header.DA2 = result[21];
                response.Header.SNA = result[22];
                response.Header.SA1 = result[23];
                response.Header.SA2 = result[24];
                response.Header.SID = result[25];
                // command code
                response.CommandCode.MR = result[26];
                response.CommandCode.SR = result[27];
                // end code
                response.EndCode.MainCode = result[28];
                response.EndCode.SubCode = result[29];
                // TODO 处理错误码

                // text
                response.hasText = false;
                if (result.Length > 30)
                {
                    var buffer = new byte[result.Length - 30];
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
