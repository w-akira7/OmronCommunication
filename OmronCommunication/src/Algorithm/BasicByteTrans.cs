using OmronCommunication.Algorithm;

namespace OmronCommunication.Algorithm
{
    public class BasicByteTrans : IByteTrans
    {

        public virtual ushort TransToUInt16(byte[] data,int index)
        {
            return BitConverter.ToUInt16(data, index);
        }

        public virtual ushort[] TransToUInt16(byte[] data)
        {
            if (data.Length % sizeof(ushort) == 0)
            {
                ushort[] result = new ushort[data.Length / sizeof(ushort)];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = BitConverter.ToUInt16(data, i * sizeof(ushort));
                }
                return result;
            }
        }

        public virtual short TransToInt16(byte[] data,int index)
        {
            return BitConverter.ToInt16(data, index);
        }

        public virtual short[] TransToInt16(byte[] data)
        {
            if (data.Length % sizeof(short) == 0)
            {
                short[] result = new short[data.Length / sizeof(short)];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = BitConverter.ToInt16(data, i * sizeof(short));
                }
                return result;
            }
        }

        public virtual uint TransToUInt32(byte[] data,int index)
        {
            return BitConverter.ToUInt32(data, index);
        }

        public virtual uint[] TransToUInt32(byte[] data)
        {
            if (data.Length % sizeof(uint) == 0)
            {
                uint[] result = new uint[data.Length / sizeof(uint)];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = BitConverter.ToUInt32(data, i * sizeof(uint));
                }
                return result;
            }
        }

        public virtual int TransToInt32(byte[] data,int index)
        {
            return BitConverter.ToInt32(data, index);
        }

        public virtual int[] TransToInt32(byte[] data)
        {
            if (data.Length % sizeof(int) == 0)
            {
                int[] result = new int[data.Length / sizeof(int)];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = BitConverter.ToInt32(data, i * sizeof(int));
                }
                return result;
            }
        }

        public virtual ulong TransToUInt64(byte[] data,int index)
        {
            return BitConverter.ToUInt64(data, index);
        }

        public virtual float[] TransToSingle(byte[] data)
        {
            if (data.Length % sizeof(float) == 0)
            {
                float[] result = new float[data.Length / sizeof(float)];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = BitConverter.ToSingle(data, i * sizeof(float));
                }
                return result;
            }
        }

        public virtual double TransToDouble(byte[] data,int index)
        {
            return BitConverter.ToDouble(data, index);
        }

        public virtual double[] TransToDouble(byte[] data)
        {
            if (data.Length % sizeof(double) == 0)
            {
                double[] result = new double[data.Length / sizeof(double)];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = BitConverter.ToDouble(data, i * sizeof(double));
                }
                return result;
            }
        }
    }
}
