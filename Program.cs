using System;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace InterceptorPOC
{
    class Program
    {
        private static IContainer Container { get; set; }
        private static IServiceProvider ServiceProvider { get; set; }

        public static void WriteMessage()
        {
            using(var scope = Container.BeginLifetimeScope())
            {
                var messageMaker = scope.Resolve<IMessageMaker>();
                messageMaker.WriteMessage();
                messageMaker.Preach();
            }
        }

        public static async Task WriteMessageAsync()
        {
            using(var scope = Container.BeginLifetimeScope())
            {
                var messageMaker = scope.Resolve<IMessageMaker>();
                await messageMaker.WriteMessage();
                await messageMaker.Preach();
            }
        }

        public static void DoOtherStuff()
        {
            using(var scope = Container.BeginLifetimeScope())
            {
                var otherThing = scope.Resolve<IOther>();
                otherThing.DoStuff();
            }
        }

        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            
            var builder = new ContainerBuilder();
            builder.Populate(serviceCollection);            

            var messageMaker = new AsyncMessageMaker();
            var generator = new ProxyGenerator();
            var interceptor = new AsyncProfilerInterceptor();
            var messageMakerProxy = generator.CreateInterfaceProxyWithTargetInterface<IMessageMaker>(messageMaker, interceptor);
            var otherProxy = generator.CreateInterfaceProxyWithTargetInterface<IOther>(new ConcreteOther(), interceptor);

            builder.Register<IOther>(t => otherProxy);
            builder.Register<IMessageMaker>(t => messageMakerProxy);

            // builder.RegisterType<MeanMessageMaker>()
            //        .As<IMessageMaker>()
            //        .EnableInterfaceInterceptors();
            //        .InterceptedBy(typeof(AsyncProfilerInterceptor));

            // builder.RegisterType<AsyncMessageMaker>()
            //        .As<IMessageMaker>()
            //        .EnableInterfaceInterceptors();
            //        .InterceptedBy(typeof(AsyncProfilerInterceptor));

            Container = builder.Build();
            ServiceProvider = new AutofacServiceProvider(Container);
            
            await WriteMessageAsync();
            DoOtherStuff();
        }
    }
}
