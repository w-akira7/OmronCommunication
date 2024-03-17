using OmronCommunication;
using OmronCommunication.DataTypes;
using OmronCommunication.TinyNet;
using System;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;

namespace OmronCommunication.Profinet
{
    public abstract class AbstractFinsDevice
    {
        private readonly FinsHeader _header;
        public FinsHeader Header => _header;
        public abstract INetDevice NetDevice { get; }
        public AbstractFinsDevice() { _header = new FinsHeader(); }

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
                default: throw new Exception("Wrong Address Format.");
            }
        }

        /// <summary>
        /// Parse the address into byte data in FINS format. "C0010.13" Bit 13 of CIO 0010, i.e., 0x30 0x00 0x0A 0x0D
        /// </summary>
        /// <param name="address">目标起始地址</param>
        /// <param name="isBit">是否位操作</param>
        /// <returns>地址码：一个4字节的字节数组,包括 I/O Memory area code 和 Beginning address</returns>
        /// <exception cref="Exception"></exception> 
        protected byte[] AnalyzeAddress(string address, bool isBit)
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
                buffer[2] = BitConverter.GetBytes(addr)[0];
                if(splits.Length > 1) buffer[3] = byte.Parse(splits[1]);
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
            var header = Header.ToByteArray();

            Header.SID++;

            //build complete command 
            var result = new byte[10 + command.Length];
            Array.Copy(header, 0, result, 0, 10);
            Array.Copy(command, 0, result, 10, command.Length);
            return result;
        }

        /// <summary>
        /// 组合基础的 FINS Read Command
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="length">读取长度</param>
        /// <param name="isBit">是否字操作</param>
        /// <returns></returns>
        public byte[] BuildFinsReadCommand(string address, ushort length, bool isBit)
        {
            //读存储器操作码，固定 01 01 hex
            var readCommandCode = new byte[2] { FinsCommandCode.MemoryAreaRead.MR, FinsCommandCode.MemoryAreaRead.SR };
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
        /// 组合基础的 FINS Write Command
        /// </summary>
        /// <param name="address"></param>
        /// <param name="data"></param>
        /// <param name="isBit"></param>
        /// <returns></returns>
        public byte[] BuildFinsWriteCommand(string address, byte[] data, bool isBit)
        {
            //写存储器操作码，固定 01 02 hex
            var writeCommandCode = new byte[2] { FinsCommandCode.MemoryAreaWrite.MR, FinsCommandCode.MemoryAreaWrite.SR };                                    
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
        /// 从fins response中解析出需要的信息
        /// </summary>
        /// <returns></returns>
        public abstract FinsResponse AnalyzeFinsResponse(byte[] result);
        
        public async Task Write(string address, byte[] data, bool isBit)
        {
            //BuildWriteCommand
            var command = BuildFinsWriteCommand(address, data, isBit);

            //ReadFromPLC
            var response = await NetDevice.RequestWaitResponse(command);

            //DataAnalysis
            AnalyzeFinsResponse(response);
        }
                
        public async Task<byte[]> Read(string address, ushort length, bool isBit)
        {
            //BuildReadCommand
            var command = BuildFinsReadCommand(address, length, isBit);

            //ReadFromPLC
            var response = await NetDevice.RequestWaitResponse(command);

            //DataAnalysis
            var result = AnalyzeFinsResponse(response);
            return result.Text;
        }
   
    }
}
