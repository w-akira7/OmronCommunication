
namespace OmronCommunication.DataTypes
{
    public class FinsHeader
    {
        public FinsHeader()
        {
        }
        /// <summary>
        /// Information Control Field.
        /// </summary>
        public byte ICF { get; set; } = 0x80;
        /// <summary>
        /// RSV (Reserved) is always 00 hex.
        /// </summary>
        public byte RSV { get; set; } = 0x00;
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
        /// 
        /// </summary>
        /// <returns>an array of 10 bytes that contains FINS header</returns>
        public byte[] ToByteArray()
        {
            return [ICF, RSV, GCT, DNA, DA1, DA2, SNA, SA1, SA2, SID];
        }
    }

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

    public class FinsResponse
    {       
        public FinsHeader Header = new FinsHeader();             // 10 bytes
        public FinsCommandCode CommandCode;   // 2 bytes
        public FinsEndCode EndCode;           // 2 bytes
        public bool hasText;
        public byte[]? Text;
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

    public struct FinsEndCode
    {
        public FinsEndCode(byte mainCode,byte subCode)
        {
            MainCode=mainCode;
            SubCode=subCode;
        }
        public byte MainCode { get; set; }
        public byte SubCode { get; set; }

    }
}
