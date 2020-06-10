using System;
using System.Threading.Tasks;
using InterceptorPOC.Definition;

namespace InterceptorPOC.Implementation
{
    public class MeanMessageMaker : IMessageMaker
    {
        public Task WriteMessage()
        {
            Console.WriteLine("I hope you have a terrible day!");
            System.Threading.Thread.Sleep(1000);
            return Task.CompletedTask;
        }

        public Task Preach()
        {
            Console.WriteLine("Go to hell!");
            System.Threading.Thread.Sleep(1000);
            return Task.CompletedTask;
        }
    }
}