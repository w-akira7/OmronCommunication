using System.Net;
using System.Net.Sockets;
using OmronCommunication;


namespace OmronCommunication.Profinet
{
    public class FinsUdp : FinsCommand, IProfinet
    {
        private readonly UdpClient? _udpClient;
        private IPAddress? _localIP;
        private IPAddress? _remoteIP;

        public FinsUdp(string localIP, string remoteIP, int remotePort)
        {
            _udpClient = new UdpClient();
            _remoteIP = IPAddress.Parse(remoteIP);
            _localIP = IPAddress.Parse(localIP);
            RemotePort = remotePort;
            DA1 = RemoteIP!.GetAddressBytes()[3];
            SA1 = LocalIP!.GetAddressBytes()[3];
            FinsUdpClient.Client.ReceiveTimeout = ReceiveTimeout;
        }

        public UdpClient FinsUdpClient => _udpClient!;
        /// <summary>
        /// 上位机网络地址
        /// </summary>
        public IPAddress? LocalIP => _localIP;    
        /// <summary>
        /// 目标网络设备的IP地址
        /// </summary>
        /// 
        public IPAddress? RemoteIP => _remoteIP;
        /// <summary>
        /// 目标网络设备的端口, omron plc 默认 9600
        /// </summary>
        public int RemotePort { get; private set; } = 9600;
        /// <summary>
        /// 接收超时
        /// </summary>
        public int ReceiveTimeout { get; set; } = 5000;

        public OperationResult Send(byte[] data)
        {
            var result = new OperationResult();
            try
            {
                FinsUdpClient.Send(data, data.Length, new IPEndPoint(RemoteIP!, RemotePort));
                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                //日志记录异常
                result.IsSuccess = false;
                result.Message = ex.Message;
                return result;
            }
        }

        public OperationResult<byte[]> Receive()
        {
            var result = new OperationResult<byte[]>();

            var remoteEp = new IPEndPoint(RemoteIP!, RemotePort);
            try
            {
             
                var buffer = FinsUdpClient.Receive(ref remoteEp);

                result.IsSuccess = true;
                result.Value = buffer;

                return result;
            }
            catch (Exception ex)
            {
                //日志记录异常
                result.IsSuccess = false;
                result.Message = ex.Message;
                return result;
            }
        }


        public OperationResult<byte[]> SendAndReceive(byte[] data)
        {
            Send(data);

            var result = Receive();

            return result;
        }

        //
        //TODO 由于UDP要么发送要么丢包，需要增加发送失败检测功能
        //

        #region Read Write

        public OperationResult Write(string address, byte[] data, bool isBit)
        {
            //BuildWriteCommand
            var command = BuildFinsWriteCommand(address, data, isBit);
            if (!command.IsSuccess) return OperationResult.CreateFailResult<byte[]>();

            //ReadFromPLC
            var response = SendAndReceive(command.Value);
            if (!response.IsSuccess) return OperationResult.CreateFailResult<byte[]>();

            //DataAnalysis
            var result = AnalyzeFinsResponse(response);
            return result;
        }

        public OperationResult<byte[]> Read(string address, ushort length, bool isBit)
        {
            //BuildReadCommand
            var command = BuildFinsReadCommand(address, length, isBit);
            if (!command.IsSuccess) return OperationResult.CreateFailResult<byte[]>();

            //ReadFromPLC
            var response = SendAndReceive(command.Value);
            if (!response.IsSuccess) return OperationResult.CreateFailResult<byte[]>();
           
            //DataAnalysis
            var result = AnalyzeFinsResponse(response);
            return result;
        }
        #endregion
    }
}
