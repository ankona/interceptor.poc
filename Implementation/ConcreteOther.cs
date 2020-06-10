using System;
using InterceptorPOC.Definition;

namespace InterceptorPOC.Implementation
{
    public class ConcreteOther : IOther
    {
        public void DoStuff()
        {
            Console.WriteLine("Doing some stuff synchronously.");
        }
    }
}
