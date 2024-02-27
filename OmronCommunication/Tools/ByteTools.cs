using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace OmronCommunication.Tools
{
    public class ByteTools
    {
        public ByteTools() { }

        /// <summary>
        /// 字节高低位反转.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] ReverseWordBytes(byte[] data) 
        { 
            var length = data.Length;
            if (length > 1)
            {
                var result = new byte[length];
                foreach (byte b in data)
                {
                    result[length - 1] = b;
                    length--;
                }
                return result;
            }
            else
            {
                return data;
            }
        }

        public static int Toint(byte[] data)
        {
            return (int)(data[0] & 0xff);
        }
    }
}
