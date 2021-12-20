using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class OperationResult : OperationResult<object>
    {
        public OperationResult()
        {
        }

        public OperationResult(ResultStatus status, string msg)
        {
            Status = status;
            Msg = msg;
        }

        public OperationResult(ResultStatus status, string msg, object data)
        {
            Status = status;
            Msg = msg;
            Data = data;
        }

        #region Method

        public new static OperationResult Success()
        {
            return new(ResultStatus.Success, string.Empty);
        }

        public new static OperationResult Success(string msg)
        {
            return new(ResultStatus.Success, msg);
        }

        public static OperationResult Success(object data)
        {
            return new(ResultStatus.Success, string.Empty, data);
        }


        public new static OperationResult Error()
        {
            return new(ResultStatus.Error, string.Empty);
        }

        public new static OperationResult Error(string msg)
        {
            return new(ResultStatus.Error, msg);
        }

        #endregion
    }

    public class OperationResult<T>
    {
        protected OperationResult()
        {
        }

        private OperationResult(ResultStatus status, string msg)
        {
            Status = status;
            Msg = msg;
        }

        private OperationResult(ResultStatus status, T data, string msg)
        {
            Status = status;
            Msg = msg;
            Data = data;
        }

        protected ResultStatus Status { get; set; }

      
        public string Msg { get; set; }


        protected T Data { get; set; }

      
        public bool Ok => Status == ResultStatus.Success;

        #region Method

      
        public static OperationResult<T> Success()
        {
            return new(ResultStatus.Success, string.Empty);
        }

     
        public static OperationResult<T> Success(string msg)
        {
            return new(ResultStatus.Success, msg);
        }

    
        public static OperationResult<T> Success(T data, string msg = null)
        {
            return new(ResultStatus.Success, data, msg );
        }

       
        public static OperationResult<T> Error()
        {
            return new(ResultStatus.Error, string.Empty);
        }

       
        public static OperationResult<T> Error(string msg)
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
