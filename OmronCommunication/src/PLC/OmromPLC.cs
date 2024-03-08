using OmronCommunication;
using OmronCommunication.Profinet;
using OmronCommunication.Tools;

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
        public OmromPLC(IProfinet device) { _device = device; }

        #region Connect

        //TODO
        public OperationResult Connect()
        {
            if (false)
            {
                //_device.close();
                ////_device.connect();
                //IsConnect = true;
                return new OperationResult(true);

            }
            return new OperationResult(false);
        }

        // TODO
        public OperationResult Close()
        {
            if (false)
            {
                ////_device.close();
                //IsConnect = false;
                return new OperationResult();
            }
            return new OperationResult();
        }

        #endregion

        #region Read & Write

        public bool ReadBool(string address)
        {
            var readResult = _device.Read(address, 1, true);
            if (!readResult.IsSuccess) throw new NotImplementedException();

            var i = readResult.Value.Select(eachByte => eachByte != 0x00 ? true : false).ToArray();
            return i[0];
        }

        public bool[] ReadBool(string address, ushort length)
        {
            var readResult = _device.Read(address, length, true);
            if (!readResult.IsSuccess) throw new NotImplementedException();

            var i = readResult.Value.Select(eachByte => eachByte != 0x00 ? true : false).ToArray();
            return i;
        }
        public short ReadShort(string address)
        {
            var readResult = _device.Read(address, 1, false);
            if (!readResult.IsSuccess) throw new Exception(readResult.Message);

            readResult.Value = ByteTransTools.ReverseWordByte(readResult.Value);
            return ByteTransTools.WordToInt16(readResult.Value);
        }
        public short[] ReadShort(string address, ushort length)
        {
            var readResult = _device.Read(address, length, false);
            if (!readResult.IsSuccess) throw new NotImplementedException();

            readResult.Value = ByteTransTools.ReverseWordByte(readResult.Value);
            return ByteTransTools.WordsToInt16(readResult.Value);
        }

        public int ReadInt(string address)
        {
            var readResult = _device.Read(address, 1, false);
            if (!readResult.IsSuccess) throw new NotImplementedException();

            //TODO 字节翻转
            return ByteTransTools.DWordToInt32(readResult.Value);
        }

        public int[] ReadInt(string address, ushort length)
        {
            var readResult = _device.Read(address, length, false);
            if (!readResult.IsSuccess) throw new NotImplementedException();

            //TODO 字节翻转
            return ByteTransTools.DWordsToInt32(readResult.Value);

        }

        public float ReadFoalt(string address)
        {
            var readResult = _device.Read(address, 1, true);
            if (!readResult.IsSuccess) throw new NotImplementedException();

            //TODO 字节翻转
            return ByteTransTools.DWordToFloat(readResult.Value);
        }

        public float[] ReadFoalt(string address, ushort length)
        {
            var readResult = _device.Read(address, length, true);
            if (!readResult.IsSuccess) throw new NotImplementedException();

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
