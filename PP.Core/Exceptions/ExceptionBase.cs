using System;

namespace PP.Core.Exceptions
{
    class  ExceptionBase : Exception, IException
    {
        public new string Data { get; set; }

        public ExceptionBase(string message)
            : base(message)
        { }
    }
}

