using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using StackExchange.Profiling;

namespace InterceptorPOC.AOP
{
    public class ProfilerInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var methodName = $"{invocation.Method.DeclaringType.Name}.{invocation.Method.Name}";
            if (invocation.Method.DeclaringType.IsAbstract)
            {
                methodName = $"{invocation.TargetType.FullName}.{invocation.Method.Name}";
            }
            Console.WriteLine($"Calling {methodName}.");
            using (MiniProfiler.Current.Step(methodName))
            {
                invocation.Proceed();

            }
            Console.WriteLine($"Exiting {methodName}.");
        }
    }

    public class AsyncProfilerInterceptor : IAsyncInterceptor
    {
        public void InterceptAsynchronous(IInvocation invocation)
        {
            invocation.ReturnValue = InternalInterceptAsynchronous(invocation);
        }

        private async Task InternalInterceptAsynchronous(IInvocation invocation)
        {
            var methodName = $"{invocation.Method.DeclaringType.Name}.{invocation.Method.Name}";
            if (invocation.Method.DeclaringType.IsAbstract)
            {
                methodName = $"{invocation.TargetType.FullName}.{invocation.Method.Name}";
            }
            Console.WriteLine($"Calling {methodName}.");
            using (MiniProfiler.Current.Step(methodName))
            {
                invocation.Proceed();
                var task = (Task)invocation.ReturnValue;
                await task;
            }
            Console.WriteLine($"Exiting {methodName}.");
        }

        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            invocation.ReturnValue = InternalInterceptAsynchronous<TResult>(invocation);
        }

        private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
        {
            var methodName = $"{invocation.Method.DeclaringType.Name}.{invocation.Method.Name}";
            if (invocation.Method.DeclaringType.IsAbstract)
            {
                methodName = $"{invocation.TargetType.FullName}.{invocation.Method.Name}";
            }
            Console.WriteLine($"Calling {methodName}.");
            TResult result;
            using (MiniProfiler.Current.Step(methodName))
            {
                invocation.Proceed();
                var task = (Task<TResult>)invocation.ReturnValue;
                result = await task;
            }
            Console.WriteLine($"Exiting {methodName}.");
            return result;
        }

        public void InterceptSynchronous(IInvocation invocation)
        {
            var methodName = $"{invocation.Method.DeclaringType.Name}.{invocation.Method.Name}";
            if (invocation.Method.DeclaringType.IsAbstract)
            {
                methodName = $"{invocation.TargetType.FullName}.{invocation.Method.Name}";
            }
            Console.WriteLine($"Calling {methodName}.");
            using (MiniProfiler.Current.Step(methodName))
            {
                invocation.Proceed();
            }
            Console.WriteLine($"Exiting {methodName}.");
        }
    }
}
