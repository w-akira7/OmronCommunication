using System.Net;
using System.Net.Sockets;


namespace OmronCommunication.TinyNet
{
    public abstract class AbstractTinyClient : ITinyNetDevice
    {

        protected readonly string? _deviceID;
        protected readonly EndPoint? _deviceAddress;
        protected Socket? _coresocket;
        protected int _receiveBufferSize;

        public AbstractTinyClient(EndPoint deviceAddress)
        {
            _deviceAddress = deviceAddress;
        }
        public AbstractTinyClient(EndPoint deviceAddress, string deviceID)
        {
            _deviceID = deviceID;
            _deviceAddress = deviceAddress;
        }

        public virtual string? DeviceID => _deviceID;
        public virtual EndPoint? DeviceAddress => _deviceAddress;
        public Socket? CoreSocket => _coresocket;
        public int ReceiveBufferSize { get => _receiveBufferSize; set => _receiveBufferSize = value; }

        /// <summary>
        /// Bind to a specified socket
        /// </summary>
        public virtual void InitWithBind(Socket socket)
        {
            ArgumentNullException.ThrowIfNull(socket);
            _coresocket = socket;     
        }
        public virtual void InitWithBind(AddressFamily family,SocketType socketType, ProtocolType protocolType, IPEndPoint localAddress)
        {
            _coresocket = new Socket(family,socketType,protocolType);
            _coresocket.Bind(localAddress);
        }
        public virtual void InitWithNoBind(AddressFamily family, SocketType socketType, ProtocolType protocolType)
        {
            _coresocket = new Socket(family, socketType, protocolType);
        }

        public virtual Task ConnectAsync()
        {
            ArgumentNullException.ThrowIfNull(DeviceAddress);
            return ConnectAsync(DeviceAddress);
        }

        public virtual Task ConnectAsync(EndPoint deviceAddress)
        {
            ArgumentNullException.ThrowIfNull(CoreSocket);
           return CoreSocket.ConnectAsync(deviceAddress);
        }
  
        public virtual void Close()
        {
            CoreSocket!.Close();
        }

        public abstract Task<byte[]> RequestWaitResponseAsync(byte[] send);
        public abstract byte[] RequestWaitResponse(byte[] send);
    }
}
