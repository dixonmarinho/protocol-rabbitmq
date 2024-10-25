using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace protocol.rabbitmq.shared.Interfaces
{
    public interface IServiceLog
    {
        void Info(string message, bool separator = false);
        void Error(Exception e, bool separator = false);
        void Error(string message, bool separator = false);
        void Warn(string message, bool separator = false);
        void Warn(string message, Exception exception, bool separator = false);
        void Error(string message, Exception exception, bool separator = false);
        void Info(string message, Exception exception, bool separator = false);
    }
}
