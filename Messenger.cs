namespace InterceptorPOC
{
    public class Messenger
    {
        private readonly IMessageMaker _messageMaker;

        public Messenger(IMessageMaker messageMaker)
        {
            _messageMaker = messageMaker;
        }

        public void TellMe() => _messageMaker.WriteMessage();

        public void Preach() => _messageMaker.Preach();
    }
}
