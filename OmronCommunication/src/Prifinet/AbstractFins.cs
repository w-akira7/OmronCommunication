using OmronCommunication;
using OmronCommunication.TinyNet;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;

namespace OmronCommunication.Profinet
{
    public abstract class AbstractFins : AbstractFinsHeader, IProfinet
    {
        private IPAddress? _localIP;
        private IPAddress? _remoteIP;

        public abstract AbstractTinyClient Client { get; protected set; }
        public abstract int Timeout { get; set; }

        public AbstractFins([NotNull] IPAddress remoteIP, [NotNull] int remotePort)
        {
            RemoteIP = remoteIP;
            RemotePort = remotePort;
        }

        /// <summary>
        /// 上位机网络地址
        /// </summary>
        public IPAddress? LocalIP
        {
            get => _localIP;
            protected set
            {
                _localIP = value;
                SA1 = value!.GetAddressBytes()[3];
            }
        }

        /// <summary>
        /// 上位机的端口
        /// </summary>
        public int LocalPort { get; protected set; }

        /// <summary>
        /// 目标网络设备的IP地址
        /// </summary>
        public IPAddress? RemoteIP
        {
            get => _remoteIP;
            private set
            {
                _remoteIP = value;
                DA1 = value!.GetAddressBytes()[3];
            }
        }

        /// <summary>
        /// 目标网络设备的端口, omron plc 默认 9600
        /// </summary>
        public int RemotePort { get; private set; } = 9600;

        // 解析地址。要求的输入格式“C100.12”、“D100”
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


        // Parse the address into byte data in FINS format. "C0010.13" Bit 13 of CIO 0010, i.e., 0x30 0x00 0x0A 0x0D
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


        // 按 UDP 传输方式将命令与 FINS 头组合成完整的 FINS 数据包,在 TCP 中重写
        public virtual byte[] BuildCompleteCommand(byte[] command)
        {
            byte[] header = [ICF, RSV, GCT, DNA, DA1, DA2, SNA, SA1, SA2, SID];

            SID++;

            //build complete command 
            var result = new byte[10 + command.Length];
            Array.Copy(header, 0, result, 0, 10);
            Array.Copy(command, 0, result, 10, command.Length);
            return result;
        }


        // 组合基础的 Fins Read Command
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

        // 组合基础的 Fins Write Command
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

        public void Write(string address, byte[] data, bool isBit)
        {
            var command = BuildFinsWriteCommand(address, data, isBit);

            var response = Client.RequestWaitResponse(command);

            AnalyzeFinsResponse(response);
        }

        public byte[] Read(string address, ushort length, bool isBit)
        {
            var command = BuildFinsReadCommand(address, length, isBit);

            var response = Client.RequestWaitResponse(command);

            var result = AnalyzeFinsResponse(response);   
            return result.Text;
        }

        public async Task WriteAsync(string address, byte[] data, bool isBit)
        {
            var command = BuildFinsWriteCommand(address, data, isBit);

            var response = await Client.RequestWaitResponseAsync(command);

            AnalyzeFinsResponse(response);
        }
                
        public async Task<byte[]> ReadAsync(string address, ushort length, bool isBit)
        {
            var command = BuildFinsReadCommand(address, length, isBit);

            var response = await Client.RequestWaitResponseAsync(command);

            var result = AnalyzeFinsResponse(response);
            return result.Text;
        }

        // 从fins response中解析出需要的信息,交给finstcp finsudp分别实现
        public abstract FinsResponse AnalyzeFinsResponse(byte[] result);

        public abstract Task ConnectAsync();

        public abstract void Close();
    }
}
