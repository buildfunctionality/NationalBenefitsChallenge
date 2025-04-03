using System.Reflection;

namespace Products.Api.Utils
{
    public class LoggingDecorator<T> : DispatchProxy
    {
        private T _instance;
        private ILogger<T> _logger;

        public static T Create(T instance, ILogger<T> logger)
        {
            var proxy = Create<T, LoggingDecorator<T>>() as LoggingDecorator<T>;
            if (proxy == null) throw new InvalidOperationException("Failed to create proxy instance.");

            proxy._instance = instance;
            proxy._logger = logger;
            return (T)(object)proxy; 
        }

        //log every call method
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            _logger.LogInformation($"Calling method: {targetMethod.Name}");
            var result = targetMethod.Invoke(_instance, args);
            _logger.LogInformation($"Method {targetMethod.Name} executed");
            return result;
        }
    }

}
