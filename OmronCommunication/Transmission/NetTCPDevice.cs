using OmronCommunication.DataTypes;
using OmronCommunication.Thread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OmronCommunication.Transmission
{
    internal class NetTCPDevice : NetBase, IDevice


    {

        public NetTCPDevice(IPAddress ipaddress) { IpAddress = ipaddress; coreSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); }
        public NetTCPDevice(string ipaddress) { IpAddress = IPAddress.Parse(ipaddress); coreSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); }
        public NetTCPDevice(string ipaddress, int port) { IpAddress = IPAddress.Parse(ipaddress); Port = port; coreSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); }
        public NetTCPDevice(string ipaddress, int port, string name) { IpAddress = IPAddress.Parse(ipaddress); Port = port; Name = name; coreSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); }

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


        public void Connect()
        {
            if (!IsConnected)
            {
                coreSocket!.Connect(IpAddress, Port);
                IsConnected = true;
            }
        }

        public void Close()
        {
            coreSocket!.Close();
            IsConnected = false;
        }



        public OperateResult Send(byte[] data)
        {
            var result = new OperateResult();

            if (IsConnected) 
            {

                coreSocket!.Send(data);

                result.IsSuccess = true;
            }


            return result;
        }
        public OperateResult<byte[]> Receive()
        {
            var result = new OperateResult<byte[]>();
            var buff = new byte[CacheLength];
            var remoteEp = new IPEndPoint(IpAddress, Port);
            var remote = (EndPoint)remoteEp;
            if (IsConnected)
            {
                coreSocket!.Receive(buff);

                result.IsSuccess = true;
                result.Value = buff;
            }
            return result;
        }

        public OperateResult<byte[]> SendAndReceive(byte[] data)
        {
            Send(data);

            var result = Receive();

            return result;
        }
    }
}
