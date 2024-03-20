using System.Net;
using System.Net.Sockets;


namespace OmronCommunication.TinyNet
{
    public interface ITinyNetDevice
    {
        string? DeviceID { get; }
        EndPoint? DeviceAddress { get; }
        Socket? CoreSocket { get; }
        int ReceiveBufferSize { get; set; }

        void Close();
        public Task ConnectAsync();
        public Task ConnectAsync(EndPoint remoteAddress);
        void InitWithBind(Socket socket);
        void InitWithBind(AddressFamily family, SocketType socketType, ProtocolType protocolType, IPEndPoint localAddress);
        void InitWithNoBind(AddressFamily family, SocketType socketType, ProtocolType protocolType);
        public Task<byte[]> RequestWaitResponseAsync(byte[] send);
        public byte[] RequestWaitResponse(byte[] send);

    }
}
