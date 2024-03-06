using OmronCommunication.DataTypes;

namespace OmronCommunication.Profinet
{
    /// <summary>
    /// 传输方式的抽象接口
    /// </summary>
    public interface IProfinet
    {

        public OperationResult Write(string address, byte[] data, bool isBit);

        public OperationResult<byte[]> Read(string address,ushort length,bool isBit);

    }
}
