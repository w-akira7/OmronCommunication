using OmronCommunication.DataTypes;
using OmronCommunication.PLC;
using OmronCommunication.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OmronCommunication.Profinet
{
    public class FinsCommand:FinsCommandBase
    {
        public FinsCommand() { }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private OperateResult<FinsDataTypes> AnalyzeAddressType(string address)
        {
            var result = new OperateResult<FinsDataTypes>();
            switch (address[0])
            {
                //CIO
                case 'c':
                case 'C':
                    {
                        result.Value = FinsDataTypes.CIO;
                        break;
                    }
                //WR
                case 'w':
                case 'W':
                    {
                        result.Value = FinsDataTypes.WR;
                        break;
                    }
                //HR
                case 'h':
                case 'H':
                    {
                        result.Value = FinsDataTypes.HR;
                        break;
                    }
                //AR
                case 'a':
                case 'A':
                    {
                        result.Value = FinsDataTypes.AR;
                        break;
                    }
                //DM
                case 'd':
                case 'D':
                    {
                        result.Value = FinsDataTypes.DM;
                        break;
                    }
                //TODO 完善错误信息类
                default: throw new Exception("错误的地址格式");
            }
            return result;
        }

        /// <summary>
        /// Parse the address into byte data in FINS format. "C0010.13" Bit 13 of CIO 0010, i.e., 0x30 0x00 0x0A 0x0D
        /// </summary>
        /// <param name="address">目标起始地址</param>
        /// <param name="isBit">是否位操作</param>
        /// <returns>地址码：一个4字节的字节数组,包括 I/O Memory area code 和 Beginning address</returns>
        /// <exception cref="Exception"></exception> 
        private OperateResult<byte[]> AnalyzeAddress(string address, bool isBit)
        {   
            var result = new OperateResult<byte[]>();
            result.Value = new byte[4];
            var datatype = AnalyzeAddressType(address);
            //位操作 PLC中 1 word = 2 bytes = 16 bits
            if (isBit) 
            {
                result.Value[0] = datatype.Value.BitCode;
                var splits = address.Substring(1).Split('.', StringSplitOptions.RemoveEmptyEntries);
                var addr = ushort.Parse(splits[0]);
                result.Value[1] = BitConverter.GetBytes(addr)[1];
                result.Value[2] = BitConverter.GetBytes(addr)[2];
                result.Value[3] = byte.Parse(splits[1]);

            }
            //字操作
            else
            {
                result.Value[0] = datatype.Value.WordCode;
                var addr = ushort.Parse(address[1..]);
                result.Value[1] = BitConverter.GetBytes(addr)[1];
                result.Value[2] = BitConverter.GetBytes(addr)[0];
            }
            return result;
        }

        /// <summary>
        /// 将命令与 FINS 头组合成完整的 FINS 数据包
        /// </summary>
        /// <param name="command">command code and text</param>
        /// <returns>完整的可发送的FINS包</returns>
        private byte[] BuildCompleteCommand(byte[] command)
        {
            //build fins header
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
        public OperateResult<byte[]> BuildReadFinsCommand(string address,ushort length, bool isBit) 
        {
            //读存储器操作码，固定 01 01 hex
            var readCommandCode = new byte[2] { 0x01, 0x01 };

            //地址码
            var addressCommand = AnalyzeAddress(address, isBit);
            //TODO 完善 错误信息 错误码
            if(!addressCommand.IsSuccess) return new OperateResult<byte[]>(false,"地址解析错误",0);

            //长度码
            var lengthCode = new byte[2] { (byte)(length / 256), (byte)(length % 256) };

            //组合
            var readCommand = new byte[8];
            readCommandCode.CopyTo(readCommand, 0);
            addressCommand.Value!.CopyTo(readCommand, 2);
            lengthCode.CopyTo(readCommand, 6);
            var completeCommand = BuildCompleteCommand(readCommand);

            //TODO 完善格式
            return new OperateResult<byte[]>(true, "", 0) { Value = completeCommand};
        }

        public OperateResult<byte[]> BuildWriteFinsCommand(string address, byte[] data, bool isBit) 
        {
            //写存储器操作码，固定 01 02 hex
            var readCommandCode = new byte[2] { 0x01, 0x02 };

            //地址码
            var addressCommand = AnalyzeAddress(address, isBit);
            //TODO 完善 错误信息 错误码
            if (!addressCommand.IsSuccess) return new OperateResult<byte[]>(false, "地址解析错误", 0);

            //组合
            var writeCommand = new byte[6+data.Length];
            Array.Copy(readCommandCode, 0, writeCommand, 0, 2);
            Array.Copy(addressCommand.Value!, 0, writeCommand, 2, 4);
            var completeCommand = BuildCompleteCommand(writeCommand);

            //TODO 完善格式
            return new OperateResult<byte[]>(true, "", 0) { Value = completeCommand };
        }

        /// <summary>
        /// 从finscommand中解析出需要的信息
        /// </summary>
        /// <returns></returns>
        public OperateResult<byte[]> AnalyzeFinsCommand(OperateResult<byte[]> result) 
        {
            return new OperateResult<byte[]>(); 
        }
    }

    /// <summary>
    /// I/O Memory Area Designation.
    /// Get the defination in《Omron Communications Commands REFERENCE MANUAL》section 5-2-2 (page 187).
    /// </summary>
    public class FinsDataTypes
    {
        public FinsDataTypes(byte bitCode,byte wordCode) 
        {
            BitCode = bitCode;
            WordCode = wordCode;
        }

        public byte BitCode { get; private set; }
        public byte WordCode { get; private set; }


        /// <summary>
        /// CIO Memory area code.
        /// </summary>
        public static readonly FinsDataTypes CIO = new FinsDataTypes(0x30, 0xB0);
        /// <summary>
        /// WR Memory area code.
        /// </summary>
        public static readonly FinsDataTypes WR = new FinsDataTypes(0x31, 0xB1);
        /// <summary>
        /// HR Memory area code
        /// </summary>
        public static readonly FinsDataTypes HR = new FinsDataTypes(0x32, 0xB2);
        /// <summary>
        /// AR Memory area code
        /// </summary>
        public static readonly FinsDataTypes AR = new FinsDataTypes(0x33, 0xB3);
        /// <summary>
        /// DM Memory area code
        /// </summary>
        public static readonly FinsDataTypes DM = new FinsDataTypes(0x02, 0x82);

    }
}
