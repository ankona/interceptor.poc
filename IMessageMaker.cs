using System.Threading.Tasks;

namespace dt
{
    public interface IMessageMaker
    {
        Task WriteMessage();
        Task Preach();
    }


    public interface IAsyncMessageMaker
    {
        Task WriteMessage();
        Task Preach();
    }
}