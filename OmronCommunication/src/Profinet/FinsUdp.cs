using System.Net;
using System.Net.Sockets;
using OmronCommunication.DataTypes;


namespace OmronCommunication.Profinet
{
    public class FinsUdp : FinsCommand, IProfinet
    {
        private readonly UdpClient? _udpClient;

        public FinsUdp(string remoteIP, int remotePort)
        {
            _udpClient = new UdpClient();
            RemoteIP = IPAddress.Parse(remoteIP);
            RemotePort = remotePort;
            LocalIP = ((IPEndPoint)UdpClient.Client.LocalEndPoint!).Address;
        }
        
        public UdpClient UdpClient => _udpClient!;

        /// <summary>
        /// 上位机网络地址
        /// </summary>
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
            private set
            {
                RemoteIP = value;
                DA1 = value.GetAddressBytes()[3];
            }
        }

        /// <summary>
        /// 目标网络设备的端口, omron plc 默认 9600
        /// </summary>
        public int RemotePort { get; private set; } = 9600;

        #region Networkstream

        public OperationResult Send(byte[] data)
        {
            var result = new OperationResult();

            try
            {
                UdpClient.Send(data, data.Length, new IPEndPoint(RemoteIP, RemotePort));
                result.IsSuccess = true;
                return result;

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
                return result;
            }

        }

        public OperationResult<byte[]> Receive()
        {
            var result = new OperationResult<byte[]>();

            var remoteEp = new IPEndPoint(RemoteIP, RemotePort);
            try
            {
                var buffer = UdpClient.Receive(ref remoteEp);

                result.IsSuccess = true;
                result.Value = buffer;

                return result;
            }
            catch (Exception ex)
            {
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

        #endregion

        #region Read Write

        public OperationResult Write(string address, byte[] data, bool isBit)
        {
            //BuildWriteCommand
            var command = BuildWriteFinsCommand(address, data, isBit);


            //ReadFromPLC
            var response = SendAndReceive(command.Value);


            //DataAnalysis
            var result = AnalyzeFinsResponse(response);


            return result;
        }

        public OperationResult<byte[]> Read(string address, ushort length, bool isBit)
        {
            //BuildReadCommand
            var command = BuildReadFinsCommand(address, length, isBit);


            //ReadFromPLC
            var response = SendAndReceive(command.Value);


            //DataAnalysis
            var result = AnalyzeFinsResponse(response);

            return result;
        }
        #endregion

    }
}
