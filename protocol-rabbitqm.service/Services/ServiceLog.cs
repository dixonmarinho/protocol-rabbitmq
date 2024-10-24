using protocol.rabbitqm.shared.Interfaces;

namespace protocol.rabbitqm.service.Services
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
            Serilog.Log.Information(message);
        }

        public void Error(string message, Exception exception, bool separator = false)
        {
            ShowLineSeparator(separator);
            Serilog.Log.Error(message, exception);
        }

        public void Error(Exception e, bool separator = false)
        {
            ShowLineSeparator(separator);
            throw new NotImplementedException();
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
