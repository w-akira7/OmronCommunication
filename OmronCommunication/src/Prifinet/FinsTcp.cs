using System.Net.Sockets;
using System.Net;
using OmronCommunication.TinyNet;

namespace OmronCommunication.Profinet
{
    public sealed class FinsTcp : AbstractFins
    {
 
        public override AbstractTinyClient Client { get; protected set; }

        public FinsTcp(IPAddress remoteIP, int remotePort):base(remoteIP,remotePort)
        {
            var remoteAddress = new IPEndPoint(remoteIP, remotePort);
            Client = new TinyUdpClient(remoteAddress);
        }

        /// <summary>
        /// 响应超时设置(ms)
        /// </summary>
        public override int Timeout { get; set; } = 3000;
        public int ReceiveBufferSize { get; set; } = 1024;
        /// <summary>
        /// Fins/Tcp握手信号
        /// </summary>
        public byte[] HandSignal =>
            [0x46, 0x49, 0x4E, 0x53,
            0x00, 0x00, 0x00, 0x0C,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x01];

        /// <summary>
        /// 异步建立连接
        /// </summary>
        public override async Task ConnectAsync()
        {
            Client.InitWithNoBind(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //TODO 如果超出长度会出错，需要更完善的网络库
            Client.ReceiveBufferSize = 1024;
            // 建立TCP连接
            await Client.ConnectAsync();

            // 与PLC握手
            await Client.RequestWaitResponseAsync(HandSignal);

            // TODO 完善握手返回消息的解析
        }

        /// <summary>
        /// 关闭并释放连接
        /// </summary>
        public override void Close()
        {
            Client.Close();
        }

        // 按 FINS/TCP 传输组合完整的 FINS 数据包
        public override byte[] BuildCompleteCommand(byte[] command)
        {
            //基于UDP Command构建数据包 
            var udpCommand = base.BuildCompleteCommand(command);
            var completeCommand = new byte[udpCommand.Length + 16];

            //在开头加上TCP握手信号16字节
            Array.Copy(HandSignal, 0, completeCommand, 0, 16);
            Array.Copy(udpCommand, 0, completeCommand, 16, udpCommand.Length);

            //握手信号的第4至8字节表示发送的字节长度
            var dataLength = BitConverter.GetBytes(completeCommand.Length - 8);
            Array.Reverse(dataLength);
            Array.Copy(dataLength, 0, completeCommand, 4, dataLength.Length);

            completeCommand[11] = 0x02;
            return completeCommand;
        }

        public override FinsResponse AnalyzeFinsResponse(byte[] result)
        {
            var response = new FinsResponse();
            // a correct fins/tcp response contains at least 30 bytes including:handsignal, fins header, command code, end code 
            if (result.Length >= 30)
            {
                // header
                response.ICF = result[16];
                response.RSV = result[17];
                response.GCT = result[18];
                response.DNA = result[19];
                response.DA1 = result[20];
                response.DA2 = result[21];
                response.SNA = result[22];
                response.SA1 = result[23];
                response.SA2 = result[24];
                response.SID = result[25];
                // command code
                response.CommandCode.MR = result[26];
                response.CommandCode.SR = result[27];
                // end code
                response.EndCode.MainCode = result[28];
                response.EndCode.SubCode = result[29];
                // TODO 处理错误码

                // text
                response.hasText = false;
                if (result.Length > 30)
                {
                    var buffer = new byte[result.Length - 30];
                    Array.Copy(result, 30, buffer, 0, buffer.Length);
                    response.Text = buffer;
                    response.hasText = true;
                }
                return response;
            }
            throw new Exception("The response was not a fins response.");
        }
    }
}
