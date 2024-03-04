using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using OmronCommunication.DataTypes;
using OmronCommunication.Thread;


namespace OmronCommunication.Transmission
{
    public class NetUDPDevice : NetBase, IDevice
    {
        public NetUDPDevice(IPAddress ipaddress) { IpAddress = ipaddress; coreSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); }
        public NetUDPDevice(string ipaddress) { IpAddress = IPAddress.Parse(ipaddress); coreSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); }
        public NetUDPDevice(string ipaddress, int port) { IpAddress = IPAddress.Parse(ipaddress); Port = port; coreSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); }
        public NetUDPDevice(string ipaddress, int port, string name) { IpAddress = IPAddress.Parse(ipaddress); Port = port; Name = name; coreSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); }

        /// <summary>
        /// 目标网络设备的名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 目标网络设备的IP地址
        /// </summary>
        public IPAddress IpAddress { get; set; }
        /// <summary>
        /// 目标网络设备的端口
        /// </summary>
        public int Port { get; set; } = 9600;
        /// <summary>
        /// 目标网络设备的连接状态
        /// </summary>
        public bool IsConnected { get; private set; }

        public int CacheLength { get; set; } = 2048;


        public OperateResult Send(byte[] data)
        {
            var result = new OperateResult();

            coreSocket!.SendTo(data, data.Length, SocketFlags.None, new IPEndPoint(IpAddress, Port));

            result.IsSuccess = true;

            return result;
        }
        public OperateResult<byte[]> Receive()
        {
            var result = new OperateResult<byte[]>();
            var buff = new byte[CacheLength];
            var remoteEp = new IPEndPoint(IpAddress, Port);
            var remote = (EndPoint)remoteEp;
            coreSocket!.ReceiveFrom(buff, ref remote);

            result.IsSuccess = true;
            result.Value = buff;

            return result;
        }      

        public OperateResult<byte[]> SendAndReceive(byte[] data)
        {
            Send(data);

            var result =  Receive();

            return result;
        }
    }
}
