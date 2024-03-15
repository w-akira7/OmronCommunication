using OmronCommunication;
using OmronCommunication.Algorithm;
using OmronCommunication.Profinet;
using OmronCommunication.TinyNet;
using OmronCommunication.Tools;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace OmronCommunication.PLC
{
    /// <summary>
    /// OMRON 系列PLC
    /// </summary>
    public class OmromPLC : IPLC
    {
        private readonly IDevice? _device;
        private IByteTrans _byteTrans;

        /// <summary>
        /// 
        /// </summary>
        public OmromPLC(IPAddress remoteIP,int remotrPort, ProfinetType profinetType)
        {
            switch (profinetType)
            {
                case ProfinetType.FinsTcp:
                    _device = new FinsTcpDevice(remoteIP, remotrPort);
                    break;
                case ProfinetType.FinsUdp:
                    _device = new FinsUdpDevice(remoteIP, remotrPort);
                    break;
                default: throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 初始化设备并建立连接
        /// </summary>
        public async Task Connect()
        {
            await _device.ConnectAsync();
        }
        /// <summary>
        /// 关闭连接并释放设备
        /// </summary>
        public void Close()
        {
            _device.Close();
        }

        /// <summary>
        /// 读单个地址的BOOL值
        /// </summary>
        /// <param name="address">目标地址.格式: "C100.01" "D100.01"</param>
        /// <returns></returns>
        /// <exception cref="OperationReadFailedException"></exception>
        public async Task<bool> ReadBoolAsync(string address)
        {
            var readResult = await _device.Read(address, 1, true);
             
            var i = readResult.Select(eachByte => eachByte != 0x00 ? true : false).ToArray();
            return i[0];
        }

        public async Task<bool[]> ReadBoolAsync(string address, ushort length)
        {
            var readResult = await _device.Read(address, 1, true);

            var i = readResult.Select(eachByte => eachByte != 0x00 ? true : false).ToArray();
            return i;
        }
   
        /// <summary>
        /// 读单个地址的SHORT值
        /// </summary>
        /// <param name="address">目标地址.格式: "D100" "H100"</param>
        /// <returns></returns>
        /// <exception cref="OperationReadFailedException"></exception>
        public async Task<short> ReadShortAsync(string address)
        {
            var readResult = await _device.Read(address, 1, false);

            readResult = ByteTransTools.ReverseWordByte(readResult);
            return ByteTransTools.WordToInt16(readResult);
        }
        public async Task<short[]> ReadShortAsync(string address, ushort length)
        {
            var readResult =await _device.Read(address, length, false);

            readResult = ByteTransTools.ReverseWordByte(readResult);
            return ByteTransTools.WordsToInt16(readResult);
        }


        public async Task<int> ReadIntAsync(string address)
        {
            var readResult = await _device.Read(address, 1, false);

            //TODO 字节翻转
            return ByteTransTools.DWordToInt32(readResult);
        }

        public async Task<int[]> ReadIntAsync(string address, ushort length)
        {
            var readResult = await _device.Read(address, length, false);

            //TODO 字节翻转
            return ByteTransTools.DWordsToInt32(readResult);

        }

        public async Task<float> ReadFoaltAsync(string address)
        {
            var readResult = await _device.Read(address, 1, true);

            //TODO 字节翻转
            return ByteTransTools.DWordToFloat(readResult);
        }

        public async Task<float[]> ReadFoaltAsync(string address, ushort length)
        {
            var readResult =await _device.Read(address, length, true);

            //TODO 字节翻转
            return ByteTransTools.DWordsToFloat(readResult);
        }

        public Task WriteAsync(string address, bool value)
        {
            throw new NotImplementedException();
        }

        public Task WriteAsync(string address, int value)
        {
            throw new NotImplementedException();
        }

        public Task WriteAsync(string address, float value)
        {
            throw new NotImplementedException();
        }

    }
}
