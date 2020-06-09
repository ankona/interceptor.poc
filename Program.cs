using System;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using System.Threading.Tasks;
using Castle.DynamicProxy;
// using Castle.DynamicProxy;

namespace dt
{
    class Program
    {
        private static IContainer Container { get; set; }
        private static IServiceProvider ServiceProvider { get; set; }

        public static double GetCourseAgeRelativeTo(DateTime courseDate, DateTime relativeTo)
        {
            Console.WriteLine($"courseDate={courseDate}, relativeTo={relativeTo}");
            
            var zeroTime = new DateTime(1, 1, 1);
            var dateDiff = zeroTime + (relativeTo - courseDate);

            var years = (dateDiff.Year - 1);
            var months = (dateDiff.Month - 1);

            var age = years + (months * 1.0) / 12;
            return age;
        }

        public static double GetCourseAgeRelativeTo2(DateTime courseDate, DateTime relativeTo)
        {
            if (courseDate == default(DateTime) || relativeTo == default(DateTime))
            {
                throw new ArgumentOutOfRangeException($"Unable to compute relative age with invalid inputs. Params: [courseDate: {courseDate}, relativeTo: {relativeTo}]");
            }

            var dateDiff = relativeTo - courseDate;
            return Math.Abs(dateDiff.TotalDays / 365);
        }

        public static void WriteMessage()
        {
            using(var scope = Container.BeginLifetimeScope())
            {
                var messageMaker = scope.Resolve<IAsyncMessageMaker>();
                messageMaker.WriteMessage();
                messageMaker.Preach();
            }
        }

        public static async Task WriteMessageAsync()
        {
            using(var scope = Container.BeginLifetimeScope())
            {
                var messageMaker = scope.Resolve<IAsyncMessageMaker>();
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
            // Console.WriteLine("Hello World!");

            // var courseDate= new DateTime(2019, 11, 1);
            // var relativeTo=new DateTime(2018, 9, 19);
            // var age = GetCourseAgeRelativeTo(courseDate, relativeTo);
            // Console.WriteLine($"courseAge in years: {age}");

            var serviceCollection = new ServiceCollection();
            
            var builder = new ContainerBuilder();
            builder.Populate(serviceCollection);            

            var messageMaker = new AsyncMessageMaker();
            var generator = new ProxyGenerator();
            var interceptor = new AsyncProfilerInterceptor();
            var messageMakerProxy = generator.CreateInterfaceProxyWithTargetInterface<IAsyncMessageMaker>(messageMaker, interceptor);
            var otherProxy = generator.CreateInterfaceProxyWithTargetInterface<IOther>(new ConcreteOther(), interceptor);

            builder.Register<IOther>(t => otherProxy);
            builder.Register<IAsyncMessageMaker>(t => messageMakerProxy);



            // builder.RegisterType<MeanMessageMaker>()
            //        .As<IMessageMaker>()
            //        .EnableInterfaceInterceptors()
            //        .InterceptedBy(typeof(ProfilerInterceptor));

            // builder.RegisterType<AsyncMessageMaker>()
            //        .As<IAsyncMessageMaker>()
            //        .EnableInterfaceInterceptors()
            //        .InterceptedBy(typeof(AsyncProfilerInterceptor));

            // builder.Register(i => new AsyncProfilerInterceptor());
            // builder.Register(i => new ExInterceptor());

            Container = builder.Build();
            ServiceProvider = new AutofacServiceProvider(Container);
            
            await WriteMessageAsync();
            DoOtherStuff();
        }
    }
}
