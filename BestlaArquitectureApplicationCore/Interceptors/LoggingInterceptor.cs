using System;
using System.Threading.Tasks;
using BestlaArquitectureApplicationCore.Extentions;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;

namespace ApplicationCore.Interceptors
{
    public class LoggingAsyncInterceptor : AsyncInterceptorBase
    {
        private readonly ILogger<LoggingAsyncInterceptor> _logger;

        public LoggingAsyncInterceptor(ILogger<LoggingAsyncInterceptor> logger)
        {
            _logger = logger;
        }
        protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            try
            {
                // Cannot simply return the the task, as any exceptions would not be caught below.
                await proceed(invocation, invocation.CaptureProceedInfo()).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling {invocation.Method.Name}.", ex);
                throw;
            }
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            try
            {
                _logger.LogInformation($"Calling method {invocation.TargetType}.{invocation.Method.Name} input {invocation.Arguments.ToJson()}." );
                var result= await proceed(invocation, invocation.CaptureProceedInfo()).ConfigureAwait(false);
                _logger.LogInformation($"Salida method {invocation.TargetType}.{invocation.Method.Name} salida {result.ToJson()}.");
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError($"*********************************Error calling {invocation.Method.Name}. {ex.ToString()}", ex);
                throw;
            }
        }
    }
}
        