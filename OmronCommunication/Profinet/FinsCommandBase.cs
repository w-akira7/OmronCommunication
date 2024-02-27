using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OmronCommunication.Profinet
{
    public abstract class FinsCommandBase
    {
        public FinsCommandBase() { }

        /// <summary>
        /// Information Control Field.
        /// </summary>
        public byte ICF { get; set; } = 0x80;
        /// <summary>
        /// RSV (Reserved) is always 00 hex.
        /// </summary>
        public byte RSV { get; private set; } = 0x00;
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
        /// 10 to 1F hex: CPU Bus Unit
        /// </summary>
        public byte SA2 { get; set;}
        /// <summary>
        /// Service ID. Used to identify the process generating the transmission. Set the SID to any number between 00 and FF
        /// </summary>
        public byte SID { get; set; } = 0x00;

    }
}
