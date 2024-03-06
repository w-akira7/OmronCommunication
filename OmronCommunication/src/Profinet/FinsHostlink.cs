using OmronCommunication.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmronCommunication.Profinet
{
    public class FinsHostlink : FinsCommand, IProfinet
    {







        public bool IsTcp()
        {
            throw new NotImplementedException();
        }

        public bool IsUdp()
        {
            throw new NotImplementedException();
        }

        public OperationResult<byte[]> Read(string address, ushort length, bool isBit)
        {
            throw new NotImplementedException();
        }

        public OperationResult Write(string address, byte[] data, bool isBit)
        {
            throw new NotImplementedException();
        }


        public override OperationResult<byte[]> AnalyzeFinsResponse(OperationResult<byte[]> result)
        {
            return base.AnalyzeFinsResponse(result);
        }
    }
}
