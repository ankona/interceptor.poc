using System;

namespace dt
{
    public class ConcreteOther : IOther
    {
        public void DoStuff()
        {
            Console.WriteLine("Doing some stuff synchronously.");
        }
    }
}