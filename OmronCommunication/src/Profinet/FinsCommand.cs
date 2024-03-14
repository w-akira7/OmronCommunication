using OmronCommunication;
using OmronCommunication.DataTypes;
using System;

namespace OmronCommunication.Profinet
{
    public class FinsCommand
    {
        /// <summary>
        /// Information Control Field.
        /// </summary>
        public byte ICF { get; set; } = 0x80;
        /// <summary>
        /// RSV (Reserved) is always 00 hex.
        /// </summary>
        public byte RSV { get;  set; } = 0x00;
        /// <summary>
        /// Gateway Count: Number of Bridges Passed Through.
        /// When communicating across up to 8 network layers, set it to 07 hex.
        /// Otherwise, set the GCT to 02 hex.
        /// </summary>
        public byte GCT { get; set; } = 0x02;
        /// <summary>
        /// Destination network address.
        /// 00 hex: Local network.
        /// 01 to 7F hex : Remote network address (decimal: 1 to 127).
        /// </summary>
        public byte DNA { get; set; } = 0x00;
        /// <summary>
        /// Destination node address.
        /// </summary>
        public byte DA1 { get; set; }
        /// <summary>
        /// Destination unit address.
        /// 00 hex:CPU Unit.
        /// FE hex: Controller Link Unit or Ethernet Unit connected to network.
        /// 10 to 1F hex: CPU Bus Unit.
        /// E1 hex: Inner Board.
        /// </summary>
        public byte DA2 { get; set; } = 0x00;
        /// <summary>
        /// Source network address.
        /// 00 hex: Local network.
        /// 01 to 7F hex: Remote network(1 to 127 decimal).
        /// </summary>
        public byte SNA { get; set; } = 0x00;
        /// <summary>
        /// Source node address.
        /// 00 hex: Internal communications in PLC
        /// 01 to 20 hex: Node address in Controller Link Network(1 to 32 decimal)
        /// 01 to FE hex: Ethernet(1 to 254 decimal,for Ethernet Units with model numbers ending in ETN21)
        /// </summary>
        public byte SA1 { get; set; }
        /// <summary>
        /// Source unit address.
        /// 00 hex: CPU Unit
        /// </summary>
        /// 10 to 1F hex: CPU Bus Unit
        public byte SA2 { get; set; } = 0x00;
        /// <summary>
        /// Service ID. Used to identify the process generating the transmission. Set the SID to any number between 00 and FF
        /// </summary>
        public byte SID { get; set; } = 0x00;

        /// <summary>
        /// 解析地址。要求的输入格式“C100.12”、“D100”
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected FinsAddressTypes AnalyzeAddressType(string address)
        {
            switch (address[0])
            {
                //CIO
                case 'c':
                case 'C':
                    {
                        return FinsAddressTypes.CIO;
                    }
                //WR
                case 'w':
                case 'W':
                    {
                        return FinsAddressTypes.WR;
                    }
                //HR
                case 'h':
                case 'H':
                    {
                        return FinsAddressTypes.HR;
                    }
                //AR
                case 'a':
                case 'A':
                    {
                        return FinsAddressTypes.AR;
                    }
                //DM
                case 'd':
                case 'D':
                    {
                        return FinsAddressTypes.DM;
                    }
                //TODO 完善错误信息类
                default: throw new Exception("错误的地址格式");
            }
        }

        /// <summary>
        /// Parse the address into byte data in FINS format. "C0010.13" Bit 13 of CIO 0010, i.e., 0x30 0x00 0x0A 0x0D
        /// </summary>
        /// <param name="address">目标起始地址</param>
        /// <param name="isBit">是否位操作</param>
        /// <returns>地址码：一个4字节的字节数组,包括 I/O Memory area code 和 Beginning address</returns>
        /// <exception cref="Exception"></exception> 
        protected virtual byte[] AnalyzeAddress(string address, bool isBit)
        {
            var buffer = new byte[4];
            var addresstype = AnalyzeAddressType(address);
            //位操作 PLC中 1 word = 2 bytes = 16 bits
            if (isBit)
            {
                buffer[0] = addresstype.BitCode;
                var splits = address.Substring(1).Split('.', StringSplitOptions.RemoveEmptyEntries);
                var addr = ushort.Parse(splits[0]);
                buffer[1] = BitConverter.GetBytes(addr)[1];
                buffer[2] = BitConverter.GetBytes(addr)[2];
                buffer[3] = byte.Parse(splits[1]);

            }
            //字操作
            else
            {
                buffer[0] = addresstype.WordCode;
                var addr = ushort.Parse(address[1..]);
                buffer[1] = BitConverter.GetBytes(addr)[1];
                buffer[2] = BitConverter.GetBytes(addr)[0];
            }
            return buffer;
        }

        /// <summary>
        /// 按 UDP 传输方式将命令与 FINS 头组合成完整的 FINS 数据包,在 TCP 和 Hostlink 中重写
        /// </summary>
        /// <param name="command">command code and text</param>
        /// <returns>完整的可发送的FINS包</returns>
        public virtual byte[] BuildCompleteCommand(byte[] command)
        {
            //fins header
            var header = new byte[10] { ICF, RSV, GCT, DNA, DA1, DA2, SNA, SA1, SA2, SID };
            //build complete command 
            var result = new byte[10 + command.Length];
            Array.Copy(header, 0, result, 0, 10);
            Array.Copy(command, 0, result, 10, command.Length);
            return result;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="length">读取长度</param>
        /// <param name="isBit">是否字操作</param>
        /// <returns></returns>
        public virtual byte[] BuildFinsReadCommand(string address, ushort length, bool isBit)
        {
            //读存储器操作码，固定 01 01 hex
            var readCommandCode = new byte[2] { 0x01, 0x01 };
            //地址码
            var addressCommand = AnalyzeAddress(address, isBit);

            //长度码
            var lengthCode = new byte[2] { (byte)(length / 256), (byte)(length % 256) };
            //组合
            var readCommand = new byte[8];
            readCommandCode.CopyTo(readCommand, 0);
            addressCommand.CopyTo(readCommand, 2);
            lengthCode.CopyTo(readCommand, 6);
            return BuildCompleteCommand(readCommand);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="data"></param>
        /// <param name="isBit"></param>
        /// <returns></returns>
        public virtual byte[] BuildFinsWriteCommand(string address, byte[] data, bool isBit)
        {
            //写存储器操作码，固定 01 02 hex
            var writeCommandCode = new byte[2] { 0x01, 0x02 };                                    
            //地址码
            var addressCommand = AnalyzeAddress(address, isBit);                                 
    
            //长度码
            var lengthCode = new byte[2];   
            if (isBit)
            {
                //位操作，每个字节就是一个数据，对应数据长度就是字节长度
                lengthCode = [(byte)(data.Length / 256), (byte)(data.Length % 256)];              
            }
            else
            {
                //字操作，每两个字节就是一个数据，对应数据长度是字节长度的一半
                lengthCode = [(byte)(data.Length / 2 / 256), (byte)(data.Length / 2 % 256)];      
            }
            //组合
            var writeCommand = new byte[8 + data.Length];                                         
            Array.Copy(writeCommandCode, 0, writeCommand, 0, 2);
            Array.Copy(addressCommand, 0, writeCommand, 2, 4);
            Array.Copy(lengthCode, 0, writeCommand, 6, 2);
            Array.Copy(data, 0, writeCommand, 8, data.Length);
            return BuildCompleteCommand(writeCommand);
        }

        /// <summary>
        /// 从fins udp response中解析出需要的信息,在 tcp 和 hostlink 中重写
        /// </summary>
        /// <returns></returns>
        public virtual FinsResponse AnalyzeFinsResponse(byte[] result)
        {
            //TODO 在此处抛出设备异常
            //TODO 该部分代码需要根据omron 通讯手册 5-1-3 END CODES 修改
            //正确的返回，至少包含 fins header. command code. end code 共 14字节 
            if (result.Length > 14)                                                        
            {
                //有返回数据,拆分出数据
                var buffer = new byte[result.Length - 14];
                Array.Copy(result, 14, buffer, 0, buffer.Length);
                return new FinsResponse() { Data = buffer };
            }
            //无返回数据
            if (result.Length == 14)
            {
                return FinsResponse.NormalSuccess;
            }
            return FinsResponse.NormalFail;
        }

    }

    
}
