using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface ILoggerManager
    {
        void LogInfo(string message);
        void LogInfo(string message, object o);
        void LogWarn(string message);
        void LogDebug(string message);
        void LogError(string message);
        void LogError(string message, object o);

    }
}
