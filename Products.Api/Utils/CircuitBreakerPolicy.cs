using Npgsql;
using Polly;
using Polly.CircuitBreaker;
using Serilog;

namespace Products.Api.Utils
{
    public static class DatabaseCircuitBreakerPolicy 
    {
        public static AsyncCircuitBreakerPolicy CreatePolicy()
        {
            Log.Logger = new LoggerConfiguration()
                        .WriteTo.Console()
                        .CreateLogger();

            return Policy
            .Handle<PostgresException>() // Handles PostgreSQL exceptions
            .Or<NpgsqlException>()       // Handles general database errors
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 1, // If 3 failures occur, open the circuit
                durationOfBreak: TimeSpan.FromSeconds(30), // Wait 30 seconds before retrying
                onBreak: (exception, breakDelay) =>
                {
                   
                  
                    Log.Error($"🔴 Circuit OPEN! Database unavailable: {exception.Message}");
                    Log.Error($"⏳ Waiting {breakDelay.TotalSeconds} seconds before retrying...");
                },
                onReset: () => Log.Information("🟢 Circuit CLOSED! Database is back online."),
                onHalfOpen: () => Log.Information("🟡 Circuit HALF-OPEN, testing DB connection.")
            );
        }
    }

}
