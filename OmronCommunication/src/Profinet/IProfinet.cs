
namespace OmronCommunication.Profinet
{

    public interface IProfinet
    {
        public int Timeout { get; set; }

        public Task ConnectAsync();

        public void Close();
        
        public void Write(string address, byte[] data, bool isBit);

        public Task WriteAsync(string address, byte[] data, bool isBit);

        public byte[] Read(string address, ushort length, bool isBit);

        public Task<byte[]> ReadAsync(string address, ushort length, bool isBit);
    }
}
