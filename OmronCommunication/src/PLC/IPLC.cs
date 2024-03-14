using OmronCommunication;

namespace OmronCommunication.PLC
{
    public interface IPLC
    {
        public abstract Task<bool> ReadBool(string address);
        public abstract Task<bool[]> ReadBool(string address, ushort length);
        public abstract Task<short> ReadShort(string address);
        public abstract Task<short[]> ReadShort(string address, ushort length);
        public abstract Task<int> ReadInt(string address);
        public abstract Task<int[]> ReadInt(string address, ushort length);
        public abstract Task<float> ReadFoalt(string address);
        public abstract Task<float[]> ReadFoalt(string address, ushort length);
        public abstract void WriteBool(string address, bool value);
        public abstract void WriteInt(string address, int value);
        public abstract void WriteFoalt(string address, float value);
    }
}
