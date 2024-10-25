using protocol.rabbitmq.shared.Interfaces;

namespace protocol.rabbitmq.service.Services
{
    public class ServiceLog : IServiceLog
    {
        public ServiceLog()
        {
        }

        private void ShowLineSeparator(bool separator)
        {
            if (separator)
                Serilog.Log.Information(Environment.NewLine + "--------------------------------------------------------------------------------");
        }

        public void Error(string message, bool separator = false)
        {
            ShowLineSeparator(separator);
            Serilog.Log.Error(message);
        }

        public void Error(string message, Exception exception, bool separator = false)
        {
            ShowLineSeparator(separator);
            Serilog.Log.Error(exception, message);
        }

        public void Error(Exception e, bool separator = false)
        {
            ShowLineSeparator(separator);
            Error(e.Message);
        }

        public void Info(string message, bool separator = false)
        {
            ShowLineSeparator(separator);
            Serilog.Log.Information(message);
        }

        public void Info(string message, Exception exception, bool separator = false)
        {
            ShowLineSeparator(separator);
            Serilog.Log.Information(message, exception);
        }

        public void Warn(string message, bool separator = false)
        {
            ShowLineSeparator(separator);
            Serilog.Log.Warning(message);
        }

        public void Warn(string message, Exception exception, bool separator = false)
        {
            ShowLineSeparator(separator);
            Serilog.Log.Warning(message, exception);
        }
    }
}
