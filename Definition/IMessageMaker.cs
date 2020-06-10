using System.Threading.Tasks;

namespace InterceptorPOC.Definition
{
    public interface IMessageMaker
    {
        Task WriteMessage();
        Task Preach();
    }
}
