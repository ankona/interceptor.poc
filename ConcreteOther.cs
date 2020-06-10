using System;

namespace InterceptorPOC
{
    public class ConcreteOther : IOther
    {
        public void DoStuff()
        {
            Console.WriteLine("Doing some stuff synchronously.");
        }
    }
}
