using System.Net;
using OmronCommunication.Internal.ByteTransfer;
using OmronCommunication.Profinet;
using OmronCommunication.Exceptions;

namespace OmronCommunication.PLC
{
    /// <summary>
    /// OMRON 系列PLC
    /// </summary>
    public class OmromPLC : IPLC
    {
        private readonly IProfinet _profinet;
        private readonly IByteTransfer _transfer;

        public string Name { get; }

        /// <summary>
        /// 创建一个OMRON PLC的实例
        /// </summary>
        /// <param name="remoteIP">PLC IP地址</param>
        /// <param name="remotrPort">PLC 端口号</param>
        /// <param name="profinet">PLC 以太网协议</param>
        /// <param name="byteOrder">PLC 字节序</param>
        /// <exception cref="InvalidProfinetException">使用了一个非 OMROM FINS 协议</exception>
        public OmromPLC(IPAddress remoteIP,int remotrPort, ProfinetType profinet, ByteOrder byteOrder = ByteOrder.CDAB)
        {
            switch (profinet)
            {
                case ProfinetType.FinsTcp:
                    _profinet = new FinsTcp(remoteIP, remotrPort);
                    break;
                case ProfinetType.FinsUdp:
                    _profinet = new FinsUdp(remoteIP, remotrPort);
                    break;
                default: throw new InvalidProfinetException("请使用Fins协议");
            }
            _transfer = new ReverseByteTransfer(byteOrder);
        }

        /// <summary>
        /// 初始化设备并建立连接
        /// </summary>
        public async Task Connect()
        {
            await _profinet.ConnectAsync();
        }

        /// <summary>
        /// 关闭连接并释放设备
        /// </summary>
        public void Close()
        {
            _profinet.Close();
        }

        public async Task<bool> ReadBoolAsync(string address)
        {
            var result = await _profinet.ReadAsync(address, 1, true);
            var i = result.Select(eachByte => eachByte != 0x00 ? true : false).ToArray();
            return i[0];
        }

        public async Task<bool[]> ReadBoolAsync(string address, ushort length)
        {
            var result = await _profinet.ReadAsync(address, length, true);
            var i = result.Select(eachByte => eachByte != 0x00 ? true : false).ToArray();
            return i;
        }
   
        public async Task<short> ReadShortAsync(string address)
        {
            var result = await _profinet.ReadAsync(address, 1, false);
            return _transfer.ToInt16(result)[0];
        }

        public async Task<short[]> ReadShortAsync(string address, ushort length)
        {
            var result = await _profinet.ReadAsync(address, length, false);
            return _transfer.ToInt16(result);
        }

        public async Task<int> ReadIntAsync(string address)
        {
            var result = await _profinet.ReadAsync(address, 2, false);
            return _transfer.ToInt32(result)[0];
        }

        public async Task<int[]> ReadIntAsync(string address, ushort length)
        {
            var result = await _profinet.ReadAsync(address, (ushort)(length * 2), false);
            return _transfer.ToInt32(result);
        }

        public async Task<float> ReadFoaltAsync(string address)
        {
            var result = await _profinet.ReadAsync(address, 2, false);
           return _transfer.ToSingle(result)[0];
        }

        public async Task<float[]> ReadFoaltAsync(string address, ushort length)
        {
            var result =await _profinet.ReadAsync(address, (ushort)(length * 2), false);
            return _transfer.ToSingle(result);
        }

        public Task WriteAsync(string address, bool value)
        {
            var buffer = _transfer.GetBytes(value);
            return _profinet.WriteAsync(address, buffer, true);
        }

        public Task WriteAsync(string address, bool[] value)
        {
            var buffer = _transfer.GetBytes(value);
            return _profinet.WriteAsync(address, buffer, true);
        }

        public Task WriteAsync(string address, short value)
        {
            var buffer = _transfer.GetBytes(value);
            return _profinet.WriteAsync(address, buffer, false);
        }

        public Task WriteAsync(string address, short[] value)
        {
            var buffer = _transfer.GetBytes(value);
            return _profinet.WriteAsync(address, buffer, false);
        }

        public Task WriteAsync(string address, int value)
        {
            var buffer = _transfer.GetBytes(value);
            return _profinet.WriteAsync(address, buffer, false);
        }

        public Task WriteAsync(string address, int[] value)
        {
            var buffer = _transfer.GetBytes(value);
            return _profinet.WriteAsync(address, buffer, false);
        }

        public Task WriteAsync(string address, float value)
        {
            var buffer = _transfer.GetBytes(value);
            return _profinet.WriteAsync(address, buffer, false);
        }

        public Task WriteAsync(string address, float[] value)
        {
            var buffer = _transfer.GetBytes(value);
            return _profinet.WriteAsync(address, buffer, false);
        }
    }
}
