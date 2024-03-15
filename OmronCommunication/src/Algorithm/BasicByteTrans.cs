namespace OmronCommunication.Algorithm
{
    public class BasicByteTrans : IByteTrans
    {

        public virtual ushort TransToUInt16(byte[] data)
        {
            if (data.Length == sizeof(ushort))
            {
                return BitConverter.ToUInt16(data, 0);
            }
            throw new ByteLengthExceededException();
        }

        public virtual short TransToInt16(byte[] data)
        {
            if (data.Length == sizeof(short))
            {
                return BitConverter.ToInt16(data, 0);
            }
            throw new ByteLengthExceededException();
        }

        public virtual uint TransToUInt32(byte[] data)
        {
            if (data.Length == sizeof(uint))
            {
                return BitConverter.ToUInt32(data, 0);
            }
            throw new ByteLengthExceededException();
        }

        public virtual int TransToInt32(byte[] data)
        {
            if (data.Length == sizeof(int))
            {
                return BitConverter.ToInt32(data, 0);
            }
            throw new ByteLengthExceededException();
        }

        public virtual float TransToSingle(byte[] data)
        {
            if (data.Length == sizeof(float))
            {
                return BitConverter.ToSingle(data, 0);
            }
            throw new ByteLengthExceededException();
        }

        public virtual double TransToDouble(byte[] data)
        {
            if (data.Length == sizeof(double))
            {
                return BitConverter.ToDouble(data, 0);
            }
            throw new ByteLengthExceededException();
        }
    }
}
