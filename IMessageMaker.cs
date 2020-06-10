using System.Threading.Tasks;

namespace InterceptorPOC
{
    public interface IMessageMaker
    {
        Task WriteMessage();
        Task Preach();
    }
}
