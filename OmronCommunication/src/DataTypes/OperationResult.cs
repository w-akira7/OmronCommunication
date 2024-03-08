using OmronCommunication.Resource;

namespace OmronCommunication
{
    public class OperationResult
    {
        public OperationResult() { }
        public OperationResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
        public OperationResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
        public OperationResult(bool isSuccess, string message, int errorcode)
        {
            IsSuccess = isSuccess;
            Message = message;
            Errorcode = errorcode;
        }

        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public int? Errorcode { get; set; }

        public static OperationResult CreateSuccessResult()
            => new OperationResult(true, StringResources.Success, ErrorCode.NormalSuccess);       
        public static OperationResult CreateFailResult()
            => new OperationResult(false, StringResources.Fail, ErrorCode.NormalFail);
        
        public static OperationResult CreateFailResult(string msg, int errorCode)
           => new OperationResult(false, msg, errorCode);

        public static OperationResult<T> CreateSuccessResult<T>(T value)
            =>new OperationResult<T>(true, StringResources.Success, ErrorCode.NormalSuccess) {Value = value  };

        public static OperationResult<T> CreateFailResult<T>()
            =>new OperationResult<T>(false, StringResources.Fail, ErrorCode.NormalFail);

        public static OperationResult<T> CreateFailResult<T>(string msg,int errorCode)
           => new OperationResult<T>(false, msg, errorCode);
    }

    public class OperationResult<T> : OperationResult
    {
        public OperationResult() : base() { }
        public OperationResult(bool isSuccess) : base(isSuccess) { }
        public OperationResult(bool isSuccess,string message) : base(isSuccess, message) { }
        public OperationResult(bool isSuccess, string message, int errorcode) : base(isSuccess, message, errorcode) { }
        public T? Value { get; set; }
    }

    public class OperationResult<T1, T2> : OperationResult
    {
        public OperationResult() : base() { }
        public OperationResult(bool isSuccess) : base(isSuccess) { }
        public OperationResult(bool isSuccess, string message) : base(isSuccess, message) { }
        public OperationResult(bool isSuccess, string message, int errorcode) : base(isSuccess, message, errorcode) { }
        public T1? Value1 { get; set; }
        public T2? Value2 { get; set; }
    }
}
