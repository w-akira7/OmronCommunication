using OmronCommunication.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmronCommunication.src.Algorithm
{
    public sealed class ReverseByteTrans: BasicByteTrans
    {
        public ByteOrder ByteOrder { get; set; }

        public ReverseByteTrans(ByteOrder byteOrder) : base()
        {
            ByteOrder = byteOrder;
        }

        public override ushort TransToUInt16(byte[] data)
        {
            return base.TransToUInt16(data);
        }
    }
}
