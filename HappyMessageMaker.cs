using System;
using System.Threading.Tasks;

namespace InterceptorPOC
{
    public class HappyMessageMaker : IMessageMaker
    {
        public Task WriteMessage()
        {
            Console.WriteLine("I hope you have a wonderful day!");
            return Task.CompletedTask;
        }

        public Task Preach()
        {
            Console.WriteLine("Feel the love!");
            return Task.CompletedTask;
        }
    }
}
