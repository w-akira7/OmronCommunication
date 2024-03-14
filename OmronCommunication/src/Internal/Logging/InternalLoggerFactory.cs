using OmronCommunication.Internal.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmronCommunication.Internal.Logging
{
    public static class InternalLoggerFactory
    {
        public static IInternalLogger CreateLogger()
        {
            return new GenericLogger();
        }


    }
}
