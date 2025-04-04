using Serilog;

namespace Products.Api.Utils
{
    public class Logger
    {
       

        public void SetInformationLogMessage(string message, string caller) {
            Log.Logger = new LoggerConfiguration()
              .WriteTo.Console()
              .CreateLogger();

            Log.Information("SetLogMessage");
            Log.Information($"message {message} , call method {caller}");
        }

        public void SetErrorLogMessage(string message, string caller, string exception)
        {
            Log.Logger = new LoggerConfiguration()
              .WriteTo.Console()
              .CreateLogger();

            Log.Error("SetLogMessage");
            Log.Error($"message {message} , call method {caller}, exception on {exception}");
        }

        public void SetFatalLogMessage(string message, string caller)
        {
            Log.Logger = new LoggerConfiguration()
              .WriteTo.Console()
              .CreateLogger();

            Log.Fatal("SetLogMessage");
            Log.Fatal($"message {message} , call method {caller}");
        }
    }
}
