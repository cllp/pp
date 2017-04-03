using System;

namespace PP.Core.Model
{
    public class ErrorCode
    {
        private int code = 1; //default error code
        private string message = "General application exception";
        //private Exception exception;
        private DateTime occuredAt;

        public int Code
        {
            get { return code; }
            set { code = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        //public Exception Exception
        //{
        //    get { return exception; }
        //    set { exception = value; }
        //}

        public DateTime OccuredAt
        {
            get { return occuredAt; }
            set { occuredAt = value; }
        }
    }
}
