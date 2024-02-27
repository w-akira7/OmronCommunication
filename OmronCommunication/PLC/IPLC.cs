using OmronCommunication.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OmronCommunication.PLC
{
    public interface IPLC
    {
        //public PLCBase() { }

        public abstract bool[] ReadBool(string address, ushort length);

        public abstract bool ReadBool(string address);

        public abstract int ReadInt(string address);

        public abstract float ReadFoalt(string address);

        public abstract OperateResult WriteBool(string address, bool value);

        public abstract OperateResult WriteInt(string address, int value);

        public abstract OperateResult WriteFoalt(string address, float value);

        public abstract OperateResult<byte[]> Read(string address, ushort length);
    }
}
