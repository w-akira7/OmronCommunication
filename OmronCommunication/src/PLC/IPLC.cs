using OmronCommunication;

namespace OmronCommunication.PLC
{
    public interface IPLC
    {
        public abstract Task<bool> ReadBoolAsync(string address);
        public abstract Task<bool[]> ReadBoolAsync(string address, ushort length);
 
        public abstract Task<short> ReadShortAsync(string address);
        public abstract Task<short[]> ReadShortAsync(string address, ushort length);
 
        public abstract Task<int> ReadIntAsync(string address);
        public abstract Task<int[]> ReadIntAsync(string address, ushort length);

        public abstract Task<float> ReadFoaltAsync(string address);
        public abstract Task<float[]> ReadFoaltAsync(string address, ushort length);
 
        public abstract Task WriteAsync(string address, bool value);
        public abstract Task WriteAsync(string address, int value);
        public abstract Task WriteAsync(string address, float value);
    }
}
