using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Immerse
{
    public interface IReceiver<T> 
    {
        public void Send(T value);
    }
}
