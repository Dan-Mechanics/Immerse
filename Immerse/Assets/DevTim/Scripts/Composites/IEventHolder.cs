using System;

namespace Immerse
{
    public interface IEventHolder
    {
        public event Action OnEvent;
    }
}
