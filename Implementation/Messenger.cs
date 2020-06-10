using InterceptorPOC.Definition;

namespace InterceptorPOC.Implementation
{
    public class Messenger : IMessenger
    {
        private readonly IMessageMaker _messageMaker;

        public Messenger(IMessageMaker messageMaker)
        {
            _messageMaker = messageMaker;
        }

        public void GetMessage() => _messageMaker.WriteMessage();

        public void GetAnotherMessage() => _messageMaker.Preach();
    }
}
