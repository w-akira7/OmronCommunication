using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using OmronCommunication.DataTypes;


namespace OmronCommunication.Network
{
    public class NetworkDeviceBase:NetworkBase
    {
        public NetworkDeviceBase(IPAddress ipaddress) { IpAddress = ipaddress; }
        public NetworkDeviceBase(string ipaddress) { IpAddress = IPAddress.Parse(ipaddress); }
        public NetworkDeviceBase(string ipaddress, int port) { IpAddress = IPAddress.Parse(ipaddress); Port = port; }
        public NetworkDeviceBase(string ipaddress, int port, string name) { IpAddress = IPAddress.Parse(ipaddress); Port = port; Name = name; }

        /// <summary>
        /// 目标网络设备的名称
        /// </summary>
        public virtual string? Name { get; set; }
        /// <summary>
        /// 目标网络设备的IP地址
        /// </summary>
        public virtual IPAddress IpAddress { get; set; }
        /// <summary>
        /// 目标网络设备的端口
        /// </summary>
        public virtual int Port { get; set; }
        /// <summary>
        /// 目标网络设备的连接状态
        /// </summary>
        public virtual bool IsConnected { get; private set; }
        /// <summary>
        /// 连接目标网络设备
        /// </summary>
        /// <returns></returns>
        protected virtual OperateResult Connect() 
        {
            IPEndPoint endPoint = new IPEndPoint(IpAddress, Port);

            CoreSocket?.Close();
            CoreSocket?.Connect(endPoint);
            return new OperateResult(false);
        }

    }
}
