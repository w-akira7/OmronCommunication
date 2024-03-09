using OmronCommunication;

namespace OmronCommunication.Profinet
{

    public interface IProfinet
    {
        public void Connect();

        public void Close();

        public OperationResult Write(string address, byte[] data, bool isBit);

        public OperationResult<byte[]> Read(string address,ushort length,bool isBit);

    }
}
