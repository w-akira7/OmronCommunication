using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using OmronCommunication.DataTypes;

namespace OmronCommunication.Transmission
{
    public abstract class NetBase
    {
        public NetBase()
        {

        }

        protected Socket? coreSocket;


        protected virtual OperateResult<byte[]> ReceiveFrom(Socket socket)
        {
            var resullt = new OperateResult<byte[]>();

            var statebuff = new byte[1024];
            socket.BeginReceive(statebuff, 0, 1024, SocketFlags.None, ReceiveCallback, null);

            return resullt;
        }

        protected virtual OperateResult SendTo(Socket socket, byte[] data)
        {
            return new OperateResult();
        }

        private void ReceiveCallback(IAsyncResult ar) { }

    }
}
