using OmronCommunication.DataTypes;
using OmronCommunication.Profinet;

namespace OmronCommunication.PLC
{
    /// <summary>
    /// OMRON 系列PLC
    /// </summary>
    public class OmromPLC : IPLC
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        public OmromPLC(IProfinet device) { _device = device; }


        private IProfinet? _device;

        #region Connect

        //TODO
        public OperationResult Connect()
        {
            if (_device!.IsTcp())
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
            if (_device!.IsTcp())
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

            // bytes => bool
            var i = readResult.Value.Select(s => s != 0x00 ? true : false).ToArray();
            return i[0];
        }

        public bool[] ReadBool(string address, ushort length)
        {
            var readResult = _device.Read(address, length, true);
            if (!readResult.IsSuccess) throw new NotImplementedException();

            // bytes => bool[]
            var i = readResult.Value.Select(s => s != 0x00 ? true : false).ToArray();
            return i;
        }

        public int ReadInt(string address)
        {
            var readResult = _device.Read(address, 1, false);
            if (!readResult.IsSuccess) throw new NotImplementedException();

            //TODO bytes => int
            throw new NotImplementedException();

        }

        public int[] ReadInt(string address, ushort length)
        {
            var readResult = _device.Read(address, length, false);
            if (!readResult.IsSuccess) throw new NotImplementedException();

            //TODO bytes => int[]
            throw new NotImplementedException();
        }

        public float ReadFoalt(string address)
        {
            var readResult = _device.Read(address, 1, true);
            if (!readResult.IsSuccess) throw new NotImplementedException();

            //TODO bytes => float
            throw new NotImplementedException();
        }

        public float[] ReadFoalt(string address, ushort length)
        {
            var readResult = _device.Read(address, length, true);
            if (!readResult.IsSuccess) throw new NotImplementedException();

            //TODO bytes => float[]
            throw new NotImplementedException();
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
