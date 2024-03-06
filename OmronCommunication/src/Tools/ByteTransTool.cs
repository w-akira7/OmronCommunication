namespace OmronCommunication.Tools
{
    public static class ByteTransTool
    {
        /// <summary>
        /// 一个WORD的2字节高低位反转.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] ReverseWordBytes(byte[] data)
        {
            if (data.Length == sizeof(short))
            {
                return
                [
                    data[1],
                    data[0],
                ];
            }
            //TODO 
            throw new Exception();
        }

        /// <summary>
        /// 一个字转换short
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static short WordToInt16(byte[] data)
        {
            if (data.Length == sizeof(short))
            {
               return BitConverter.ToInt16(data,0);
            }
            //TODO 
            throw new Exception();
        }

        /// <summary>
        /// 一个字高低位反转后转换short
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static short ResverseWordToInt16(byte[] data)
        {
            return WordToInt16 (ReverseWordBytes(data));
        }

        /// <summary>
        /// 双字转换int32
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int DWordToInt32(byte[] data)
        {
            if (data.Length == sizeof(int))
            {
                return BitConverter.ToInt32(data, 0);
            }
            //TODO 
            throw new Exception();
        }
    }
}
