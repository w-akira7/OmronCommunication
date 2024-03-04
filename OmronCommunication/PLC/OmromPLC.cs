using OmronCommunication.DataTypes;
using OmronCommunication.Profinet;
using OmronCommunication.Transmission;

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
        public OmromPLC(IDevice device) { _device = device; _finsCommand = new FinsCommand(); }

        private FinsCommand? _finsCommand;

        private IDevice? _device;

        public bool IsConnect {  get; private set; }

        #region Connect

        public OperateResult Connect()
        {
            if(_device != null)
            {
                //_device.close();
                //_device.connect();
                IsConnect = true;
                return new OperateResult(true);

            }
            else
            {
                IsConnect = false;
                return new OperateResult(false);
            }
        }

        public OperateResult Close()
        {
            //_device.close();
            IsConnect = false;
            return new OperateResult();
        }

        private OperateResult Send(byte[] data)
        {
            return _device.Send(data);
        }

        private OperateResult<byte[]> Receive()
        {
            return _device.Receive();

        }

        private OperateResult<byte[]> SendAndReceive(byte[] data)
        {
            return _device.SendAndReceive(data);
        }

        #endregion


        #region Read & Write


        public OperateResult<byte[]> Read(string address, ushort length)
        {
            //BuildReadCommand
            var command = _finsCommand.BuildReadFinsCommand(address, length,false);

            //ReadFromPLC

            var byteResult = _device.SendAndReceive(command.Value);


            //DataAnalysis
            var result = _finsCommand.AnalyzeFinsCommand(byteResult);

            return result;
        }


        public bool ReadBool(string address)
        {
            var result = Read(address, 1);
            throw new NotImplementedException();

        }

        public bool[] ReadBool(string address, ushort length)
        {
            throw new NotImplementedException();
        }

        public int ReadInt(string address)
        {
            throw new NotImplementedException();
        }

        public int[] ReadInt(string address, ushort length)
        {
            throw new NotImplementedException();
        }

        public float ReadFoalt(string address)
        {
            throw new NotImplementedException();
        }

        public float[] ReadFoalt(string address, ushort length)
        {
            throw new NotImplementedException();
        }

        public OperateResult WriteBool(string address, bool value)
        {
            throw new NotImplementedException();
        }

        public OperateResult WriteInt(string address, int value)
        {
            throw new NotImplementedException();
        }

        public OperateResult WriteFoalt(string address, float value)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
