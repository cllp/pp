using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PP.Core.Model.Enum;

namespace PP.Core.Interfaces
{
    public interface ILog
    {
        void Info(string message);
        void Info(string message, bool identity);
        void Warning(string message);
        void Warning(string message, Exception ex);
        void Error(string message);
        void Error(string message, Exception ex);
        void Debug(string message, LogLevel level);

        void Warning(string message, bool identity);
        void Warning(string message, Exception ex, bool identity);
        void Error(string message, bool identity);
        void Error(string message, Exception ex, bool identity);
        void Debug(string message, LogLevel level, bool identity);
    }
}
