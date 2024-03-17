namespace OmronCommunication.Internal.ByteTransfer
{
    public interface IByteTransfer
    {
        public ushort[] ToUInt16(byte[] data);
        public short[] ToInt16(byte[] data);
        public uint[] ToUInt32(byte[] data);
        public int[] ToInt32(byte[] data);
        public float[] ToSingle(byte[] data);
        public double[] ToDouble(byte[] data);

        public byte[] GetBytes(bool value);
        public byte[] GetBytes(bool[] value);
        public byte[] GetBytes(ushort value);
        public byte[] GetBytes(ushort[] value);
        public byte[] GetBytes(short value);
        public byte[] GetBytes(short[] value);
        public byte[] GetBytes(uint value);
        public byte[] GetBytes(uint[] value);
        public byte[] GetBytes(int value);
        public byte[] GetBytes(int[] value);
        public byte[] GetBytes(float value);
        public byte[] GetBytes(float[] value);
        public byte[] GetBytes(double value);
        public byte[] GetBytes(double[] value);
    }
}
