using System.Net;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using OmronCommunication;
using OmronCommunication.Resource;
using OmronCommunication.TinyNet;


namespace OmronCommunication.Profinet
{
    public sealed class FinsUdp : AbstractFins
    {
        public FinsUdp(IPAddress remoteIP, int remotePort) : base(remoteIP, remotePort)
        {
            var remoteAddress= new IPEndPoint(remoteIP, remotePort);
            Client = new TinyUdpClient(remoteAddress);
        }

        /// <summary>
        /// 响应超时设置(ms)
        /// </summary>
        public override int Timeout { get; set; } = 3000;

        public override AbstractTinyClient Client { get; protected set; }

        /// <summary>
        /// 异步建立连接
        /// </summary>
        public override Task ConnectAsync()
        {
            Client.InitWithNoBind(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp);

            //TODO 如果超出长度会出错，需要更完善的网络库
            Client.ReceiveBufferSize = 1024;
            return Client.ConnectAsync(); 
        }

        /// <summary>
        /// 关闭并释放连接
        /// </summary>
        public override void Close()
        {
            Client.Close();
        }

        public override FinsResponse AnalyzeFinsResponse(byte[] result)
        {
            var response = new FinsResponse();
            // a correct fins/udp response contains at least 14 bytes including: fins header, command code, end code 
            if (result.Length >= 14)
            {
                // header
                response.ICF = result[0];
                response.RSV = result[1];
                response.GCT = result[2];
                response.DNA = result[3];
                response.DA1 = result[4];
                response.DA2 = result[5];
                response.SNA = result[6];
                response.SA1 = result[7];
                response.SA2 = result[8];
                response.SID = result[9];
                // command code
                response.CommandCode.MR = result[10];
                response.CommandCode.SR = result[11];
                // end code
                response.EndCode.MainCode = result[12];
                response.EndCode.SubCode = result[13];
                // TODO 处理错误码

                // text
                response.hasText = false;
                if (result.Length > 14)
                {
                    var buffer = new byte[result.Length - 14];
                    Array.Copy(result, 14, buffer, 0, buffer.Length);
                    response.Text = buffer;
                    response.hasText = true;
                }
                return response;
            }
            throw new Exception("The response was not a fins response.");
        }
    }
}
