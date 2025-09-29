namespace Immerse
{
    public interface IReceiver<T> 
    {
        public void Send(T value);
    }
}
