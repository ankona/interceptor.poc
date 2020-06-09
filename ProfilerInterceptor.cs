// using Autofac.Extensions.DependencyInjection;
// using Autofac;
// using Microsoft.Extensions.DependencyInjection;
// using Autofac.Extras.DynamicProxy;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using StackExchange.Profiling;

namespace dt
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

    public class ExInterceptor : IInterceptor
    {
        private string MethodName { get;set ; }

        public void Intercept(IInvocation invocation)
        {
            MethodName = $"{invocation.Method.DeclaringType.Name}.{invocation.Method.Name}";
            if (invocation.Method.DeclaringType.IsAbstract)
            {
                MethodName = $"{invocation.TargetType.FullName}.{invocation.Method.Name}";
            }
            Console.WriteLine($"Executing {MethodName}.");

            invocation.Proceed();
            var method = invocation.MethodInvocationTarget;
            var isAsync = method.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null;
            if (isAsync && typeof(Task).IsAssignableFrom(method.ReturnType))
            {
                invocation.ReturnValue = InterceptAsync((dynamic)invocation.ReturnValue);
            }

            // Console.WriteLine($"Exiting {MethodName}.");
            Console.WriteLine($"Exiting {MethodName}.");
        }

        private async Task InterceptAsync(Task task)
        {
            await task.ConfigureAwait(false);
            Console.WriteLine($"Continuing {MethodName}.");
        }

        private  async Task<T> InterceptAsync<T>(Task<T> task)
        {
            T result = await task.ConfigureAwait(false);
            Console.WriteLine($"Continuing {MethodName}.");
            return result;
        }
    }
}