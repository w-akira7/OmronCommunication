using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmronCommunication.DataTypes
{   
    /// <summary>
    /// I/O Memory Area Designation.
    /// Get the defination in《Omron Communications Commands REFERENCE MANUAL》section 5-2-2 (page 187).
    /// </summary>
    public struct FinsAddressTypes
    {
        public FinsAddressTypes(byte bitCode, byte wordCode)
        {
            BitCode = bitCode;
            WordCode = wordCode;
        }

        public byte BitCode { get; private set; }
        public byte WordCode { get; private set; }


        /// <summary>
        /// CIO Memory area code.
        /// </summary>
        public static readonly FinsAddressTypes CIO = new FinsAddressTypes(0x30, 0xB0);
        /// <summary>
        /// WR Memory area code.
        /// </summary>
        public static readonly FinsAddressTypes WR = new FinsAddressTypes(0x31, 0xB1);
        /// <summary>
        /// HR Memory area code
        /// </summary>
        public static readonly FinsAddressTypes HR = new FinsAddressTypes(0x32, 0xB2);
        /// <summary>
        /// AR Memory area code
        /// </summary>
        public static readonly FinsAddressTypes AR = new FinsAddressTypes(0x33, 0xB3);
        /// <summary>
        /// DM Memory area code
        /// </summary>
        public static readonly FinsAddressTypes DM = new FinsAddressTypes(0x02, 0x82);

    }

    public struct FinsResponse
    {
        public FinsResponse(bool isSuccess) { IsSuccess = isSuccess; }
        public FinsResponse(FinsCommandCode cmd, FinsEndCode end)
        {
            CommandCode = cmd;
            EndCode = end;
        }
        public bool IsSuccess { get; set; }
        public FinsCommandCode CommandCode { get; }
        public FinsEndCode EndCode { get; set; }
        public byte[]? Data { get; set; }

        // TODO
        public static readonly FinsResponse NormalSuccess
            = new FinsResponse(true);
        public static readonly FinsResponse NormalFail
            = new FinsResponse(false);
    }

    public struct FinsCommandCode
    {
        public FinsCommandCode(byte mr, byte sr)
        {
            MR = mr;
            SR = sr;
        }
        public byte MR { get; set; }
        public byte SR { get; set; }

        public static readonly FinsCommandCode MemoryAreaRead = new FinsCommandCode(0x01, 0x01);
        public static readonly FinsCommandCode MemoryAreaWrite = new FinsCommandCode(0x01, 0x02);
        // TODO 
    }

    // TODO 
    public struct FinsEndCode
    {
        public FinsEndCode(byte mainCode,byte subCode)
        {
            MainCode=mainCode;
            SubCode=subCode;
        }
        public byte MainCode { get; set; }
        public byte SubCode { get; set; }

        public static readonly FinsEndCode NormalCompletion = new FinsEndCode(0x00, 0x00);
    }
}
