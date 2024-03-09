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
        public void Connect()
        {
            _device.Connect();
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
        public bool ReadBool(string address)
        {
            var readResult = _device.Read(address, 1, true);
            if (!readResult.IsSuccess) throw new OperationReadFailedException();

            var i = readResult.Value.Select(eachByte => eachByte != 0x00 ? true : false).ToArray();
            return i[0];
        }

        public bool[] ReadBool(string address, ushort length)
        {
            var readResult = _device.Read(address, length, true);
            if (!readResult.IsSuccess) throw new OperationReadFailedException();

            var i = readResult.Value.Select(eachByte => eachByte != 0x00 ? true : false).ToArray();
            return i;
        }
        /// <summary>
        /// 读单个地址的SHORT值
        /// </summary>
        /// <param name="address">目标地址.格式: "D100" "H100"</param>
        /// <returns></returns>
        /// <exception cref="OperationReadFailedException"></exception>
        public short ReadShort(string address)
        {
            var readResult = _device.Read(address, 1, false);
            if (!readResult.IsSuccess) throw new OperationReadFailedException();

            readResult.Value = ByteTransTools.ReverseWordByte(readResult.Value);
            return ByteTransTools.WordToInt16(readResult.Value);
        }
        public short[] ReadShort(string address, ushort length)
        {
            var readResult = _device.Read(address, length, false);
            if (!readResult.IsSuccess) throw new OperationReadFailedException();

            readResult.Value = ByteTransTools.ReverseWordByte(readResult.Value);
            return ByteTransTools.WordsToInt16(readResult.Value);
        }

        public int ReadInt(string address)
        {
            var readResult = _device.Read(address, 1, false);
            if (!readResult.IsSuccess) throw new OperationReadFailedException();

            //TODO 字节翻转
            return ByteTransTools.DWordToInt32(readResult.Value);
        }

        public int[] ReadInt(string address, ushort length)
        {
            var readResult = _device.Read(address, length, false);
            if (!readResult.IsSuccess) throw new OperationReadFailedException();

            //TODO 字节翻转
            return ByteTransTools.DWordsToInt32(readResult.Value);

        }

        public float ReadFoalt(string address)
        {
            var readResult = _device.Read(address, 1, true);
            if (!readResult.IsSuccess) throw new OperationReadFailedException();

            //TODO 字节翻转
            return ByteTransTools.DWordToFloat(readResult.Value);
        }

        public float[] ReadFoalt(string address, ushort length)
        {
            var readResult = _device.Read(address, length, true);
            if (!readResult.IsSuccess) throw new OperationReadFailedException();

            //TODO 字节翻转
            return ByteTransTools.DWordsToFloat(readResult.Value);
        }

        public OperationResult WriteBool(string address, bool value)
        {
            throw new NotImplementedException();
        }

        public OperationResult WriteInt(string address, int value)
        {
            throw new NotImplementedException();
        }

        public OperationResult WriteFoalt(string address, float value)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
