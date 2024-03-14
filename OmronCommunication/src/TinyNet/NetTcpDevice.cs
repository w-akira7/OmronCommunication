using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OmronCommunication.TinyNet
{
    public class NetTcpDevice : AbstractNetDevice, INetDevice
    {
        public NetTcpDevice(EndPoint deviceAddress) : base(deviceAddress)
        {
        }
    }
}
