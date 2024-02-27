using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmronCommunication.DataTypes
{
    public class OperateResult
    {
        public OperateResult() { }
        public OperateResult(bool isSuccess) 
        {
            IsSuccess = isSuccess;
        }
        public OperateResult(string message)
        {
            Message = message;
        }
        public OperateResult(string message, int errorcode)
        {
            Message = message;
            Errorcode = errorcode;
        }
        public OperateResult(bool isSuccess, string message, int errorcode)
        {
            IsSuccess = isSuccess;
            Message = message;
            Errorcode = errorcode;
        }

        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public int? Errorcode { get; set; }

    }

    public class OperateResult<T> : OperateResult
    {
        public OperateResult():base() { }
        public OperateResult(bool isSuccess): base (isSuccess) { }
        public OperateResult(string message):base(message) { }
        public OperateResult(string message, int errorcode):base(message, errorcode) { }
        public OperateResult(bool isSuccess, string message, int errorcode):base(isSuccess, message, errorcode) { }
        public T? Value { get; set; }
    }

    public class OperateResult<T1,T2> : OperateResult
    {
        public OperateResult() : base() { }
        public OperateResult(bool isSuccess) : base(isSuccess) { }
        public OperateResult(string message) : base(message) { }
        public OperateResult(string message, int errorcode) : base(message, errorcode) { }
        public OperateResult(bool isSuccess, string message, int errorcode) : base(isSuccess, message, errorcode) { }
        public T1? Value1 { get; set; }
        public T2? Value2 { get; set; }
    }
}
