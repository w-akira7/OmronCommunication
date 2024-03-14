using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmronCommunication.Internal.Logging
{
    sealed class GenericLogger : AbstractInternalLogger
    {
        public GenericLogger() { }

        public override void Debug(string message)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override void Error(string message)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void Info(string message)
        {
            throw new NotImplementedException();
        }

        public override void Log(string message)
        {
            throw new NotImplementedException();
        }

        public override string? ToString()
        {
            return base.ToString();
        }

        public override void Warn(string message)
        {
            throw new NotImplementedException();
        }
    }
}
