namespace OmronCommunication.DataTypes
{
    public class OperationResult
    {
        public OperationResult() { }
        public OperationResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
        public OperationResult(string message)
        {
            Message = message;
        }
        public OperationResult(string message, int errorcode)
        {
            Message = message;
            Errorcode = errorcode;
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

    }

    public class OperationResult<T> : OperationResult
    {
        public OperationResult() : base() { }
        public OperationResult(bool isSuccess) : base(isSuccess) { }
        public OperationResult(string message) : base(message) { }
        public OperationResult(string message, int errorcode) : base(message, errorcode) { }
        public OperationResult(bool isSuccess, string message, int errorcode) : base(isSuccess, message, errorcode) { }
        public T? Value { get; set; }
    }

    public class OperationResult<T1, T2> : OperationResult
    {
        public OperationResult() : base() { }
        public OperationResult(bool isSuccess) : base(isSuccess) { }
        public OperationResult(string message) : base(message) { }
        public OperationResult(string message, int errorcode) : base(message, errorcode) { }
        public OperationResult(bool isSuccess, string message, int errorcode) : base(isSuccess, message, errorcode) { }
        public T1? Value1 { get; set; }
        public T2? Value2 { get; set; }
    }
}
