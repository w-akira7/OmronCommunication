using OmronCommunication;

namespace OmronCommunication.PLC
{
    public interface IPLC
    {
        public string Name { get; }
        /// <summary>
        /// 读单个位地址的BOOL值
        /// </summary>
        /// <param name="address">目标地址.格式: "C100.01" "D100.01"</param>
        public Task<bool> ReadBoolAsync(string address);
        /// <summary>
        /// 读取多个连续位地址的bool值
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="length">读取长度</param>
        public Task<bool[]> ReadBoolAsync(string address, ushort length);
        /// <summary>
        /// 读单个字地址的short值
        /// </summary>
        /// <param name="address">目标地址.格式: "D100" "H100"</param>
        public Task<short> ReadShortAsync(string address);
        /// <summary>
        /// 读取多个连续字地址的short值
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="length">读取长度</param>
        public Task<short[]> ReadShortAsync(string address, ushort length);
        /// <summary>
        /// 读单个字地址的int值
        /// </summary>
        /// <param name="address">目标地址</param>
        public Task<int> ReadIntAsync(string address);
        /// <summary>
        /// 读取多个连续字地址的int值
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="length">读取长度</param>
        public Task<int[]> ReadIntAsync(string address, ushort length);
        /// <summary>
        /// 读单个字地址的float值
        /// </summary>
        /// <param name="address">目标地址</param>
        public Task<float> ReadFoaltAsync(string address);
        /// <summary>
        /// 读取多个连续字地址的float值
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="length">读取长度</param>
        public Task<float[]> ReadFoaltAsync(string address, ushort length);
        /// <summary>
        /// 向单个位地址写入bool值
        /// </summary>
        /// <param name="address">目标地址</param>
        /// <param name="value"><see href="true"/> or <see href="false"/></param>
        public Task WriteAsync(string address, bool value);
        /// <summary>
        /// 向连续的多个位地址写入bool值
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="value">a bool array</param>
        public Task WriteAsync(string address, bool[] value);
        /// <summary>
        /// 向单个字地址写入short值
        /// </summary>
        /// <param name="address">目标地址</param>
        /// <param name="value"></param>
        public Task WriteAsync(string address, short value);
        /// <summary>
        /// 向连续的多个字地址写入short值
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="value">a short array</param>
        public Task WriteAsync(string address, short[] value);
        /// <summary>
        /// 向单个字地址写入int值
        /// </summary>
        /// <param name="address">目标地址</param>
        /// <param name="value"></param>
        public Task WriteAsync(string address, int value);
        /// <summary>
        /// 向连续的多个字地址写入int值
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="value">a int array</param>
        public Task WriteAsync(string address, int[] value);
        /// <summary>
        /// 向单个字地址写入float值
        /// </summary>
        /// <param name="address">目标地址</param>
        /// <param name="value"></param>
        public Task WriteAsync(string address, float value);
        /// <summary>
        /// 向连续的多个字地址写入float值
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="value">a float array</param>
        public Task WriteAsync(string address, float[] value);
    }
}
