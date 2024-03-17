
namespace OmronCommunication.Internal.ByteTransfer
{
    public sealed class ReverseByteTransfer : BasicByteTransfer
    {
        public ByteOrder ByteOrder { get; set; }

        public ReverseByteTransfer(ByteOrder byteOrder) : base()
        {
            ByteOrder = byteOrder;
        }

        // 针对2个字节的特例转换
        private byte[] Convert2ByteOrder(byte[] data, ByteOrder byteOrder)
        {
            switch (byteOrder)
            {
                case ByteOrder.ABCD:
                    for (int i = 0;i< data.Length; i = i + 2)
                    {
                        Array.Reverse(data, i,  2);
                    }
                    return data;
                case ByteOrder.BADC:
                    break;
                case ByteOrder.CDAB:
                    for (int i = 0; i < data.Length; i = i + 2)
                    {
                        Array.Reverse(data, i,  2);
                    }
                    return data;
                case ByteOrder.DCBA:
                    break;
            }
            return data;
        }

        // 4个字节 8个字节倍数的转换
        private byte[] Convert4ByteOrder(byte[] data, ByteOrder byteOrder)
        {
            var buffer = new byte[data.Length];
            switch (byteOrder)
            {
                case ByteOrder.ABCD:
                    for (int i = 0; i < data.Length; i = i + 4)
                    {
                        buffer[i] = data[i + 3];
                        buffer[i + 1] = data[i + 2];
                        buffer[i + 2] = data[i + 1];
                        buffer[i + 3] = data[i];
                    }
                    break;
                case ByteOrder.BADC:
                    for (int i = 0; i < data.Length; i = i + 4)
                    {
                        buffer[i] = data[i + 2];
                        buffer[i + 1] = data[i + 3];
                        buffer[i + 2] = data[i];
                        buffer[i + 3] = data[i + 1];
                    }
                    break;
                case ByteOrder.CDAB:
                    for (int i = 0; i < data.Length; i = i + 2)
                    {
                        buffer[i] = data[i + 1];
                        buffer[i + 1] = data[i];
                    }
                    break;
                case ByteOrder.DCBA:
                    return data;
            }
            return buffer;
        }

        public override ushort[] ToUInt16(byte[] data)
        {
            data = Convert2ByteOrder(data, ByteOrder);
            return base.ToUInt16(data);
        }

        public override short[] ToInt16(byte[] data)
        {
            data = Convert2ByteOrder(data, ByteOrder);
            return base.ToInt16(data);
        }

        public override uint[] ToUInt32(byte[] data)
        {
            data = Convert4ByteOrder(data, ByteOrder);
            return base.ToUInt32(data);
        }

        public override int[] ToInt32(byte[] data)
        {
            data = Convert4ByteOrder(data, ByteOrder);
            return base.ToInt32(data);
        }

        public override float[] ToSingle(byte[] data)
        {
            data = Convert4ByteOrder(data, ByteOrder);
            return base.ToSingle(data);
        }

        public override double[] ToDouble(byte[] data)
        {
            data = Convert4ByteOrder(data, ByteOrder);
            return base.ToDouble(data);
        }

        public override byte[] GetBytes(ushort value)
        {
            var buff = base.GetBytes(value);
            return Convert2ByteOrder(buff, ByteOrder);
        }

        public override byte[] GetBytes(ushort[] value)
        {
            var buff = base.GetBytes(value);
            return Convert2ByteOrder(buff, ByteOrder);
        }

        public override byte[] GetBytes(short value)
        {
            var buff = base.GetBytes(value);
            return Convert2ByteOrder(buff, ByteOrder);
        }

        public override byte[] GetBytes(short[] value)
        {
            var buff = base.GetBytes(value);
            return Convert2ByteOrder(buff, ByteOrder);
        }

        public override byte[] GetBytes(uint value)
        {
            var buff = base.GetBytes(value);
            return Convert4ByteOrder(buff, ByteOrder);
        }

        public override byte[] GetBytes(uint[] value)
        {
            var buff = base.GetBytes(value);
            return Convert4ByteOrder(buff, ByteOrder);
        }

        public override byte[] GetBytes(int value)
        {
            var buff = base.GetBytes(value);
            return Convert4ByteOrder(buff, ByteOrder);
        }

        public override byte[] GetBytes(int[] value)
        {
            var buff = base.GetBytes(value);
            return Convert4ByteOrder(buff, ByteOrder);
        }

        public override byte[] GetBytes(float value)
        {
            var buff = base.GetBytes(value);
            return Convert4ByteOrder(buff, ByteOrder);
        }

        public override byte[] GetBytes(float[] value)
        {
            var buff = base.GetBytes(value);
            return Convert4ByteOrder(buff, ByteOrder);
        }

        public override byte[] GetBytes(double value)
        {
            var buff = base.GetBytes(value);
            return Convert4ByteOrder(buff, ByteOrder);
        }

        public override byte[] GetBytes(double[] value)
        {
            var buff = base.GetBytes(value);
            return Convert4ByteOrder(buff, ByteOrder);
        }
    }
}
