using System.Net;
using System.Net.Sockets;
using OmronCommunication;
using OmronCommunication.Resource;


namespace OmronCommunication.Profinet
{
    public class FinsUdp : FinsCommand, IProfinet
    {
        private UdpClient? _udpclient;
        private IPAddress? _localIP;
        private IPAddress? _remoteIP;
        private bool _isActive = false;

        public FinsUdp(IPAddress remoteIP, int remotePort)
        {
            RemoteIP = remoteIP;
            RemotePort = remotePort;
        }
        public FinsUdp(int localPort, IPAddress remoteIP, int remotePort)
        {
            LocalPort = localPort;
            RemoteIP = remoteIP;
            RemotePort = remotePort;
        }

        public bool IsActive => _isActive; 
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

        /// <summary>
        /// 建立连接
        /// </summary>
        public void Connect()
        {
            if (!_isActive)
            {
                if (LocalPort > 0)
                {
                    //指定一个端口
                    _udpclient = new UdpClient(LocalPort);
                }
                else
                {
                    _udpclient = new UdpClient();
                }
                var remoteEP = new IPEndPoint(RemoteIP, RemotePort);
                Connect(remoteEP);
                _isActive = true;

                _udpclient.Client.ReceiveTimeout = ReceiveTimeout;
                LocalIP = ((IPEndPoint)_udpclient.Client.LocalEndPoint).Address;
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 关闭并释放连接
        /// </summary>
        public void Close()
        {
            if (_isActive)
            {
                _udpclient.Close();
                _isActive = false;
            }
        }

        private void Connect(IPEndPoint remoteEP)
        {
            _udpclient.Connect(remoteEP);
        }

        /// <summary>
        /// 基于UDP报文的一次交互
        /// </summary>
        public OperationResult<byte[]> SendAndReceive(byte[] data)
        {
            try
            {
                //同步发送
                _udpclient.Send(data, data.Length);

                //同步接收
                var remoteEP = new IPEndPoint(RemoteIP, RemotePort);
                var received = _udpclient.Receive(ref remoteEP);

                return OperationResult.CreateSuccessResult(received);
            }
            catch (Exception e)
            {
                return OperationResult.CreateFailResult<byte[]>(e.Message, ErrorCode.NetSendReceiveError);
            }
        }

        #region Read Write

        public OperationResult Write(string address, byte[] data, bool isBit)
        {
            //BuildWriteCommand
            var command = BuildFinsWriteCommand(address, data, isBit);
            if (!command.IsSuccess) return OperationResult.CreateFailResult<byte[]>(command);

            //ReadFromPLC
            var response = SendAndReceive(command.Value);
            if (!response.IsSuccess) return OperationResult.CreateFailResult<byte[]>(response);

            //DataAnalysis
            var result = AnalyzeFinsResponse(response);
            return result;
        }

        public OperationResult<byte[]> Read(string address, ushort length, bool isBit)
        {
            //BuildReadCommand
            var command = BuildFinsReadCommand(address, length, isBit);
            if (!command.IsSuccess) return OperationResult.CreateFailResult<byte[]>(command);

            //ReadFromPLC
            var response = SendAndReceive(command.Value);
            if (!response.IsSuccess) return OperationResult.CreateFailResult<byte[]>(response);
           
            //DataAnalysis
            var result = AnalyzeFinsResponse(response);
            return result;
        }

        #endregion
    }
}
