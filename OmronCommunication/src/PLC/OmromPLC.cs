using OmronCommunication;
using OmronCommunication.Profinet;
using OmronCommunication.Tools;
using System.Diagnostics.CodeAnalysis;

namespace OmronCommunication.PLC
{
    /// <summary>
    /// OMRON 系列PLC
    /// </summary>
    public class OmromPLC : IPLC
    {
        private readonly IProfinet? _device;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device">选择设备的传输协议</param>
        public OmromPLC([NotNull] IProfinet device) { _device = device; }

        #region Connect
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

        #endregion

        #region Read & Write
        /// <summary>
        /// 读单个地址的BOOL值
        /// </summary>
        /// <param name="address">目标地址.格式: "C100.01" "D100.01"</param>
        /// <returns></returns>
        /// <exception cref="OperationReadFailedException"></exception>
        public async Task<bool> ReadBool(string address)
        {
            var readResult = await _device.Read(address, 1, true);
             
            var i = readResult.Select(eachByte => eachByte != 0x00 ? true : false).ToArray();
            return i[0];
        }

        public async Task<bool[]> ReadBool(string address, ushort length)
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
        public async Task<short> ReadShort(string address)
        {
            var readResult = await _device.Read(address, 1, false);

            readResult = ByteTransTools.ReverseWordByte(readResult);
            return ByteTransTools.WordToInt16(readResult);
        }
        public async Task<short[]> ReadShort(string address, ushort length)
        {
            var readResult =await _device.Read(address, length, false);

            readResult = ByteTransTools.ReverseWordByte(readResult);
            return ByteTransTools.WordsToInt16(readResult);
        }

        public async Task<int> ReadInt(string address)
        {
            var readResult = await _device.Read(address, 1, false);

            //TODO 字节翻转
            return ByteTransTools.DWordToInt32(readResult);
        }

        public async Task<int[]> ReadInt(string address, ushort length)
        {
            var readResult = await _device.Read(address, length, false);

            //TODO 字节翻转
            return ByteTransTools.DWordsToInt32(readResult);

        }

        public async Task<float> ReadFoalt(string address)
        {
            var readResult = await _device.Read(address, 1, true);

            //TODO 字节翻转
            return ByteTransTools.DWordToFloat(readResult);
        }

        public async Task<float[]> ReadFoalt(string address, ushort length)
        {
            var readResult =await _device.Read(address, length, true);

            //TODO 字节翻转
            return ByteTransTools.DWordsToFloat(readResult);
        }

        public void WriteBool(string address, bool value)
        {
            throw new NotImplementedException();
        }

        public void WriteInt(string address, int value)
        {
            throw new NotImplementedException();
        }

        public void WriteFoalt(string address, float value)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
