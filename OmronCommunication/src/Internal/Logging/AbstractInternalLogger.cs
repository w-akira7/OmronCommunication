using OmronCommunication.Internal.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmronCommunication.Internal.Logging
{
    public abstract class AbstractInternalLogger : IInternalLogger
    {
        public abstract void Debug(string message);
        public abstract void Error(string message);
        public abstract void Info(string message);
        public abstract void Log(string message);
        public abstract void Warn(string message);
    }
}
