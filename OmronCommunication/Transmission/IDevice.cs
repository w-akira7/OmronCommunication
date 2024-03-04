using OmronCommunication.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmronCommunication.Transmission
{
    /// <summary>
    /// 传输方式的抽象接口
    /// </summary>
   public interface IDevice
   {
        public OperateResult Send(byte[] data);

        public OperateResult<byte[]> Receive();

        public OperateResult<byte[]> SendAndReceive(byte[] data);



   }
}
