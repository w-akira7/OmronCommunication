using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmronCommunication
{

    public class ByteLengthExceededException:Exception
    { 
        public ByteLengthExceededException() :base(){ }
    }


}
