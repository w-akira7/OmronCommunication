using OmronCommunication.DataTypes;

namespace OmronCommunication.PLC
{
    public interface IPLC
    {
        public abstract bool ReadBool(string address);
        public abstract bool[] ReadBool(string address, ushort length);

        public abstract int ReadInt(string address);
        public abstract int[] ReadInt(string address, ushort length);

        public abstract float ReadFoalt(string address);
        public abstract float[] ReadFoalt(string address, ushort length);

        public abstract OperationResult WriteBool(string address, bool value);

        public abstract OperationResult WriteInt(string address, int value);

        public abstract OperationResult WriteFoalt(string address, float value);

    }
}
