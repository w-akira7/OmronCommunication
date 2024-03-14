using OmronCommunication;

namespace OmronCommunication.Profinet
{

    public interface IProfinet
    {
        public Task ConnectAsync();

        public void Close();

        public Task Write(string address, byte[] data, bool isBit);

        public Task<byte[]> Read(string address,ushort length,bool isBit);

    }
}
