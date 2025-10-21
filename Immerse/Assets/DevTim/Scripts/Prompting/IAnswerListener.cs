using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Immerse
{
    public interface IAnswerListener
    {
        void GetAnswer(int index, Option option);
        void Dismiss();
    }
}
