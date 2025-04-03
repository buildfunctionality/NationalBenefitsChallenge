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
    }
}
