using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmronCommunication.Algorithm
{
    public interface IByteTrans
    {
        public ushort TransToUInt16(byte[] data);
        public short TransToInt16(byte[] data);
        public uint TransToUInt32(byte[] data);
        public int TransToInt32(byte[] data);
        public float TransToSingle(byte[] data);
        public double TransToDouble(byte[] data);
    }
}
