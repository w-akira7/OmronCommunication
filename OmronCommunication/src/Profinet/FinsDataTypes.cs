namespace OmronCommunication.Profinet
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

    public class FinsResponse : AbstractFinsHeader
    {
        public FinsCommandCode commandCode;
        public FinsEndCode endCode;
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
        public FinsEndCode(byte mainCode, byte subCode)
        {
            MainCode = mainCode;
            SubCode = subCode;
        }
        public byte MainCode { get; set; }
        public byte SubCode { get; set; }

        public static readonly FinsEndCode NormalSuccess = new FinsEndCode(0x00, 0x00);
        // TODO
    }
}
