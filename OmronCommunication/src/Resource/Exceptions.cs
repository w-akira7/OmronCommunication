using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmronCommunication.Exceptions
{

    public class LengthErrorException:Exception
    { 
        public LengthErrorException() :base(){ }

    }

    public class InvalidProfinetException : Exception
    {
        public InvalidProfinetException(string? message) : base(message)
        {
        }
    }
}
