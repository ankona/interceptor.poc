using System;
using System.Text;
using System.Threading.Tasks;

namespace dt
{
    public class AsyncMessageMaker : IAsyncMessageMaker
    {
        public async Task WriteMessage()
        {
            var sb = new StringBuilder();
            using(var writer = new System.IO.StringWriter(sb))
            {
                foreach(char c in "I'm an async message.")
                {
                    await writer.WriteAsync(c);
                }
                Console.WriteLine(sb.ToString());
                System.Threading.Thread.Sleep(500);
            }
        }

        public async Task Preach() 
        {
            var sb = new StringBuilder();
            using(var writer = new System.IO.StringWriter(sb))
            {
                foreach(char c in "I'm an async message preach.")
                {
                    await writer.WriteAsync(c);
                }
                Console.WriteLine(sb.ToString());
                System.Threading.Thread.Sleep(500);
            }
        }
    }
}