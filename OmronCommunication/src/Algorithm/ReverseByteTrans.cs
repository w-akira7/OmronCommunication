using OmronCommunication.Algorithm;
using OmronCommunication.Algorithm;

namespace OmronCommunication.Algorithm
{
    public sealed class ReverseByteTrans: BasicByteTrans
    {
        public ByteOrder ByteOrder { get; set; }

        public ReverseByteTrans(ByteOrder byteOrder) : base()
        {
            ByteOrder = byteOrder;
        }

        public override ushort[] TransToUInt16(byte[] data)
        {
            data = ByteTransTools.TransToLittleEdian(data, ByteOrder);
            return base.TransToUInt16(data);
        }

        public override short[] TransToInt16(byte[] data)
        {
            data = ByteTransTools.TransToLittleEdian(data, ByteOrder);
            return base.TransToInt16(data);
        }

        public override uint[] TransToUInt32(byte[] data)
        {
            data = ByteTransTools.TransToLittleEdian(data, ByteOrder);
            return base.TransToUInt32(data);
        }

        public override int[] TransToInt32(byte[] data)
        {
            data = ByteTransTools.TransToLittleEdian(data, ByteOrder);
            return base.TransToInt32(data);
        }

        public override float[] TransToFloat(byte[] data)
        {
            data = ByteTransTools.TransToLittleEdian(data, ByteOrder);
            return base.TransToFloat(data);
        }  
    }
}
