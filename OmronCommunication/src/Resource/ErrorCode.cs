using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmronCommunication.Resource
{
    public static class ErrorCode
    {
        public static int NormalSuccess { get; } = 0;
        public static int NormalFail { get; } = 1;
        public static int NetSendReceiveError { get; } = 2;
    }
}
