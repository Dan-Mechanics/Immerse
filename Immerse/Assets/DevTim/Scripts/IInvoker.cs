using System;

namespace Immerse
{
    public interface IInvoker
    {
        public event Action OnInvoke;
    }
}
