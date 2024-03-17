using OmronCommunication.Exceptions;

namespace OmronCommunication.Internal.ByteTransfer
{
    public class BasicByteTransfer : IByteTransfer
    {

        public virtual ushort[] ToUInt16(byte[] data)
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
            throw new LengthErrorException();
        }

        public virtual short[] ToInt16(byte[] data)
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
            throw new LengthErrorException();
        }

        public virtual uint[] ToUInt32(byte[] data)
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
            throw new LengthErrorException();
        }

        public virtual int[] ToInt32(byte[] data)
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
            throw new LengthErrorException();
        }

        public virtual float[] ToSingle(byte[] data)
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
            throw new LengthErrorException();
        }

        public virtual double TransToDouble(byte[] data, int index)
        {
            return BitConverter.ToDouble(data, index);
        }

        public virtual double[] ToDouble(byte[] data)
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
            throw new LengthErrorException();
        }

        public virtual byte[] GetBytes(ushort value)
        {
            return BitConverter.GetBytes(value);
        }

        public virtual byte[] GetBytes(ushort[] value)
        {
            var buffer = new byte[sizeof(ushort) * value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                BitConverter.GetBytes(value[i]).CopyTo(buffer, sizeof(ushort) * i);
            }
            return buffer;
        }

        public virtual byte[] GetBytes(short value)
        {
            return BitConverter.GetBytes(value);
        }

        public virtual byte[] GetBytes(short[] value)
        {
            var buffer = new byte[sizeof(short) * value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                BitConverter.GetBytes(value[i]).CopyTo(buffer, sizeof(ushort) * i);
            }
            return buffer;
        }

        public virtual byte[] GetBytes(uint value)
        {
            return BitConverter.GetBytes(value);
        }

        public virtual byte[] GetBytes(uint[] value)
        {
            var buffer = new byte[sizeof(uint) * value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                BitConverter.GetBytes(value[i]).CopyTo(buffer, sizeof(uint) * i);
            }
            return buffer;
        }

        public virtual byte[] GetBytes(int value)
        {
            return BitConverter.GetBytes(value);
        }

        public virtual byte[] GetBytes(int[] value)
        {
            var buffer = new byte[sizeof(int) * value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                BitConverter.GetBytes(value[i]).CopyTo(buffer, sizeof(int) * i);
            }
            return buffer;
        }

        public virtual byte[] GetBytes(float value)
        {
            return BitConverter.GetBytes(value);
        }

        public virtual byte[] GetBytes(float[] value)
        {
            var buffer = new byte[sizeof(float) * value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                BitConverter.GetBytes(value[i]).CopyTo(buffer, sizeof(float) * i);
            }
            return buffer;
        }

        public virtual byte[] GetBytes(double value)
        {
            return BitConverter.GetBytes(value);
        }

        public virtual byte[] GetBytes(double[] value)
        {
            var buffer = new byte[sizeof(double) * value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                BitConverter.GetBytes(value[i]).CopyTo(buffer, sizeof(double) * i);
            }
            return buffer;
        }

        public byte[] GetBytes(bool value)
        {
            return BitConverter.GetBytes(value);
        }

        public byte[] GetBytes(bool[] value)
        {
            var buffer = new byte[sizeof(bool) * value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                BitConverter.GetBytes(value[i]).CopyTo(buffer, sizeof(bool) * i);
            }
            return buffer;
        }
    }
}
