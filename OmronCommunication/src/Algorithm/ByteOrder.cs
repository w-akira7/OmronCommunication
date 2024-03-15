using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmronCommunication.Algorithm
{
    public enum ByteOrder
    {
        /// <summary>
        /// 大端序
        /// </summary>
        ABCD = 0,
        /// <summary>
        /// 大端序字节交换
        /// </summary>
        BADC = 1,
        /// <summary>
        /// 小端序字节交换
        /// </summary>
        CDAB = 2,
        /// <summary>
        /// 小端序
        /// </summary>
        DCBA = 3
    }
}
