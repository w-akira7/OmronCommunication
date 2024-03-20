using OmronCommunication.TinyNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OmronCommunication.TinyNet
{
    public class TinyUdpClient : AbstractTinyClient
    {
        public TinyUdpClient(EndPoint deviceAddress) : base(deviceAddress)
        {

        }

        public override byte[] RequestWaitResponse(byte[] send)
        {
            throw new NotImplementedException();
        }

        public override async Task<byte[]> RequestWaitResponseAsync(byte[] send)
        {
            await CoreSocket!.SendAsync(send);

            var buffer = new byte[ReceiveBufferSize];
            var rev = CoreSocket!.ReceiveAsync(buffer);

            await rev;
            var newbuffer = new byte[rev.Result];
            Array.Copy(buffer, 0, newbuffer, 0, rev.Result);
            return newbuffer;
        }
    }
}
