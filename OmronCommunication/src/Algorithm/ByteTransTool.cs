namespace OmronCommunication.Tools
{
    public static class ByteTransTool
    {
        /// <summary>
        /// 每两个字节高低位反转.ABCD => BADC
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] ReverseWordBytes(byte[] data)
        {
            //字节长度必须符合要求
            if (data.Length % 2 == 0)
            {
                var buffer = new byte[data.Length];
                //每两个字节反转
                for (int i = 0; i < data.Length; i = i + 2)
                {
                    buffer[i] = data[i + 1];
                    buffer[i + 1] = data[i];
                }
                return buffer;
            }
            //TODO 
            throw new Exception();
        }

        /// <summary>
        /// 每四个字节高低位反转.ABCD => DCBA
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] ReverseDWordBytes(byte[] data)
        {
            //字节长度必须符合要求
            if (data.Length % 4 == 0)
            {
                var buffer = new byte[data.Length];
                //每4个字节反转
                for (int i = 0; i < data.Length; i = i + 4)
                {
                    buffer[i] = data[i + 3];
                    buffer[i + 1] = data[i + 2];
                    buffer[i + 2] = data[i + 1];
                    buffer[i + 3] = data[i];
                }
                return buffer;
            }
            //TODO 
            throw new Exception();
        }

        /// <summary>
        /// 计算机小端序方式单字转换short
        /// </summary>
        /// <param name="data">2字节数组</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static short WordToInt16(byte[] data)
        {
            if (data.Length == sizeof(short))
            {
                return BitConverter.ToInt16(data, 0);
            }
            //TODO 
            throw new Exception();
        }

        /// <summary>
        /// 计算机小端序方式单字组转换short组
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static short[] WordsToInt16(byte[] data)
        {
            var res = new short[data.Length / sizeof(short)];
            //每2个字节一组
            for (int i = 0; i < data.Length; i = i + 2)
            {
                byte[] buffer = { data[i], data[i + 1] };
                res[i / 2] = WordToInt16(buffer);
            }
            return res;
        }

        /// <summary>
        /// 计算机小端序方式双字转换int
        /// </summary>
        /// <param name="data">小端序字节数组</param>
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

        /// <summary>
        /// 计算机小端序方式双字组转换int组
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int[] DWordsToInt32(byte[] data)
        {
            var res = new int[data.Length / sizeof(int)];
            //每4个字节一组
            for (int i = 0; i < data.Length; i = i + 4)
            {
                byte[] buffer = { data[i], data[i + 1], data[i + 2], data[i + 3] };
                res[i / 4] = DWordToInt32(buffer);
            }
            return res;
        }

        /// <summary>
        /// 计算机小端序方式双字转换float
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static float DWordToFloat(byte[] data)
        {
            if (data.Length == sizeof(float))
            {
                return BitConverter.ToSingle(data, 0);
            }
            //TODO 
            throw new Exception();

        }

        /// <summary>
        /// 计算机小端序方式双字组转换float组
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static float[] DWordsToFloat(byte[] data)
        {
            var res = new float[data.Length / sizeof(float)];
            //每4个字节一组
            for (int i = 0; i < data.Length; i = i + 4)
            {
                byte[] buffer = { data[i], data[i + 1], data[i + 2], data[i + 3] };
                res[i / 4] = DWordToFloat(buffer);
            }
            return res;

        }

        /// <summary>
        /// short转换为计算机小端序方式字节组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ShortToBytes(short value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// short组转换为计算机小端序方式字节组
        /// </summary>
        /// <param name="value"></param>
        /// <returns>按计算机小端序方式存储的字节数组</returns>
        public static byte[] ShortToBytes(short[] value)
        {
            var buffer = new byte[value.Length * sizeof(short)];
            for (int i = 0; i < value.Length; i++)
            {
                BitConverter.GetBytes(value[i]).CopyTo(buffer, i * 2);
            }
            return buffer;
        }

        /// <summary>
        /// int转换为计算机小端序方式字节组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] IntToBytes(int value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// int组转换为计算机小端序方式字节组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] IntToBytes(int[] value)
        {
            var buffer = new byte[value.Length * sizeof(int)];
            for (int i = 0; i < value.Length; i++)
            {
                BitConverter.GetBytes(value[i]).CopyTo(buffer, i * 4);
            }
            return buffer;
        }

        /// <summary>
        /// float转换为计算机小端序方式字节组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] FloatToBytes(float value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// float组转换为计算机小端序方式字节组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] FloatToBytes(float[] value)
        {
            var buffer = new byte[value.Length * sizeof(float)];
            for (int i = 0; i < value.Length; i++)
            {
                BitConverter.GetBytes(value[i]).CopyTo(buffer, i * 4);
            }
            return buffer;
        }
    }
}
