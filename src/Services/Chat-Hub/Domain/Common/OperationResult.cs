namespace Domain.Common
{
    public class OperationResult : OperationResult<object>


    {
      

        public OperationResult(ResultStatus status, string msg = null): base(status, msg)
        {
            Status = status;
            Msg = msg;
        }

        public OperationResult(ResultStatus status,  object data, string msg = null) : base(status, msg)
        {
            Status = status;
            Msg = msg;
            Data = data;
        }

    }

    public class OperationResult<T>
    {
        protected OperationResult(ResultStatus status, string msg = null)
        {
            Status = status;
            Msg = msg;
        }

        protected OperationResult(ResultStatus status, T data, string msg = null)
        {
            Status = status;
            Msg = msg;
            Data = data;
        }

        protected ResultStatus Status { get; set; }
        public string Msg { get; set; }
        public T Data { get; set; }
        public bool Ok => Status == ResultStatus.Success;

        #region Method

        public static OperationResult<T> Success(string msg = null)
        {
            return new(ResultStatus.Success, msg);
        }

    
        public static OperationResult<T> Success(T data, string msg = null)
        {
            return new(ResultStatus.Success, data, msg );
        }
        public static OperationResult<T> Error(string msg = null)
        {
            return new(ResultStatus.Error, msg);
        }


        #endregion
    }


    

    public enum ResultStatus
    {
        Error = 0,
        Success = 1
    }
}
